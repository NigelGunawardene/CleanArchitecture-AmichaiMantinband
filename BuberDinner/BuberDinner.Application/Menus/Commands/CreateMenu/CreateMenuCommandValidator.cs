using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BuberDinner.Application.Menus.Commands.CreateMenu;
public class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.Description).NotEmpty();
        RuleFor(m => m.Sections).NotEmpty();
    }
}
