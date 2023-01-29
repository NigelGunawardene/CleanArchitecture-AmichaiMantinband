using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.UserAggregate;

namespace BuberDinner.Application.Services.Authentication.Common;
public record AuthenticationResult(
    User User,
    string Token);
