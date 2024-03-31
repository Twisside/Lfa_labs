using System.Text.RegularExpressions;

namespace LFA_lab4;

public class Lexer
{
    private readonly string _input;
    private readonly List<Token> _tokens;
    private int _position;

    private static readonly Dictionary<Regex, TokenType> TokenRegexes = new Dictionary<Regex, TokenType>
    {
        {new Regex(@"[a-zA-Z_]\w+"), TokenType.Identifier},
        {new Regex(@"^\("), TokenType.LeftParenthesis},
        {new Regex(@"^\)"), TokenType.RightParenthesis},
        {new Regex(@"^\|"), TokenType.Or},
        {new Regex(@"\b^\d+\b"), TokenType.Power},
        {new Regex(@"^\*"), TokenType.Star},
        {new Regex(@"^\+"), TokenType.Plus}
        
    };

    public Lexer(string input)
    {
        _input = input;
        _tokens = new List<Token>();
        _position = 0;
    }

    public List<Token> Tokenize()
    {
        while (_position < _input.Length)
        {
            bool matchFound = false;

            foreach (var kvp in TokenRegexes)
            {
                var regex = kvp.Key;
                var tokenType = kvp.Value;

                var match = regex.Match(_input.Substring(_position));

                if (match.Success)
                {
                    _tokens.Add(new Token(tokenType, match.Value));
                    _position += match.Length;
                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)
            {
                // If no match found, it's an unknown token
                _tokens.Add(new Token(TokenType.Unknown, _input[_position].ToString()));
                _position++;
            }
        }

        return _tokens;
    }
}