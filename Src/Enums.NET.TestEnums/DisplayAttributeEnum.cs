#if DISPLAY_ATTRIBUTE
using System.ComponentModel.DataAnnotations;
using EnumsNET.Tests.TestEnums.Properties;

namespace EnumsNET.Tests.TestEnums
{
    public enum DisplayAttributeEnum
    {
        [Display(Name = nameof(Resources.Up), ResourceType = typeof(Resources), Order = 2)]
        Up,
        [Display(Name = nameof(Resources.Down), ResourceType = typeof(Resources))]
        Down,
        [Display(Name = nameof(Resources.Left), ResourceType = typeof(Resources), Order = 1)]
        Left,
        [Display(Name = nameof(Resources.Right), ResourceType = typeof(Resources))]
        Right
    }
}
#endif