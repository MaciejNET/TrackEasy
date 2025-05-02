using System.ComponentModel;
using System.Reflection;

namespace TrackEasy.Shared.Infrastructure;

public static class EnumExtensions
{
    public static int GetEnumId(this Enum value)
    {
        return (int)(object)value;
    }
    
    public static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }
}