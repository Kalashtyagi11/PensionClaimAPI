using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PensionClaimAPI.Models;
using PensionClaimAPI.Services;

namespace PensionClaimAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpPost]
        public async Task<ActionResult<Claim>> CreateClaim(Claim claim)
        {
            try
            {
                var result = await _claimService.CreateClaimAsync(claim);
                return CreatedAtAction(nameof(GetClaim), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetAllClaims()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Claim>>> GetApprovedClaims()
        {
            var claims = await _claimService.GetApprovedClaimsAsync();
            return Ok(claims);
        }

        [HttpPut("{id}/approve")]
        public async Task<ActionResult<Claim>> ApproveClaim(Guid id, [FromBody] ClaimActionRequest request)
        {
            try
            {
                var claim = await _claimService.ApproveClaimAsync(id);
                return Ok(claim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/reject")]
        public async Task<ActionResult<Claim>> RejectClaim(Guid id, [FromBody] ClaimActionRequest request)
        {
            try
            {
                var claim = await _claimService.RejectClaimAsync(id);
                return Ok(claim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/final-approve")]
        public async Task<ActionResult<Claim>> FinalApproveClaim(Guid id, [FromBody] ClaimActionRequest request)
        {
            try
            {
                var claim = await _claimService.FinalApproveClaimAsync(id);
                return Ok(claim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/final-reject")]
        public async Task<ActionResult<Claim>> FinalRejectClaim(Guid id, [FromBody] ClaimActionRequest request)
        {
            try
            {
                var claim = await _claimService.FinalRejectClaimAsync(id);
                return Ok(claim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetClaim(Guid id)
        {
            var claims = await _claimService.GetAllClaimsAsync();
            var claim = claims.FirstOrDefault(c => c.Id == id);
            
            if (claim == null)
                return NotFound();

            return Ok(claim);
        }
    }
} 