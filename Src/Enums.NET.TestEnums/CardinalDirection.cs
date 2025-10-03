using System.ComponentModel.DataAnnotations;

namespace EnumsNET.Tests.TestEnums;

public enum CardinalDirection
{
    [Display(Description = "N")]
    North,
    [Display(Description = "S")]
    South,
    [Display(Description = "E")]
    East,
    [Display(Description = "W")]
    West
}