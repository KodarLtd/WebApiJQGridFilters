using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Kodar.JQGridFilters.ActionParameters;

namespace Kodar.JQGridFilters
{
    public static class ExpressionHelpers
    {
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> typeInformation = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        private static Dictionary<string, PropertyInfo> GetTypeInformation(Type t)
        {
            Dictionary<string, PropertyInfo> result;

            if (!typeInformation.TryGetValue(t, out result))
            {
                result = new Dictionary<string, PropertyInfo>();

                foreach (PropertyInfo p in t.GetProperties())
                {
                    var nameAttribute = p.GetCustomAttribute<UrlParameterAttribute>();
                    string name = p.Name;

                    if (nameAttribute != null)
                    {
                        if (!nameAttribute.Allowed)
                        {
                            continue;
                        }

                        name = nameAttribute.UrlName;
                    }

                    if (result.ContainsKey(name))
                    {
                        throw new Exception("Url name " + nameAttribute.UrlName + " is already used in type " + t.FullName);
                    }

                    result[name] = p;
                }

                typeInformation[t] = result;
            }

            return result;
        }

        public static IQueryable<TSource> ApplyFilters<TSource>(this IQueryable<TSource> source, Filter filter)
        {
            if (filter == null)
            {
                return source;
            }

            var result = source;
            var queries = new List<Expression>();

            Dictionary<string, PropertyInfo> typeInfo = GetTypeInformation(typeof(TSource));

            ParameterExpression param = Expression.Parameter(typeof(TSource), "s");

            Expression predicateBody = null;

            foreach (FilterRule filterRule in filter.Rules)
            {
                PropertyInfo filterProperty;

                if (typeInfo.TryGetValue(filterRule.Field, out filterProperty))
                {
                    switch (filterRule.Operation)
                    {
                        case "eq":
                            {
                                queries.Add(result.IsEqual(filterRule.Data, filterProperty, param));
                            } break;
                        case "ne":
                            {
                                queries.Add(result.NotEqualTo(filterRule.Data, filterProperty, param));
                            } break;
                        case "lt":
                            {
                                queries.Add(result.LessThan(filterRule.Data, filterProperty, param));
                            } break;
                        case "le":
                            {
                                queries.Add(result.LessThanOrEqual(filterRule.Data, filterProperty, param));
                            } break;
                        case "gt":
                            {
                                queries.Add(result.GreaterThan(filterRule.Data, filterProperty, param));
                            } break;
                        case "ge":
                            {
                                queries.Add(result.GreaterThanOrEqual(filterRule.Data, filterProperty, param));
                            } break;
                        case "bw":
                            {
                                queries.Add(result.StringCondition(filterRule.Data, filterProperty, "StartsWith", param));
                            } break;
                        case "ew":
                            {
                                queries.Add(result.StringCondition(filterRule.Data, filterProperty, "EndsWith", param));
                            } break;
                        case "cn":
                            {
                                queries.Add(result.StringCondition(filterRule.Data, filterProperty, "Contains", param));
                            } break;
                        default:
                            {
                                throw new FilterExpressionException("Unsupported operation " + filterRule.Operation);
                            }
                    }
                }
                else
                {
                    throw new FilterExpressionException("Cannot filter by " + filterRule.Field);
                }
            }

            if (filter.GroupOp == "AND")
            {
                predicateBody = AndExpressionBuilder(queries);
            }
            else if (filter.GroupOp == "OR")
            {
                predicateBody = OrExpressionBuilder(queries);
            }
            else
            {
                throw new Exception("Invalid group operation: " + filter.GroupOp);
            }

            return result.Where(Expression.Lambda<Func<TSource, bool>>(predicateBody, param));

        }

        #region FiltersHelperMethods
        public static Expression AndExpressionBuilder(List<Expression> expressions)
        {
            Expression predicateBody = Expression.Constant(true);

            foreach (var expr in expressions)
            {
                predicateBody = Expression.AndAlso(predicateBody, expr);
            }

            return predicateBody;
        }

        public static Expression OrExpressionBuilder(List<Expression> expressions)
        {
            Expression predicateBody = Expression.Constant(false);

            foreach (var expr in expressions)
            {
                predicateBody = Expression.OrElse(predicateBody, expr);
            }

            return predicateBody;
        }

        public static BinaryExpression IsEqual<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.Equal(Expression.Property(param, propertyInfo), Expression.Constant(callInfo.Argument, propertyInfo.PropertyType));
        }

