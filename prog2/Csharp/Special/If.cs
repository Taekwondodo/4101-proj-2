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
           return args.getCar().eval(env).apply(
               new Cons(
                        args.getCdr().getCar().eval(env), 
                        args.getCdr().getCdr().eval(env)
               )
           );
        }
    }
}

