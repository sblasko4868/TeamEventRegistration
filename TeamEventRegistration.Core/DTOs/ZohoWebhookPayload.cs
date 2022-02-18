using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.DTOs.ZohoWebhookPayload
{
    /*
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

    public class ZohoWebhookPayload
    {
        public Requests requests { get; set; }
        public Notifications notifications { get; set; }
    }

    public class Requests
    {
        public string request_status { get; set; }
        public string owner_email { get; set; }
        public Document_Ids[] document_ids { get; set; }
        public bool self_sign { get; set; }
        public string owner_id { get; set; }
        public string request_name { get; set; }
        public long modified_time { get; set; }
        public long action_time { get; set; }
        public bool is_deleted { get; set; }
        public bool is_sequential { get; set; }
        public string owner_first_name { get; set; }
        public string request_type_name { get; set; }
        public string owner_last_name { get; set; }
        public string request_id { get; set; }
        public string request_type_id { get; set; }
        public Action[] actions { get; set; }
    }

    public class Document_Ids
    {
        public string document_name { get; set; }
        public int document_size { get; set; }
        public string document_order { get; set; }
        public bool is_editable { get; set; }
        public int total_pages { get; set; }
        public string document_id { get; set; }
    }

    public class Action
    {
        public bool verify_recipient { get; set; }
        public string action_type { get; set; }
        public string action_id { get; set; }
        public bool is_revoked { get; set; }
        public string recipient_email { get; set; }
        public bool is_embedded { get; set; }
        public int signing_order { get; set; }
        public string recipient_name { get; set; }
        public bool allow_signing { get; set; }
        public string recipient_phonenumber { get; set; }
        public string recipient_countrycode { get; set; }
        public string action_status { get; set; }
    }

    public class Notifications
    {
        public string performed_by_email { get; set; }
        public long performed_at { get; set; }
        public string country { get; set; }
        public string activity { get; set; }
        public string operation_type { get; set; }
        public float latitude { get; set; }
        public string performed_by_name { get; set; }
        public string ip_address { get; set; }
        public float longitude { get; set; }
    }

}
