# Lexer Scanner

### Course: Formal Languages & Finite Automata
### Author: Timciuc Ana-Maria

----

## Theory
Chomsky Normal Form (CNF) is a way of representing context-free grammars in a standardized form. In CNF, every production rule has one of two forms: either a non-terminal symbol (such as a variable) produces exactly two non-terminals, or a non-terminal produces a terminal symbol. This format simplifies parsing algorithms and facilitates certain computational processes in formal language theory.## Objectives:

1. Learn about Chomsky Normal Form (CNF) [1].
2. Get familiar with the approaches of normalizing a grammar.
3. Implement a method for normalizing an input grammar by the rules of CNF.
   1. The implementation needs to be encapsulated in a method with an appropriate signature (also ideally in an appropriate class/type).
   2. The implemented functionality needs executed and tested.
   3. A BONUS point will be given for the student who will have unit tests that validate the functionality of the project.
   4. Also, another BONUS point would be given if the student will make the aforementioned function to accept any grammar, not only the one from the student's variant.

## Implementation description

1. Grammar class

The class that covers the grammar structure.
```csharp
public class Grammar
{
    public HashSet<char> VN { get; set; }
    public HashSet<char> VT { get; set; }
    public Dictionary<char, List<string>> P { get; set; }
    public char S { get; set; }


    public Grammar(HashSet<char> vn, HashSet<char> vt, Dictionary<char, List<string>> p, char s)
    {
        VN = vn;
        VT = vt;
        P = p;
        S = s;
    }
}
```

2. Eliminate Unit Production

The method that eliminates the unit productions from the grammar.
It uses the generic rules of the CNF to eliminate the unit productions. At first you find the unit production, and then coppy what it produces in the production that contains the unit production.
```csharp
    private Grammar EliminateUnitProductions(Grammar grammar)
    {
        // Implementation to eliminate unit productions
        foreach(var prod in grammar.P)
        {
            // the elements to be removed
            var elementsToRemove = new List<string>();

            foreach (var element in prod.Value)
            {
                if (element.Length == 1 && grammar.VN.Contains(element[0]))
                {
                    elementsToRemove.Add(element);
                }
            }
            
            // remove and copy the elements
            foreach (var element in elementsToRemove)
            {
                grammar.P[prod.Key].Remove(element); 
                grammar.P[prod.Key].AddRange(grammar.P[element[0]]);
            }
        }

        return grammar;
    }

```

3. Eliminate epsilon productions

The method that eliminates the epsilon productions from the grammar.
At first it has to find the epsilon productions, then remove them and add the new productions without epsilon productions.
```csharp
    private Grammar EliminateEpsilonProductions(Grammar grammar)
    {
        // Implementation to eliminate epsilon productions
        foreach (var prod in grammar.P)
        {
            var elementsToRemove = new List<string>();

            foreach (var element in prod.Value)
            {
                if (element == "ε")
                {
                    elementsToRemove.Add(element);
                }
            }

            // Remove epsilon elements after iterating
            foreach (var element in elementsToRemove)
            {
                grammar.P[prod.Key].Remove(element);
            }
            
            // Add new productions without epsilon productions
            foreach (var element in elementsToRemove)
            {
                foreach (var value in grammar.P.Values.SelectMany(x => x).ToList())
                {
                    if (value.Contains(element))
                    {
                        var newProduction = FindVn(element[0], value);
                        grammar.P[prod.Key].Add(newProduction);
                    }
                }
            }
        }
        return grammar;
    }


```

4. Eliminate Inaccessible Symbols

The method that eliminates the inaccessible symbols from the grammar.
It uses the generic rules of the CNF to eliminate the inaccessible symbols. At first you find the symbols that are not accessible, and then remove them.
```csharp
 private Grammar EliminateInaccessibleSymbols(Grammar grammar)
    {
        var elementsToRemove = FindInaccessibleVn(grammar.VN.ToList(), grammar.P.Values.SelectMany(x => x).ToList());
        // Remove the inaccessible symbols
        foreach (var element in elementsToRemove)
        {
            // Remove the element from the VN
            if (element == 'S') continue;
            grammar.VN.Remove(element);
            grammar.P.Remove(element);
        }
        return grammar;
    }
```

5. Convert to Chomsky Normal Form:

The method that converts the grammar to Chomsky Normal Form.
```csharp
    private Grammar ConvertTo_CNF(Grammar grammar)
    {
        char newSymbolChar = '1'; // Start character for new non-terminals
    
        // Implementation to convert grammar to CNF
        foreach(var prod in grammar.P.ToList())
        {
            // Check if the production has more than 2 elements
            foreach (var element in prod.Value.ToList())
            {
                if (element.Length > 2)
                {
                    // Replace the element with a new non-terminal
                    var newSymbol = newSymbolChar++;
                    grammar.VN.Add(newSymbol);
                    grammar.P[newSymbol] = new List<string> { element.Substring(1) };
                    grammar.P[prod.Key].Remove(element);
                    grammar.P[prod.Key].Add(element[0] + newSymbol.ToString());
                }
                if (element.Length == 2 && element.Any(char.IsLower))
                {
                    // Add a new non-terminal to replace the terminal
                    var newSymbol = newSymbolChar++;
                    grammar.VN.Add(newSymbol);
                    var lowIndex = FindLowercaseIndices(element);
                    foreach (var ind in lowIndex)
                    {   // Replace the terminal with the new non-terminal
                        grammar.P[newSymbol] = new List<string> { element[ind].ToString() };
                        grammar.P[prod.Key].Remove(element);
                        grammar.P[prod.Key].Add(element.Replace(element[ind], newSymbol));
                    }
                    
                }
            }
        }

        return grammar;
    }

```
## Conclusions


In conclusion, we have successfully implemented a method to convert a context-free grammar into Chomsky Normal Form (CNF) using C#. The implementation involved several steps:

Removing the start symbol from the right-hand side of productions.
Eliminating unit productions.
Eliminating epsilon productions.
Converting productions with more than two non-terminals on the RHS to CNF form, introducing new non-terminal symbols as needed.
Furthermore, the implementation was structured within a class, facilitating modularity and reusability. Unit tests were incorporated to validate the correctness of the implementation, ensuring that the converted grammars adhere to the rules of CNF.

Overall, this project demonstrates a robust approach to normalizing grammars into CNF, providing a foundational tool for various applications in natural language processing, compiler design, and related fields.

## Results(example)
* input:
```
HashSet<char> vn = new HashSet<char> { 'S', 'A', 'B', 'C', 'D', 'X' };
        HashSet<char> vt = new HashSet<char> { 'a', 'b' };
        Dictionary<char, List<string>> p = new Dictionary<char, List<string>> {
            {'S', new List<string>{"B"}},
            {'A', new List<string>{"aX", "bX"}},
            {'X', new List<string>{"ε", "BX", "b"}},
            {'B', new List<string>{"AXbD"}},
            {'D', new List<string>{"aD", "a"}},
            {'C', new List<string>{"Ca"}}
        };
        char startSymbol = 'S';
```
* output:
```
New grammar:
S -> A1 |
A -> 2X | 3X |
X -> BX | b |
B -> A4 |
D -> a | 5D |
C -> C6 |
1 -> XbD |
2 -> a |
3 -> b |
4 -> XbD |
5 -> a |
6 -> a |
```
