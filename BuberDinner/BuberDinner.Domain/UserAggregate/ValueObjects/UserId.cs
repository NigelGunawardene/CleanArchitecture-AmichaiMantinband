using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.UserAggregate.ValueObjects;
public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
