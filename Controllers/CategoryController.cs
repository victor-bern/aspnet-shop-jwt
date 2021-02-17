using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shop.Models;
using System.Collections.Generic;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{

    [Route("categories")]
    public class CategoryController : Controller
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent",
         Location = ResponseCacheLocation.Any,
         Duration = 30)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories
            .AsNoTracking()
            .ToListAsync();
            return Ok(categories);

        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById([FromServices] DataContext context, int id)
        {
            var category = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {

                return BadRequest(new { message = "Não foi possível criar a categoria" });
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Update([FromBody] Category model, [FromServices] DataContext context, int id)
        {
            if (model.Id != id)
                return NotFound(new { message = "Categoria não encontrada" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esse registro já foi atualizado" });

            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a categoria" });

            }


        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Delete([FromServices] DataContext context, int id)
        {
            var category = await context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Categoria Não encontrada" });
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível excluir a categoria" });
            };
        }

    }


}