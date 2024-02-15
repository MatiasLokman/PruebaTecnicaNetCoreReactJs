using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Commands.UpdateClienteCommand
{
  public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
  {
    private readonly ControlGlobalContext _context;
    public UpdateClienteCommandValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(c => c.IdCliente)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(ClienteExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un cliente existente");

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
    }

    private async Task<bool> ClienteExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(c => c.IdCliente == id);
      return existe;
    }

  }
}
