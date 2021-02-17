using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<List<Product>>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<List<Product>>> GetByCategorie([FromServices] DataContext context, int id)
        {
            var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Ocorreu um erro ao criar produto" });
            }

        }



    }
}