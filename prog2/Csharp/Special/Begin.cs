// Begin -- Parse tree node strategy for printing the special form begin

using System;

namespace Tree
{
    public class Begin : Special
    {
	public Begin() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printBegin(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            if (args.isNull() == true)
                return new StringLit("Error: 'begin' requires at least 1 argument.");

            Node evaluated = new Cons(args.getCdr().getCar().eval(env), args.getCdr().getCdr().eval(env));

            // evaluate the car which is 'begin', then begin.apply()
            return args.getCar().eval(env).apply(evaluated);

        }
    }
}

