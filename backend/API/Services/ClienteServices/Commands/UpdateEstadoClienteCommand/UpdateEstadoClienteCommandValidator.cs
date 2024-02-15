using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Commands.UpdateEstadoClienteCommand
{
  public class UpdateEstadoClienteCommandValidator : AbstractValidator<UpdateEstadoClienteCommand>
  {
    private readonly ControlGlobalContext _context;

    public UpdateEstadoClienteCommandValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(p => p.IdCliente)
         .NotEmpty().WithMessage("El id no puede estar vacío")
         .NotNull().WithMessage("El id no puede ser nulo")
         .MustAsync(ClienteExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un cliente existente");

      RuleFor(p => p.Estado)
      .NotNull().WithMessage("El estado no puede ser nulo");
    }

    private async Task<bool> ClienteExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(p => p.IdCliente == id);
      return existe;
    }

  }
}
