using EstadoCuenta_Backend.Models.DTO;
using FluentValidation;

namespace EstadoCuenta_Backend.Services.Validators
{
    public class InsertarCompraCommandDTOValidator : AbstractValidator<InsertarCompraCommandDTO>
    {
        public InsertarCompraCommandDTOValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .WithMessage("La descripción es requerida")
                .MaximumLength(255).WithMessage("La descripción no debe exceder los 255 caracteres");
            RuleFor(x => x.Monto)
                .NotEmpty()
                .WithMessage("El monto es requerido")
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0");
            RuleFor(x => x.TarjetaID)
                .NotEmpty()
                .WithMessage("La tarjeta es requerida")
                .GreaterThan(0).WithMessage("La tarjeta debe ser mayor a 0");
        }
    }
}
