using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand
{
  public class CreateTransferenciaCuentaCommandValidator : AbstractValidator<CreateTransferenciaCuentaCommand>
  {
    private readonly ControlGlobalContext _context;

    public CreateTransferenciaCuentaCommandValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(p => p.Importe)
      .NotEmpty().WithMessage("El importe no puede estar vacío")
      .NotNull().WithMessage("El importe no puede ser nulo");

      RuleFor(p => p.Descripcion)
      .NotEmpty().WithMessage("La descripcion no puede estar vacía")
      .NotNull().WithMessage("La descripcion no puede ser nula");

      RuleFor(p => p.IdCliente)
      .NotEmpty().WithMessage("El id del cliente no puede estar vacío")
      .NotNull().WithMessage("El id del cliente no puede ser nulo")
      .MustAsync(ClienteExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un cliente existente");

      RuleFor(p => p.Identificacion)
      .NotEmpty().WithMessage("La identificacion del receptor no puede estar vacía")
      .NotNull().WithMessage("La identificacion del receptor no puede ser nula")
      .MustAsync(ReceptorExiste).WithMessage("El id: {PropertyValue} no existe, ingrese un id de un receptor existente");
    }


    private async Task<bool> ClienteExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(p => p.IdCliente == id);
      return existe;
    }

    private async Task<bool> ReceptorExiste(string identificacion, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(p => p.Identificacion == identificacion);
      return existe;
    }

  }
}
