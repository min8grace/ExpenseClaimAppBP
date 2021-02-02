using StoreManager.API.Controllers;
using StoreManager.Application.Features.LineItems.Commands.Create;
using StoreManager.Application.Features.LineItems.Commands.Delete;
using StoreManager.Application.Features.LineItems.Commands.Update;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using StoreManager.Application.Features.LineItems.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class LineItemController : BaseApiController<LineItemController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllLineItemsQuery());
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetLineItemByIdQuery() { Id = id });
            return Ok(brand);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateLineItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateLineItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteLineItemCommand { Id = id }));
        }
    }
}