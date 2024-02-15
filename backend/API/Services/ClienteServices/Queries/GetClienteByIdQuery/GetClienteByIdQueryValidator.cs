using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Queries.GetClienteByIdQuery
{
  public class GetClienteByIdQueryValidator : AbstractValidator<GetClienteByIdQuery>
  {
    private readonly ControlGlobalContext _context;

    public GetClienteByIdQueryValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(ClienteExiste).WithMessage("No hay ningun cliente con el id: {PropertyValue}");
    }

    private async Task<bool> ClienteExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Clientes.AnyAsync(p => p.IdCliente == id);
      return existe;
    }

  }
}
