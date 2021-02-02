using StoreManager.API.Controllers;
using StoreManager.Application.Features.Brands.Commands.Create;
using StoreManager.Application.Features.Brands.Commands.Delete;
using StoreManager.Application.Features.Brands.Commands.Update;
using StoreManager.Application.Features.Brands.Queries.GetAllCached;
using StoreManager.Application.Features.Brands.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    public class BrandController : BaseApiController<BrandController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var brands = await _mediator.Send(new GetAllBrandsCachedQuery());
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetBrandByIdQuery() { Id = id });
            return Ok(brand);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateBrandCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateBrandCommand command)
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
            return Ok(await _mediator.Send(new DeleteBrandCommand { Id = id }));
        }
    }
}