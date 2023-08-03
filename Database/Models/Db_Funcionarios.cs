using CadastroFuncionarios.Classes;
using Microsoft.EntityFrameworkCore;



namespace CadastroFuncionarios.Context
{
    public class Db_Funcionarios : DbContext
    {
        public Db_Funcionarios(DbContextOptions<Db_Funcionarios> context) : base(context)
        { }
        public DbSet<LoginDTO> Users { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
 }