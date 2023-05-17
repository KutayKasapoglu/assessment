using System.ComponentModel;
using System.Reflection;

namespace ConsoleApp.Utilities
{
    public static class EnumHelper
    {
        /// <remarks>
        /// Author: Kutay Kasapoglu
        /// </remarks>
        /// 
        /// <summary>
        /// EnumHelper class is created to provide a reusable and convenient way to retrieve the descriptions of enum values.
        /// The GetEnumDescription method uses reflection to retrieve the Description attribute for the specified enum value.
        /// </summary>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
