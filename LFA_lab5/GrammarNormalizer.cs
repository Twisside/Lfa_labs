namespace LFA_lab5;

public class GrammarNormalizer
{
    public Grammar Normalization (Grammar grammar)
    {
        grammar = EliminateEpsilonProductions(grammar);
        grammar = EliminateUnitProductions(grammar);
        grammar = EliminateInaccessibleSymbols(grammar);
        grammar = ConvertTo_CNF(grammar);
        return grammar;
    }

    // private static Grammar RemoveStartSymbolFrom_RHS(Grammar grammar)
    // {
    //     var startSymbol = grammar.Keys.First();
    //     foreach (var key in grammar.Keys.ToList())
    //     {
    //         grammar[key] = grammar[key].Select(prod => prod.Replace(startSymbol, "")).ToList();
    //     }
    //     return grammar;
    // }

    private Grammar EliminateUnitProductions(Grammar grammar)
    {
        // Implementation to eliminate unit productions
        foreach(var prod in grammar.P)
        {
            var elementsToRemove = new List<string>();

            foreach (var element in prod.Value)
            {
                if (element.Length == 1 && grammar.VN.Contains(element[0]))
                {
                    elementsToRemove.Add(element);
                }
            }
            
            foreach (var element in elementsToRemove)
            {
                grammar.P[prod.Key].Remove(element); 
                grammar.P[prod.Key].AddRange(grammar.P[element[0]]);
            }
        }

        return grammar;
    }

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
    
    private Grammar EliminateInaccessibleSymbols(Grammar grammar)
    {
        var elementsToRemove = FindInaccessibleVn(grammar.VN.ToList(), grammar.P.Values.SelectMany(x => x).ToList());
        
        foreach (var element in elementsToRemove)
        {
            if (element == 'S') continue;
            grammar.VN.Remove(element);
            grammar.P.Remove(element);
        }
        return grammar;
    }
    
    private string FindVn(char eps, string production)
    {

        // Iterate over each non-terminal
        bool found = production.Contains(eps);
        
        // get the index of the eps
        var index = production.IndexOf(eps);
        // If the non-terminal is not found, add to the inaccessible list
        if (found)
        {
            production = production.Remove(index, 1);
        }

        return production;
    }
    
    private List<char> FindInaccessibleVn(List<char> vn, List<string> productions)
    {
        // List to hold inaccessible non-terminals
        List<char> inaccessible = new List<char>();

        // Iterate over each non-terminal
        foreach (char nonTerminal in vn)
        {
            bool found = false;

            // Check if the current non-terminal is in any production
            foreach (string production in productions)
            {
                if (production.Contains(nonTerminal))
                {
                    found = true;
                    break;
                }
            }

            // If the non-terminal is not found, add to the inaccessible list
            if (!found)
            {
                inaccessible.Add(nonTerminal);
            }
        }

        return inaccessible;
    }

    private Grammar ConvertTo_CNF(Grammar grammar)
    {
        char newSymbolChar = '1'; // Start character for new non-terminals
    
        // Implementation to convert grammar to CNF
        foreach(var prod in grammar.P.ToList())
        {
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
                    {
                        grammar.P[newSymbol] = new List<string> { element[ind].ToString() };
                        grammar.P[prod.Key].Remove(element);
                        grammar.P[prod.Key].Add(element.Replace(element[ind], newSymbol));
                    }
                    
                }
            }
        }

        return grammar;
    }

    static int[] FindLowercaseIndices(string str)
    {
        // Initialize a list to store the indices of lowercase symbols
        var lowercaseIndices = new System.Collections.Generic.List<int>();

        // Loop through each character in the string
        for (int i = 0; i < str.Length; i++)
        {
            // Check if the character is lowercase
            if (char.IsLower(str[i]))
            {
                // If it is, add its index to the list
                lowercaseIndices.Add(i);
            }
        }

        // Convert the list to an array and return it
        return lowercaseIndices.ToArray();
    }


}
