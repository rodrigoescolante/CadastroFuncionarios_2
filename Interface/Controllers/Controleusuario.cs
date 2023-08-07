using Aplicação.Models;
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serviço.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
    {
    private readonly Db_Funcionarios _context;

    public LoginController(Db_Funcionarios context)
    {
        _context = context;
    }

    /// <summary>
    /// Autentica usuário para operações de escrita no banco de dados.
    /// </summary>
    [HttpPost, Route("login")]
    public IActionResult Login([FromQuery(Name = "Username"), Required] string username, [FromQuery(Name = "Senha"), Required] string password)
    {
        
        if (Usuarios.VerificarUsuario(username,_context)==false)
        {
            return Unauthorized($"Usuário {username} não está cadastrado.");
        }
        else if (Usuarios.VerificarSenha(username, password, _context) == false)
        {
            return Unauthorized("Senha incorreta. Verifique o campo digitado.");
        }
        else
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
            return Ok($"Usuário {username} logado com sucesso. Copie o token {token} e utilize para autenticar seu acesso.");
        }      
    }
    /// <summary>
    /// Cadastra novos usuários para operações de escrita no banco de dados.
    /// </summary>
    [HttpPost(Name = "CadastroUsuarios"), Authorize]
    public IActionResult CadastroUsuarios([FromQuery(Name = "Username"), Required] string username, [FromQuery(Name = "Digite uma senha"), Required] string password1, [FromQuery(Name = "Digite novamente a senha"), Required] string password2)
    {
        var usuario = new Usuarios();
        usuario.Login = username;
        usuario.Password1 = password1;
        usuario.Password2 = password2;
        LoginDTO user = new();
        user.Usuario = username;
        user.Senha = password1;
        return Ok(usuario.Cadastrar(user,_context));
    }
    /// <summary>
    /// Exclui usuários de operações de escrita no banco de dados.
    /// </summary>    
    [HttpDelete(Name = "DeleteUsuarios"), Authorize]
    public IActionResult DeleteUsuarios([FromQuery(Name = "Usuário"), Required] string username, [FromQuery(Name = "Senha cadastrada do Usuário"), Required] string password)
    {
        LoginDTO user2 = new();
        user2.Usuario = username;
        user2.Senha = password;
        user2.UI = Usuarios.ObterUI(username,_context);
        return Ok(Usuarios.Excluir(user2, _context));
    }

    /// <summary>
    /// Lista o nome dos usuários com permissão de escrita no banco de dados.
    /// </summary>
    [HttpGet(Name = "GetUsuarios"),AllowAnonymous]    
    public ActionResult<List<string>> GetUsuarios()
    {
        var usuarios = _context.Users.Select(u => u.Usuario).ToList();
        if (usuarios.IsNullOrEmpty())
        {
            return BadRequest("Nenhum usuário com direitos administrativos cadastrado.");
        }
        else
        {
            return usuarios;
        }
    }
}