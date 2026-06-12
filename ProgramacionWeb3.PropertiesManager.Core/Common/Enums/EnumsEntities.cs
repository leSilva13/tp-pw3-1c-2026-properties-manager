using System.Runtime.Serialization;

namespace ProgramacionWeb3.PropertiesManager.Core.Common.Enums
{
    public enum UserRoleEnum
    {
        [EnumMember(Value = "System Admin")]
        Admin = 1,
        [EnumMember(Value = "Administrador de Edificios")]
        PropertiesAdministrator = 2,
        [EnumMember(Value = "Dueño")]
        Owner = 3,
    }
}
