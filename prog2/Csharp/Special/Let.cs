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
            // Evaluate the inits in the current env and bind the variables to them in the new env
            Node binding = args.getCdr().getCar();
            Environment letEnv = new Environment(env);

            while (binding.isPair())
            {
                Node init = binding.getCar().getCdr().getCar().eval(env);
                Node variable = binding.getCar().getCar();
                letEnv.define(variable, init);

                binding = binding.getCdr();
            }

            Node bodyEval = args.getCdr().getCdr().eval(letEnv);
            return args.getCar().eval(env).apply(bodyEval);






        }
    }
}


