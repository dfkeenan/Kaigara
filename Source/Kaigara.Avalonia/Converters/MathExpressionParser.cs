// -----------------------------------------------------------------------------
// Original code from Sprache project. https://github.com/sprache/Sprache
// Greetings to Sprache Group. Original code published with the following license:
// -----------------------------------------------------------------------------
//The MIT License
//
//Copyright(c) 2011 Nicholas Blumhardt
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sprache;

namespace Kaigara.Avalonia.Converters
{
    internal static class MathExpressionParser
    {
        public static Expression<Func<double>> ParseExpression(string text)
        {
            return Lambda.Parse(text);
        }

        public static bool TryParseExpression(string text, out Expression<Func<double>> expression)
        {
            var result = Lambda.TryParse(text);

            expression = result.WasSuccessful ? result.Value : null!;

            return result.WasSuccessful;
        }

        static Parser<ExpressionType> Operator(string op, ExpressionType opType)
        {
            return Parse.String(op).Token().Return(opType);
        }

        static readonly Parser<ExpressionType> Add = Operator("+", ExpressionType.AddChecked);
        static readonly Parser<ExpressionType> Subtract = Operator("-", ExpressionType.SubtractChecked);
        static readonly Parser<ExpressionType> Multiply = Operator("*", ExpressionType.MultiplyChecked);
        static readonly Parser<ExpressionType> Divide = Operator("/", ExpressionType.Divide);
        static readonly Parser<ExpressionType> Modulo = Operator("%", ExpressionType.Modulo);
        static readonly Parser<ExpressionType> Power = Operator("^", ExpressionType.Power);

        static readonly Parser<Expression> Function =
            from name in Parse.Letter.AtLeastOnce().Text()
            from lparen in Parse.Char('(')
            from expr in Parse.Ref(() => Expr).DelimitedBy(Parse.Char(',').Token())
            from rparen in Parse.Char(')')
            select CallFunction(name, expr.ToArray());

        static Expression CallFunction(string name, Expression[] parameters)
        {
            var methodInfo = typeof(Math).GetTypeInfo().GetMethod(name, parameters.Select(e => e.Type).ToArray());
            if (methodInfo == null)
                throw new ParseException(string.Format("Function '{0}({1})' does not exist.", name,
                                                       string.Join(",", parameters.Select(e => e.Type.Name))));

            return Expression.Call(methodInfo, parameters);
        }

        static readonly Parser<Expression> Constant =
             Parse.Decimal
             .Select(x => Expression.Constant(double.Parse(x)))
             .Named("number");

        static readonly Parser<Expression> Factor =
            (from lparen in Parse.Char('(')
             from expr in Parse.Ref(() => Expr)
             from rparen in Parse.Char(')')
             select expr).Named("expression")
             .XOr(Constant)
             .XOr(Function);

        static readonly Parser<Expression> Operand =
            ((from sign in Parse.Char('-')
              from factor in Factor
              select Expression.Negate(factor)
             ).XOr(Factor)).Token();

        static readonly Parser<Expression> InnerTerm = Parse.ChainOperator(Power, Operand, Expression.MakeBinary);

        static readonly Parser<Expression> Term = Parse.ChainOperator(Multiply.Or(Divide).Or(Modulo), InnerTerm, Expression.MakeBinary);

        static readonly Parser<Expression> Expr = Parse.ChainOperator(Add.Or(Subtract), Term, Expression.MakeBinary);

        static readonly Parser<Expression<Func<double>>> Lambda =
            Expr.End().Select(body => Expression.Lambda<Func<double>>(body));
    }
}
