using System.Runtime.InteropServices.JavaScript;
using System.Linq.Expressions;
using System.Text;

namespace LFA_lab2;

public class Grammar
{
    private HashSet<char> VN;
    private HashSet<char> VT;
    private Dictionary<char, List<string>> P;
    private char S;

    public char getS()
    {
        return S;
    }
    public Dictionary<char, List<string>> getP()
    {
        return P;
    }
    
    public HashSet<char> getVN()
    {
        return VN;
    }
    public HashSet<char> getVT()
    {
        return VT;
    }

    public Grammar(HashSet<char> vn, HashSet<char> vt, Dictionary<char, List<string>> p, char s)
    {
        VN = vn;
        VT = vt;
        P = p;
        S = s;
    }

    public string GenerateString()
    {
        Random rand = new Random();
        string result = S.ToString(); // Start with the start symbol

        while (result.Any(char.IsUpper))
        {
            StringBuilder newResult = new StringBuilder();

            foreach (char symbol in result)
            {
                if (VN.Contains(symbol))
                {
                    // Replace non-terminal with a random expansion
                    List<string> expansions = P[symbol];
                    string expansion = expansions[rand.Next(expansions.Count)];
                    newResult.Append(expansion);
                }
                else
                {
                    // If the symbol is terminal, keep it as it is
                    newResult.Append(symbol);
                }
            }

            result = newResult.ToString();
        }

        return result;
    }


    public FiniteAutomaton ToFiniteAutomaton()
    {
        // Convert Grammar to Finite Automaton
        HashSet<char> Q = new HashSet<char>(); // States
        HashSet<char> F = new HashSet<char>(); // Accepting states
        char q0 = S; // Initial state

        Dictionary<(char, char), HashSet<char>> delta = new Dictionary<(char, char), HashSet<char>>(); // Transition function

        // Adding transitions based on productions
        foreach (var entry in P)
        {
            char fromState = entry.Key;
            foreach (var expansion in entry.Value)
            {
                char inputSymbol = expansion[0]; // First symbol in production is the input symbol
                char toState;

                if (expansion.Length > 1)
                {
                    toState = expansion[1]; // Second symbol in production is the next state
                    if (!Q.Contains(toState))
                        Q.Add(toState);
                }
                else
                {
                    // If the expansion has only one symbol, it's a terminal symbol,
                    // so we create a new state to represent it
                    toState = inputSymbol;
                    Q.Add(toState);
                    F.Add(toState); // Terminal symbols are accepting states
                }

                if (!delta.ContainsKey((fromState, inputSymbol)))
                    delta[(fromState, inputSymbol)] = new HashSet<char>();

                delta[(fromState, inputSymbol)].Add(toState);
            }
        }

        // Convert transitions to single state transitions
        Dictionary<(char, char), HashSet<char>> simplifiedDelta = new Dictionary<(char, char), HashSet<char>>();
        foreach (var transition in delta)
        {
            char fromState = transition.Key.Item1;
            char inputSymbol = transition.Key.Item2;
            HashSet<char> toState = new HashSet<char> { transition.Value.First() }; // Choose any state as the target state
            simplifiedDelta[(fromState, inputSymbol)] = toState;
        }

        return new FiniteAutomaton(Q, VT, simplifiedDelta, q0, F);
    }

    
    // new method for lab 2 ------------------------------------------
    public GrammarType GetGrammarType()
    {
        // Check if the grammar meets the conditions for each type
        if (P.All(entry => entry.Value.All(expansion => expansion.Length == 1 && VT.Contains(expansion[0]))))
        {
            return GrammarType.Regular;
        }
        else if (P.All(entry => entry.Value.All(expansion => VN.Contains(expansion[0]))))
        {
            return GrammarType.ContextFree;
        }
        else if (P.All(entry => entry.Key == S && entry.Value.All(expansion => expansion.Length >= 2)))
        {
            return GrammarType.ContextSensitive;
        }
        else
        {
            return GrammarType.Unrestricted;
        }
    }
    
    
}