using System;
using System.Linq.Expressions;

namespace Mathematica
{
    public static class Differentiation
    {
        private static Expression Differentiate(this Expression expression)
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
                            e.Arguments[0].Differentiate());
                }
                throw new ArgumentException();
            }
            if (expression is BinaryExpression)
            {
                var e = (BinaryExpression) expression;
                if (e.NodeType == ExpressionType.Add)
                {
                    return Expression.Add(
                        e.Left.Differentiate(), 
                        e.Right.Differentiate());
                }
                if (e.NodeType == ExpressionType.Multiply)
                {
                        return Expression.Add(
                                Expression.Multiply(
                                    e.Left, 
                                    e.Right.Differentiate()),
                                Expression.Multiply(
                                    e.Right, 
                                    e.Left.Differentiate()));
                }
                throw new ArgumentException();
            }
            throw new ArgumentException();
        }

        public static Func<double, double> GetDerivativeFunc(this Expression<Func<double, double>> primitive)
        {
            var body = primitive.Body.Differentiate();
            var type = primitive.Parameters[0];
            return ((Expression<Func<double, double>>)Expression.Lambda(body, type)).Compile();
        }
    }
}
