using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.User.ValueObjects;
public sealed class UserRatingId : ValueObject
{
    public Guid Value { get; }

    private UserRatingId(Guid value)
    {
        Value = value;
    }

    public static UserRatingId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
