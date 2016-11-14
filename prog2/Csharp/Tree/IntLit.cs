// IntLit -- Parse tree node class for representing integer literals

using System;

namespace Tree
{
    public class IntLit : Node
    {
        private int intVal;

        public IntLit(int i)
        {
            intVal = i;
        }

        public int getInt()
        {
            return intVal;
        }

        public override void print(int n)
        {
            Printer.printIntLit(n, intVal);
        }

        public override bool isNumber()
        {
            return true;
        }

        public override Node eval(Environment env, Node args = null)
        {
            return new IntLit(intVal); // not sure if we should create a new one or just return this. Creating a new one to be safe
        }
    }
}
