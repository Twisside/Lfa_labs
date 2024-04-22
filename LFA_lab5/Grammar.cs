namespace LFA_lab5;
using System.Text;


public class Grammar
{
    public HashSet<char> VN { get; set; }
    public HashSet<char> VT { get; set; }
    public Dictionary<char, List<string>> P { get; set; }
    public char S { get; set; }


    public Grammar(HashSet<char> vn, HashSet<char> vt, Dictionary<char, List<string>> p, char s)
    {
        VN = vn;
        VT = vt;
        P = p;
        S = s;
    }

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

    public Grammar Normalize()
    {
        GrammarNormalizer normalizer = new GrammarNormalizer();
        return normalizer.Normalization(this);
    }

    public void PrintProd()
    {
        foreach (var prod in P)
        {
            Console.Write(prod.Key + " -> ");
            foreach (var element in prod.Value)
            {
                Console.Write(element + " | ");
            }
            Console.WriteLine();
        }
    }
    
}