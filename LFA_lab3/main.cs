namespace LFA_lab3;

public class LFA_lab3
{
    static void Main(string[] args)
    {
        string file =  @"C:\Users\TwisSide\OneDrive - Technical University of Moldova\LFA\LFA\LFA_lab3\textFile.txt";
        string[] input = File.ReadAllLines(file);
        foreach (var put in input)
        {
            Lexer lexer = new Lexer(put);
            List<Token> tokens = lexer.Tokenize();
            
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}