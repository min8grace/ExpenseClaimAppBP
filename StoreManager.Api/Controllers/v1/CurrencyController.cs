using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManager.API.Controllers;
using StoreManager.Application.Features.Currencies.Commands.Create;
using StoreManager.Application.Features.Currencies.Commands.Delete;
using StoreManager.Application.Features.Currencies.Commands.Update;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Application.Features.Currencies.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class CurrencyController : BaseApiController<CurrencyController>
    {
        [HttpGet]
        public async Task<ActionResult<List<GetAllCurrenciesResponse>>> GetAll()
        {
            var currencies = (await _mediator.Send(new GetAllCurrenciesQuery())).Data;
            return currencies;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currency = (_mediator.Send(new GetCurrencyByIdQuery() { Id = id })).Result.Data;
            return Ok(currency);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCurrencyCommand command)
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

            var createdCurrency = await _mediator.Send(command);

            return Created("currency", createdCurrency);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCurrencyCommand command)
        {
            if (command == null)
                return BadRequest();

            if (command.Id != id)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currencyToUpdate = (_mediator.Send(new GetCurrencyByIdQuery() { Id = id })).Result.Data;

            if (currencyToUpdate == null)
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

            var employeeToDelete = (_mediator.Send(new GetCurrencyByIdQuery() { Id = id })).Result.Data;
            if (employeeToDelete == null)
                return NotFound();

            return Ok(await _mediator.Send(new DeleteCurrencyCommand { Id = id }));
        }
    }
}
//using StoreManager.API.Controllers;
//using StoreManager.Application.Features.Currencies.Commands.Create;
//using StoreManager.Application.Features.Currencies.Commands.Delete;
//using StoreManager.Application.Features.Currencies.Commands.Update;
//using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
//using StoreManager.Application.Features.Currencies.Queries.GetById;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;

//namespace StoreManager.Api.Controllers.v1
//{
//    [AllowAnonymous]
//    [ApiVersion("1.0")]
//    public class CurrencyController : BaseApiController<CurrencyController>
//    {
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var brands = await _mediator.Send(new GetAllCurrenciesQuery());
//            return Ok(brands);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var brand = await _mediator.Send(new GetCurrencyByIdQuery() { Id = id });
//            return Ok(brand);
//        }

//        // POST api/<controller>
//        [HttpPost]
//        public async Task<IActionResult> Post(CreateCurrencyCommand command)
//        {
//            return Ok(await _mediator.Send(command));
//        }

//        // PUT api/<controller>/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, UpdateCurrencyCommand command)
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
//            return Ok(await _mediator.Send(new DeleteCurrencyCommand { Id = id }));
//        }
//    }
//}