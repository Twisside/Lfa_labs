# Lexer Scanner

### Course: Formal Languages & Finite Automata
### Author: Timciuc Ana-Maria

----

## Theory
A regular expression is a sequence of characters that specifies a match pattern in text. Typically, string-searching algorithms employ these patterns for input validation or to perform "find" or "find and replace" operations on strings.
## Objectives:

1. Write a code that will generate valid combinations of symbols conform given regular expressions (examples will be shown).
2. b. In case you have an example, where symbol may be written undefined number of times, take a limit of 5 times (to evade generation of extremely long combinations).


## Implementation description

1. Using Fare library

The Fare library has the thing i was trying to implement, that being the xeger, the opposite of a regex. If the Regex class just checks if a string is a match for the given regex, well xeger generates that string for the regex. There where no limitations for using external libraries sooo, yeah.
As you can see in the second regular expression i used 'OOO' instead of 'O^3', because the xeger and the regex libraries do not support this kind of syntax, so the specific number of elements needs to be typed manually.
```csharp
static void Main(string[] args)
    {
        // there was no limitation on using 3rd party libraries
        // so
        // until i figure out how to make it all by myself
        // i will use xeger :)
        string[] regexes = new[]
        {
            // writing theexpresions as strings of input for the regex
            "(S|T)(U|V)W*Y+24",
            "L(M|N)OOOP*Q(2|3)",
            "R*S(T|U|V)W(X|Y|Z)(X|Y|Z)"
        };
        // the for loop that will go through the regexes and generate the strings 
        foreach (string regex in regexes)
        {
            Xeger xeger = new Xeger(regex, new Random());
            for(var i=0; i<10; i++)
                Console.WriteLine($"Input text matching regex: '{regex}' is: '{xeger.Generate()}'");

        }
    }
```

2. Xeger Class:

When you create an instance of the Xeger class, you provide it with a regular expression. This regular expression is then used to create an automaton representation of the regex. An automaton is essentially a state machine that represents all possible sequences of characters that match the regular expression.

Traversal of Automaton: The Generate() method starts at the initial state of the automaton and traverses it randomly, one transition at a time, until it reaches an accepting state. At each state, it randomly selects a transition to follow based on the sorted list of transitions from that state.

Appending Characters: As it traverses the automaton, it appends characters to a StringBuilder object. The characters are chosen randomly based on the transitions from each state. This ensures that the generated string satisfies the conditions specified by the regular expression.
```csharp
private void Generate(StringBuilder builder, State state) // input the regex and the automaton for it
    {
      IList<Transition> sortedTransitions = state.GetSortedTransitions(true);
      if (sortedTransitions.Count == 0)
      {
        if (!state.Accept)
          throw new InvalidOperationException(nameof (state));
      }
      else
      {// generating the string itself, and the magic i cant comprehend
        int randomInt = Xeger.GetRandomInt(0, state.Accept ? sortedTransitions.Count : sortedTransitions.Count - 1, this.random);
        if (state.Accept && randomInt == 0)
          return;
        Transition transition = sortedTransitions[randomInt - (state.Accept ? 1 : 0)];
        this.AppendChoice(builder, transition);
        this.Generate(builder, transition.To);
      }
    }
```

3. Lexer Class:

I tried doing it on my own, got the lexer for the strings and was ready to build a generator. here is the lexer prepared: 
```csharp
private static readonly Dictionary<Regex, TokenType> TokenRegexes = new Dictionary<Regex, TokenType>
    {   // creation of regexes for each token type
        {new Regex(@"[a-zA-Z_]\w+"), TokenType.Identifier},
        {new Regex(@"^\("), TokenType.LeftParenthesis},
        {new Regex(@"^\)"), TokenType.RightParenthesis},
        {new Regex(@"^\|"), TokenType.Or},
        {new Regex(@"\b^\d+\b"), TokenType.Power},
        {new Regex(@"^\*"), TokenType.Star},
        {new Regex(@"^\+"), TokenType.Plus}
        
    };

```
Then I realized I can use the technique for creating tokens to just input the expression in there, but the regex was not enough.

## Conclusions


In conclusion, I did what I was asked to do. Did I do it write? Hopefully. I used the xeger library to generate the strings that match the regexes. I tried to do it on my own, but the regex was not enough for me to generate the strings. I will try to do it on my own in the future, but for now, I am happy with the result. The xeger did the job and I provided the explanation of how it works. 
## Results(example)
* input:
```
Variant 4:
"(S|T)(U|V)W*Y+24",
"L(M|N)OOOP*Q(2|3)",
"R*S(T|U|V)W(X|Y|Z)(X|Y|Z)"
```
* output:
```
'(S|T)(U|V)W*Y+24' is: 'SUWWWWWWWYY24'
'(S|T)(U|V)W*Y+24' is: 'SVWWYY24'
'(S|T)(U|V)W*Y+24' is: 'TVYYY24'
'(S|T)(U|V)W*Y+24' is: 'SUWWWYYY24'
'(S|T)(U|V)W*Y+24' is: 'TUWY24'
'(S|T)(U|V)W*Y+24' is: 'SUWWWY24'
'(S|T)(U|V)W*Y+24' is: 'TVWY24'
'(S|T)(U|V)W*Y+24' is: 'SUY24'
'(S|T)(U|V)W*Y+24' is: 'TVYYY24'
'(S|T)(U|V)W*Y+24' is: 'SVY24'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOPPPQ2'
'L(M|N)OOOP*Q(2|3)' is: 'LMOOOQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOPQ2'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOQ2'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LNOOOPPQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LMOOOQ3'
'L(M|N)OOOP*Q(2|3)' is: 'LMOOOPQ2'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'STWXZ'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'SUWYZ'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'RRRSTWYZ'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'SVWXX'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'STWXX'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'RSVWZX'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'SUWYY'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'SVWXX'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'RRSTWXX'
'R*S(T|U|V)W(X|Y|Z)(X|Y|Z)' is: 'RRRSTWXZ'
```
