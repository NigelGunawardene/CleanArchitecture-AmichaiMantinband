using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Common.ValueObjects;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Guest.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.Menu.ValueObjects;
using BuberDinner.Domain.MenuReview.ValueObjects;

namespace BuberDinner.Domain.MenuReview;
public sealed class MenuReview : AggregateRoot<MenuReviewId>
{
    public Rating Rating { get; }
    public string Comment { get; }
    public HostId HostId { get; }
    public MenuId MenuId { get; }
    public GuestId GuestId { get; }
    public DinnerId DinnerId { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    private MenuReview(MenuReviewId menuReviewId, Rating rating, string comment, HostId hostId, MenuId menuId, GuestId guestId, DinnerId dinnerId, DateTime createdDateTime, DateTime updatedDateTime)
    : base(menuReviewId)
    {
        Rating = rating;
        Comment = comment;
        HostId = hostId;
        MenuId = menuId;
        GuestId = guestId;
        DinnerId = dinnerId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static MenuReview Create(Rating rating, string comment, HostId hostId, MenuId menuId, GuestId guestId, DinnerId dinnerId)
    {
        return new(MenuReviewId.CreateUnique(), rating, comment, hostId, menuId, guestId, dinnerId, DateTime.UtcNow, DateTime.UtcNow);
    }
}
