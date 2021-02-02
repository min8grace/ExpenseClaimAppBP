using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManager.API.Controllers;
using StoreManager.Application.Features.Categories.Commands.Create;
using StoreManager.Application.Features.Categories.Commands.Delete;
using StoreManager.Application.Features.Categories.Commands.Update;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Categories.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManager.Api.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class CategoryController : BaseApiController<CategoryController>
    {
        [HttpGet]
        public async Task<ActionResult<List<GetAllCategoriesResponse>>> GetAll()
        {
            var categories = (await _mediator.Send(new GetAllCategoriesQuery())).Data;
            return categories;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = (_mediator.Send(new GetCategoryByIdQuery() { Id = id })).Result.Data;
            return Ok(category);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
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

            var createdCategory = await _mediator.Send(command);

            return Created("category", createdCategory);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (command == null)
                return BadRequest();

            if (command.Id != id)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryToUpdate = (_mediator.Send(new GetCategoryByIdQuery() { Id = id })).Result.Data;

            if (categoryToUpdate == null)
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

            var employeeToDelete = (_mediator.Send(new GetCategoryByIdQuery() { Id = id })).Result.Data;
            if (employeeToDelete == null)
                return NotFound();

            return Ok(await _mediator.Send(new DeleteCategoryCommand { Id = id }));
        }
    }
}