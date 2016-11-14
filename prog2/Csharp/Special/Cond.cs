// Cond -- Parse tree node strategy for printing the special form cond

using System;

namespace Tree
{
    public class Cond : Special
    {
	public Cond() { }

        public override void print(Node t, int n, bool p)
        { 
            Printer.printCond(t, n, p);
        }

        public override Node eval(Environment env, Node args)
        {
            Node result = BoolLit.getInstance(false), test, cases = args;

            // Iterate through each case test until it evaluates to true or we hit the end
            do
            {
                cases = cases.getCdr();
                test = cases.getCar().getCar();
                if (test.getName() == "else")
                    break;
                result = test.eval(env);

            } while (cases.getCdr() != null && result == BoolLit.getInstance(false));

            // if a expr value returned true or we hit an else clause
            if (result != BoolLit.getInstance(false) || test.getName() == "else")
            {
                // If there was a test with no other expressions, apply the test
                if (cases.getCar().getCdr() == null)
                    return args.getCar().apply(result);
                // Otherwise apply the following expressions
                else
                    return args.getCar().apply(cases.getCar().getCdr().eval(env));
            }
            else
                return new StringLit("#unspecified");
             

        }
    }
}


