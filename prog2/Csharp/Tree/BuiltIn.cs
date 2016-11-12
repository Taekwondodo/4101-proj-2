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

        public int numArguments(Node args)
        {
            if (args.isPair() == true)
                return 1 + numArguments(args.getCdr());
            else
                return 0;
        }

        // TODO: The method apply() should be defined in class Node
        // to report an error.  It should be overridden only in classes
        // BuiltIn and Closure.
        public override Node apply(Node args)
        {
            Node result = new Node();
            int numArgs = numArguments(args);

            switch (symbol.getName())
            {
                case "read":
                    result = (Node)args.parser.parseExp();
                    break;
                case "write":
                    args.print(0);
                    // Not sure what Node to return here. Probably just return the blank Node
                    break;
                case "symbol?":
                    // symbol? only accepts 1 argument
                    if (numArgs != 1)
                        result = new StringLit("Error: wrong number of arguments for 'symbol?'");
                    else
                    {
                        if (args.getCar().isSymbol())
                            result = BoolLit.getInstance(true);
                        else
                            result = BoolLit.getInstance(false);
                    }
                    break; 
                case "number?":
                    // number? only accepts 1 argument
                    if (args.getCdr() != null)
                    // number? only accepts 1 argument
                    if (numArgs != 1)
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
                    if (args.isPair() == false)
                        result = new StringLit("Error: Argument to 'car' is not a pair.");
                    else
                        result = args.getCar();
                    break;
                case "cdr":
                    if (args.isPair() == false)
                        result = new StringLit("Error: Argument to 'cdr' is not a pair.");
                    else
                        result = args.getCdr();
                    break;
                case "cons":
                    // make sure only two arguments are provided
                    if (args.getCdr().isPair() != true && args.getCdr().getCdr() != null) 
                        result = new StringLit("Error: Function 'cons' requires two arguments.");
                    else
                        result = args;
                    break;
                case "set-car!":
                    // make sure only a list and value are provided
                    if (args.getCar().isPair() == false || args.getCdr().getCar().isPair() == true || args.getCdr().getCdr() != null)
                        result = new StringLit("Error: Function 'set-car!' requires a list and value");
                    else
                    {
                        args.getCar().setCar(args.getCdr().getCar());
                        result = new StringLit("no values returned;");
                    }        
                    break;
                case "set-cdr!":
                    // make sure only a list and value are provided
                    if (args.getCar().isPair() == false || args.getCdr().getCar().isPair() == true || args.getCdr().getCdr() != null)
                        result = new StringLit("Error: Function 'set-cdr!' requires a list and value");
                    else
                    {
                        args.getCar().setCdr(args.getCdr().getCar());
                        result = new StringLit("no values returned;");
                    }
                    break;
                case "null?":
                    if (args.getCar() == null)
                        result = BoolLit.getInstance(true);
                    // null? only accepts one argument
                    else if (args.getCdr().isPair() == true)
                        result = new StringLit("Error: 'null?' only accept one argument.");
                    else
                        result = BoolLit.getInstance(false);
                    break;
                case "pair?":
                    // pair? only accept one argument
                    if (args.getCdr().isPair() == true)
                        result = new StringLit("Error: 'pair?' only accepts one argument.");
                    else if (args.getCar().isPair())
                        result = BoolLit.getInstance(true);
                    else
                        result = BoolLit.getInstance(false);
                    break;
                case "eq?":
                    // eq? only accepts two arguments
                    if (args.getCdr().getCdr() != null)
                        result = new StringLit("Error: 'eq?' only accepts one argument");
                    else if (args.getCar() == args.getCdr().getCar()) // Not sure if this is going to work, actually
                        result = BoolLit.getInstance(true);
                    else
                        result = BoolLit.getInstance(false);
                    break;
                case "procedure?":
                    //procedure only accepts 1 argument (I think)
                    if (args.getCdr() != null)
                        result = new StringLit("Error: 'procedure?' only accepts one argument");
                    else if (args.getCar().isProcedure() == true)
                        result = BoolLit.getInstance(true);
                    else
                        result = BoolLit.getInstance(false);
                    break;
                default:
                    result = new StringLit("Error: BuiltIn.apply() called for non BuiltIn function.");
                    break;
            }

            // Now we check for the binary arithmetic builtins
            IntLit num1 = new IntLit(0), num2;
            if (symbol.getName() == "b/") // The division function has 1 as the default value instead of 0 when only 1 argument is provided
                num1 = new IntLit(1);

            // If the arguments aren't numerical, the IntLit casts will fail and the catch block executes.
            // If there are 0 arguments the first cast will fail, and if there are more than 2 an Exception is thrown
            try
            {
                num2 = (IntLit)args.getCar();
                // Check for a second argument
                if (args.getCdr().isPair() == true)
                {
                    // We do this swap because if we have only 1 argument, we're doing 0 - num2. If we have 2, we're doing num1-num2.
                    num1 = num2;
                    num2 = (IntLit)args.getCdr().getCar();
                    if (args.getCdr().getCdr() != null)
                        throw new Exception();
                }                   
                else if (symbol.getName() == "b=" || symbol.getName() == "b<")
                    return result = new StringLit("Error: binary arithmetic operation '" + symbol.getName() + "' expects two 2 arguments.");
            }
            catch
            {
                return result = new StringLit("Error: binary arithmetic operations expect 1 or 2 numerical arguments.");
            }

            if (symbol.getName() == "b+")
                result = new IntLit(num1.getInt() + num2.getInt());
            else if (symbol.getName() == "b-")
                result = new IntLit(num1.getInt() - num2.getInt());
            else if (symbol.getName() == "b*")
                result = new IntLit(num1.getInt() * num2.getInt());
            else if (symbol.getName() == "b/")
            {
                if (num2.getInt() == 0)
                    return result = new StringLit("Error: Dividing by 0.");
                result = new IntLit(num1.getInt() / num2.getInt());
            }
            else if (symbol.getName() == "b=")
            {
                if (num1.getInt() == num2.getInt())
                    result = BoolLit.getInstance(true);
                else
                    result = BoolLit.getInstance(false);
            }
            else if (symbol.getName() == "b<")
            {
                if (num1.getInt() < num2.getInt())
                    result = BoolLit.getInstance(true);
                else
                    result = BoolLit.getInstance(false);
            }
               
            return result;
    	}
    }    
}

