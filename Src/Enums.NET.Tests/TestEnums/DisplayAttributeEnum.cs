using System.ComponentModel.DataAnnotations;
using EnumsNET.Tests.Properties;

namespace EnumsNET.Tests.TestEnums
{
    public enum DisplayAttributeEnum
    {
        [Display(Name = nameof(Resources.Up), ResourceType = typeof(Resources))]
        Up,
        [Display(Name = nameof(Resources.Down), ResourceType = typeof(Resources))]
        Down,
        [Display(Name = nameof(Resources.Left), ResourceType = typeof(Resources))]
        Left,
        [Display(Name = nameof(Resources.Right), ResourceType = typeof(Resources))]
        Right
    }
}
