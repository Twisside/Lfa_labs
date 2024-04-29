namespace LFA_lab6;

public class Parser
{
    private Lexer lexer;
    private Token currentToken;

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
        currentToken = lexer.GetNextToken();
    }

    private void Eat(TokenType tokenType)
    {
        if (currentToken.Type == tokenType)
        {
            currentToken = lexer.GetNextToken();
        }
        else
        {
            throw new Exception($"Expected token type {tokenType}, but got {currentToken.Type} instead.");
        }
    }

    public AstNode Parse()
    {
        return Expr();
    }

    private AstNode Expr()
    {
        var node = Term();

        while (currentToken.Type == TokenType.Plus || currentToken.Type == TokenType.Minus)
        {
            var token = currentToken;
            if (token.Type == TokenType.Plus)
            {
                Eat(TokenType.Plus);
                node = new AddNode(node, Term());
            }
            else if (token.Type == TokenType.Minus)
            {
                Eat(TokenType.Minus);
                node = new AddNode(node, new IntNode(-1 * ((IntNode)Term()).Value));
            }
        }

        return node;
    }

    private AstNode Term()
    {
        var node = Factor();

        while (currentToken.Type == TokenType.Multiply || currentToken.Type == TokenType.Divide)
        {
            var token = currentToken;
            if (token.Type == TokenType.Multiply)
            {
                Eat(TokenType.Multiply);
                node = new MultiplyNode(node, Factor());
            }
            else if (token.Type == TokenType.Divide)
            {
                Eat(TokenType.Divide);
                node = new MultiplyNode(node, new IntNode(1 / ((IntNode)Factor()).Value));
            }
        }

        return node;
    }

    private AstNode Factor()
    {
        var token = currentToken;
        if (token.Type == TokenType.Integer)
        {
            Eat(TokenType.Integer);
            return new IntNode(int.Parse(token.Value));
        }
        else if (token.Type == TokenType.LeftParenthesis)
        {
            Eat(TokenType.LeftParenthesis);
            var node = Expr();
            Eat(TokenType.RightParenthesis);
            return node;
        }
        else
        {
            throw new Exception($"Unexpected token: {token.Type}");
        }
    }
}
