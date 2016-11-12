// Regular -- Parse tree node strategy for printing regular lists

using System;

namespace Tree
{
    public class Regular : Special
    {
        public Regular() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printRegular(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            // Evaluate the list
            Node evaluated = new Cons(args.getCar().eval(env), args.getCdr().eval(env));

            // If we the car is a closure, apply it
            if (evaluated.getCar().isProcedure())
                return evaluated.getCar().apply(evaluated.getCdr());
            else
                return evaluated;
  
        }

    }
}


