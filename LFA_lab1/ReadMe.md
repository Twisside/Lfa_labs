## Laboratory work n1

### Objectives
    a. Implement a type/class for your grammar;

    b. Add one function that would generate 5 valid strings from the language expressed by your given grammar;

    c. Implement some functionality that would convert and object of type Grammar to one of type Finite Automaton;

    d. For the Finite Automaton, please add a method that checks if an input string can be obtained via the state transition from it;

### Description
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