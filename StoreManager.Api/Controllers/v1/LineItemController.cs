using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManager.API.Controllers;
using StoreManager.Application.Features.LineItems.Commands.Create;
using StoreManager.Application.Features.LineItems.Commands.Delete;
using StoreManager.Application.Features.LineItems.Commands.Update;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using StoreManager.Application.Features.LineItems.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class LineItemController : BaseApiController<LineItemController>
    {
        [HttpGet]
        public async Task<ActionResult<List<GetAllLineItemsResponse>>> GetAll()
        {
            var lineItems = (await _mediator.Send(new GetAllLineItemsQuery())).Data;
            return lineItems;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var lineItem = (_mediator.Send(new GetLineItemByIdQuery() { Id = id })).Result.Data;
            return Ok(lineItem);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLineItemCommand command)
        {
            if (command == null)
                return BadRequest();

            //VAlidation
            if (command.ClaimId == 0 || command.Payee == string.Empty)
            {
                ModelState.AddModelError("ClaimId/Payee", "The ClaimId or Payee shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdLineItem = await _mediator.Send(command);
            if(command.Receipt.Length > 0 && createdLineItem !=null) await _mediator.Send(new UpdateLineItemImageCommand() { Id = createdLineItem.Data, Receipt = command.Receipt });
            return Created("lineItem", createdLineItem);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateLineItemCommand command)
        {
            if (command == null)
                return BadRequest();

            if (command.Id != id)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lineItemToUpdate = (_mediator.Send(new GetLineItemByIdQuery() { Id = id })).Result.Data;
           
            if (lineItemToUpdate == null)
                return NotFound();

            await _mediator.Send(command);
            await _mediator.Send(new UpdateLineItemImageCommand() { Id = command.Id, Receipt = command.Receipt });
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var employeeToDelete = (_mediator.Send(new GetLineItemByIdQuery() { Id = id })).Result.Data;
            if (employeeToDelete == null)
                return NotFound();

            return Ok(await _mediator.Send(new DeleteLineItemCommand { Id = id }));
        }
    }
}