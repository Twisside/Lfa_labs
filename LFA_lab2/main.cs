using System;
using System.Collections.Generic;

namespace LFA_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the finite automaton
            HashSet<char> Q = new HashSet<char> { '0', '1', '2', '3' };
            HashSet<char> Sigma = new HashSet<char> { 'a', 'b', 'c' };
            Dictionary<(char, char), HashSet<char>> delta = new Dictionary<(char, char), HashSet<char>> {
                {('0', 'a'), new HashSet<char> {'0', '1'}},
                {('1', 'a'), new HashSet<char> {'1'}},
                {('1', 'c'), new HashSet<char> {'2'}},
                {('1', 'b'), new HashSet<char> {'3'}},
                {('0', 'b'), new HashSet<char> {'2'}},
                {('2', 'b'), new HashSet<char> {'3'}}
            };
            char q0 = '0';
            HashSet<char> F = new HashSet<char> { '3' };

            // Create finite automaton object
            FiniteAutomaton finiteAutomaton = new FiniteAutomaton(Q, Sigma, delta, q0, F);

            // a. Convert finite automaton to regular grammar
            Grammar regularGrammar = finiteAutomaton.ToRegularGrammar();
            Console.WriteLine("Regular Grammar from Finite Automaton:");
            Console.WriteLine("VN: " + string.Join(", ", regularGrammar.P.Keys));
            Console.WriteLine("VT: " + string.Join(", ", regularGrammar.VT));
            Console.WriteLine("Productions:");
            foreach (var entry in regularGrammar.P)
            {
                Console.WriteLine(entry.Key + " -> " + string.Join(" | ", entry.Value));
            }
            Console.WriteLine("Start symbol: " + regularGrammar.S);

            // b. Determine whether the finite automaton is deterministic or non-deterministic
            bool isDeterministic = finiteAutomaton.IsDeterministic();
            Console.WriteLine("\nThis finite automaton is " + (isDeterministic ? "deterministic" : "non-deterministic"));

            // c. Convert NFA to DFA
            if (!isDeterministic)
            {
                FiniteAutomaton dfa = finiteAutomaton.ConvertToDFA();
                Console.WriteLine("\nDFA Converted from NFA:");
                Console.WriteLine("Q: " + string.Join(", ", dfa.Q));
                Console.WriteLine("Sigma: " + string.Join(", ", dfa.Sigma));
                Console.WriteLine("Delta: ");
                foreach (var transition in dfa.delta)
                {
                    Console.WriteLine("(" + transition.Key.Item1 + ", " + transition.Key.Item2 + ") -> " + string.Join(", ", transition.Value));
                }
                Console.WriteLine("q0: " + dfa.q0);
                Console.WriteLine("F: " + string.Join(", ", dfa.F));
            }
            else
            {
                Console.WriteLine("\nThe finite automaton is already deterministic. No conversion needed.");
            }
        }
    }
}
