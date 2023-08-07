using CadastroFuncionarios.Context;
using Microsoft.IdentityModel.Tokens;
using Funcionario = CadastroFuncionarios.Classes.Funcionario;

namespace Serviço.Models
{
    public class FuncionarioValidator 
    {
        private static bool BeUniqueNome(string nome, Db_Funcionarios _context)
        {
            return !_context.Funcionarios.Any(x => x.Nome.Equals(nome));
        }

        private static bool BeUniqueEmail(string email, Db_Funcionarios _context)
        {
            return !_context.Funcionarios.Any(x => x.Email.Equals(email));
        }
        private static bool BeUniqueID(int id, Db_Funcionarios _context)
        {
            return !_context.Funcionarios.Any(x => x.ID.Equals(id));
        }
        public static string PreValidate(Db_Funcionarios context)
        {
            if (!context.Funcionarios.Any())
            {
                return "Nenhum funcionário cadastrado.";
            }
            else
            {
                return null;
            }
        }
        public static string FuncionariosExists(int id, Db_Funcionarios _context)
        {
            var resultado = _context.Funcionarios?.Any(e => e.ID == id);
            if (resultado == false)
            {
                return "Digite um ID cadastrado.";
            }
            else
            {
                return null;
            }
        }

       
        public static string Cadastro(Funcionario func, Db_Funcionarios _context)
        {
            if (!BeUniqueID(func.ID,_context))
            {
                return "Este ID já está em uso. Digite um id válido.";
            }
            else if (func.ID < 1 || func.ID > 1000)
            {
                return "O ID deve ser entre 1 e 1000.";
            }
            else if(!BeUniqueNome(func.Nome, _context))
            {
                return "O nome digitado já está cadastrado na base de dados.";            
            }
            else if (!BeUniqueEmail(func.Email, _context))
            {
                return "O e-mail digitado já está cadastrado na base de dados.";
            }
            else
            {
                return Validacao(func);
            }
        }

        public static string Update(int id, Funcionario func, Db_Funcionarios _context)
        {
            var existingFuncionario = _context.Funcionarios.FirstOrDefault(x => x.Nome.Equals(func.Nome) && x.ID != id);
            var existingEmail = _context.Funcionarios.FirstOrDefault(x => x.Email.Equals(func.Email) && x.ID != id);
            var funcExists = FuncionariosExists(id, _context);
            if (!funcExists.IsNullOrEmpty())
            {
                return funcExists;
            }
            else if (existingFuncionario != null)
            {
                return "Este nome já está cadastrado para outro funcionário. Digite um nome válido.";
            }
            else if (existingEmail != null)
            {
                return "Este e-mail já está cadastrado para outro funcionário. Digite um e-mail válido.";
            }
            else
            {
                return Validacao(func);
            }
        }
        private static string Validacao(Funcionario func)
        {
            if (func.Idade < 18 || func.Idade > 65)
            {
                return "A idade deve estar entre 18 e 65 anos.";
            }
            else if (!func.Genero.Equals("Masculino") && !func.Genero.Equals("Feminino"))
            {
                return "O gênero deve ser Masculino ou Feminino.";
            }
            else if (!func.Email.Contains("@"))
            {
                return "Digite um e-mail válido.";
            }
            else
            {
                return null;
            }
        }    
    }
}
