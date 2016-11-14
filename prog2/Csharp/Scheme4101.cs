// SPP -- The main program of the Scheme pretty printer.

using System;
using Parse;
using Tokens;
using Tree;

public class Scheme4101
{
    public static int Main(string[] args)
    {
        // Create scanner that reads from standard input
        Scanner scanner = new Scanner(Console.In);
        
        if (args.Length > 1 ||
            (args.Length == 1 && ! args[0].Equals("-d")))
        {
            Console.Error.WriteLine("Usage: mono SPP [-d]");
            return 1;
        }
        
        // If command line option -d is provided, debug the scanner.
        if (args.Length == 1 && args[0].Equals("-d"))
        {
            // Console.Write("Scheme 4101> ");
            Token tok = scanner.getNextToken();
            while (tok != null)
            {
                TokenType tt = tok.getType();

                Console.Write(tt);
                if (tt == TokenType.INT)
                    Console.WriteLine(", intVal = " + tok.getIntVal());
                else if (tt == TokenType.STRING)
                    Console.WriteLine(", stringVal = " + tok.getStringVal());
                else if (tt == TokenType.IDENT)
                    Console.WriteLine(", name = " + tok.getName());
                else
                    Console.WriteLine();

                // Console.Write("Scheme 4101> ");
                tok = scanner.getNextToken();
            }
            return 0;
        }

        // Create parser
        TreeBuilder builder = new TreeBuilder();
        Parser parser = new Parser(scanner, builder);
        Node root = new Node(parser);

        // TODO: Create and populate the built-in environment and
        Tree.Environment globalEnv = new Tree.Environment();

        Ident id = new Ident("begin");
        globalEnv.define(id, new BuiltIn(id));
        id = new Ident("cond");
        globalEnv.define(id, new BuiltIn(id));
        id = new Ident("if");
        globalEnv.define(id, new BuiltIn(id));
        id = new Ident("read");
        globalEnv.define(id, new BuiltIn(id));

        id = new Ident("b+");
        globalEnv.define(id, new BuiltIn(id));
        id = new Ident("b<");
        globalEnv.define(id, new BuiltIn(id));

        // create the top-level environment
        Tree.Environment topEnv = new Tree.Environment(globalEnv);

        // Read-eval-print loop

        root = (Node)parser.parseExp();
        while (root != null)
        {
            root.eval(topEnv).print(0);
            root = (Node)parser.parseExp();
        }
        /*
        // TODO: print prompt and evaluate the expression
        root = (Node) parser.parseExp();
        while (root != null) 
        {
            root.print(0);
            root = (Node) parser.parseExp();
        }
        */
        return 0;
    }
}
