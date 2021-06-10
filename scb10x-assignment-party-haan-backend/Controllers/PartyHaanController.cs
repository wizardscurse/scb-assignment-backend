using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.Extensions;
using scb10x_assignment_party_haan_backend.Infrastructure.Services;

namespace scb10x_assignment_party_haan_backend.Controllers
{
    [ApiController]
    [Route("parties")]
    public class PartyHaanController : ControllerBase
    {
        private readonly IPartyHaanService _partyHaanService;

        public PartyHaanController(IPartyHaanService partyHaanService)
        {
            this._partyHaanService = partyHaanService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetParties()
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();
                var result = await _partyHaanService.GetParties(Int32.Parse(userId));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetPartHaanById(string id)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();
                var result = await _partyHaanService.GetPartHaanById(Int32.Parse(userId), Int32.Parse(id));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePartyHaan([FromBody] PartyHaanRequest request)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _partyHaanService.CreatePartyHaan(Int32.Parse(userId), request);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePartyHaan(string id, [FromBody] PartyHaanRequest request)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _partyHaanService.UpdatePartyHaan(Int32.Parse(userId), Int32.Parse(id), request);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete ]
        [Authorize]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeletePartyHaan(string id)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _partyHaanService.DeletePartyHaan(Int32.Parse(userId), Int32.Parse(id));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/add-member")]
        public async Task<IActionResult> AddPartyHaanMember(string id)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _partyHaanService.AddPartyHaanMember(Int32.Parse(userId), Int32.Parse(id));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}/delete-member")]
        public async Task<IActionResult> DeletePartyHaanMember(string id)
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _partyHaanService.DeletePartyHaanMember(Int32.Parse(userId), Int32.Parse(id));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
