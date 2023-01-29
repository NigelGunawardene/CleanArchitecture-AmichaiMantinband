using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.MenuAggregate;
using BuberDinner.Domain.MenuAggregate.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Menus.Commands.CreateMenu;
public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
    private readonly IMenuRepository _menuRepository;

    public CreateMenuCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ErrorOr<Menu>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask; // cheating for now
        // Create Menu
        // still missing enforcing invariants inside the menu, missing validation on the request properties
        var menu = Menu.Create(
            hostId: HostId.Create(request.HostId),
            name: request.Name,
            description: request.Description,
            sections: request.Sections.ConvertAll(section => MenuSection.Create(
                section.Name,
                section.Description,
                section.Items.ConvertAll(item => MenuItem.Create(
                    item.Name,
                    item.Description)))));
        // Whenever we have a Select and a ToList, we can just replace it with ConvertAll as above
        //var menu = Menu.Create(
        //    hostId: HostId.Create(request.HostId),
        //    name: request.Name,
        //    description: request.Description,
        //    sections: request.Sections.Select(section => MenuSection.Create(
        //        section.Name,
        //        section.Description,
        //        section.Items.Select(item => MenuItem.Create(
        //            item.Name,
        //            item.Description)).ToList())).ToList());

        // Persist Menu
        _menuRepository.Add(menu);

        // Return Menu
        return menu;
    }
}
