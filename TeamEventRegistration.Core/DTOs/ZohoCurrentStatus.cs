using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.DTOs.ZohoCurrentStatus
{

    public class ZohoCurrentStatus
    {
        public int code { get; set; }
        public Requests requests { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

    public class Requests
    {
        public string request_status { get; set; }
        public string notes { get; set; }
        public string owner_id { get; set; }
        public string description { get; set; }
        public string request_name { get; set; }
        public long modified_time { get; set; }
        public long action_time { get; set; }
        public bool is_deleted { get; set; }
        public int expiration_days { get; set; }
        public bool is_sequential { get; set; }
        public long sign_submitted_time { get; set; }
        public string owner_first_name { get; set; }
        public float sign_percentage { get; set; }
        public long expire_by { get; set; }
        public bool is_expiring { get; set; }
        public string owner_email { get; set; }
        public long created_time { get; set; }
        public Document_Ids[] document_ids { get; set; }
        public bool self_sign { get; set; }
        public bool in_process { get; set; }
        public string request_type_name { get; set; }
        public string request_id { get; set; }
        public string request_type_id { get; set; }
        public string owner_last_name { get; set; }
        public Action[] actions { get; set; }
    }

    public class Document_Ids
    {
        public string document_name { get; set; }
        public int document_size { get; set; }
        public string document_order { get; set; }
        public int total_pages { get; set; }
        public string document_id { get; set; }
    }

    public class Action
    {
        public bool verify_recipient { get; set; }
        public string action_id { get; set; }
        public string action_type { get; set; }
        public string private_notes { get; set; }
        public string recipient_name { get; set; }
        public string recipient_email { get; set; }
        public int signing_order { get; set; }
        public bool allow_signing { get; set; }
        public string action_status { get; set; }
        public string recipient_phonenumber { get; set; }
        public string recipient_countrycode { get; set; }
    }

}
