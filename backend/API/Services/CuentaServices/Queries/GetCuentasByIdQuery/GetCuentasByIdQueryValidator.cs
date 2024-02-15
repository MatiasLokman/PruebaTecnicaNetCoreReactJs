using API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CuentaServices.Queries.GetCuentasByIdQuery
{
  public class GetCuentasByIdQueryValidator : AbstractValidator<GetCuentasByIdQuery>
  {
    private readonly ControlGlobalContext _context;

    public GetCuentasByIdQueryValidator(ControlGlobalContext context)
    {
      _context = context;

      RuleFor(p => p.id)
          .NotEmpty().WithMessage("El id no puede estar vacío")
          .NotNull().WithMessage("El id no puede ser nulo")
          .MustAsync(CuentaExiste).WithMessage("No hay cuentas con el id: {PropertyValue}");
    }

    private async Task<bool> CuentaExiste(int id, CancellationToken token)
    {
      bool existe = await _context.Cuentas.AnyAsync(p => p.IdClienteNavigation.IdCliente == id);
      return existe;
    }

  }
}
