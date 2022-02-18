using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;
using TeamEventRegistration.Core.DTOs;
using TeamEventRegistration.Core.DTOs.ZohoCreationResponse;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.DTOs.ZohoCurrentStatus;

namespace TeamEventRegistration.Core.Services
{
    public class ZohoRegistrationService : IRegistrationService
    {
        /*Example webhook when completed
        {
    "requests": {
        "request_status": "completed",
        "owner_email": "sean@blaskoconsulting.com",
        "document_ids": [
            {
                "document_name": "Waiver and Release of Liability.pdf",
                "document_size": 115138,
                "document_order": "0",
                "is_editable": false,
                "total_pages": 3,
                "document_id": "222008000000016069"
            }
        ],
        "self_sign": false,
        "owner_id": "222008000000010003",
        "request_name": "Beer Olympics Waiver",
        "modified_time": 1622831221323,
        "action_time": 1623138029358,
        "is_deleted": false,
        "is_sequential": true,
        "owner_first_name": "Sean",
        "request_type_name": "Others",
        "owner_last_name": "Blasko",
        "request_id": "222008000000016075",
        "request_type_id": "222008000000000135",
        "actions": [
            {
                "verify_recipient": false,
                "action_type": "SIGN",
                "action_id": "222008000000016078",
                "is_revoked": false,
                "recipient_email": "sblasko4868@gmail.com",
                "is_embedded": false,
                "signing_order": 1,
                "recipient_name": "TestUser",
                "allow_signing": false,
                "recipient_phonenumber": "",
                "recipient_countrycode": "",
                "action_status": "SIGNED"
            }
        ]
    },
    "notifications": {
        "performed_by_email": "System Generated",
        "performed_at": 1623138029512,
        "country": "UNITED STATES",
        "activity": "Document has been completed",
        "operation_type": "RequestCompleted",
        "latitude": 41.08144,
        "performed_by_name": "System Generated",
        "ip_address": "65.25.99.217",
        "longitude": -81.51901
    }
}
        */

        private static string CLIENT_ID = "[REMOVED]";
        private static string CLIENT_SECRET = "[REMOVED]";
        private static string REFRESH_TOKEN = "[REMOVED]";
        private static string REDIRECT_URI = "https://localhost:44306/api/ZohoAuth/GetAuth";
        private static string REFRESH_TOKEN_URI = $"https://accounts.zoho.com/oauth/v2/token?refresh_token={REFRESH_TOKEN}&client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}&redirect_uri={REDIRECT_URI}&grant_type=refresh_token";
        private static string TEMPLATE_ID = "[REMOVED]";
        private static string ACTION_ID = "[REMOVED]";
        private static string SEND_TEMPLATE_FOR_SIGN_URI = $"https://sign.zoho.com/api/v1/templates/{TEMPLATE_ID}/createdocument";
        private static readonly HttpClient client = new HttpClient();
        private readonly TeamEventRegistrationDbContext _context;

        public ZohoRegistrationService(TeamEventRegistrationDbContext context)
        {
            _context = context;
        }

        private string AuthToken { get; set; }

        private async Task SetAuthTokenAsync()
        {
            var response = await client.PostAsync(REFRESH_TOKEN_URI, null);
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string,string>>(responseContent);
            string outValue;
            if (jsonObject.TryGetValue("access_token", out outValue))
            {
                AuthToken = outValue;
                //DEV Token
                //AuthToken = "1000.7b693f4983699e0870601caa0719f57b.54584bc540e7942c39647c5fe373071b";
            }
        }
        
