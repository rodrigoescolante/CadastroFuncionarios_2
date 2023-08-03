
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;
using Microsoft.EntityFrameworkCore;

namespace Aplicação.Models
{
    public class Usuarios
    {
        public string Login { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public static Dictionary<string, string> Credenciais = new() { { "admin", "admin" } };        

        public string Cadastrar()
        {
            if (Credenciais.ContainsKey(Login))
            {
                return "Usuário já possui login e senha.";

            }
            else if (!Password1.Equals(Password2))
            {
                return "O Password1 e Password2 devem ser iguais.";
            }
            else
            {
                //BuscarUsuarios(Credenciais);
                Credenciais.Add(Login, Password1);
                var db = new Db_Funcionarios(new DbContextOptionsBuilder<Db_Funcionarios>().UseSqlServer("ConexaoDb").Options);
                var user = new LoginDTO();
                user.Usuario = Login;
                user.Senha = Password1;
                db.Users.Add(user);
                db.SaveChanges();
                return "Usuário cadastrado com sucesso";
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
                //BuscarUsuarios(Credenciais);
                Credenciais.TryGetValue(user, out senha);
                Credenciais.Remove(user);
                var registro = new LoginDTO();
                registro.Usuario = user;
                registro.Senha = senha;
                var db = new Db_Funcionarios(new DbContextOptionsBuilder<Db_Funcionarios>().UseSqlServer("ConexaoDb").Options);
                db.Users.Remove(registro);
                db.SaveChanges();
                return $"{user} não tem mais permissões de escrita no banco de dados.";
            }
        }
        private Dictionary<string, string> BuscarUsuarios(Dictionary<string, string> usuarios)
        {
            var db = new Db_Funcionarios(new DbContextOptionsBuilder<Db_Funcionarios>().UseSqlServer("ConexaoDb").Options);
            var users = db.Users.ToList();
            foreach (var registro in users)
            {
                  usuarios.Add(registro.Usuario, registro.Senha);
            }
            return usuarios;
        }                
    }
}
