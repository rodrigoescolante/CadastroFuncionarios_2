
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;

namespace Aplicação.Models
{
    public class Usuarios
    {
        public string Login { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public string Cadastrar(LoginDTO registro, Db_Funcionarios context)
        {
            if (VerificarUsuario(registro.Usuario,context)==true)
            {
                return $"Usuário {registro.Usuario} já possui login e senha.";

            }
            else if (!Password1.Equals(Password2))
            {
                return "As senhas digitadas devem ser iguais.";
            }
            else
            {
                context.Users.Add(registro);
                context.SaveChanges();
                return $"Usuário {registro.Usuario} foi cadastrado com sucesso.";
            }                
        }
        public static string Excluir(LoginDTO registro, Db_Funcionarios context)
        {
            if (VerificarUsuario(registro.Usuario, context)==false)
            {
                return $"{registro.Usuario} não está cadastrado no banco de dados. Digite um usuário cadastrado.";
            }
            else if (VerificarSenha(registro.Usuario, registro.Senha,context)==false)
                {
                    return $"A senha digitada não é a cadastrada para {registro.Usuario}.";
                }
            else
                {
                var user = context.Users.Find(registro.UI);
                context.Users.Remove(user);
                context.SaveChanges();
                return $"Usuário {registro.Usuario} não tem mais permissões de escrita no banco de dados.";
                }            
        }
        
        public static bool VerificarUsuario(string username, Db_Funcionarios context)
        {
           if (context.Users.Any(u => u.Usuario == username) || username == "admin")
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
            if (context.Users.Any(u => u.Usuario == username && u.Senha == senha) || (username == "admin" && senha == "admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }             

            public static int ObterUI(string username, Db_Funcionarios context)
        {
            var user = context.Users.FirstOrDefault(u => u.Usuario == username);
            if (user != null)
            {
                return user.UI;
            }
            else
            {
                return 0;
            }        
        }
    }
}
