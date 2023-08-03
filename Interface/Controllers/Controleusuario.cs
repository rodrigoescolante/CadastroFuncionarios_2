using CadastroFuncionarios.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
[HttpPost, Route("login")]
public IActionResult Login(string username, string password)
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
    public IActionResult CadastroUsuarios(string username, string password1, string password2)
    {
        try
        {
            Usuarios usuario = new();
            usuario.Username = username;
            usuario.Password1= password1;
            usuario.Password2= password2;
            return Ok(usuario.Cadastrar());
        }
        catch
        {
            return BadRequest("Verifique os dados digitados.");
        }
    }
    /// <summary>
    /// Exclui usuários de operações de escrita no banco de dados.
    /// </summary>    
    [HttpDelete(Name = "DeleteUsuarios"), Authorize]
    public IActionResult DeleteUsuarios(string username)
    {
        try
        {
            return Ok(Usuarios.Excluir(username));
        }
        catch
        {
            return BadRequest("Verifique os dados digitados.");
        }
    }
}

public class LoginDTO 
{
    public string Usuario { get; set; }
    public string Senha { get; set; }
     
 }

public class Usuarios
{
    public string Username { get; set; }
    public string Password1 { get; set; }
    public string Password2 { get; set; }

    public static Dictionary<string, string> Credenciais = new() { { "admin", "admin" } };
    public string Cadastrar()
    {
        if (Credenciais.ContainsKey(Username))
        {
            return "Usuário já possui login e senha.";

        }
        else if (!Password1.Equals(Password2))
        {
            return "O Password1 e Password2 devem ser iguais.";
        }
        else
        {
            Credenciais.Add(Username, Password1);
            return "Usuário cadastrado com sucesso";
        }
    }
    public static string Excluir(string user)
    {
        if (!Credenciais[user].Any())
        {
            return $"{user} não está cadastrado no banco de dados. Digite um usuário cadastrado.";
        }
        else if (user.Equals("admin"))
        {
            return "Não é possível excluir usuário do sistema.";
        }
        else
        {
            Credenciais.Remove(user);
            return $"{user} não tem mais permissões de escrita no banco de dados.";
        }
    }
}
