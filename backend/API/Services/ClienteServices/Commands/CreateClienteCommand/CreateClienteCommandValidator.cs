using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Commands.CreateClienteCommand
{
  public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
  {
    private readonly ControlGlobalContext _context;

    public CreateClienteCommandValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(c => c.Nombre)
          .NotEmpty().WithMessage("El nombre no puede estar vacío")
          .NotNull().WithMessage("El nombre no puede ser nulo");

      RuleFor(c => c.Apellido)
      .NotEmpty().WithMessage("El apellido no puede estar vacío")
      .NotNull().WithMessage("El apellido no puede ser nulo");

      RuleFor(c => c.Identificacion)
      .NotEmpty().WithMessage("La identifiacion no puede estar vacía")
      .NotNull().WithMessage("La identifiacion no puede ser nula");

      RuleFor(p => p.Saldo)
           .NotNull().WithMessage("El saldo no puede ser nulo");

      RuleFor(c => c.Estado)
          .NotNull().WithMessage("El estado no puede ser nulo");

      RuleFor(c => c)
          .MustAsync(ClienteExiste).WithMessage("Este cliente ya se encuentra registrado");
    }

    private async Task<bool> ClienteExiste(CreateClienteCommand command, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(c => c.Identificacion == command.Identificacion);

      return !existe;
    }

  }
}