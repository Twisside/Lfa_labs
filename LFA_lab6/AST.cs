namespace LFA_lab6;

public abstract class AstNode
{
    public abstract void Print(int depth);
}

public class IntNode : AstNode
{
    public int Value { get; }

    public IntNode(int value)
    {
        Value = value;
    }

    public override void Print(int depth)
    {
        Console.WriteLine($"{new string(' ', depth * 4)}IntNode: {Value}");
    }
}

public class AddNode : AstNode
{
    public AstNode Left { get; }
    public AstNode Right { get; }

    public AddNode(AstNode left, AstNode right)
    {
        Left = left;
        Right = right;
    }

    public override void Print(int depth)
    {
        Console.WriteLine($"{new string(' ', depth * 4)}AddNode:");
        Left.Print(depth + 1);
        Right.Print(depth + 1);
    }
}

public class MultiplyNode : AstNode
{
    public AstNode Left { get; }
    public AstNode Right { get; }

    public MultiplyNode(AstNode left, AstNode right)
    {
        Left = left;
        Right = right;
    }

    public override void Print(int depth)
    {
        Console.WriteLine($"{new string(' ', depth * 4)}MultiplyNode:");
        Left.Print(depth + 1);
        Right.Print(depth + 1);
    }
}