//Programmer: Joseph Morga
//Red ID: 817281186
//Class: CompE 361
//File Name: Rational.cs
//Instructor: Scott Amack

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RationalInfInt
{
    public class Rational : IComparable
    {
        private int Numerator { get; }
        private int Denominator { get; }

        public Rational()
        {
            //default constructor 
            Numerator = 0;
            Denominator = 0;
        }
        /// <summary>
        ///     It will accept 2 int to initialize Numerator and Denomitator. If the denominator is 0, an
        ///     exception will be thrown. If the numerator and denominator are both 0, the program will 
        ///     assume that 0/0 = 0.
        ///     If an exception is thrown, both values will be initialized to 0.
        /// </summary>
        public Rational(int numerator, int denominator)
        {
            bool exception = false;

            try
            {
                if (denominator == 0 && numerator != 0) throw new DivideByZeroException("Cannot Divide By Zero");
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.ToString());
                exception = true;
            }
            if (!exception && numerator != 0 && denominator != 0)
            {
                int greatestCommonFactor = 0;

                if (Math.Abs(numerator) < Math.Abs(denominator))
                    greatestCommonFactor = getGreatestCommonFactor(Math.Abs(denominator), Math.Abs(numerator), Math.Abs(numerator));
                else
                    greatestCommonFactor = getGreatestCommonFactor(Math.Abs(numerator), Math.Abs(denominator), Math.Abs(denominator));

                if (numerator < 0 && denominator < 0) greatestCommonFactor *= -1;

                this.Numerator = numerator / greatestCommonFactor;
                this.Denominator = denominator / greatestCommonFactor;
            }
            else
            {
                numerator = 0;
                denominator = 0;
            }
        }

        /// <summary>
        ///     The decimal number will obtained for this instance and obj. Then their values will be substracted to obtain
        ///     the difference.
        ///     If the difference is a positive number, a 1 will be returned.
        ///     If the difference is a negative number, a -1 wull be returned.
        ///     Otherwise a 0 will be returned.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            double result = (double)this.Numerator / this.Denominator - (double)((Rational)obj).Numerator / ((Rational)obj).Denominator;

            if (result > 0.0)
                return 1;
            if (result < 0.0)
                return -1;

            return 0;
        }

        /// <summary>
        ///     If the deniminator of this instance and obj are equal, and the numerator of this instance and obj are equal.
        ///     True ill be returned, otherwise a false will be returned.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Rational) || obj == null) return false;

            if (this.Numerator == ((Rational)obj).Numerator &&
               this.Denominator == ((Rational)obj).Denominator)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            //you do not have to implement this
            return base.GetHashCode();
        }

        //Returns the fraction in the form a/b in a string
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static Rational operator +(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Denominator + b.Numerator * a.Denominator,
                a.Denominator * b.Denominator);
        }
        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Rational operator -(Rational a, Rational b)
        {
            return new Rational((a.Numerator * b.Denominator) - (b.Numerator * a.Denominator), a.Denominator * b.Denominator);
        }

        public static Rational operator /(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        /// <summary>
        ///     Returns the decimal representation of this instace
        /// </summary>
        public string RationalToDecimal()
        {
            if (Numerator == 0 && Denominator == 0) return "0.0";

            return $"{(double)Numerator/(double)Denominator}";
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
        private int getGreatestCommonFactor(int bigNumber, int smallNumber, int previousReminder)
        {
            int reminder = bigNumber % smallNumber;

            if (reminder == 0)
                return previousReminder;

            return getGreatestCommonFactor(smallNumber, reminder, reminder);
        }
    }
}
