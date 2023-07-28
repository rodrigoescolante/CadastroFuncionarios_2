using CadastroFuncionarios.Classes;
using Microsoft.EntityFrameworkCore;



namespace CadastroFuncionarios.Context
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> context) : base(context)
        { }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
 }
