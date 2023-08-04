
using CadastroFuncionarios.Classes;
using CadastroFuncionarios.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Linq;

namespace Aplicação.Models
{
    public class Usuarios
    {
        private readonly Db_Funcionarios _context;
        public Usuarios(Db_Funcionarios context)
        {
            _context = context;
            Credenciais = _context.Users.ToDictionary(u => u.Usuario, u => u.Senha);
            Credenciais.Add("admin", "admin");
        }
        public string Login { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }

        public Dictionary<string, string> Credenciais;        

        public string Cadastrar(string login, string senha)
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
                var registro = new LoginDTO();
                registro.Usuario = login;
                registro.Senha = senha;
                registro.ID = GerarID();
                _context.Users.Add(registro);
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
                Credenciais.TryGetValue(user, out senha);
                var registro = new LoginDTO();
                registro.Usuario = user;
                registro.Senha = senha;
                registro.ID = GerarID();
                _context.Users.Remove(registro);
                _context.SaveChanges();
                return $"{user} não tem mais permissões de escrita no banco de dados.";
            }
        }
              
        private int GerarID()
        {
            Random numAleatorio = new();
            int a;
            do
            {
              a = numAleatorio.Next(1, 1000);
            } while (_context.Users.Any(x => x.ID == a));
            return a;
        }
    }
}
