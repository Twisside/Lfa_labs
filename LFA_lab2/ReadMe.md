# Grammar and Finite Automaton Conversion

### Overview
This project implements the conversion of a finite automaton (FA) to a regular grammar, determines whether the FA is deterministic or non-deterministic, and provides functionality to convert a non-deterministic finite automaton (NFA) to a deterministic finite automaton (DFA).

### Theory
Languages are sets of strings defined by rules, and grammars provide a systematic way to generate these strings. A finite automaton is a mathematical model used to recognize languages by processing strings of symbols according to predefined rules.

### Objectives
1. Implement conversion of a finite automaton to a regular grammar.
2. Determine whether the finite automaton is deterministic or non-deterministic.
3. Implement functionality to convert a non-deterministic finite automaton to a deterministic one.

### Implementation Description
- The project includes classes for Grammar, FiniteAutomaton, and a Program class for demonstration.
- The Grammar class provides methods to generate strings, determine grammar type, and convert to a finite automaton.
- The FiniteAutomaton class represents a finite automaton and provides methods for string recognition, conversion to a regular grammar, and determination of determinism.
- The Program class demonstrates the usage of the implemented functionality with sample input.

### Code Snippets
```csharp
// Convert finite automaton to regular grammar
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
```
```csharp
// Determine whether the finite automaton is deterministic
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
```
```csharp
// Convert NFA to DFA
public FiniteAutomaton ConvertToDFA()
        {
            var dStates = new Dictionary<string, HashSet<char>>(); // Новые состояния ДКА
            var dStateQueue = new Queue<string>(); // Очередь для обработки состояний
            var dDelta = new Dictionary<(char, char), HashSet<char>>(); // Транзиции ДКА
            var initialState = new string(new[] { q0 }); // Начальное состояние ДКА
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

                        // Добавляем эпсилон-замыкание, если есть эпсилон-переходы
                        nextStateSet = EpsilonClosure(nextStateSet);
                    }

                    var nextStateId = new string(nextStateSet.ToArray());
                    if (!dStates.ContainsKey(nextStateId))
                    {
                        dStates[nextStateId] = nextStateSet;
                        dStateQueue.Enqueue(nextStateId);
                    }

                    // Обновляем dDelta для текущего символа и состояния
                    dDelta[(currentState[0], symbol)] = new HashSet<char>(nextStateId);
                }
            }

            // Создаем новый DFA с использованием dStates и dDelta
            var newQ = new HashSet<char>(dStates.Keys.SelectMany(id => id).Distinct());
            var newF = new HashSet<char>(dStates.Where(kvp => kvp.Value.Any(s => F.Contains(s))).Select(kvp => kvp.Key[0]));

            return new FiniteAutomaton(newQ, Sigma, dDelta, initialState[0], newF);
        }
```

### Conclusions / Results
- The project successfully achieves the objectives of converting finite automata to regular grammars, determining determinism, and converting NFAs to DFAs.
- It provides a robust framework for language recognition and conversion between different formal language representations.

This README provides an overview of the project, its objectives, implementation details, and usage instructions.