
namespace LFA_lab2
{
    public class FiniteAutomaton
    {
        public HashSet<char> Q {get; set;}  
        // States
        public HashSet<char> Sigma { get; set; } // Alphabet
        public Dictionary<(char, char), HashSet<char>> delta { get; set; } // Transition function
        public char q0 { get; set; } // Initial state
        public HashSet<char> F { get; set; } // Accepting states

        public FiniteAutomaton(HashSet<char> q, HashSet<char> sigma, Dictionary<(char, char), HashSet<char>> d, char q0, HashSet<char> f)
        {
            Q = q;
            Sigma = sigma;
            delta = d;
            this.q0 = q0;
            F = f;
        }


        public bool StringBelongToLanguage(string inputString)
        {
            HashSet<char> currentStates = new HashSet<char>();
            currentStates.Add(q0);

            foreach (char symbol in inputString)
            {
                // Check if the symbol belongs to the alphabet
                if (!Sigma.Contains(symbol))
                {
                    // Symbol not in alphabet
                    return false;
                }

                HashSet<char> nextStates = new HashSet<char>();

                foreach (char state in currentStates)
                {
                    // Check if there exists a transition for the current state and symbol
                    if (delta.ContainsKey((state, symbol)))
                    {
                        // Transition exists, add next states
                        nextStates.UnionWith(delta[(state, symbol)]);
                    }
                }

                if (nextStates.Count == 0)
                {
                    // No transition for this symbol, string does not belong to language
                    return false;
                }

                currentStates = nextStates;
            }

            // Check if at least one of the final states is an accepting state
            return currentStates.Any(s => F.Contains(s));
        }

        public Grammar ToRegularGrammar()
        {
            HashSet<char> vn = new HashSet<char>(Q); // Non-terminals
            HashSet<char> vt = new HashSet<char>(Sigma); // Terminals
            Dictionary<char, List<string>> p = new Dictionary<char, List<string>>(); // Productions
            char s = q0; // Start symbol

            foreach (var state in Q)
            {
                foreach (var symbol in Sigma)
                {
                    if (delta.ContainsKey((state, symbol)))
                    {
                        foreach (var nextState in delta[(state, symbol)])
                        {
                            if (!p.ContainsKey(state))
                            {
                                p[state] = new List<string>();
                            }
                            p[state].Add(symbol.ToString() + nextState);
                        }
                    }
                }
            }

            return new Grammar(vn, vt, p, s);
        }

        public bool IsDeterministic()
        {
            foreach (var state in Q)
            {
                foreach (var symbol in Sigma)
                {
                    if (delta.ContainsKey((state, symbol)) && delta[(state, symbol)].Count > 1)
                    {
                        return false; // Non-deterministic
                    }
                }
            }
            return true; // Deterministic
        }

        public FiniteAutomaton ConvertToDFA()
{
    var dStates = new Dictionary<string, HashSet<char>>(); // New DFA states
    var dStateQueue = new Queue<string>(); // Queue for processing states
    var dDelta = new Dictionary<(char, char), HashSet<char>>(); // DFA transitions
    var initialState = new string(new[] { q0 }); // Initial state of DFA
    dStates[initialState] = new HashSet<char> { q0 };
    dStateQueue.Enqueue(initialState);

    while (dStateQueue.Count > 0)
    {
        var currentState = dStateQueue.Dequeue();

        foreach (var symbol in Sigma)
        {
            var nextStateSet = new HashSet<char>();

            foreach (var state in dStates[currentState])
            {
                if (delta.ContainsKey((state, symbol)))
                {
                    nextStateSet.UnionWith(delta[(state, symbol)]);
                }

                // Add epsilon closure if there are epsilon transitions
                nextStateSet.UnionWith(EpsilonClosure(nextStateSet));
            }

            var nextStateId = new string(nextStateSet.ToArray());
            if (!dStates.ContainsKey(nextStateId))
            {
                dStates[nextStateId] = nextStateSet;
                dStateQueue.Enqueue(nextStateId);
            }
        }
    }

    // Construct DFA transitions
    foreach (var kvp in dStates)
    {
        var currentState = kvp.Key;
        foreach (var symbol in Sigma)
        {
            var nextStates = new HashSet<char>();
            foreach (var state in kvp.Value)
            {
                if (delta.ContainsKey((state, symbol)))
                {
                    nextStates.UnionWith(delta[(state, symbol)]);
                }
            }

            // If no next state is found, continue to the next symbol
            if (nextStates.Count == 0)
                continue;

            // Find the ID for the next state set
            var nextStateId = dStates.First(kvp2 => kvp2.Value.SetEquals(nextStates)).Key;

            // Add the transition to dDelta
            dDelta[(currentState.Min(), symbol)] = new HashSet<char>(nextStateId);
        }
    }

    // Create a new DFA using dStates and dDelta
    var newQ = new HashSet<char>(dStates.Keys.SelectMany(id => id).Distinct());
    var newF = new HashSet<char>(dStates.Where(kvp => kvp.Value.Any(s => F.Contains(s))).Select(kvp => kvp.Key[0]));

    return new FiniteAutomaton(newQ, Sigma, dDelta, initialState[0], newF);
}

        


        // Helper method to compute epsilon closure of a set of states
        private HashSet<char> EpsilonClosure(HashSet<char> states)
        {
            HashSet<char> closure = new HashSet<char>(states);
            Stack<char> stack = new Stack<char>(states);

            while (stack.Count > 0)
            {
                char currentState = stack.Pop();
                if (delta.ContainsKey((currentState, '~'))) // Assuming '~' represents epsilon transition
                {
                    foreach (char nextState in delta[(currentState, '~')])
                    {
                        if (!closure.Contains(nextState))
                        {
                            closure.Add(nextState);
                            stack.Push(nextState);
                        }
                    }
                }
            }

            return closure;
        }

    }
}
