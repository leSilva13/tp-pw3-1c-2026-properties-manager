using System.Reflection;
using System.Runtime.Serialization;

namespace ProgramacionWeb3.PropertiesManager.Core.Common.Helpers
{
    public static class EnumExtensions
    {
        public static string ToEnumMemberString(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return value.ToString();

            var field = type.GetField(name);
            var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();

            return attribute?.Value ?? name;
        }
    }
}
