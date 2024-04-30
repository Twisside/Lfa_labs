namespace LFA_lab6;

public class Lab6
{
    public static void Main(string[] args)
    {
        string file =  @"C:\Users\TwisSide\OneDrive - Technical University of Moldova\LFA\LFA\LFA_lab6\textFile.txt";
        string[] input = File.ReadAllLines(file);
        foreach (var put in input)
        {
            Lexer lexer = new Lexer(put);
            var parser = new Parser(lexer);
            var ast = parser.Parse();
            
            Console.WriteLine("Abstract Syntax Tree:");
            ast.Print(0);
        }
    } 
}