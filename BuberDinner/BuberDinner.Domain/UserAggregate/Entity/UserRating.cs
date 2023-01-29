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
using BuberDinner.Domain.UserAggregate.ValueObjects;

namespace BuberDinner.Domain.UserAggregate.Entity;
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

