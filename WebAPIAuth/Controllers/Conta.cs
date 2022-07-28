using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIAuth.Services;

namespace WebAPIAuth.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class Conta : ControllerBase
    {
        private List<Usuario> DbUsuarios = new List<Usuario>
        {
            new Usuario {
                Id = 1,
                Apelido = "willian",
                Senha = "123456",
                Cargo = "Professor"
            },
            new Usuario {
                Id = 2,
                Apelido = "ariel",
                Senha = "123456",
                Cargo = "Aluno"
            },
        };

        [HttpPost]
        [Route("cadastrar")]
        [AllowAnonymous]
        public ActionResult Cadastrar(Usuario usuario)
        {
            if (DbUsuarios.Any(u => u.Id == usuario.Id))
            {
                return BadRequest();
            }
            else
            {
                DbUsuarios.Add(usuario);
                return Ok();
            }
        }

        [HttpPost]
        [Route("autenticar")]
        [AllowAnonymous]
        public ActionResult<dynamic> Autenticar(Credencial credencial)
        {
            var usuario = DbUsuarios.Where(x => x.Apelido == credencial.Username && credencial.Senha == x.Senha).FirstOrDefault();
            
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            var token = TokenService.GerarChaveToken(usuario);
            usuario.Senha = String.Empty;

            return Created(String.Empty, new 
            {
                Usuario = usuario,
                token = token,
            });
        }

        [HttpGet]
        [Route("usuarios")]
        [AllowAnonymous]
        public ActionResult<List<Usuario>> ListUsuarios()
        {
            return Ok(DbUsuarios);
        }

    }
}
