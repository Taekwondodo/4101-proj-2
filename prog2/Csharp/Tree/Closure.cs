// Closure.java -- the data structure for function closures

// Class Closure is used to represent the value of lambda expressions.
// It consists of the lambda expression itself, together with the
// environment in which the lambda expression was evaluated.

// The method apply() takes the environment out of the closure,
// adds a new frame for the function call, defines bindings for the
// parameters with the argument values in the new frame, and evaluates
// the function body.

using System;

namespace Tree
{
    public class Closure : Node
    {
        private Node fun;		// a lambda expression
        private Environment env;	// the environment in which
                                        // the function was defined

        public Closure(Node f, Environment e)	{ fun = f;  env = e; }

        public Node getFun()		{ return fun; }
        public Environment getEnv()	{ return env; }

        // TODO: The method isProcedure() should be defined in
        // class Node to return false.
        public override bool isProcedure()	{ return true; }

        public override void print(int n) {
            // there got to be a more efficient way to print n spaces
            for (int i = 0; i < n; i++)
                Console.Write(' ');
            Console.WriteLine("#{Procedure");
            if (fun != null)
                fun.print(Math.Abs(n) + 4);
            for (int i = 0; i < Math.Abs(n); i++)
                Console.Write(' ');
            Console.WriteLine('}');
        }

        // TODO: The method apply() should be defined in class Node
        // to report an error.  It should be overridden only in classes
        // BuiltIn and Closure.
        public override Node apply (Node args)
        {
            Environment closureEnv = new Environment(env);
            Node parameters = fun.getCdr().getCar(), body = fun.getCdr().getCdr().getCar();

            // lambda expression of the form 'lambda identifier exp+'
            if (parameters.isPair() == false)
            {
                if (args.getCdr().isNull() == false)
                    return new StringLit("Error: Too many arguments supplied for lambda expression.");

                closureEnv.define(parameters, args.getCar());
            }
            // lambda expression of the form 'lambda ( [ parm ] ) exp+'
            else
            {
                while (parameters.isPair() == true && args.isPair() == true)
                {
                    closureEnv.define(parameters.getCar(), args.getCar());
                    parameters = parameters.getCdr();
                    args = args.getCdr();
                }  
                   
                // If both aren't null, then the # of arguments and parameters don't match
                if (!(parameters.isNull() == true && args.isNull() == true))
                    return new StringLit("Error: # of arguments does not match # of parameters in lambda expression.");
            }

           // Evaluate the body
           return body.eval(closureEnv);
        }
    }    
}
