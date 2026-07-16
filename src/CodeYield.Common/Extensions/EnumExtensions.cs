using System.ComponentModel;
using System.Reflection;

namespace CodeYield.Common.Extensions
{
    /// <summary>
    /// Extension methods for working with <see cref="Enum"/> types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the value of the <see cref="DescriptionAttribute"/> on the enum member,
        /// or the member name if no description is defined.
        /// </summary>
        public static string GetDescription<T>(this T value) where T : struct, Enum
        {
            var field = typeof(T).GetField(value.ToString())!;
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>Returns all defined values of the specified enum type.</summary>
        public static T[] GetValues<T>() where T : struct, Enum =>
            Enum.GetValues<T>();
    }
}
