using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Common.ValueObjects;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.Menu.Entities;
using BuberDinner.Domain.Menu.ValueObjects;
using BuberDinner.Domain.User.ValueObjects;

namespace BuberDinner.Domain.User.Entity;
public sealed class UserRating : Entity<UserRatingId>
{
    public HostId HostId { get; }
    public DinnerId DinnerId { get; }
    public Rating Rating { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    private UserRating(UserRatingId userRatingId, HostId hostid, DinnerId dinnerId, Rating rating, DateTime createdDateTime, DateTime updatedDateTime)
        : base(userRatingId)
    {
        HostId = hostid;
        DinnerId = dinnerId;
        Rating = rating;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static UserRating Create(HostId hostId, DinnerId dinnerId, Rating rating)
    {
        return new(UserRatingId.CreateUnique(), hostId, dinnerId, rating, DateTime.UtcNow, DateTime.UtcNow);
    }
}

