using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Domain.DinnerAggregate.Enums;
public enum DinnerStatus
{
    Upcoming = 0,
    InProgress = 1,
    Ended = 2,
    Cancelled = 3,
}
