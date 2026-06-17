using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ProgramacionWeb3.PropertiesManager.Core.Common.Enums
{
    public enum UserRoleEnum
    {
        [EnumMember(Value = "Admin")]
        Admin = 1,
        [EnumMember(Value = "Properties Administrator")]
        PropertiesAdministrator = 2,
        [EnumMember(Value = "Owner")]
        Owner = 3,
    }

    public enum MultipurposeRoomShiftEnum
    {
        [Display(Name = "Mediodía (12:00 - 16:00)")]
        [EnumMember(Value = "Mediodía")]
        MidDay = 1,
        [Display(Name = "Noche (20:00 - 00:00)")]
        [EnumMember(Value = "Noche")]
        Night = 2,
    }
}
