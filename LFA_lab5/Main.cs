namespace LFA_lab5;

public class LFA_lab5
{
    static void Main(string[] args)
    {
        // Define the grammar
        HashSet<char> vn = new HashSet<char> { 'S', 'A', 'B', 'C', 'D', 'X' };
        HashSet<char> vt = new HashSet<char> { 'a', 'b' };
        Dictionary<char, List<string>> p = new Dictionary<char, List<string>> {
            {'S', new List<string>{"B"}},
            {'A', new List<string>{"aX", "bX"}},
            {'X', new List<string>{"ε", "BX", "b"}},
            {'B', new List<string>{"AXbD"}},
            {'D', new List<string>{"aD", "a"}},
            {'C', new List<string>{"Ca"}}
        };
        char startSymbol = 'S';

        Grammar grammar = new Grammar(vn, vt, p, startSymbol);
        Grammar newGrammar = grammar.Normalize();
        
        Console.WriteLine("New grammar:");
        newGrammar.PrintProd();
    }
}