using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    /// <summary>
    /// Autentica usuário para operações de escrita no banco de dados.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     {
    ///        "username": "admin",
    ///        "password": "admin"
    ///     }
    ///
    /// </remarks>
    [HttpPost, Route("login")]    
    public IActionResult Login(LoginDTO loginDTO)
    {
        if (string.IsNullOrEmpty(loginDTO.Username) ||
            string.IsNullOrEmpty(loginDTO.Password))
            {
                return BadRequest("Usuario ou senha não informados");
            }
        else if (loginDTO.Username.Equals("admin") &&
            loginDTO.Password.Equals("admin"))
            {
                var secretKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("cadastrodefuncionarios"));
                var signinCredentials = new SigningCredentials
               (secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "ABCXYZ",
                    audience: "http://localhost:51398",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok($"Usuário logado com sucesso. Copie o token {token} e utilize para autenticar seu acesso.");
            }
        else
            {
                return Unauthorized("Usuário ou senha incorretos. Verifique os dados informados.");
            }       
    }
}

public class LoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}
