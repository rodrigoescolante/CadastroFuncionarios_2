
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;


namespace Aplicação.Models
{
    public class Usuarios
    {
        private readonly Db_Funcionarios _context;
        public Usuarios(Db_Funcionarios context)
        {
            _context = context;
            var registro = new LoginDTO();
        }
        public string Login { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public string Cadastrar(LoginDTO registro)
        {
            if (VerificarUsuario(registro.Usuario,_context))
            {
                return "Usuário já possui login e senha.";

            }
            else if (!Password1.Equals(Password2))
            {
                return "As senhas digitadas devem ser iguais.";
            }
            else
            {
                _context.Users.Add(registro);
                _context.SaveChanges();
                return "Usuário cadastrado com sucesso.";
            }                
        }
        public string Excluir(LoginDTO registro)
        {
            if (!VerificarUsuario(registro.Usuario, _context))
            {
                return $"{registro.Usuario} não está cadastrado no banco de dados. Digite um usuário cadastrado.";
            }
            else if (registro.Usuario.Equals("admin"))
            {
                return "Não é possível excluir usuário do sistema.";
            }
            else if (VerificarSenha(registro.Usuario, registro.Senha,_context)==false)
                {
                    return $"A senha digitada não é a cadastrada para {registro.Usuario}.";
                }
            else
                {
                    _context.Users.Remove(registro);
                    _context.SaveChangesAsync();
                    return $"{registro.Usuario} não tem mais permissões de escrita no banco de dados.";
                }            
        }
        
        public static bool VerificarUsuario(string username, Db_Funcionarios context)
        {
           if (context.Users.Any(u => u.Usuario == username))
            {
                return true;
            }
           else
            {
                return false;
            }
        }

        public static bool VerificarSenha(string username, string senha, Db_Funcionarios context)
        {
            if (context.Users.Any(u => u.Usuario == username && u.Senha == senha))
            {
                return true;
            }
            else
            {
                return false;
            }
        }             

        public int GerarUI()
        {
            Random numAleatorio = new();
            int a;
            do
            {
              a = numAleatorio.Next(1, 1000);
            } while (_context.Users.Any(x => x.ID == a));
            return a;
        }

        public int ObterUI(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Usuario == username);
            if (user != null)
            {
                return user.ID;
            }
            else
            {
                return 0;
            }        
        }
    }
}
