using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.Parser;
using vcal.VariableResolving;

namespace Repl
{
    class Program
    {
        public static bool done = false;
        public static SymbolTable environment = new SymbolTable();

        public static object Exit(List<Node> parameters, SymbolTable symTable)
        {
            done = true;
            return 0f;
        }

        public static object ClearEnv(List<Node> parameters, SymbolTable symTable)
        {
            environment = new SymbolTable();
            SetupEnvironment();
            return 0f;
        }

        static void SetupEnvironment()
        {
            environment.RegFunc("exit", Exit);
            environment.RegFunc("let", Runtime.Let);
            environment.RegFunc("env", Runtime.Env);
            environment.RegFunc("ClearEnv", ClearEnv);
        }

        static void Main(string[] args)
        {
            SetupEnvironment();

            TermParse prs = new TermParse();
            while(!done)
            {
                var cmd = Console.ReadLine();

                try
                {
                   Console.ForegroundColor = ConsoleColor.Green;
                   Console.WriteLine(">> " + prs.EvalString(cmd, environment));                   
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}
