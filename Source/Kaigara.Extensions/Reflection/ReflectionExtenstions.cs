using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Kaigara.Extentions;

namespace Kaigara.Reflection;

public static class ReflectionExtenstions
{
    public static bool IsInstanceOfGenericType(this Type type, Type genericType)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }

    public static bool IsInstanceOfGenericInterface(this Type type, Type interfaceType)
    {
        return type.IsInstanceOfGenericType(interfaceType)
            || type.GetInterfaces().Any(i => i.IsInstanceOfGenericType(interfaceType));
    }

    public static Type[]? GetGenericArgumentsForInterface(this Type type, Type interfaceType)
    {
        return type.GetInterfaces()
                .Where(i => i.IsInstanceOfGenericType(interfaceType))
                .Select(i => i.GetGenericArguments())
                .FirstOrDefault();
    }

    public static bool CanBeActivatedWithoutArguments(this Type type)
    {
        return type.IsValueType || (type.IsClass && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null);
    }

    public static string GetDisplayName(this MemberInfo member)
    {
        return member.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
               member.GetCustomAttribute<DisplayAttribute>()?.Name ??
               member.Name.SeparateWords();
    }

    public static object? GetValue(this MemberInfo member, object? instance, object[]? index = null)
    {
        if (member is PropertyInfo property)
        {
            return property.GetValue(instance, index);
        }
        else if (member is FieldInfo field)
        {
            return field.GetValue(instance);
        }
        else
        {
            throw new ArgumentException("Member must be Field or Property", nameof(member));
        }
    }

    public static IDictionary<string, object?> GetMemberValues(this object instance, Func<MemberInfo, bool> filter)
    {
        var memberValues = new Dictionary<string, object?>();

        instance.GetMemberValues(filter, memberValues);

        return memberValues;
    }

    public static void GetMemberValues(this object instance, Func<MemberInfo, bool> filter, IDictionary<string, object?> memberValues)
    {
        if (instance is null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        var members = instance.GetType()
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => (m is PropertyInfo p && !p.HasIndexParameters()) || m is FieldInfo);

        if (filter != null)
        {
            members = members.Where(filter);
        }

        foreach (var member in members)
        {
            memberValues[member.Name] = member.GetValue(instance, null);
        }

    }

    public static Type TryGetMemberType(this MemberInfo member)
    {
        Type? memberType = null;

        if (member is PropertyInfo property)
        {
            memberType = property.PropertyType;
        }
        else if (member is FieldInfo field)
        {
            memberType = field.FieldType;
        }

        return memberType ?? throw new ArgumentException($"'{nameof(member)}' is not a PropertyInfo or FieldInfo", nameof(member));
    }

    public static bool IsRuntimeType(this Type type)
    {
        return typeof(Type).IsAssignableFrom(type);
    }

    public static Type EnsureRuntimeType(this Type type)
    {
        if (type.IsRuntimeType() is false)
        {
            return Type.GetTypeFromHandle(type.TypeHandle);
        }

        return type;
    }


    public static bool IsReadOnly(this FieldInfo fieldInfo)
    {
        return !fieldInfo.IsPublic || fieldInfo.IsInitOnly || fieldInfo.IsAssembly;
    }

    public static bool IsReadOnly(this PropertyInfo propertyInfo)
    {
        return !propertyInfo.CanWrite || propertyInfo is { SetMethod.IsAssembly: true };
    }

    public static bool HasIndexParameters(this PropertyInfo propertyInfo)
    {
        return propertyInfo.GetIndexParameters().Length > 0;
    }

    public static bool HasAttribute<T>(this Type type)
        where T : Attribute
        => type.GetCustomAttribute<T>() is object;

    public static bool IsConstructable(this Type type)
    {
        return type.IsClass && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) is object;
    }
}
