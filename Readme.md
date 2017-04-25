# VCal - A Minimalistic Calcuation System

## Intro
VCal is a library designed to programatically evaluate strings containing
mathematical terms without the need for a full-fledged scripting language.

## Usage

```C#
void Evaluate()
{
  string function = "1 + 4";
  var result = new TermParse.EvalString(function);
  System.Console.Println(result);   // Prints 5
}
```

### Variables
VCal terms can contain variables, that are evaluated, when the term is evaluated.
e.g.:

```C#
void EvaluateWithVariables()
{
  Symboltable sym = new Symboltable;
  sym.RegisterSymbol("x", 24);
  string function = "x - 20";
  var term = new TermParse.Parse(function);
  var result = term.Eval(sym);
  System.Console.Prinln(result);    // Prints 4.
}
```

Note, that the variable x in the above example does not need to exist, when the
term "x - 20" is parsed. The interpreter will store a transient reference and
only try to access x, when it evaluates the term - i.e. x must exist, when
the term is evaluated, but there is no need beforehand.

### Termvariables
A VCal variable can be a term itself and dependent on other variables, thus,
we can do fancy stuff like this:

```C#
void EvaluateWithTermVars()
{
  Symboltable sym = new Symboltable;
  sym.RegisterSymbolResolver("x", new TermResolver("4 * y + 1"));
  sym.RegisterSymbol("y", 2)
  string function = "x - 20";
  var term = new TermParse.Parse(function);
  var result = term.Eval(sym);
  System.Console.Prinln(result);    // Prints -11
}
```

A word of explaination: At first we register a termresolver with the name "x",
which evaluates to the term "4 * y + 1", afterwards we register the variable y,
which evaluates to 2. At last we evaluate the function "x - 20", which is expanded
to:
4 * 2 + 1 - 20
yielding 11

## Built-In Functionality:
- Standard Operators + - * /
- Listprocessing: Min, Max, Sum
- Arithmetics: Sin, Cos, Pow, Sqrt

## Extending the functionality

### User Functions
VCal allows to define functions that take a given number of parameters and return
a single value. A function is defined using the "Def" command, which needs to
be supplied 3 paramters:
- Name of the function to define
- A list of parameters
- The expression to evaluate.
e.g.:
Def(TestFunc,{p0, p1}, p0 + p1)
Defines the function "TestFunc", taking two parameters(p0 and p1).

### Custom Built-In Functions
User functions provide an easy way to extend the functionality, however they are
of limited use due to the fact that they are:
- Written in VCal syntax, which is intenionally simple.
- Consist of interpreted code that will easily be outperformed by a built-in function.

These limitations do not exist when writing a built-in function. A built-in can
be any function, that corresponds to the signature:

object FunctionCall(List<AST.Node> parameters, SymbolTable symTable);

The function should always yield a value (i.e. not null), even if the value is "0".

Now, the runtime will pass all function parameters as a list with elements of the
typa AST.Node. This type can represent anything in VCal, it might be a list, a
value or, indeed, a fragment of code. Most functions will have to do some conversion
with the parameters in order to do something meaningful. To do so, call the
"Eval" function on the parameters you intend to use. Any Node, that can be evaluated
to a number will yield an object containing a float (i.e. it is safe to cast) to
a float. If you are dealing with a list, eval will evaluate to a ListNode and can
be cast as well.

Example from one of the built-in functions already defined:

```C#
public static object Cos(List<Node> parameters, SymbolTable symTable)
{
    if (parameters.Count != 1)
        throw new InvalidOperationException("Cos needs exactly 1 parameter.");
    return (float)Math.Cos((float)parameters[0].Eval(symTable));
}
```

Here the result of eval is cast directly to float. Note that we don't care for
errorhandling here, yet. This will probably happen at a later point in development.

We've not yet talked about the symbol table passed as the second parameter. This
parameter contains the entire scope, in which the function is called. If the
function needs access to other variables or similar, the data it wants to access
must be present in the symbol table.

So, at last: We know, how to define a built-in, now, how do we actually use it?
Easy - in the first examples, we passed a symboltable into the eval function. Now,
if we add our built-in to that table, it is accessible for all other functions.
Example:

```C#
public static object MyFirstBuiltIn(List<Node> parameters, SymbolTable symTable)
{
    if (parameters.Count != 2)
        throw new InvalidOperationException("Needs exactly 2 parameters.");
    return (float)parameters[0].Eval(symTable) + (float)parameters[1].Eval(symTable);
}

void EvaluateCustomFunction()
{
  Symboltable sym = new Symboltable;
  sym.RegFunc("MyFirstBuiltIn", MyFirstBuiltIn)
  var term = new TermParse.Parse("MyFirstBuiltIn(2,5)");
  var result = term.Eval(sym);
  System.Console.Prinln(result);    // Prints 7
}
```


## The REPL
VCal comes with a simple "Read-Eval-Print-Loop" (REPL) program, that can be used
to test the functionality of the library. The REPL is a pure commandline interface
that accepts VCal syntax and directly evaluates it. The REPL uses a single global
symboltable.

e.g.:
Type
1 + 1 (return)
Result:
>> 2

Special commands of the reple:
"exit" - Terminates the program.
"env" - Shows all defined functions and symbols in the global symboltable.
"ClearEnv" - Resets the global symboltable to starting conditions, thus cleareing
everything you entered.
"Let" - defines a symbol. Let basically does the same as this:
        Symboltable sym = new Symboltable;
        sym.RegisterSymbolResolver("x", new TermResolver("4 * y + 1"));
        except, that it always works on the same symbolresolver.
        Syntax for let:
        Let (<symbolName>, <expression>)
        example:
        Let (foo, 10 * x)
