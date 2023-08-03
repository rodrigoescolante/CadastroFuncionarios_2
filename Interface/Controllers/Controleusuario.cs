using Aplicação.Models;
using CadastroFuncionarios.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
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
[HttpPost, Route("login")]
public IActionResult Login([FromQuery(Name = "Username"), Required] string username, [FromQuery(Name = "Senha"), Required] string password)
    {
        LoginDTO loginDTO = new();
        loginDTO.Usuario = username;
        loginDTO.Senha = password;
        string? senha;        
        Usuarios.Credenciais.TryGetValue(loginDTO.Usuario, out senha);

        if (string.IsNullOrEmpty(loginDTO.Usuario) ||
            string.IsNullOrEmpty(loginDTO.Senha))
            {
                return BadRequest("Usuario ou senha não informados");
            }
        else if (loginDTO.Senha.Equals(senha))
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
    /// <summary>
    /// Cadastra novos usuários para operações de escrita no banco de dados.
    /// </summary>
    [HttpPost(Name = "CadastroUsuarios"), Authorize]
    public IActionResult CadastroUsuarios([FromQuery(Name = "Username"), Required] string username, [FromQuery(Name = "Digite uma senha"),Required] string password1, [FromQuery(Name = "Digite novamente a senha"), Required] string password2)
    {
            Usuarios usuario = new();
            usuario.Login = username;
            usuario.Password1= password1;
            usuario.Password2= password2;
            return Ok(usuario.Cadastrar());        
    }
    /// <summary>
    /// Exclui usuários de operações de escrita no banco de dados.
    /// </summary>    
    [HttpDelete(Name = "DeleteUsuarios"), Authorize]
    public IActionResult DeleteUsuarios([FromQuery(Name = "Username"), Required] string username)
    {
        var user = new Usuarios();
        return Ok(user.Excluir(username));        
    }
}