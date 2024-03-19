﻿# Lexer Scanner

### Course: Formal Languages & Finite Automata
### Author: Timciuc Ana-Maria

----

## Theory
Lexical tokenization is the process of converting a text into semantically or syntactically relevant lexical tokens that fall into categories established by a "lexer" software. Natural language categories include nouns, verbs, adjectives, punctuation, and so on. In a computer language, the categories are identifiers, operators, grouping symbols, and data types.

## Objectives:

1. Understand what lexical analysis is.
2. Get familiar with the inner workings of a lexer/scanner/tokenizer.
3. Implement a sample lexer and show how it works.


## Implementation description

1. Token Types Enumeration (TokenType):

Defines an enumeration to represent the different types of tokens that the lexer will recognize, 
such as numbers, arithmetic operators, keywords (e.g., "int", "float"), identifiers (variable names), and special characters (e.g., semicolon).

```csharp
public enum TokenType // Tocken types enumeration
{
    Identifier,
    Integer,
    Float,
    Plus,
    Minus,
    Multiply,
    Divide,
    LeftParenthesis,
    RightParenthesis,
    Semicolon,
    Equal,
    Int,
    FloatKeyword,
    Unknown
}
```

2. Token Class:

Represents a token with properties for its type and value. This class is used to store individual tokens generated by the lexer during the tokenization process.

3. Lexer Class:

* Implements the lexer, which scans through the input string and generates tokens based on predefined rules and regular expressions.
* Contains a dictionary (TokenRegexes) mapping regular expressions to token types. It iterates through these regexes to find matches in the input string and generates tokens accordingly.
```csharp
private static readonly Dictionary<Regex, TokenType> TokenRegexes = new Dictionary<Regex, TokenType>
    {   // creation of regexes for each token type
        {new Regex(@"\bint\b"), TokenType.Int},
        {new Regex(@"\bfloat\b"), TokenType.FloatKeyword},
        {new Regex(@"[a-zA-Z_]\w+"), TokenType.Identifier},
        {new Regex(@"^\d+(\.\d+)?"), TokenType.Float},
        {new Regex(@"^\d+"), TokenType.Integer},
        {new Regex(@"^\+"), TokenType.Plus},
        {new Regex(@"^-"), TokenType.Minus},
        {new Regex(@"^\*"), TokenType.Multiply},
        {new Regex(@"^/"), TokenType.Divide},
        {new Regex(@"^\("), TokenType.LeftParenthesis},
        {new Regex(@"^\)"), TokenType.RightParenthesis},
        {new Regex(@"^;"), TokenType.Semicolon},
        {new Regex(@"^="), TokenType.Equal}
        
    };

```
* Provides a Tokenize method to tokenize the input string and return a list of tokens.

```csharp
public List<Token> Tokenize()
    {   
        while (_position < _input.Length)   // iterate through the input string
        {
            bool matchFound = false;

            foreach (var kvp in TokenRegexes)   // iterate through the regexes
            {
                var regex = kvp.Key;
                var tokenType = kvp.Value;

                var match = regex.Match(_input.Substring(_position));   // match the regex with the input string

                if (match.Success)  // if a match is found
                {
                    _tokens.Add(new Token(tokenType, match.Value));
                    _position += match.Length;
                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)    // if no match found
            {
                // If no match found, it's an unknown token
                _tokens.Add(new Token(TokenType.Unknown, _input[_position].ToString()));
                _position++;
            }
        }

        return _tokens;
    }
```


## Conclusions 


In conclusion, the implemented lexer successfully achieves the objectives of understanding and implementing the inner workings of a lexer/tokenizer in the context of programming languages. By utilizing regular expressions and predefined token types, the lexer accurately identifies and tokenizes various elements within a given input string, including numbers, arithmetic operators, keywords, and identifiers (variable names). This work demonstrates the fundamental principles of lexical analysis and provides a foundation for building more complex compilers or interpreters in the future.

## Results(example)
* input: 
```
int a = 3;
float num = 3.5;

a + num * a;

5 + 7 * (2 + 3);
8 * 6 / 2 + 3 * 4;
9 + 8 * 7 + 6 / 5;
7 * 2 + 7 / 4 + 5; 
3.2 + 4.5 * 6.7 / 2.3;
```
* output:
```
Type: Int, Value: int
Type: Unknown, Value:
Type: Unknown, Value: a
Type: Unknown, Value:
Type: Equal, Value: =
Type: Unknown, Value:
Type: Float, Value: 3
Type: Semicolon, Value: ;
Type: FloatKeyword, Value: float
Type: Identifier, Value: num
Type: Unknown, Value: m
Type: Unknown, Value:
Type: Equal, Value: =
Type: Unknown, Value:
Type: Float, Value: 3.5
Type: Semicolon, Value: ;
Type: Unknown, Value: a
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Identifier, Value: num
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Unknown, Value: a
Type: Semicolon, Value: ;
Type: Float, Value: 5
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 7
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: LeftParenthesis, Value: (
Type: Float, Value: 2
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 3
Type: RightParenthesis, Value: )
Type: Semicolon, Value: ;
Type: Float, Value: 8
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Float, Value: 6
Type: Unknown, Value:
Type: Divide, Value: /
Type: Unknown, Value:
Type: Float, Value: 2
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 3
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Float, Value: 4
Type: Semicolon, Value: ;
Type: Float, Value: 9
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 8
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Float, Value: 7
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 6
Type: Unknown, Value:
Type: Divide, Value: /
Type: Unknown, Value:
Type: Float, Value: 5
Type: Semicolon, Value: ;
Type: Float, Value: 7
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Float, Value: 2
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 7
Type: Unknown, Value:
Type: Divide, Value: /
Type: Unknown, Value:
Type: Float, Value: 4
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 5
Type: Semicolon, Value: ;
Type: Unknown, Value:
Type: Float, Value: 3.2
Type: Unknown, Value:
Type: Plus, Value: +
Type: Unknown, Value:
Type: Float, Value: 4.5
Type: Unknown, Value:
Type: Multiply, Value: *
Type: Unknown, Value:
Type: Float, Value: 6.7
Type: Unknown, Value:
Type: Divide, Value: /
Type: Unknown, Value:
Type: Float, Value: 2.3
Type: Semicolon, Value: ;
```