namespace LFA_lab1;

public class FiniteAutomaton
{
    private HashSet<char> Q; // States
    private HashSet<char> Sigma; // Alphabet
    private Dictionary<(char, char), char> delta; // Transition function
    private char q0; // Initial state
    private HashSet<char> F; // Accepting states

    public FiniteAutomaton(HashSet<char> q, HashSet<char> sigma, Dictionary<(char, char), char> d, char q0, HashSet<char> f)
    {
        Q = q;
        Sigma = sigma;
        delta = d;
        this.q0 = q0;
        F = f;
    }

    
    public bool StringBelongToLanguage(string inputString)
    {
        char currentState = q0;

        foreach (char symbol in inputString)
        {
            // Check if the symbol belongs to the alphabet
            if (!Sigma.Contains(symbol))
            {
                // Symbol not in alphabet
                return false;
            }

            // Check if there exists a transition for the current state and symbol
            if (delta.ContainsKey((currentState, symbol)))
            {
                // Transition exists, move to next state
                currentState = delta[(currentState, symbol)];
            }
            else
            {
                // No transition for this symbol, string does not belong to language
                return false;
            }
        }

        // Check if the final state is an accepting state
        Console.WriteLine("huraaayyy!");
        return F.Contains(currentState);
    }
}