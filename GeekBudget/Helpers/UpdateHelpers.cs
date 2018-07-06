using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GeekBudget.Helpers
{
    public static class UpdateHelpers
    {
        public static void MapNewValues<T>(this T target, T source, params Expression<Func<T, object>>[] propertySelectors)
        {
            foreach (var selector in propertySelectors)
            {
                var memberExpression = GetMemberExpression(selector);
                if (memberExpression == null)
                    throw new ArgumentException("Must be a property selector", nameof(selector));
                
                var property = memberExpression.Member as PropertyInfo;
                
                if (property == null)
                    throw new ArgumentException("Expression refers to a field, not a property", nameof(selector));
                
                var newValue = property.GetValue(source); // value from new object
                
                if(newValue != null)
                    property.SetValue(target, newValue, null);
            }
        }

        public static void MapNewValues<T1, T2>(this T1 target, T2 source, params (Expression<Func<T1, object>> target, Expression<Func<T2, object>> source)[] expressions)
        {
            foreach (var expression in expressions)
            {
                var sourceMemberExpression = GetMemberExpression(expression.source);
                if (sourceMemberExpression == null)
                    throw new ArgumentException("Must be a property selector", nameof(expression.source));

                var targetMemberExpression = GetMemberExpression(expression.target);
                if (targetMemberExpression == null)
                    throw new ArgumentException("Must be a property selector", nameof(expression.target));

                var sourceProperty = sourceMemberExpression.Member as PropertyInfo;
                if (sourceProperty == null)
                    throw new ArgumentException("Expression refers to a field, not a property", nameof(expression.source));

                var targetProperty = targetMemberExpression.Member as PropertyInfo;
                if (targetProperty == null)
                    throw new ArgumentException("Expression refers to a field, not a property", nameof(expression.target));

                var newValue = sourceProperty.GetValue(source); // value from new object

                if (newValue != null)
                    targetProperty.SetValue(target, newValue, null);
            }
        }

        private static MemberExpression GetMemberExpression<T>(
            Expression<Func<T,object>> exp
        ) {
            var member = exp.Body as MemberExpression;
            var unary = exp.Body as UnaryExpression;
            return member ?? unary?.Operand as MemberExpression;
        }
    }
}