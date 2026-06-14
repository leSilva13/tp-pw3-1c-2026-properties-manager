using System.Runtime.Serialization;

namespace ProgramacionWeb3.PropertiesManager.Core.Common.Enums
{
    public enum UserRoleEnum
    {
        [EnumMember(Value = "Admin")]
        Admin = 1,
        [EnumMember(Value = "Administrador de Edificios")]
        PropertiesAdministrator = 2,
        [EnumMember(Value = "Dueño")]
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
