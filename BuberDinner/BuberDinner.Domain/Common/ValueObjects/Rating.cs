using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Common.ValueObjects;
public sealed class Rating : ValueObject
{
    private Rating(double value)
    {
        Value = value;
    }

    public double Value { get; private set; }

    public static Rating CreateNew(double rating = 0)
    {
        return new Rating(rating);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
