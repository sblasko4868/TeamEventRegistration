using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeamEventRegistration.Core.DTOs;
using TeamEventRegistration.Core.Services;

namespace TeamEventRegistration.Web.Api
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class ZohoController : ControllerBase
    {
        private ZohoRegistrationService _service;

        public ZohoController(ZohoRegistrationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Webhook([FromBody] Core.DTOs.ZohoWebhookPayload.ZohoWebhookPayload zohoWebhook)
        {
            await _service.UpdateRegistrationData(zohoWebhook.requests.request_status, zohoWebhook.requests.request_id, zohoWebhook);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Test(int id)
        {
            if (id == 0) return NotFound();
            try
            {
                await _service.Test(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
