using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BuberDinner.Domain.Bill.ValueObjects;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Common.ValueObjects;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Guest.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.Menu.Entities;
using BuberDinner.Domain.Menu.ValueObjects;
using BuberDinner.Domain.MenuReview.ValueObjects;
using BuberDinner.Domain.User.Entity;
using BuberDinner.Domain.User.ValueObjects;
using static System.Collections.Specialized.BitVector32;

namespace BuberDinner.Domain.Guest;
public sealed class Guest : AggregateRoot<GuestId>
{
    private readonly List<DinnerId> _upcomingDinnerIds = new();
    private readonly List<DinnerId> _pastDinnerIds = new();
    private readonly List<DinnerId> _pendingDinnerIds = new();
    private readonly List<BillId> _billIds = new();
    private readonly List<MenuReviewId> _menuReviewIds = new();
    private readonly List<UserRating> _ratings = new();
    public string FirstName { get; }
    public string LastName { get; }
    public string ProfileImage { get; }
    public AverageRating AverageRating { get; }
    public UserId UserId { get; }

    public IReadOnlyList<DinnerId> UpcomingDinnerIds => _upcomingDinnerIds;
    public IReadOnlyList<DinnerId> PastDinnerIds => _pastDinnerIds;
    public IReadOnlyList<DinnerId> PendingDinnerIds => _pendingDinnerIds;
    public IReadOnlyList<BillId> BillIds => _billIds;
    public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds;
    public IReadOnlyList<UserRating> Ratings => _ratings;
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    private Guest(GuestId guestId, string firstName, string lastName, string profileImage, AverageRating averageRating, UserId userId, DateTime createdDateTime, DateTime updatedDateTime)
    : base(guestId)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        AverageRating = averageRating;
        UserId = userId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Guest Create(string firstName, string lastName, string profileImage, AverageRating averageRating, UserId userId)
    {
        return new(GuestId.CreateUnique(), firstName, lastName, profileImage, averageRating, userId, DateTime.UtcNow, DateTime.UtcNow);
    }
}