        public async Task<RegistrationRequirementMetResponse> CheckRequirementMet(int registrationId)
        {
            var registrationToCheck = _context.Registrations.Find(registrationId);
            if (registrationToCheck == null) throw new KeyNotFoundException($"registrationId {registrationId} not found");
            if (registrationToCheck.RegistrationExternalId.Length == 0) throw new KeyNotFoundException($"RegistrationExternalId '{registrationToCheck.RegistrationExternalId}' is invalid");
            await SetAuthTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", AuthToken);
            var response = await client.GetAsync($"https://sign.zoho.com/api/v1/requests/{registrationToCheck.RegistrationExternalId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ZohoCurrentStatus>(responseContent);
            await UpdateRegistrationData(responseObject.requests.request_status, responseObject.requests.request_id, responseObject);
            var registrationToCheckAfter = _context.Registrations.Find(registrationId);
            return new RegistrationRequirementMetResponse() {
                IsMet = registrationToCheckAfter.RegistrationComplete,
                Message = $"Document Status: <br/>{responseObject.requests.actions[0].action_status}"
            };
        }

        public async Task InvokeRequirementAsync(int registrationId)
        {
            //var data = "{\"templates\":{\"notes\":\"Addingnotes\",\"field_data\":{\"field_text_data\":{\"Fullname\":\"\",\"participantAddress\":\"AddressInsert\"}},\"actions\":[{\"action_id\":\"222008000000013016\",\"action_type\":\"SIGN\",
            //\"recipient_name\":\"TestUser\",\"role\":\"Participant\",\"recipient_email\":\"sblasko4868@gmail.com\",\"recipient_phonenumber\":\"\",\"recipient_countrycode\":\"\",\"verify_recipient\":false,\"verification_type\":\"EMAIL\"}]}}";
            var registration = _context.Registrations
                                    .Include(r => r.Member)
                                    .FirstOrDefault(r => r.Id == registrationId);
                                    
            var data = System.Text.Json.JsonSerializer.Serialize(new Hashtable()
            {
                { "templates", new Hashtable()
                    {
                        { "notes", registration.Member.Email},
                        { "field_data", new Hashtable()
                            {
                            // TODO: Fix prefill data on Zoho template
                                { "field_text_data", new Hashtable()
                                    {
                                        { "Fullname", registration.Member.FirstName + " " + registration.Member.LastName },
                                        { "participantAddress", registration.Member.StreetAddress}
                                    }
                                }
                            }
                        },
                        { "actions", new List<Hashtable>() {
                            new Hashtable()
                            {
                                { "action_id", ACTION_ID },
                                { "action_type", "SIGN" },
                                { "recipient_name", registration.Member.FirstName + " " + registration.Member.LastName },
                                { "recipient_email", registration.Member.Email },
                                { "recipient_phonenumber", "" },
                                { "recipient_countrycode", "" },
                                { "verify_recipient", false },
                                { "verification_type", "EMAIL" }
                            } }
                        }
                    }
                }
            });
            await SetAuthTokenAsync();

            using (var formContent = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "data", data },
                { "is_quicksend", true.ToString() }
            }))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", AuthToken);
                var response = await client.PostAsync(SEND_TEMPLATE_FOR_SIGN_URI, formContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ZohoCreationResponse>(responseContent);
                registration.RegistrationData = responseContent;
                registration.RegistrationExternalId = responseObject.requests.request_id;
                registration.RegistrationComplete = false;
                registration.RegistrationMessage = RegistrationRequirementEnumerations.Statuses.InProgress.ToString();
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(string memberId, int eventId)
        {
            await CreateAsync(new Registration()
            {
                EventId = eventId,
                MemberId = memberId,
                RegistrationComplete = false,
                RegistrationMessage = RegistrationRequirementEnumerations.Statuses.Initializing.ToString()
            });
        }

        public async Task CreateAsync(string memberId, int eventId, int teamId, bool isCaptain)
        {
            await CreateAsync(new Registration()
            {
                TeamId = teamId,
                EventId = eventId,
                MemberId = memberId,
                IsCaptain = isCaptain,
                RegistrationComplete = false,
                RegistrationMessage = RegistrationRequirementEnumerations.Statuses.Initializing.ToString()
            });
        }

        protected async Task CreateAsync(Registration registration)
        {
            if (_context.Registrations.Where(r => r.MemberId == registration.MemberId && r.EventId == registration.EventId).Any())
            {
                throw new ArgumentException($"Registration exists between Member ID {registration.MemberId} and {registration.EventId}");
            }
            else
            {
                var result = _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();
                await InvokeRequirementAsync(result.Entity.Id);
            }
        }

        public async Task UpdateRegistrationData(string status, string registrationExternalId, object jsonData)
        {
            var registrationToUpdate = _context.Registrations.FirstOrDefault(r => r.RegistrationExternalId == registrationExternalId);
            if (registrationToUpdate != null)
            {
                registrationToUpdate.RegistrationData = JsonConvert.SerializeObject(jsonData);
                switch (status)
                {
                    case "declined":
                        registrationToUpdate.RegistrationMessage = RegistrationRequirementEnumerations.Statuses.Cancelled.ToString();
                        registrationToUpdate.RegistrationComplete = false;
                        break;
                    case "completed":
                        registrationToUpdate.RegistrationMessage = RegistrationRequirementEnumerations.Statuses.Complete.ToString();
                        registrationToUpdate.RegistrationComplete = true;
                        break;
                    case "inprogress":
                        registrationToUpdate.RegistrationMessage = RegistrationRequirementEnumerations.Statuses.InProgress.ToString();
                        registrationToUpdate.RegistrationComplete = false;
                        break;
                    default:
                        throw new ArgumentException($"request_status '{status}' not recognized");
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new KeyNotFoundException($"RegistrationExternalId '{registrationExternalId}' not found");
            }
        }

        public async Task Test(int registrationId)
        {
            try
            {
                var registration = _context.Registrations.Find(registrationId);
                if (registration == null) throw new KeyNotFoundException($"registrationId {registrationId} not found");
                registration.RegistrationData = null;
                registration.RegistrationExternalId = null;
                registration.RegistrationMessage = RegistrationRequirementEnumerations.Statuses.Initializing.ToString();
                await _context.SaveChangesAsync();
                await InvokeRequirementAsync(registrationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
