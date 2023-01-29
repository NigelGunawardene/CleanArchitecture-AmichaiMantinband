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
    public string Name { get; }
    public string Description { get; }
    public AverageRating AverageRating { get; }
    public IReadOnlyList<MenuSection> Sections => _sections;
    public HostId HostId { get; }
    public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds;
    public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds;
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }


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

    public static Menu Create(HostId hostId, string name, string description, List<MenuSection> sections)
    {
        return new(MenuId.CreateUnique(), hostId, name, description, sections, DateTime.UtcNow, DateTime.UtcNow);
    }
}
