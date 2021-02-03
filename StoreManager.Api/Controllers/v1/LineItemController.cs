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

            ////VAlidation
            //if (command. == string.Empty || command.LastName == string.Empty)
            //{
            //    ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdLineItem = await _mediator.Send(command);

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
//using StoreManager.API.Controllers;
//using StoreManager.Application.Features.LineItems.Commands.Create;
//using StoreManager.Application.Features.LineItems.Commands.Delete;
//using StoreManager.Application.Features.LineItems.Commands.Update;
//using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
//using StoreManager.Application.Features.LineItems.Queries.GetById;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;

//namespace StoreManager.Api.Controllers.v1
//{
//    [AllowAnonymous]
//    [ApiVersion("1.0")]
//    public class LineItemController : BaseApiController<LineItemController>
//    {
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var brands = await _mediator.Send(new GetAllLineItemsQuery());
//            return Ok(brands);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var brand = await _mediator.Send(new GetLineItemByIdQuery() { Id = id });
//            return Ok(brand);
//        }

//        // POST api/<controller>
//        [HttpPost]
//        public async Task<IActionResult> Post(CreateLineItemCommand command)
//        {
//            return Ok(await _mediator.Send(command));
//        }

//        // PUT api/<controller>/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, UpdateLineItemCommand command)
//        {
//            if (id != command.Id)
//            {
//                return BadRequest();
//            }
//            return Ok(await _mediator.Send(command));
//        }

//        // DELETE api/<controller>/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            return Ok(await _mediator.Send(new DeleteLineItemCommand { Id = id }));
//        }
//    }
//}