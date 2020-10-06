//Programmer: Joseph Morga
//Red ID: 817281186
//Class: CompE 361
//File Name: InfInt.cs
//Instructor: Scott Amack

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RationalInfInt
{
    public class InfInt : IComparable
    {
        private const int DIGITS = 40; //max size is 40
        private int[] Integer { get; set; } //array containing our infint
        private bool Positive { get; set; } //is it positive

        //default value constructor
        public InfInt()
        {
            Integer = new int[DIGITS];
            Positive = true;
        }

        /// <summary>
        ///     This constructor will take a string that represents a whole number.
        ///     The maximum lenght of the string is 40 if the number is positive and 41 if the number is negative.
        ///     It cannot contain any character that is not a digit, the only exception is that it can contain
        ///     only one negative sign. If the requirements are not met, an exceotion will be thrown and the variable will 
        ///     be set to 0.
        /// </summary>
        public InfInt(string input)
        {
            Positive = true;
            bool exception = false;

            Integer = new int[DIGITS];
            try
            {
                if ((input[0] == '-' && input.Length > DIGITS + 1) || (input[0] != '-' && input.Length > DIGITS))
                    throw new ArgumentOutOfRangeException("Only numbers up to 40 digits allowed.");

                for (int i = input.Length - 1, j = DIGITS - 1; i >= 1; i--, j--)
                    Integer[j] = Int32.Parse(input[i] + "");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.ToString());
                exception = true;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Only Digits Are Allowed");
                exception = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Don't know what happened :(");
                exception = true;
            }

            if (!exception)
            {
                if (input[0] == '-')
                    Positive = false;
                else
                    Integer[DIGITS - input.Length] = Int32.Parse(input[0] + "");
            }
            else   //Default values if an exception was thrown
            {
                Integer = new int[DIGITS];
                Positive = true;
            }

        }

        //freebie add courtesy of professor amack
        public InfInt Add(InfInt addValue)
        {
            InfInt temp = new InfInt();
            if (Positive == addValue.Positive)
            {
                temp = AddPositives(addValue);
            }
            //addvalue is negative
            else if (Positive && (!addValue.Positive))
            {
                addValue.Positive = true;
                if (IsGreaterThan(addValue))
                {
                    temp = SubtractPositives(addValue);
                }
                else
                {
                    temp = addValue.SubtractPositives(this);
                    temp.Positive = false;
                }

                addValue.Positive = false;
            }
            else if (!Positive && addValue.Positive)
            {
                addValue.Positive = false;

                if (IsGreaterThan(addValue))
                {
                    temp = addValue.SubtractPositives(this);
                }
                else
                {
                    temp = SubtractPositives(addValue);
                    temp.Positive = false;
                }

                addValue.Positive = true;
            }
            return temp;
        }

        /// <summary>
        ///     This method will perform a substraction of the instance minus the parameter. The absolute
        ///     values will be used for the substraction.
        /// </summary>
        /// <param name="addValue"> Value to be added to this instance</param>
        /// <returns> The result of the substraction</returns>
        public InfInt SubtractPositives(InfInt addValue)
        {
            var temp = new InfInt();
            int borrow = 0;
            int[] top, bottom;
            int magnitude = compareMagnitude(addValue);

            if (magnitude > 0) { top = Integer; bottom = addValue.Integer; }

            else
            {
                top = addValue.Integer;
                bottom = Integer;
                temp.Positive = false;
            }

            for (int i = DIGITS - 1; i >= 0; i--)
            {
                temp.Integer[i] = top[i] - bottom[i] - borrow;

                if (top[i] < bottom[i])
                {
                    temp.Integer[i] += 10;
                    borrow = 1;
                }
                else
                    borrow = 0;
            }

            return temp;
        }
        /// <summary>
        ///     Returns true if this instance is greater than the parameter. False otherwise.
        ///     It will use the signs and magnitudes of the values to determine the what to return. 
        /// <summary>
        private bool IsGreaterThan(InfInt addValue)
        {
            int magnitude;

            if (this.Positive && !addValue.Positive) return true;

            magnitude = compareMagnitude(addValue);

            if (this.Positive && addValue.Positive)
                if (magnitude > 0) return true;

            if (!this.Positive && !addValue.Positive)
                if (magnitude < 0) return true;

            return false;
        }

        //Given by Instructor
        private InfInt AddPositives(InfInt addValue)
        {
            InfInt temp = new InfInt();
            int carry = 0;
            //iterate the infint
            for (int i = DIGITS - 1; i >= 0; i--)
            {
                temp.Integer[i] = Integer[i] + addValue.Integer[i] + carry;
                //determine if we need to carry the 1 
                if (temp.Integer[i] > 9)
                {
                    temp.Integer[i] %= 10; //reduce to 0-9
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }
            }

            if (!Positive)
            {
                temp.Positive = false;
            }

            return temp;
        }

        //Given by Instructor
        public InfInt Subtract(InfInt subtractValue)
        {
            InfInt temp = new InfInt(); // temporary result

            // subtractValue is negative
            if (Positive && (!subtractValue.Positive))
            {
                temp = AddPositives(subtractValue);
            }
            // this InfInt is negative
            else if (!Positive && subtractValue.Positive)
            {
                temp = AddPositives(subtractValue);
            }
            // at this point, both InfInts have the same sign
            else
            {
                bool isPositive = Positive; // original sign
                bool resultPositive = Positive; // sign of the result

                // set both to positive so we can compare absolute values
                Positive = true;
                subtractValue.Positive = true;

                if (this.IsGreaterThan(subtractValue))
                {
                    temp = this.SubtractPositives(subtractValue);
                }
                else
                {
                    temp = subtractValue.SubtractPositives(this);
                    resultPositive = !isPositive; // flip the sign
                }

                Positive = isPositive;
                subtractValue.Positive = isPositive;
                temp.Positive = resultPositive;
            }

            return temp;
        }

        /// <summary>
        ///     This method will multiply this instance and the parameter. If the product is greater than 40 digits long,
        ///     ans exception will be thrown and a 0 will be returned.
        /// </summary>
        /// 
        /// <returns>The product of this instance and the parameter</returns>
        public InfInt Multiply(InfInt multValue)
        {
            var product = new InfInt();
            bool exception = false;

            try
            {
                if ((this.getPower() + multValue.getPower()) > DIGITS - 1)
                    throw new ArgumentOutOfRangeException("Cannot multiply. The product is greater than 40 digits long.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.ToString());
                exception = true;
            }

            if (!exception)
            {
                if (this.CompareTo(new InfInt()) == 0 || multValue.CompareTo(new InfInt()) == 0)
                    return product;

                bool sign = multValue.Positive;
                multValue.Positive = true;
                   
                string multiplicator = multValue.ToString();
                

                for (int i = 0; i < multiplicator.Length; i++)
                {
                    product = product.Add(new InfInt(getProduct(new InfInt(multiplicator[multiplicator.Length - 1 - i] + ""), i)));
                }

                multValue.Positive = sign;

                product.Positive = false;

                if ((this.Positive && multValue.Positive) || (!this.Positive && !multValue.Positive))
                    product.Positive = true;
            }

            return product;
        }

        /// <summary>
        ///     This is a helper method for the multiply method. It will only multiply this instance and a one digit number,
        ///     then it will return the result. The parameter power will determine how many times the result will be multiplied 
        ///     by 10;
        /// </summary>
        public string getProduct(InfInt oneDigitNumber, int power)
        {
            var temp = new InfInt();
            string product = "";
            bool sign = this.Positive;

            this.Positive = true;


            for (var i = new InfInt(); i.compareMagnitude(oneDigitNumber) != 0; i = i.Add(new InfInt("1")))
                temp = temp.Add(this);

            this.Positive = sign;

            product = temp.ToString();

            for (int i = 0; i < power; i++)
                product = $"{product}{0}";

            return product;
        }

        /// <summary>
        ///     Divides this instance of the class by divValue, if divValue is 0, an exception will be thrown 
        ///     and a 0 will be returned.
        /// </summary>
        public InfInt Divide(InfInt divValue)
        {
            bool thisPositive = this.Positive;
            bool thatPositive = divValue.Positive;

            try
            {
                if (divValue.compareMagnitude(new InfInt()) == 0)
                    throw new DivideByZeroException("Cannot divide by zero");
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.ToString());
            }

            if (divValue.compareMagnitude(new InfInt()) == 0 || this.compareMagnitude(new InfInt()) == 0)
                return new InfInt();
            
            this.Positive = true;
            divValue.Positive = true;

            var division = new InfInt(this.ToString());
            InfInt temp;

            for (temp = new InfInt(); division.CompareTo(divValue) >= 0; temp = temp.Add(new InfInt("1")))
                division = division.Subtract(divValue);

            this.Positive = thisPositive;
            divValue.Positive = thatPositive;

            temp.Positive = false;

            if ((this.Positive && divValue.Positive) || (!this.Positive && !divValue.Positive))
                temp.Positive = true;

            if (temp.compareMagnitude(new InfInt()) == 0) temp.Positive = true;

            return temp;
        }

        /// <summary>
        ///     returns a string containg the value of this instance.
        /// </summary>
        public override string ToString()
        {
            string number = "";
            int startAt = 0;

            while (Integer[startAt] == 0 && startAt < DIGITS - 1) startAt++;

            for (int i = startAt; i < DIGITS; i++)
                number = $"{number}{Integer[i]}";

            if (!Positive)
                number = $"-{number}";

            return number;
        }
        /// <summary>
        ///     Compares this instace against obj. 
        ///     
        ///     return 0 if this instance == obj
        ///     return 1 if this instance goes after obj
        ///     return -2 if this instance goes before obj
        /// </summary>
        public int CompareTo(object obj)
        {
            var temp = (InfInt)obj;
            int magnitude;

            if (this.Positive && !temp.Positive)
                return 1;

            if (!this.Positive && temp.Positive)
                return -1;

            magnitude = this.compareMagnitude(temp);

            if (this.Positive && temp.Positive)
            {
                if (magnitude > 0) return 1;
                if (magnitude < 0) return -1;
            }

            if (magnitude > 0) return -1;
            if (magnitude < 0) return 1;

            return 0;
        }
        
        /// <summary>
        ///     This methods will conpare the absolute value of this instance against obj.
        ///     a 0 will be returned if they have the same magnitude.
        ///     a 1 if this instance has a greater magnitude
        ///     a -1 if this instace has a smaller magnitude
        /// </summary>
        public int compareMagnitude(InfInt obj)
        {
            int i = 0;

            while (this.Integer[i] == obj.Integer[i] && i < DIGITS - 1) { i++; }

            return this.Integer[i] - obj.Integer[i];
        }

        /// <summary>
        ///     It will divide this instance by divisor and return the reminder
        /// </summary>
        public InfInt getReminder(InfInt divisor)
        {
            bool thisPositive = this.Positive;
            bool thatPositive = divisor.Positive;

            if (divisor.compareMagnitude(new InfInt()) == 0) return new InfInt();

            this.Positive = true;
            divisor.Positive = true;

            var division = new InfInt(this.ToString());

            while (division.CompareTo(divisor) >= 0)
                division = division.Subtract(divisor);

            this.Positive = thisPositive;
            divisor.Positive = thatPositive;

            return division;
        }

        /// <summary>
        ///     Helper method for the multipy method. It will find the index of the first non-zero digit in the array
        ///     and then substract DIGITS minus the position to get the power i
        ///     
        ///     example:
        ///     
        ///     indexes in array   0 1 2 3 4 5 6
        ///     values             0 0 2 4 5 0 0
        ///     
        ///     first non-zero number is in index 2, which means its value is 2*10^(i), where i = array.length - index
        /// 
        /// </summary>
        private int getPower()
        {
            int i = 0;

            while (i < DIGITS)
            {
                if (Integer[i] != 0)
                    return DIGITS - 1 - i;
                i++;
            }

            return 0;
        }
    }
}
