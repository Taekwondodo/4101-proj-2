// Define -- Parse tree node strategy for printing the special form define

using System;

namespace Tree
{
    public class Define : Special
    {
	public Define() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printDefine(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            Node expr1 = args.getCar();

            if (args.getCdr().isNull() == true)
                return new StringLit("Error: Only 1 argument passed to 'define.'");
            else if (args.getCdr().getCdr().isNull() == false)
                return new StringLit("Error: Too many arguments passed to 'define.'");
            // Is this defining a function? 
            else if (expr1.isPair() == true)
            {
                if (expr1.getCar().isSymbol() == true)
                {
                    // Construct the lambda expression
                    Node formalsAndBody = new Cons(expr1.getCdr(), args.getCdr());
                    Node lambdaExpr = new Cons(new Ident("lambda"), formalsAndBody);
                    // define.apply()          
                    return env.define(expr1.getCar(), lambdaExpr.eval(env));
                }
                else
                    return new StringLit("Error: First argument to 'define' must be a <variable>");
            }
            else
            {
                if (expr1.isSymbol() == true)
                {
                    // define.apply()
                    return env.define(expr1, args.getCdr().getCar().eval(env));
                }
                else
                    return new StringLit("Error: First argument to 'define' must be a <variable>");
                
            }

        }
    }
}


