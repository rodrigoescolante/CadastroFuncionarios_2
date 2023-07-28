using CadastroFuncionarios.Context;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Funcionario = CadastroFuncionarios.Classes.Funcionario;

namespace Serviço.Models
{
    public class FuncionarioValidator : AbstractValidator<Funcionario>
    {
        private readonly Db _context;
        public FuncionarioValidator(Db context)
        {
            _context = context;

            RuleFor(x => x.ID)
                .InclusiveBetween(1, 1000).WithMessage("O ID deve estar entre 1 e 1000.")
                .Must(BeUniqueID).WithMessage("Este ID já está em uso. Digite um id válido");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Must(BeUniqueNome).WithMessage("O nome digitado já está cadastrado na base de dados.");

            RuleFor(x => x.Idade).InclusiveBetween(18, 65).WithMessage("A idade deve estar entre 18 e 65 anos.");
            RuleFor(x => x.Genero).Must(x => x == "Masculino" || x == "Feminino").WithMessage("O gênero deve ser Masculino ou Feminino.");
            RuleFor(x => x.Email)
                .Must(x => x.Contains("@")).WithMessage("O email não é válido.")
                .Must(BeUniqueEmail).WithMessage("O e-mail digitado já está cadastrado na base de dados.");

        }
        private bool BeUniqueNome(string nome)
        {
            return !_context.Funcionarios.Any(x => x.Nome.Equals(nome));
        }

        private bool BeUniqueEmail(string email)
        {
            return !_context.Funcionarios.Any(x => x.Email.Equals(email));
        }
        private bool BeUniqueID(int id)
        {
            return !_context.Funcionarios.Any(x => x.ID.Equals(id));
        }
        public bool PreValidate(Db context)
        {
            if (!context.Funcionarios.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool FuncionariosExists(int id)
        {
            return (_context.Funcionarios?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public string FuncionariosUpdate(int id, Funcionario func)
        {
            var existingFuncionario = _context.Funcionarios.FirstOrDefault(x => x.Nome.Equals(func.Nome) && x.ID != id);
            var existingEmail = _context.Funcionarios.FirstOrDefault(x => x.Email.Equals(func.Email) && x.ID != id);
            if (FuncionariosExists(id) == false)
            {
                return "Digite um ID cadastrado.";
            }
            else if (func.ID != id)
            {
                return "O ID do funcionário não pode ser alterado. Utilize no campo o id, o ID informado na busca.";
            }
            else if (existingFuncionario != null)
            {
                return "Este nome já está cadastrado para outro funcionário. Digite um nome válido.";
            }
            else if (func.Idade < 18 || func.Idade > 65)
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
            else if (existingEmail != null)
            {
                return "Este e-mail já está cadastrado para outro funcionário. Digite um e-mail válido.";
            }
            else
            {
                return null;
            }
        }
    }
}
