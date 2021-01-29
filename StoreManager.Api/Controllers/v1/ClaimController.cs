using StoreManager.API.Controllers;
using StoreManager.Application.Features.Claims.Commands.Create;
using StoreManager.Application.Features.Claims.Commands.Delete;
using StoreManager.Application.Features.Claims.Commands.Update;
using StoreManager.Application.Features.Claims.Queries.GetAllCached;
using StoreManager.Application.Features.Claims.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StoreManager.Api.Controllers.v1
{
    public class ClaimController : BaseApiController<ClaimController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllClaimsCachedQuery());
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
            return Ok(brand);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateClaimCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateClaimCommand command)
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
            return Ok(await _mediator.Send(new DeleteClaimCommand { Id = id }));
        }
    }
}