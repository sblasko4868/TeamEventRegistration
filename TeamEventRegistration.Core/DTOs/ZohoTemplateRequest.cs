using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.DTOs.ZohoTemplateRequest
{

    public class ZohoTemplateRequest
    {
        public Templates templates { get; set; }

        public ZohoTemplateRequest()
        {

        }
    }

    public class Templates
    {
        public Field_Data field_data { get; set; }
        public Action[] actions { get; set; }
        public string notes { get; set; }
    }

    public class Field_Data
    {
        public Field_Text_Data field_text_data { get; set; }
        public Field_Boolean_Data field_boolean_data { get; set; }
        public Field_Date_Data field_date_data { get; set; }
    }

    public class Field_Text_Data
    {
    }

    public class Field_Boolean_Data
    {
    }

    public class Field_Date_Data
    {
    }

    public class Action
    {
        public string recipient_name { get; set; }
        public string recipient_email { get; set; }
        public string action_id { get; set; }
        public int signing_order { get; set; }
        public string role { get; set; }
        public bool verify_recipient { get; set; }
        public string private_notes { get; set; }
    }

}
