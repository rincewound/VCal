using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.VariableResolving;

namespace vcal.Parser
{
    public class TermParse
    {
      char[] splitSigns = new char[]{ ' '};
      char[] operators = new char[]{'-' , '+', '*', '/'};

      bool IsOperator(string op)
      {
        return operators.Contains(op[0]);
      }

    public object EvalString(string line)
    {
            object tmpe = Parse(line).Eval(new VariableResolving.SymbolTable());
            return tmpe;
    }

    public object EvalString(string cmd, SymbolTable environment)
    {
        return Parse(cmd).Eval(environment);
    }

    public Node Parse(string line)
    {
        // Takes a line, e.g. 4 + 7 * 2 + y
        // and converts it to an AST object, that can be evaluated.
        var tokenizer = new Tokenizer(new[] { " ", "+", "-", "*", @"/", "(", ")", ",", "{", "}" });
        var tokens = tokenizer.Tokenize(line).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();      
        var rootNode = parseTokenStream(tokens);
        return rootNode;
    }

        Node parseTokenStream(string[] tokens)
      {
        // find first toplevel operator
        if(tokens.Length > 1)
        {
            return processMultiTokenStream(tokens);
        }

        if(tokens.Length == 0)
        {
            return null;
        }

        // tokenstreams, that have been reduced to a single token are
        // expected to be leaf nodes of the term (i.e. values or variables)
        // and are treated as such.

        // naive way: try to parse the value as float, if that fails, we assume
        // a variable..
        float outVal;
        if(float.TryParse(tokens[0], out outVal))
        {
            return new StaticValueNode(outVal);
        }

        else
        {
             return new VarRefNode(tokens[0]);
        }
      }

      OpNode CreateOpNode(string token)
      {
        if(token== "-")
          return new SubNode();
        if(token== "+")
          return new AddNode();
        if(token== "/")
          return new DivNode();
        if(token== "*")
          return new MulNode();

        throw new InvalidOperationException("Bad Operator " + token);
      }

        int GetLowestPrecendeOperatorIndex(string[] tokens)
        {
            Dictionary<string, int> precedenceTable = new Dictionary<string, int>();
            precedenceTable["+"] = 100;
            precedenceTable["-"] = 100;
            precedenceTable["*"] = 200;
            precedenceTable["/"] = 200;

            int opPos = -1;
            string lastOp = "";
            var lastOpPrecedence = 1000;
            int level = 0;            

            for(int i = 0; i < tokens.Length; i++)
            {
                if(IsOperator(tokens[i]) && level == 0)
                {
                    var prec = precedenceTable[tokens[i]];
                    if (prec >= lastOpPrecedence)
                        continue;
                    lastOpPrecedence = prec;
                    lastOp = tokens[i];
                    opPos = i;
                }

                if (tokens[i] == "(")
                    level++;
                if (tokens[i] == ")")
                    level--;             
               if (tokens[i] == "{")
                    level++;
                if (tokens[i] == "}")
                    level--;

            }
            return opPos;
      }

      Node processMultiTokenStream(string[] tokens)
      {
            // diveide and conquer: take first toplevel operator and recurse left
            // and right of it. sadly this cannot take operator precedence into account,
            // so the user will have to use brackets for this.

            //i.e. a term like 4 * 5 + 7 will become:
            //          Mul
            //      4     Add
            //           5   7
            // resulting in 4 being multiplied with 12 instead of 5.            
            int i = GetLowestPrecendeOperatorIndex(tokens);

            // No top level operator found? We might have to deal with a call to
            // an inbuilt function, e.g. Sqrt(a + 10)
            if (i == -1)
            {
                if (tokens[0] == "{" && tokens[tokens.Length - 1] == "}")
                {
                    // this is probably a plain list.
                    var listEntries = parseParamlist(tokens.Skip(1).Take(tokens.Length - 2));
                    return new ListNode(listEntries);
                }

                if (tokens[0] == "(" && tokens[tokens.Length - 1] == ")")
                {
                    return parseTokenStream(tokens.Skip(1).Take(tokens.Length - 2).ToArray());
                }

                return CreateFuncCallNode(tokens);
            }

            // slice array.:
            var left = parseTokenStream(tokens.Take(i).ToArray());
            var right = parseTokenStream(tokens.Skip(i + 1).ToArray());
            OpNode n = CreateOpNode(tokens[i]);
            n.Left = left;
            n.Right = right;
            return n;
      }

        private Node CreateFuncCallNode(string[] tokens)
        {
            // check if the second and the last token are parentheses,
            // in that case, we assume, that we have a parameterlist.
            if (tokens[1] != "(" || tokens[tokens.Length - 1] != ")")
            {
                throw new Exception("Bad token stream @ " + tokens[0]);
            }

            // Create function call node.
            // use inner part of parameterlist tokens, i.e. strip the outer brackets.
            var paramlistTokens = tokens.Skip(2).Take(tokens.Length - 3);
            var paramlistNodes = parseParamlist(paramlistTokens);
            var funcCall = tokens[0];

            return new FuncCallNode(funcCall, paramlistNodes);
        }

        private List<Node> parseParamlist(IEnumerable<string> tokens)
        {
            List<Node> nodeList = new List<Node>();
            List<string> currentSubTokenList = new List<string>();

            int level = 0;
            foreach(var str in tokens)
            {
                if (str == "(")
                    level++;
                if (str == ")")
                    level--;

                if (str == "{")
                    level++;
                if (str == "}")
                    level--;

                if (str == "," && level == 0)
                {
                    nodeList.Add(parseTokenStream(currentSubTokenList.ToArray()));
                    currentSubTokenList.Clear();
                    continue;
                }

                currentSubTokenList.Add(str);
            }
            nodeList.Add(parseTokenStream(currentSubTokenList.ToArray()));
            return nodeList;
        }
    }

}
