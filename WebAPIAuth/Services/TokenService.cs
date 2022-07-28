using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPIAuth.Services
{
    public static class TokenService
    {
        public static string GerarChaveToken(Usuario usuario)
        {
            var token = new JwtSecurityTokenHandler();
            var assinatura = Encoding.ASCII.GetBytes(Ambiente.Chave);
            var corpo = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Apelido.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Cargo.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(assinatura),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var chave = token.CreateToken(corpo);

            return token.WriteToken(chave);
        }
    }
}
