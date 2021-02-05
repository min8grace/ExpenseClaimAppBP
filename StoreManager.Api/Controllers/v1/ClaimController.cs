using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManager.API.Controllers;
using StoreManager.Application.Features.Claims.Commands.Create;
using StoreManager.Application.Features.Claims.Commands.Delete;
using StoreManager.Application.Features.Claims.Commands.Update;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Claims.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class ClaimController : BaseApiController<ClaimController>
    {
        [HttpGet]
        public async Task<ActionResult<List<GetAllClaimsResponse>>> GetAll()
        {
            var claims = (await _mediator.Send(new GetAllClaimsQuery())).Data;
            return claims;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var claim = (_mediator.Send(new GetClaimByIdQuery() { Id = id })).Result.Data;
            return Ok(claim);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] CreateClaimCommand command)
        {
            if (command == null)
                return BadRequest();

            ////VAlidation
            //if (command. == string.Empty || command.LastName == string.Empty)
            //{
            //    ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClaimId = ( _mediator.Send(command)).Result.Data; 
            Claim createdClaim = GetClaimById(createdClaimId);
            return Created("Claim", createdClaim);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateClaimCommand command)
        {
            if (command == null)
                return BadRequest();

            if (command.Id != id)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var claimToUpdate = (_mediator.Send(new GetClaimByIdQuery() { Id = id })).Result.Data;

            if (claimToUpdate == null)
                return NotFound();

            await _mediator.Send(command);

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var employeeToDelete = (_mediator.Send(new GetClaimByIdQuery() { Id = id })).Result.Data;
            if (employeeToDelete == null)
                return NotFound();

            return Ok(await _mediator.Send(new DeleteClaimCommand { Id = id }));
        }

        private Claim GetClaimById(int id)
        {
            var claim = (_mediator.Send(new GetClaimByIdQueryToClaim() { Id = id })).Result.Data;
            return claim;
        }
    }
}