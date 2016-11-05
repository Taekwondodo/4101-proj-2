// BuiltIn -- the data structure for built-in functions

// Class BuiltIn is used for representing the value of built-in functions
// such as +.  Populate the initial environment with
// (name, new BuiltIn(name)) pairs.

// The object-oriented style for implementing built-in functions would be
// to include the C# methods for implementing a Scheme built-in in the
// BuiltIn object.  This could be done by writing one subclass of class
// BuiltIn for each built-in function and implementing the method apply
// appropriately.  This requires a large number of classes, though.
// Another alternative is to program BuiltIn.apply() in a functional
// style by writing a large if-then-else chain that tests the name of
// the function symbol.

using System;

namespace Tree
{
    public class BuiltIn : Node
    {
        private Node symbol;            // the Ident for the built-in function

        public BuiltIn(Node s)		{ symbol = s; }

        public Node getSymbol()		{ return symbol; }

        // TODO: The method isProcedure() should be defined in
        // class Node to return false.
        public  override  bool isProcedure()	{ return true; }

        public override void print(int n)
        {
            // there got to be a more efficient way to print n spaces
            for (int i = 0; i < n; i++)
                Console.Write(' ');
            Console.Write("#{Built-in Procedure ");
            if (symbol != null)
                symbol.print(-Math.Abs(n));
            Console.Write('}');
            if (n >= 0)
                Console.WriteLine();
        }

        // TODO: The method apply() should be defined in class Node
        // to report an error.  It should be overridden only in classes
        // BuiltIn and Closure.
        public override Node apply(Node args)
        {
            Node result = new Node();
            switch (symbol.getName())
            {
                case "symbol?":
                    // symbol? only accepts 1 argument
                    if (args.getCdr() != null)
                        result = new StringLit("Error: wrong number of arguments for 'symbol?'");
                    else
                    {
                        if (args.getCar().isSymbol())
                            result = BoolLit.getInstance(true);
                        else
                            result = BoolLit.getInstance(false);
                    }
                    break;
                
                // I'm not sure what to do about the binary arithmetic operations yet, so I'm leaving them out
                case "number?":
                    // number? only accept 1 argument
                    if (args.getCar() != null)
                        result = new StringLit("Error: wrong number of arguments for 'number?'");
                    else
                    {
                        if (args.getCar().isNumber())
                            result = BoolLit.getInstance(true);
                        else
                            result = BoolLit.getInstance(false);
                    }
                    break;

                case "car":

                default:
                    result = new StringLit("Error: BuiltIn.apply() called for non BuiltIn function.");
                    break;
            }
            return result;
    	}
    }    
}

