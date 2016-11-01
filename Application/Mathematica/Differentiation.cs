using System;
using System.Linq.Expressions;

namespace Mathematica
{
    public class Differentiation
    {
        private static Expression Differentiate(Expression expression)
        {
            if (expression is ConstantExpression)
            {
                return Expression.Constant(0.0);
            }
            if (expression is ParameterExpression)
            {
                return Expression.Constant(1.0);
            }
            if (expression is MethodCallExpression)
            {
                var e = (MethodCallExpression)expression;
                if (e.Method.Name == "Sin")
                {
                    return Expression.Multiply(
                            Expression.Call(
                                typeof(Math).GetMethod("Cos"),
                                e.Arguments[0]),
                            Differentiate(e.Arguments[0]));
                }
                throw new ArgumentException();
            }
            if (expression is BinaryExpression)
            {
                var e = SortExpressionMembers((BinaryExpression)expression);
                if (e.NodeType == ExpressionType.Add)
                {
                    return Expression.Add(
                            Differentiate(e.Left),
                            Differentiate(e.Right));
                }
                if (e.NodeType == ExpressionType.Multiply)
                {
                    return Expression.Add(
                            Expression.Multiply(
                                e.Left,
                                Differentiate(e.Right)),
                            Expression.Multiply(
                                e.Right,
                                Differentiate(e.Left)));
                }
                throw new ArgumentException();
            }
            throw new ArgumentException();
        }

        private static BinaryExpression SortExpressionMembers(BinaryExpression e)
        {
            var reason = e.Right is ConstantExpression ||
                      e.Right is ParameterExpression && !(e.Left is ConstantExpression) ||
                      e.Right is MethodCallExpression && !(e.Left is ConstantExpression) && !(e.Left is ParameterExpression);

            return reason ? Expression.MakeBinary(e.NodeType, e.Right, e.Left) : e;
        }

        public static Func<double, double> GetDerivativeFunc(Expression<Func<double, double>> primitive)
        {
            var body = Differentiate(primitive.Body);
            var type = primitive.Parameters[0];
            return (Func<double, double>)Expression.Lambda(body, type).Compile();
        }
    }
}
