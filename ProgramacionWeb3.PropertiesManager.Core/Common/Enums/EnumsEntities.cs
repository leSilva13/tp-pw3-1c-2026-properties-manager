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
        [EnumMember(Value = "Mediodía")]
        MidDay = 1,
        [EnumMember(Value = "Noche")]
        Night = 2,
    }
}
