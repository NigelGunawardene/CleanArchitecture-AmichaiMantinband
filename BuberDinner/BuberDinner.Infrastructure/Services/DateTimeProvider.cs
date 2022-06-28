using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Services;

namespace BuberDinner.Infrastructure.Services;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
