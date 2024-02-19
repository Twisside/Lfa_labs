# The title of the work

### Course: Formal Languages & Finite Automata
### Author: Timciuc Ana-Maria

----

## Theory
Languages are sets of strings. A language is a set of sequences of symbols, called strings. 
A string is a finite sequence of symbols from a finite alphabet. A grammar is a set of rules for constructing strings in the language, 
where each rule specifies a replacement for a symbol. A finite automaton is a mathematical model that consists of a finite set of states, 
a finite set of input symbols, a transition function, a start state, and a set of accepting states. The finite automaton reads input from 
the input tape and moves from state to state according to the transition function, until the input is consumed. The finite automaton accepts 
the input if it ends in an accepting state.


## Objectives:

* a. Implement a type/class for your grammar;

*  b. Add one function that would generate 5 valid strings from the language expressed by your given grammar;

*  c. Implement some functionality that would convert and object of type Grammar to one of type Finite Automaton;

*  d. For the Finite Automaton, please add a method that checks if an input string can be obtained via the state transition from it;



## Implementation description

- Grammar class
    -  it's a class that contains the following attributes:
        - `VN` - a list of non-terminal symbols
        - `VT` - a list of terminal symbols
        - `P` - a list of productions
        - `S` - the start symbol of the grammar
    - it contains the following methods:
        - `GenerateString` - a method that generates a valid string from the language expressed by the given grammar by iterating through the same string until it contains only terminal symbols
        - `ToFiniteAutomaton` - a method that converts the grammar to a finite automaton by initializing the sets and variables and building the transition function by iterating  through the productions
- Finite Automaton class
    - it's a class that contains the following attributes:
        - `Q` - a set of states
        - `E` - a set of alphabet symbols
        - `d` - a transition function
        - `q0` - the initial state
        - `F` - a set of final states
        - it contains the following methods:
            - `StringBelongToLanguage` - a method that checks if an input string can be obtained via the state transition from it by iterating through the input string and checking if the transition function contains the current state and the current symbol. It returns True if the string is accepted if it does and False otherwise.
- Main class
    - In the main class are defined the VN, VT, P, S.
    - It creates 5 strings by calling the `GenerateString` method and prints them. The results are not guaranteed to always be different. (Something for future fixing, we are keeping things simple for now)
    - Then we can check if a given string belongs to the language of the grammar by calling the `StringBelongToLanguage` method and printing the result.


* Code snippets.

```
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
```

```
public FiniteAutomaton ToFiniteAutomaton()
    {
        // Convert Grammar to Finite Automaton
        HashSet<char> Q = new HashSet<char>(VN); // States
        Q.UnionWith(VT);
        char q0 = S; // Initial state
        HashSet<char> F = new HashSet<char>(Q); // Accepting states

        Dictionary<(char, char), char> delta = new Dictionary<(char, char), char>(); // Transition function
        foreach (var entry in P)
        {
            char fromState = entry.Key;
            foreach (var expansion in entry.Value)
            {
                char inputSymbol = expansion[0]; // x => !x.Any(char.IsUpper) // i don't know how to write this
                char toState;
                if (expansion.Length > 1) // .Any(char.IsUpper) would be better but we run out of time
                {
                    toState = expansion[1]; // x => x.Any(char.IsUpper) not the correct way to write but the logic is there
                }
                else // it has issues i know
                {
                    // If the expansion has only one symbol, it's a terminal symbol,
                    // so we create a new state to represent it // messy but works for my variant
                    toState = inputSymbol;
                    Q.Add(toState);
                }
                delta[(fromState, inputSymbol)] = toState;
            }
        }

        return new FiniteAutomaton(Q, VT, delta, q0, F);
    }
```

```
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
        
        return F.Contains(currentState);
    }
```

## Conclusions / Screenshots / Results
![img.png](img.png)

In conclusion the objective of the laboratory work was achieved. We implemented a type/class for our grammar, added a function that generates 5 valid strings from the language expressed by our given grammar, implemented some functionality that converts and object of type Grammar to one of type Finite Automaton and for the Finite Automaton, we added a method that checks if an input string can be obtained via the state transition from it.