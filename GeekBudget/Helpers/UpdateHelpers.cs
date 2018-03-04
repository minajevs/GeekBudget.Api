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
        
        private static MemberExpression GetMemberExpression<T>(
            Expression<Func<T,object>> exp
        ) {
            var member = exp.Body as MemberExpression;
            var unary = exp.Body as UnaryExpression;
            return member ?? unary?.Operand as MemberExpression;
        }
    }
}