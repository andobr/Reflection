﻿using System;
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
                var e = (BinaryExpression)expression.Differentiate();
                if (e.NodeType == ExpressionType.Add)
                {
                    return Expression.Add(
                            e.Left.Differentiate(),
                            e.Left.Differentiate());
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

        private static BinaryExpression SortExpressionMembers(this BinaryExpression e)
        {
            var reason = e.Right is ConstantExpression ||
                      e.Right is ParameterExpression && !(e.Left is ConstantExpression) ||
                      e.Right is MethodCallExpression && !(e.Left is ConstantExpression) && !(e.Left is ParameterExpression);

            return reason ? Expression.MakeBinary(e.NodeType, e.Right, e.Left) : e;
        }

        public static Func<double, double> GetDerivativeFunc(this Expression<Func<double, double>> primitive)
        {
            var body = primitive.Body.Differentiate();
            var type = primitive.Parameters[0];
            return (Func<double, double>)Expression.Lambda(body, type).Compile();
        }
    }
}
