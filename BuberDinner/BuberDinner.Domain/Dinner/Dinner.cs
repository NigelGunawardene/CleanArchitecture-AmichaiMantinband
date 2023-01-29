using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Dinner.Entities;
using BuberDinner.Domain.Dinner.Enums;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.Menu.Entities;
using BuberDinner.Domain.Menu.ValueObjects;
using BuberDinner.Domain.MenuReview.ValueObjects;
using BuberDinner.Domain.User.ValueObjects;

namespace BuberDinner.Domain.Dinner;

public sealed class Dinner : AggregateRoot<DinnerId>
{
    private readonly List<Reservation> _reservations = new();
    public string Name { get; }
    public string Description { get; }

    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }

    public DateTime StartedDateTime { get; }
    public DateTime EndedDateTime { get; }

    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    public DinnerStatus Status { get; } // Upcoming, InProgress, Ended, Cancelled

    public bool IsPublic { get; }
    public int MaxGuests { get; }

    public Price Price { get; }
    public HostId HostId { get; }
    public UserId MenuId { get; }

    public string ImageUrl { get; }
    public Location Location { get; }
    public IReadOnlyList<Reservation> Reservations => _reservations;

    private Dinner(
        DinnerId dinnerId,
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        DinnerStatus status,
        bool isPublic,
        int maxGuests,
        Price price,
        HostId hostId,
        UserId menuId,
        string imageUrl,
        Location location)
    : base(dinnerId)
    {
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        Status = status;
        IsPublic = isPublic;
        MaxGuests = maxGuests;
        Price = price;
        HostId = hostId;
        MenuId = menuId;
        ImageUrl = imageUrl;
        Location = location;
    }

    public static Dinner Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        bool isPublic,
        int maxGuests,
        Price price,
        HostId hostId,
        UserId menuId,
        string imageUrl,
        Location location)
    {
        return new(DinnerId.CreateUnique(), name, description, startDateTime, endDateTime, DateTime.UtcNow, DateTime.UtcNow, DinnerStatus.Upcoming, isPublic, maxGuests, price, hostId, menuId, imageUrl, location);
    }
}
