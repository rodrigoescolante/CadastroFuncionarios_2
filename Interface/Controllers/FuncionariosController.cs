using Microsoft.AspNetCore.Mvc;
using CadastroFuncionarios.Context;
using Microsoft.EntityFrameworkCore;
using Funcionario = CadastroFuncionarios.Classes.Funcionario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serviço.Models;

namespace CadastroFuncionarios.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly Db_Funcionarios _context;

        public FuncionariosController(Db_Funcionarios context)
        {
            _context = context;
        }

        // GET: api/Funcionarios
        /// <summary>
        /// Mostra todos os funcionários cadastrados no banco de dados.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            
            var resultado = FuncionarioValidator.PreValidate(_context);
            if (!resultado.IsNullOrEmpty())
            {
                return BadRequest(resultado);
            }
            else
            {
                return await _context.Funcionarios.ToListAsync();
            }
         }

        // GET: api/Funcionarios/5
        /// <summary>
        /// Mostra as informações de um funcionário cadastrado no banco de dados.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Funcionario>> GetFuncionarios(int id)
        {
            var resultado = FuncionarioValidator.FuncionariosExists(id, _context);
            if (!resultado.IsNullOrEmpty())
            {
                return BadRequest(resultado);
            }
            else
            {
                return await _context.Funcionarios.FindAsync(id);
            }
        }

        // PUT: api/Funcionarios/5
        /// <summary>
        /// Modifica o cadastro de um funcionário no banco de dados.
        /// </summary>
        ///<remarks>
        /// Exemplo de requisição:
        /// 
        ///     {
        ///        "id": 1,
        ///        "Nome": "Item #1",
        ///        "Idade": 23,
        ///        "Genero" "Masculino",
        ///        "Email": "teste@teste.com"
        ///     }
        ///
        /// </remarks>
        [HttpPut(Name = "PutFuncionarios"), Authorize]
        public async Task<IActionResult> PutFuncionarios(int id, Funcionario funcionarios)
        {
            var resultado = FuncionarioValidator.Update(id, funcionarios, _context);

                if (!resultado.IsNullOrEmpty())
                {
                    return BadRequest(resultado);
                }
                else
                {
                    _context.Entry(funcionarios).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return BadRequest("Cadastro alterado com sucesso.");     
                }            
        }

        // POST: api/Funcionarios
        /// <summary>
        /// Cadastra um novo funcionário no banco de dados.
        /// </summary>
        [HttpPost(Name = "PostFuncionarios"), Authorize]
        public async Task<ActionResult<Funcionario>> PostFuncionarios(int id, string nome, int idade, string genero,string email)
        {
                Funcionario funcionarios = new();
                funcionarios.ID = id;
                funcionarios.Nome = nome;
                funcionarios.Idade = idade; 
                funcionarios.Genero = genero;
                funcionarios.Email = email;
                var resultado = FuncionarioValidator.Cadastro(funcionarios, _context);

            try
            {
                if (!resultado.IsNullOrEmpty())
                {
                    return BadRequest(resultado);
                }
                else
                {
                    _context.Funcionarios.Add(funcionarios);
                    await _context.SaveChangesAsync();
                    CreatedAtAction("GetFuncionarios", new { id = funcionarios.ID }, funcionarios);
                    return BadRequest("Funcionário cadastrado com sucesso!");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Preencha todos os campos do formulário.");
            }
        }

        // DELETE: api/Funcionarios/5
        /// <summary>
        /// Deleta um funcionário do banco de dados.
        /// </summary>
        [HttpDelete(Name = "DeleteFuncionarios"), Authorize]
        public async Task<IActionResult> DeleteFuncionarios(int id)
        {
            var resultado = FuncionarioValidator.FuncionariosExists(id, _context);
                if (!resultado.IsNullOrEmpty())
            {
                    return BadRequest(resultado);
                }
                else
                {
                    var funcionarios = await _context.Funcionarios.FindAsync(id);
                    _context.Funcionarios.Remove(funcionarios);
                    await _context.SaveChangesAsync();
                    return BadRequest($"O ID {id} foi excluído da base de dados.");
                }
        }
    }
}
