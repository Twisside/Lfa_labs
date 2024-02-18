using System.Runtime.InteropServices.JavaScript;
using System.Linq.Expressions;
using System.Text;

namespace LFA_lab1;

public class Grammar
{
    private HashSet<char> VN;
    private HashSet<char> VT;
    private Dictionary<char, List<string>> P;
    private char S;

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
        HashSet<char> Q = new HashSet<char>(VN); // States
        Q.UnionWith(VT);
        char q0 = S; // Initial state
        HashSet<char> F = new HashSet<char> { S }; // Accepting states

        Dictionary<(char, char), char> delta = new Dictionary<(char, char), char>(); // Transition function
        foreach (var entry in P)
        {
            char fromState = entry.Key;
            foreach (var expansion in entry.Value)
            {
                char inputSymbol = expansion[0];
                if (expansion.Length > 1)
                {
                    char toState = expansion[1];
                    delta[(fromState, inputSymbol)] = toState;
                }
            }
        }

        return new FiniteAutomaton(Q, VT, delta, q0, F);
    }
}