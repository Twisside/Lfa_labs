namespace LFA_lab1;

public class Lab1
{
    static void Main(string[] args)
    {
        // Define the grammar
        HashSet<char> VN = new HashSet<char> { 'S', 'A', 'B', 'C' };
        HashSet<char> VT = new HashSet<char> { 'a', 'b' };
        Dictionary<char, List<string>> P = new Dictionary<char, List<string>> {
            {'S', new List<string>{"aA"}},
            {'A', new List<string>{"bS", "aB"}},
            {'B', new List<string>{"bC", "aB"}},
            {'C', new List<string>{"aA", "b"}}
        };
        char startSymbol = 'S';

        Grammar grammar = new Grammar(VN, VT, P, startSymbol);

        // Generate 5 valid strings
        for (int i = 0; i < 5; i++)
        {
            string generatedString = grammar.GenerateString();
            Console.WriteLine($"Generated String {i + 1}: {generatedString}");
        }

        // Convert Grammar to Finite Automaton
        FiniteAutomaton fa = grammar.ToFiniteAutomaton();

        // Example usage: Check if a string belongs to the language of the FA
        string inputString = "abaabb";
        bool belongs = fa.StringBelongToLanguage(inputString);
        Console.WriteLine($"Does '{inputString}' belong to the language? {belongs}");
    }
}