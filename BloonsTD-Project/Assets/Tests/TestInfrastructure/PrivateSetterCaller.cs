using System;
using System.Linq.Expressions;

namespace TMG.BloonsTD.TestInfrastructure
{
    public static class PrivateSetterCaller
    {
        public static void SetPrivate<T,TValue>(this T instance, Expression<Func<T,TValue>> propertyExpression, TValue value)
        {
            instance.GetType().GetProperty(GetName(propertyExpression))?.SetValue(instance, value, null);
        }

        private static string GetName<T, TValue>(Expression<Func<T, TValue>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression uBody = (UnaryExpression)exp.Body;
                body = uBody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}