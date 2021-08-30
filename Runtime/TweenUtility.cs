using System;
using System.Reflection;
using System.Linq.Expressions;

namespace Neat.Tweening {
    public static class TweenUtility {
        public static (Func<object, T> getter, Action<object, T> setter) GetAccess<T>(
            string componentTypeName,
            string fieldName
        ) {
            var componentType = Type.GetType(componentTypeName);

            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Static |
                                              BindingFlags.Instance |
                                              BindingFlags.DeclaredOnly;

            var maybeFieldInfo = GetFieldRecursive(componentType, fieldName, bindingFlags);

            if (maybeFieldInfo != null) {
                var getter = FastInvoke.BuildTypedGetter<T>(maybeFieldInfo);
                var setter = FastInvoke.BuildTypedSetter<T>(maybeFieldInfo);

                return (getter, setter);
            }

            var maybePropertyInfo = GetPropertyRecursive(componentType, fieldName, bindingFlags);

            if (maybePropertyInfo != null) {
                var getter = FastInvoke.BuildTypedGetter<T>(maybePropertyInfo);
                var setter = FastInvoke.BuildTypedSetter<T>(maybePropertyInfo);

                return (getter, setter);
            }

            return (null, null);
        }

        private static FieldInfo GetFieldRecursive(Type type, string name, BindingFlags bindingFlags) {
            return type.GetField(name, bindingFlags) ??
                   (type.BaseType != null ? GetFieldRecursive(type.BaseType, name, bindingFlags) : null);
        }

        private static PropertyInfo GetPropertyRecursive(Type type, string name, BindingFlags bindingFlags) {
            return type.GetProperty(name, bindingFlags) ??
                   (type.BaseType != null ? GetPropertyRecursive(type.BaseType, name, bindingFlags) : null);
        }
    }

    public static class FastInvoke {
        public static Func<object, T> BuildTypedGetter<T>(MemberInfo memberInfo) {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(typeof(object), "t");

            var exConvertFromObject =
                Expression.Convert(exInstance, targetType ?? throw new InvalidOperationException()); // Convert(t, Type)

            var exMemberAccess =
                Expression.MakeMemberAccess(exConvertFromObject, memberInfo); // Convert(t, Type).PropertyName
            var lambda = Expression.Lambda<Func<object, T>>(exMemberAccess, exInstance);

            var action = lambda.Compile();

            return action;
        }

        public static Action<object, T> BuildTypedSetter<T>(MemberInfo memberInfo) {
            var targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(typeof(object), "t");

            var exConvertFromObject =
                Expression.Convert(exInstance, targetType ?? throw new InvalidOperationException()); // Convert(t, Type)
            var exMemberAccess = Expression.MakeMemberAccess(exConvertFromObject, memberInfo);

            // (Convert(t, Type)).PropertyValue(p)
            var exValue = Expression.Parameter(typeof(T), "p");
            var exBody = Expression.Assign(exMemberAccess, exValue);

            var lambda = Expression.Lambda<Action<object, T>>(exBody, exInstance, exValue);
            var action = lambda.Compile();

            return action;
        }
    }
}
