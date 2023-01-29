using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.MenuAggregate;

namespace BuberDinner.Application.Common.Interfaces.Persistence;
public interface IMenuRepository
{
    void Add(Menu menu);
}
