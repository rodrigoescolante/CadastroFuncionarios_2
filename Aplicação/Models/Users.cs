
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;
using Microsoft.EntityFrameworkCore;

namespace Aplicação.Models
{
    public class Usuarios
    {
        private readonly Db_Funcionarios _context;
        public Usuarios(Db_Funcionarios context)
        {
            _context = context;
            BuscarUsuarios(Credenciais, _context);
        }
        public string Login { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public static Dictionary<string, string> Credenciais;        

        public string Cadastrar()
        {
            if (Credenciais.ContainsKey(Login))
            {
                return "Usuário já possui login e senha.";

            }
            else if (!Password1.Equals(Password2))
            {
                return "As senhas digitadas devem ser iguais.";
            }
            else
            {
                BuscarUsuarios(Credenciais, _context);
                var user = new LoginDTO();
                user.Usuario = Login;
                user.Senha = Password1;
                Random numAleatorio = new();
                user.ID = numAleatorio.Next(1, 1000);
                _context.Users.Add(user);
                _context.SaveChanges();
                return "Usuário cadastrado com sucesso.";
            }                
        }
    
        public string Excluir(string user)
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
                string senha;
                BuscarUsuarios(Credenciais,_context);
                Credenciais.TryGetValue(user, out senha);
                var registro = new LoginDTO();
                registro.Usuario = user;
                registro.Senha = senha;
                Random numAleatorio = new();
                registro.ID = numAleatorio.Next(1, 1000);
                _context.Users.Remove(registro);
                _context.SaveChanges();
                return $"{user} não tem mais permissões de escrita no banco de dados.";
            }
        }
        public static Dictionary<string, string> BuscarUsuarios(Dictionary<string, string> usuarios, Db_Funcionarios db)
        {
            //var users = context.Users.ToList();
            foreach (LoginDTO registro in db.Users)
            {
                  usuarios.Add(registro.Usuario, registro.Senha);
            }
            return usuarios;
        }                
    }
}
