﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroFuncionarios.Classes;

public class Funcionario
{
    public int ID { get; set; }
    public string? Nome { get; set; }
    public int? Idade { get; set; }
    public string? Genero { get; set; }
    public string? Email { get; set; }
}

public class LoginDTO
{
    [Key]
    public int UI { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }

}
