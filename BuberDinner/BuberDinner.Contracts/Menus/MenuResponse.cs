using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Contracts.Menus;
public record MenuResponse(
    Guid Id,
    string Name,
    string Description,
    float? AverageRating,
    List<MenuSectionResponse> Sections,
    string HostId,
    List<string> DinnerIds,
    List<string> MenuReviewIds,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);

public record MenuSectionResponse(
    string Id,
    string Name,
    string Description,
    float? AverageRating,
    List<MenuItemResponse> Items);

public record MenuItemResponse(
    string Id,
    string Name,
    string Description);
