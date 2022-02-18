using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.DTOs.ZohoCreationResponse
{
    /*
    {
      "code": 0,
      "requests": {
        "request_status": "inprogress",
        "notes": "sblasko4868@gmail.com",
        "attachments": [],
        "reminder_period": 5,
        "owner_id": "222008000000010003",
        "description": "",
        "request_name": "Beer Olympics Waiver",
        "modified_time": 1624333605334,
        "action_time": 1624333605604,
        "is_deleted": false,
        "expiration_days": 15,
        "is_sequential": true,
        "sign_submitted_time": 1624333605604,
        "owner_first_name": "Sean",
        "sign_percentage": 33.34,
        "expire_by": 1625630340000,
        "is_expiring": false,
        "owner_email": "sean@blaskoconsulting.com",
        "created_time": 1624333605334,
        "email_reminders": true,
        "document_ids": [
          {
            "image_string": "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",
            "document_name": "Waiver and Release of Liability.pdf",
            "pages": [
              {
                "image_string": "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",
                "page": 0,
                "is_thumbnail": true
              }
            ],
            "document_size": 115138,
            "document_order": "0",
            "is_editable": false,
            "total_pages": 3,
            "document_id": "222008000000027053"
          }
        ],
        "self_sign": false,
        "in_process": false,
        "validity": -1,
        "request_type_name": "Others",
        "request_id": "222008000000027059",
        "request_type_id": "222008000000000135",
        "owner_last_name": "Blasko",
        "actions": [
          {
            "verify_recipient": false,
            "action_type": "SIGN",
            "private_notes": "",
            "recipient_email": "sblasko4868@gmail.com",
            "send_completed_document": true,
            "allow_signing": true,
            "recipient_phonenumber": "",
            "is_bulk": false,
            "action_id": "222008000000027062",
            "is_revoked": false,
            "is_embedded": false,
            "signing_order": 1,
            "fields": [
              {
                "field_id": "222008000000027064",
                "x_coord": 137,
                "field_type_id": "222008000000000153",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 100,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": true,
                  "font_size": 10,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "textfield",
                "field_label": "Full name",
                "name_format": "FULL_NAME",
                "is_mandatory": true,
                "default_value": "",
                "page_no": 1,
                "document_id": "222008000000027053",
                "field_name": "Full name",
                "y_value": 69.44093,
                "abs_width": 184,
                "action_id": "222008000000027062",
                "width": 29.98776,
                "y_coord": 550,
                "field_type_name": "Name",
                "description_tooltip": "",
                "x_value": 22.335909,
                "height": 1.608325
              },
              {
                "field_id": "222008000000027065",
                "x_coord": 230,
                "field_type_id": "222008000000000147",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 2048,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": false,
                  "font_size": 11,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "textfield",
                "field_label": "contactName",
                "is_mandatory": true,
                "default_value": "",
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Emergency Contact Name",
                "y_value": 19.394512,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 154,
                "field_type_name": "Textfield",
                "description_tooltip": "",
                "x_value": 37.5153,
                "height": 1.702933
              },
              {
                "field_id": "222008000000027071",
                "x_coord": 230,
                "field_type_id": "222008000000004003",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 2048,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": false,
                  "font_size": 11,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "dropdown_values": [
                  {
                    "dropdown_value_id": "222008000000027072",
                    "dropdown_order": 0,
                    "dropdown_value": "Parent"
                  },
                  {
                    "dropdown_value_id": "222008000000027073",
                    "dropdown_order": 1,
                    "dropdown_value": "Spouse"
                  },
                  {
                    "dropdown_value_id": "222008000000027074",
                    "dropdown_order": 2,
                    "dropdown_value": "Other"
                  }
                ],
                "field_category": "dropdown",
                "field_label": "contactRelationship",
                "is_mandatory": true,
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Contact Relationship",
                "y_value": 23.178808,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 184,
                "field_type_name": "Dropdown",
                "description_tooltip": "",
                "x_value": 37.5153,
                "height": 1.702933
              },
              {
                "field_id": "222008000000027066",
                "x_coord": 230,
                "field_type_id": "222008000000000147",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 2048,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": false,
                  "font_size": 11,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "textfield",
                "field_label": "contactPhone",
                "is_mandatory": true,
                "default_value": "",
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Contact Telephone",
                "y_value": 26.868496,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 213,
                "field_type_name": "Textfield",
                "description_tooltip": "",
                "x_value": 37.5153,
                "height": 1.702933
              },
              {
                "field_id": "222008000000027067",
                "x_coord": 225,
                "field_type_id": "222008000000000153",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 100,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": true,
                  "font_size": 11,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "textfield",
                "field_label": "Full name",
                "name_format": "FULL_NAME",
                "is_mandatory": true,
                "default_value": "",
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Full name",
                "y_value": 44.37086,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 351,
                "field_type_name": "Name",
                "description_tooltip": "",
                "x_value": 36.780907,
                "height": 1.702933
              },
              {
                "field_id": "222008000000027068",
                "x_coord": 91,
                "field_type_id": "222008000000000147",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 2048,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": false,
                  "font_size": 11,
                  "is_fixed_height": false,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "textfield",
                "field_label": "participantAddress",
                "is_mandatory": true,
                "default_value": "",
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Address",
                "y_value": 52.601704,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 417,
                "field_type_name": "Textfield",
                "description_tooltip": "",
                "x_value": 14.871481,
                "height": 1.702933
              },
              {
                "field_id": "222008000000027070",
                "x_coord": 158,
                "field_type_id": "222008000000000141",
                "abs_height": 19,
                "field_category": "image",
                "field_label": "Signature",
                "is_mandatory": true,
                "page_no": 2,
                "document_id": "222008000000027053",
                "field_name": "Signature",
                "y_value": 58.46736,
                "abs_width": 135,
                "action_id": "222008000000027062",
                "width": 22.031824,
                "y_coord": 463,
                "field_type_name": "Signature",
                "description_tooltip": "",
                "is_resizable": true,
                "x_value": 25.764994,
                "height": 2.459792
              },
              {
                "field_id": "222008000000027069",
                "x_coord": 132,
                "field_type_id": "222008000000000151",
                "abs_height": 13,
                "text_property": {
                  "is_italic": false,
                  "max_field_length": 2048,
                  "is_underline": false,
                  "font_color": "000000",
                  "is_fixed_width": false,
                  "font_size": 11,
                  "is_fixed_height": true,
                  "is_read_only": false,
                  "is_bold": false,
                  "font": "Roboto"
                },
                "field_category": "datefield",
                "field_label": "Sign Date",
                "is_mandatory": true,
                "time_zone_offset": -240,
                "page_no": 2,
                "document_id": "222008000000027053",
                "time_zone": "IET",
                "field_name": "Sign Date",
                "y_value": 62.062443,
                "abs_width": 98,
                "action_id": "222008000000027062",
                "width": 16.034271,
                "y_coord": 492,
                "date_format": "MMM dd yyyy HH:mm z",
                "field_type_name": "Date",
                "description_tooltip": "",
                "x_value": 21.603428,
                "height": 1.702933
              }
            ],
            "recipient_name": "Sean Blasko",
            "action_status": "UNOPENED",
            "recipient_countrycode": ""
          }
        ],
        "attachment_size": 0
      },
      "message": "Document has been submitted and sent for signature",
      "status": "success"
    }     
    */
    public class ZohoCreationResponse
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
        public object[] attachments { get; set; }
        public int reminder_period { get; set; }
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
        public bool email_reminders { get; set; }
        public Document_Ids[] document_ids { get; set; }
        public bool self_sign { get; set; }
        public bool in_process { get; set; }
        public int validity { get; set; }
        public string request_type_name { get; set; }
        public string request_id { get; set; }
        public string request_type_id { get; set; }
        public string owner_last_name { get; set; }
        public Action[] actions { get; set; }
        public int attachment_size { get; set; }
    }

    public class Document_Ids
    {
        public string image_string { get; set; }
        public string document_name { get; set; }
        public Page[] pages { get; set; }
        public int document_size { get; set; }
        public string document_order { get; set; }
        public bool is_editable { get; set; }
        public int total_pages { get; set; }
        public string document_id { get; set; }
    }

    public class Page
    {
        public string image_string { get; set; }
        public int page { get; set; }
        public bool is_thumbnail { get; set; }
    }

    public class Action
    {
        public bool verify_recipient { get; set; }
        public string action_type { get; set; }
        public string private_notes { get; set; }
        public string recipient_email { get; set; }
        public bool send_completed_document { get; set; }
        public bool allow_signing { get; set; }
        public string recipient_phonenumber { get; set; }
        public bool is_bulk { get; set; }
        public string action_id { get; set; }
        public bool is_revoked { get; set; }
        public bool is_embedded { get; set; }
        public int signing_order { get; set; }
        public Field[] fields { get; set; }
        public string recipient_name { get; set; }
        public string action_status { get; set; }
        public string recipient_countrycode { get; set; }
    }

    public class Field
    {
        public string field_id { get; set; }
        public int x_coord { get; set; }
        public string field_type_id { get; set; }
        public int abs_height { get; set; }
        public Text_Property text_property { get; set; }
        public string field_category { get; set; }
        public string field_label { get; set; }
        public string name_format { get; set; }
        public bool is_mandatory { get; set; }
        public string default_value { get; set; }
        public int page_no { get; set; }
        public string document_id { get; set; }
        public string field_name { get; set; }
        public float y_value { get; set; }
        public int abs_width { get; set; }
        public string action_id { get; set; }
        public float width { get; set; }
        public int y_coord { get; set; }
        public string field_type_name { get; set; }
        public string description_tooltip { get; set; }
        public float x_value { get; set; }
        public float height { get; set; }
        public Dropdown_Values[] dropdown_values { get; set; }
        public bool is_resizable { get; set; }
        public int time_zone_offset { get; set; }
        public string time_zone { get; set; }
        public string date_format { get; set; }
    }

    public class Text_Property
    {
        public bool is_italic { get; set; }
        public int max_field_length { get; set; }
        public bool is_underline { get; set; }
        public string font_color { get; set; }
        public bool is_fixed_width { get; set; }
        public int font_size { get; set; }
        public bool is_fixed_height { get; set; }
        public bool is_read_only { get; set; }
        public bool is_bold { get; set; }
        public string font { get; set; }
    }

    public class Dropdown_Values
    {
        public string dropdown_value_id { get; set; }
        public int dropdown_order { get; set; }
        public string dropdown_value { get; set; }
    }

}
