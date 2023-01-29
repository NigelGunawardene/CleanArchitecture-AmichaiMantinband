using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.HostAggregate.ValueObjects;
public sealed class HostId : ValueObject
{
    public string Value { get; }

    private HostId(string value)
    {
        Value = value;
    }

    //public static HostId CreateUnique()
    //{
    //    return new(Guid.NewGuid());
    //}

    public static HostId Create(string id)
    {
        return new(id);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
