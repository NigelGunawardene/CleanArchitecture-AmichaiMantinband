using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Domain.DinnerAggregate.Enums;
public enum ReservationStatus
{
    PendingGuestConfirmation = 0,
    Reserved = 1,
    Cancelled = 2,
}
