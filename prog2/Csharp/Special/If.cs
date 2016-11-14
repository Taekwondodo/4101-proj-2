// If -- Parse tree node strategy for printing the special form if

using System;

namespace Tree
{
    public class If : Special
    {
	public If() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printIf(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            /*
            Node builtIn = args.getCar(), test = args.getCdr().getCar();

            if (test.eval(env) != BoolLit.getInstance(false))
                // return consequent
                return builtIn.apply(args.getCdr().getCdr().getCar().eval(env));
            else if (args.getCdr().getCdr().getCdr() != null)
                // return alternate
                return builtIn.apply(args.getCdr().getCdr().getCdr().getCar().eval(env));
            else
                return new StringLit("#unspecified");
            */

           return args.getCar().eval(env).apply(new Cons(args.getCdr().getCar().eval(env), args.getCdr().getCdr().eval(env)));

        }
    }
}

