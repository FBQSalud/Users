using FBQ.Salud_Domain.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Application.Validations
{
    public class UserValidation: AbstractValidator<UserRequest>
    {
        public UserValidation()
        {
            RuleFor(c => c.UserName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(25).WithMessage("{PropertyName} valor demasiado largo ");
            RuleFor(c => c.Password).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(10).WithMessage("{PropertyName} valor demasiado largo ");
            RuleFor(c => c.DNI).Cascade(CascadeMode.Stop)
                .GreaterThan("0").WithMessage("{PropertyName} no puede ser valor negativo")
                .Length(8).WithMessage("{PropertyName} debe ingresar 8 caracteres")
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(10).WithMessage("{PropertyName} valor demasiado largo ");
            RuleFor(c => c.Email).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .EmailAddress().WithMessage("Ingresar formato email");
        }
    }
    public class UserPutValidation : AbstractValidator<UserPut>
    {
        public UserPutValidation()
        {
            RuleFor(c => c.UserName).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(25).WithMessage("{PropertyName} valor demasiado largo ");
            RuleFor(c => c.Email).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .EmailAddress().WithMessage("Ingresar formato email");
        }
    }
}
