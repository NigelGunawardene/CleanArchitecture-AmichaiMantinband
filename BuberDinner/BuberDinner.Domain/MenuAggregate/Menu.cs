using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Common.ValueObjects;
using BuberDinner.Domain.DinnerAggregate.ValueObjects;
using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.MenuAggregate.ValueObjects;
using BuberDinner.Domain.MenuReviewAggregate.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate;
public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuSection> _sections = new();
    private readonly List<DinnerId> _dinnerIds = new();
    private readonly List<MenuReviewId> _menuReviewIds = new();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public IReadOnlyList<MenuSection> Sections => _sections.AsReadOnly();
    public HostId HostId { get; private set; }
    public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();
    public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }


    private Menu(MenuId menuId, HostId hostId, string name, string description, List<MenuSection> sections, DateTime createdDateTime, DateTime updatedDateTime)
        : base(menuId)
    {
        HostId = hostId;
        Name = name;
        Description = description;
        _sections = sections;
        AverageRating = AverageRating.CreateNew();
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

#pragma warning disable CS8618
    private Menu()
    {
    }
#pragma warning restore CS8618

    public static Menu Create(HostId hostId, string name, string description, List<MenuSection> sections)
    {
        return new(MenuId.CreateUnique(), hostId, name, description, sections, DateTime.UtcNow, DateTime.UtcNow);
    }
}
