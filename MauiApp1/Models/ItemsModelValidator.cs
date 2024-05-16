using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MauiApp1.Models
{
    internal class ItemsModelValidator : AbstractValidator<ItemsModel>
    {
        public ItemsModelValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(V.Required)
                .Length(3, 40).WithMessage(string.Format(V.StringLength1, 3, 40));

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(V.Required)
                .GreaterThan(0).WithMessage(string.Format(V.GreaterThan, "Price", 0));
        }
    }
}
