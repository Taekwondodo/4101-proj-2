// Lambda -- Parse tree node strategy for printing the special form lambda

using System;

namespace Tree
{
    public class Lambda : Special
    {
	public Lambda() { }

        public override void print(Node t, int n, bool p)
        {
            Printer.printLambda(t, n, p);
  	}
        public override Node eval(Environment env, Node args)
        {
            return new Closure(args, env);
        }
    }
}