        public static BinaryExpression NotEqualTo<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.NotEqual(Expression.Property(param, propertyInfo), Expression.Constant(callInfo.Argument, propertyInfo.PropertyType));
        }

        public static BinaryExpression LessThan<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.LessThan(
                        Expression.Call(Expression.Property(param, propertyInfo), callInfo.CompareMethod, Expression.Constant(callInfo.Argument, propertyInfo.PropertyType)),
                        Expression.Constant(0)
                    );
        }

        public static BinaryExpression LessThanOrEqual<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.LessThanOrEqual(
                        Expression.Call(Expression.Property(param, propertyInfo), callInfo.CompareMethod, Expression.Constant(callInfo.Argument, propertyInfo.PropertyType)),
                        Expression.Constant(0)
                    );
        }

        public static BinaryExpression GreaterThan<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.GreaterThan(
                        Expression.Call(Expression.Property(param, propertyInfo), callInfo.CompareMethod, Expression.Constant(callInfo.Argument, propertyInfo.PropertyType)),
                        Expression.Constant(0)
                    );
        }

        public static BinaryExpression GreaterThanOrEqual<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, ParameterExpression param)
        {
            CompareExpressionInfo callInfo = CallInfo<TSource>(value, propertyInfo);

            return Expression.GreaterThanOrEqual(
                        Expression.Call(Expression.Property(param, propertyInfo), callInfo.CompareMethod, Expression.Constant(callInfo.Argument, propertyInfo.PropertyType)),
                        Expression.Constant(0)
                    );
        }

        public static MethodCallExpression StringCondition<TSource>(this IQueryable<TSource> source, string value, PropertyInfo propertyInfo, string method, ParameterExpression param)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                var operationMethod = propertyInfo.PropertyType.GetMethod(method, new Type[] { propertyInfo.PropertyType });

                return Expression.Call(Expression.Property(param, propertyInfo), operationMethod, Expression.Constant(value, propertyInfo.PropertyType));
            }
            else
            {
                throw new FilterExpressionException("This operation can be performed only over string variables");
            }
        }

        private class CompareExpressionInfo
        {
            public object Argument { get; private set; }
            public MethodInfo CompareMethod { get; private set; }

            public CompareExpressionInfo(object arg, MethodInfo compareMethod)
            {
                Argument = arg;
                CompareMethod = compareMethod;
            }
        }

        private static CompareExpressionInfo CallInfo<TSource>(string value, PropertyInfo propertyInfo)
        {
            return new CompareExpressionInfo(
                arg: ConvertArgumentValue(value, propertyInfo),
                compareMethod: propertyInfo.PropertyType.GetMethod("CompareTo", new Type[] { propertyInfo.PropertyType })
                );

        }


        private static object ConvertArgumentValue(object value, PropertyInfo propertyInfo)
        {
            try
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    return value;
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }
                else if (propertyInfo.PropertyType == typeof(int?))
                {
                    return value == null ? null : (int?)Convert.ToInt32(value);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    return DateTime.ParseExact(value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime?))
                {
                    return value == null ? null : (DateTime?)DateTime.ParseExact(value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (propertyInfo.PropertyType == typeof(double))
                {
                    return Convert.ToDouble(value);
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    return Convert.ToBoolean(value);
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    //TODO convert to other underlying enum types
                    int intValue = Convert.ToInt32(value);
                    return Enum.ToObject(propertyInfo.PropertyType, intValue);
                }
                else
                {
                    throw new FilterExpressionException("Cannot compare property " + propertyInfo.Name + " to specified value");
                }
            }
            catch (FormatException ex)
            {
                throw new FilterExpressionException("Cannot convert value to type " + propertyInfo.PropertyType.FullName + " for property " + propertyInfo.Name, ex);
            }

        }
        #endregion

        public static IQueryable<TSource> SortBy<TSource, TSortColumn>(this IQueryable<TSource> source, Filter filter, Expression<Func<TSource, TSortColumn>> sortExpression, string sortOrder)
        {
            IQueryable<TSource> filteredItems = ApplyFilters(source, filter);

            if (sortOrder == "desc")
            {
                return filteredItems.OrderByDescending(sortExpression);
            }
            else
            {
                return filteredItems.OrderBy(sortExpression);
            }
        }

        public static IQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> source, Filter filter, string sortingColumn, string sortOrder)
        {
            IQueryable<TSource> filteredItems = ApplyFilters(source, filter);

            ParameterExpression param = Expression.Parameter(typeof(TSource), "s");

            PropertyInfo sortingProperty;

            Dictionary<string, PropertyInfo> typeInfo = GetTypeInformation(typeof(TSource));

            if (sortingColumn != null)
            {
                string[] propertypPathSegments = sortingColumn.Split('.');

                if (typeInfo.TryGetValue(propertypPathSegments[0], out sortingProperty))
                {
                    MemberExpression propertyAccess = Expression.Property(param, sortingProperty.Name);

                    //TODO: should check for attributes here or remove all attributes
                    for (int i = 1; i < propertypPathSegments.Length; i++)
                    {
                        propertyAccess = Expression.Property(propertyAccess, propertypPathSegments[i]);
                    }

                    LambdaExpression orderExpression = Expression.Lambda(propertyAccess, param);

                    MethodCallExpression expression = null;

                    if (sortOrder.ToLower() == "asc")
                    {
                        expression = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeof(TSource), propertyAccess.Type }, filteredItems.Expression, Expression.Quote(orderExpression));
                    }
                    else if (sortOrder.ToLower() == "desc")
                    {
                        expression = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(TSource), propertyAccess.Type }, filteredItems.Expression, Expression.Quote(orderExpression));
                    }
                    else
                    {
                        throw new InvalidOperationException(sortOrder + " is not a valid sorting order");
                    }

                    return filteredItems.Provider.CreateQuery<TSource>(expression);

                }
                else
                {
                    throw new FilterExpressionException("Items cannot be sorted by " + sortingColumn + " column");
                }
            }
            else
            {
                return filteredItems;
            }

        }

    }
}