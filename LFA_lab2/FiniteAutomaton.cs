using System;
using System.Collections.Generic;
using System.Linq;

namespace LFA_lab2
{
    public class FiniteAutomaton
    {
        private HashSet<char> Q; // States
        private HashSet<char> Sigma; // Alphabet
        private Dictionary<(char, char), HashSet<char>> delta; // Transition function
        private char q0; // Initial state
        private HashSet<char> F; // Accepting states

        public HashSet<char> getQ()
        {
            return Q;
        }
        public HashSet<char> getSigma()
        {
            return Sigma;
        }
        public Dictionary<(char, char), HashSet<char>> getDelta()
        {
            return delta;
        }
        public char getq0()
        {
            return q0;
        }
        public HashSet<char> getF()
        {
            return F;
        }
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
            // Initialize variables for DFA construction
            HashSet<HashSet<char>> dStates = new HashSet<HashSet<char>>();
            Queue<HashSet<char>> dStateQueue = new Queue<HashSet<char>>();
            Dictionary<HashSet<char>, char> dStateMap = new Dictionary<HashSet<char>, char>();
            HashSet<char> initialState = EpsilonClosure(new HashSet<char>() { q0 }); // Compute epsilon closure of initial state
            dStateQueue.Enqueue(initialState);
            dStates.Add(initialState);

            // Continue until all states are processed
            while (dStateQueue.Count > 0)
            {
                HashSet<char> currentState = dStateQueue.Dequeue();

                // Calculate transitions for current state
                Dictionary<char, HashSet<char>> transitions = new Dictionary<char, HashSet<char>>();
                foreach (char symbol in Sigma)
                {
                    HashSet<char> nextState = new HashSet<char>();
                    foreach (char state in currentState)
                    {
                        if (delta.ContainsKey((state, symbol)))
                        {
                            nextState.UnionWith(delta[(state, symbol)]);
                        }
                    }
                    nextState = EpsilonClosure(nextState); // Take epsilon closure of next state
                    if (nextState.Count > 0)
                    {
                        transitions[symbol] = nextState;
                        if (!dStates.Contains(nextState))
                        {
                            dStates.Add(nextState);
                            dStateQueue.Enqueue(nextState);
                        }
                    }
                }

                // Map current state to new DFA state
                dStateMap[currentState] = currentState.Min(); // Choose lexicographically smallest state as representative
            }

            // Construct DFA from calculated states and transitions
            HashSet<char> dQ = new HashSet<char>(dStateMap.Values);
            Dictionary<(char, char), HashSet<char>> dDelta = new Dictionary<(char, char), HashSet<char>>();
            foreach (var entry in dStateMap)
            {
                HashSet<char> fromState = entry.Key;
                Dictionary<char, HashSet<char>> transitions = new Dictionary<char, HashSet<char>>(); // Move declaration here
                foreach (var symbol in Sigma)
                {
                    HashSet<char> nextState = new HashSet<char>();
                    foreach (char state in fromState)
                    {
                        if (delta.ContainsKey((state, symbol)))
                        {
                            nextState.UnionWith(delta[(state, symbol)]);
                        }
                    }
                    nextState = EpsilonClosure(nextState);
                    if (nextState.Count > 0)
                    {
                        transitions[symbol] = nextState;
                        if (!dStates.Contains(nextState))
                        {
                            dStates.Add(nextState);
                            dStateQueue.Enqueue(nextState);
                        }
                    }
                }

                // Construct DFA transition for the current state
                foreach (var symbol in Sigma)
                {
                    if (transitions.ContainsKey(symbol))
                    {
                        dDelta[(entry.Value, symbol)] = transitions[symbol];
                    }
                    else
                    {
                        dDelta[(entry.Value, symbol)] = new HashSet<char>(); // Add empty set for transitions not defined
                    }
                }
            }


            // Determine accepting states in DFA
            HashSet<char> dF = new HashSet<char>();
            foreach (var stateSet in dStates)
            {
                if (stateSet.Any(s => F.Contains(s)))
                {
                    dF.Add(dStateMap[stateSet]);
                }
            }

            return new FiniteAutomaton(dQ, Sigma, dDelta, initialState.Min(), dF);
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
