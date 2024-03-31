using Fare;

namespace LFA_lab4;

public class LFA_lab4
{
    static void Main(string[] args)
    {
        // there was no limitation on using 3rd party libraries
        // so
        // until i figure out how to make it all by myself
        // i will use xeger :)
        string[] regexes = new[]
        {
            "(S|T)(U|V)W*Y+24",
            "L(M|N)OOOP*Q(2|3)",
            "R*S(T|U|V)W(X|Y|Z)(X|Y|Z)"
        };
        foreach (string regex in regexes)
        {
            Xeger xeger = new Xeger(regex, new Random());
            for(var i=0; i<10; i++)
                Console.WriteLine($"Input text matching regex: '{regex}' is: '{xeger.Generate()}'");

        }
    }
}