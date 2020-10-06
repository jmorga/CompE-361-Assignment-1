using System;

namespace RationalInfInt
{
    class Program
    {
        static void Main(string[] args)
        {
            //Testing
            InfInt number1 = new InfInt("-40000");
            InfInt number2 = new InfInt("80000");

            InfInt number3 = new InfInt("10000");
            InfInt number4 = new InfInt("30000");

            RationalInfInt rational1 = new RationalInfInt(number1, number2);
            RationalInfInt rational2 = new RationalInfInt(number3, number4);

            Console.WriteLine($"Rational 1 = {rational1}");
            Console.WriteLine($"Rational 2 = {rational2}\n\n");
            Console.WriteLine($"{rational1} * {rational2} = {rational1 * rational2}\n");
            Console.WriteLine($"{rational1} / {rational2} = {rational1 / rational2}\n");
            Console.WriteLine($"{rational1} + {rational2} = {rational1 + rational2}\n");
            Console.WriteLine($"{rational1} - {rational2} = {rational1 - rational2}\n");
        }
    }
}
