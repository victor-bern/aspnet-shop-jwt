using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("users")]

    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users
            .AsNoTracking()
            .ToListAsync();
            return Ok(users);
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                model.Password = "";

                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.UserName == model.UserName && x.Password == model.Password)
            .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha Inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Update([FromServices] DataContext context, [FromBody] User model, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Delete([FromServices] DataContext context, int id)
        {
            var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuário deletado com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível excluir o usuário" });
            }
        }
    }
}
