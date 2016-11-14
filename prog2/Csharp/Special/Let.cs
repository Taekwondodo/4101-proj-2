// Let -- Parse tree node strategy for printing the special form let

using System;

namespace Tree
{
    public class Let : Special
    {
	public Let() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printLet(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            Node expr1 = args.getCar();

            if (args.getCdr().isNull() == true)
                return new StringLit("Error: Only 1 argument passed to 'set!.'");
            else if (args.getCdr().getCdr().isNull() == false)
                return new StringLit("Error: Too many arguments passed to 'set!.'");
            // Is this defining a function? 
            else if (expr1.isPair() == true)
            {
                if (expr1.getCar().isSymbol() == true)
                {
                    // Construct the lambda expression
                    Node formalsAndBody = new Cons(expr1.getCdr(), args.getCdr());
                    Node lambdaExpr = new Cons(new Ident("lambda"), formalsAndBody);
                    // set!.apply()
                    env.assign(expr1.getCar(), lambdaExpr.eval(env));
                    return new StringLit("No values returned.");
                }
                else
                    return new StringLit("Error: First argument to 'set!' must be a <variable>");
            }
            else
            {
                if (expr1.isSymbol() == true)
                {
                    // set!.apply()
                    env.assign(expr1, args.getCdr().getCar().eval(env));
                    return new StringLit("No values returned.");
                }
                else
                    return new StringLit("Error: First argument to 'set!' must be a <variable>");
            }
        }
    }
}


