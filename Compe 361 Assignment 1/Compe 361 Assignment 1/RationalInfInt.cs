//Programmer: Joseph Morga
//Red ID: 817281186
//Class: CompE 361
//File Name: RationalInfInt.cs
//Instructor: Scott Amack

//This class is based on the Rational class but the numerator and denominator are
//InfInt objects. A few changes were made to do operations with InfInt objects but
//the algorithm should be almost the same as the Rational class.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RationalInfInt
{
    class RationalInfInt : IComparable
    {
        private InfInt Numerator { get; }
        private InfInt Denominator { get; }

        public RationalInfInt()
        {
            //default constructor 
            Numerator = new InfInt();
            Denominator = new InfInt();
        }
        /// <summary>
        ///     It will accept 2 InfInts to initialize Numerator and Denomitator. If the denominator is 0, an
        ///     exception will be thrown. If the numerator and denominator are both 0, the program will 
        ///     assume that 0/0 = 0.
        ///     If an exception is thrown, both values will be initialized to 0.
        /// </summary>
        public RationalInfInt(InfInt numerator, InfInt denominator)
        {
            bool exception = false;

            try
            {
                if (denominator.compareMagnitude(new InfInt()) == 0 && numerator.compareMagnitude(new InfInt()) != 0)
                    throw new DivideByZeroException("Cannot Divide By Zero");
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.ToString());
                exception = true;
            }

            if (!exception && numerator.CompareTo(new InfInt()) != 0 && denominator.CompareTo(new InfInt()) != 0)
            {
                var greatestCommonFactor = new InfInt();

                if (numerator.compareMagnitude(denominator) < 0)
                    greatestCommonFactor = getGreatestCommonFactor(denominator, numerator, numerator);
                else
                    greatestCommonFactor = getGreatestCommonFactor(numerator, denominator, denominator);

                if (numerator.CompareTo(new InfInt()) < 0 && denominator.CompareTo(new InfInt()) < 0)
                    greatestCommonFactor = greatestCommonFactor.Multiply(new InfInt("-1"));

                this.Numerator = numerator.Divide(greatestCommonFactor);
                this.Denominator = denominator.Divide(greatestCommonFactor);
            }
            else
            {
                Numerator = new InfInt();
                Denominator = new InfInt();
            }
        }

        /// <summary>
        ///     The values will be compared using the Equals method to determine if they are the same. It will return 0 if true.
        ///     Otherwise, their values will be substracted to obtain the difference.
        ///     If the difference is a positive number, a 1 will be returned.
        ///     Otherwise a -1 will be returned.
        /// </summary>
        public int CompareTo(object obj)
        {
            if(obj == null) return 1;

            if (this.Equals(obj)) return 0;

            RationalInfInt result = this - (RationalInfInt)obj;

            if (result.Denominator.CompareTo(new InfInt()) > 0 && result.Numerator.CompareTo(new InfInt()) > 0)
                return 1;

            return -1;
        }

        /// <summary>
        ///     It will use the Equals method of the ToStrings of this instance to compare agains obj
        ///     and return the result.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is RationalInfInt) || obj == null) return false;

            return this.ToString().Equals(((RationalInfInt)obj).ToString());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //Returns the fraction in the form a/b in a string
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static RationalInfInt operator +(RationalInfInt a, RationalInfInt b)
        {
            return new RationalInfInt( (a.Numerator.Multiply(b.Denominator)).Add(b.Numerator.Multiply(a.Denominator))
                , a.Denominator.Multiply(b.Denominator));
        }

        public static RationalInfInt operator *(RationalInfInt a, RationalInfInt b)
        {
            return new RationalInfInt(a.Numerator.Multiply(b.Numerator), a.Denominator.Multiply(b.Denominator));
        }

        public static RationalInfInt operator -(RationalInfInt a, RationalInfInt b)
        {
            return new RationalInfInt( (a.Numerator.Multiply(b.Denominator)).Subtract(b.Numerator.Multiply(a.Denominator)),
                a.Denominator.Multiply(b.Denominator));
        }

        public static RationalInfInt operator /(RationalInfInt a, RationalInfInt b)
        {
            return new RationalInfInt(a.Numerator.Multiply(b.Denominator), a.Denominator.Multiply(b.Numerator));
        }

        /// <summary>
        ///     Returns the decimal representation of this instace
        /// </summary>
        public string RationalToDecimal()
        {
            if (Numerator.CompareTo(new InfInt()) == 0 && Denominator.CompareTo(new InfInt()) == 0)
                return "0.0";

            string sign = "";

            if (Numerator.Divide(Denominator).CompareTo(new InfInt()) == 0)
                sign = "-";

            return $"{sign}{Numerator.Divide(Denominator)}.{this.getFractionalPart()}";
        }

        /// <summary>
        ///     This method uses the euclidian algorithm to find the greatest common factor of two numbers.
        ///     The algorithm was based on the following video
        ///     
        ///     https://www.youtube.com/watch?v=JUzYl1TYMcU&t=19s
        /// 
        /// </summary>
        /// <param name="bigNumber">This parameter has to have a value greater than or equal to smallNumber </param>
        /// <param name="smallNumber">This parameter has to have a value less than or equal to bigNumber</param>
        /// <param name="previousReminder">Used to save the value of the previous reminder. When this methods is not called
        ///       by itself, this parameter has to have the value of smallNumber</param>
        /// <returns>An int with the greatest common factor of 2 numbers. </returns>
        private InfInt getGreatestCommonFactor(InfInt bigNumber, InfInt smallNumber, InfInt previousReminder)
        {
            InfInt reminder = bigNumber.getReminder(smallNumber);

            if (reminder.compareMagnitude(new InfInt()) == 0)
                return previousReminder;

            return getGreatestCommonFactor(smallNumber, reminder, reminder);
        }

        /// <summary>
        ///    Helper methdo for the RationalToDecimal method. This method will return a string containing 
        ///    the numbers after the decinal point of this instance.
        /// </summary>
        private string getFractionalPart()
        {
            InfInt reminder = this.Numerator.getReminder(this.Denominator);
            string fractional = "";

            for (int i = 0; i < 40 && reminder.CompareTo(new InfInt()) != 0; i++)
            {
                reminder = reminder.Multiply(new InfInt("10"));

                fractional = $"{fractional}{reminder.Divide(this.Denominator)}";

                reminder = reminder.getReminder(this.Denominator);
            }

            if (fractional[0] == '-')
                return fractional.Substring(1);

            return fractional;
        }
    }
}