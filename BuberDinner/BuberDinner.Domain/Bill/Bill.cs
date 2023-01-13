﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.Bill.ValueObjects;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Guest.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;

namespace BuberDinner.Domain.Bill;
public sealed class Bill : AggregateRoot<BillId>
{
    public DinnerId DinnerId { get; }
    public GuestId GuestId { get; }
    public HostId HostId { get; }
    public Price Price { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    private Bill(BillId billId, DinnerId dinnerId, GuestId guestId, HostId hostId, Price price, DateTime createdDateTime, DateTime updatedDateTime)
        : base(billId)
    {
        DinnerId = dinnerId;
        GuestId = guestId;
        HostId = hostId;
        Price = price;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Bill Create(DinnerId dinnerId, GuestId guestId, HostId hostId, Price price)
    {
        return new(BillId.CreateUnique(), dinnerId, guestId, hostId, price, DateTime.UtcNow, DateTime.UtcNow);
    }
}
