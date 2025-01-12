using EstadoCuenta_Backend.Models.DTO;
using FluentValidation;
namespace EstadoCuenta_Backend.Services.Validators
{
    public class InsertarPagoCommandDTOValidator : AbstractValidator<InsertarPagoCommandDTO>
    {
        public InsertarPagoCommandDTOValidator()
        {
            RuleFor(x => x.Monto)
                .NotEmpty()
                .WithMessage("El monto es requerido")
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0");
            RuleFor(x => x.TarjetaID)
                .NotEmpty()
                .WithMessage("La tarjeta es requerida")
                .WithMessage("La tarjeta debe ser mayor a 0");
        }
    }
}
