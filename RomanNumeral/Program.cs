using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RomanNumeral
{  
    class RomanNumeral
    {
        public string StringRepresentation { get; private set; }
        public int NumberRepresentation { get; private set; }

        public string TextRepresentation 
        { get
            {
                List<int[]> numberGroups = SplitNumberInGroups(NumberRepresentation, 1000);
                string text = "";
                foreach (int[] numberGroup in numberGroups)
                {
                    text += (GetTextFromThreeDigitNumber(numberGroup[0]) != "")? GetTextFromThreeDigitNumber(numberGroup[0])+ " " + Program.CommaSeperatorDic[numberGroup[1]] + " " : "";
                }
                return text;
            } 
        }
        public RomanNumeral(int intInput)
        {
            NumberRepresentation = intInput; ;
            if (intInput >= 4000 || intInput <= 0) throw new ArgumentOutOfRangeException(intInput.ToString());

            List<int[]> numberGroups = SplitNumberInGroups(intInput, 10);

            string romanString = "";

            foreach (int[] numberGroup in numberGroups)
            {
                romanString += GetRomanStringOutOfGroup(numberGroup);
            }
            StringRepresentation = romanString;
        }

        public RomanNumeral(string strInput)
        {
            StringRepresentation = strInput;
            Regex romanRg = new Regex(@"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");//Cheated, I'm not gonna write a regex for roman numerals
            if (romanRg.Matches(strInput).Count == 0) throw new Exception();  
            int numberRepresentation = 0;
            for (int i = 0; i < strInput.Length; i++)
            {
                int thisRomanNumber = Program.RomanNumeral[strInput[i].ToString()];
                int nextRomanNumber = (i == strInput.Length - 1) ? 0 : Program.RomanNumeral[strInput[i + 1].ToString()];

                if (thisRomanNumber >= nextRomanNumber) numberRepresentation+= thisRomanNumber;
                else
                {
                    numberRepresentation += nextRomanNumber - thisRomanNumber;
                    i++;
                }
            }
            NumberRepresentation = numberRepresentation;
        }

        private string GetTextFromThreeDigitNumber (int number)
        {
            string text = "";

            List<int[]> splittedNumber = SplitNumberInGroups(number, 10);
            splittedNumber.Reverse();
            
            if(splittedNumber.Count == 3)
            {
                text += Program.NumberNames[splittedNumber[2][0]] + " hundred ";
                if (splittedNumber[1][0] == 0 && splittedNumber[0][0] == 0) return text;
            }
            if(splittedNumber.Count >= 2)
            {
                if(splittedNumber[1][0] == 1)
                {
                    int no = splittedNumber[1][0] * 10 + splittedNumber[0][0];
                    text += text == ""? Program.NumberNames[no] + " " : "and " + Program.NumberNames[no] + " ";
                    return text;
                }
                else
                {
                    int no = splittedNumber[1][0] * 10;
                    text += text == ""? Program.NumberNames[no] + " ": "and " + Program.NumberNames[no] + " ";
                }
            }
            if (splittedNumber.Count >= 1)
                text += Program.NumberNames[splittedNumber[0][0]] + " ";
            return text;
        }

        private List<int[]> SplitNumberInGroups (int input, int baseNo)
        { 
            List<int[]> result = new List<int[]>();

            int number = input;
            int counter = 0;

            while(number > 0)
            {
                int remainder = number % baseNo;
                number /= baseNo;
                result.Add(new int[] { remainder, (int)Math.Pow(baseNo, counter) });
                counter++;
            }
            result.Reverse();
            return result;
        }

        private string GetRomanStringOutOfGroup (int[] group)
        {
            int thisIndex = Program.RomanNumeral.Values.ToList().IndexOf(group[1]);
            int number = group[0];

            string romanString = Program.RomanReference[number];
            romanString = romanString.Replace("O", Program.RomanNumeral.Keys.ElementAt(thisIndex));
            if (thisIndex > 0) // checks to see if it's the biggest Roman numeral or not, if not then can proceed   
                romanString = romanString.Replace("P", Program.RomanNumeral.Keys.ElementAt(thisIndex - 1)).Replace("Q", Program.RomanNumeral.Keys.ElementAt(thisIndex - 2));

            return romanString;
        }
    }
    public class Program
    {
        public static readonly Dictionary<string, int> RomanNumeral = new Dictionary<string, int>
        {   //Add new Roman numerals here 
            {"M", 1000 },
            {"D", 500 },
            {"C", 100 },
            {"L", 50 },
            {"X", 10 },
            {"V", 5},
            {"I", 1}
        };

        public static readonly Dictionary<int, string> RomanReference = new Dictionary<int, string>
        {
            {0, "" },
            {1, "O" },
            {2, "OO" },
            {3, "OOO" },
            {4, "OP" },
            {5, "P" },
            {6, "PO" },
            {7, "POO" },
            {8, "POOO" },
            {9, "OQ" }
        };
        public static readonly Dictionary<int, string> CommaSeperatorDic = new Dictionary<int, string>
        {
            {1, "" },
            {1000, "thousand" },
            {1000000, "million" },
            {1000000000, "billion" } //Additional needs int to be double, but doesn't matter for this challenge anyways
        };
        public static readonly Dictionary<int, string> NumberNames = new Dictionary<int, string>
        {
            {0, "" },
            {1, "one" },
            {2, "two" },
            {3, "three" },
            {4, "four" },
            {5, "five" },
            {6, "six" },
            {7, "seven" },
            {8, "eight" },
            {9, "nine" },
            {10, "ten" },
            {11, "eleven" },
            {12, "twelve" },
            {13, "thirteen" },
            {14, "fourteen" },
            {15, "fifteen" },
            {16, "sixteen" },
            {17, "seventeen" },
            {18, "eighteen" },
            {19, "nineteen" },
            {20, "twenty" },
            {30, "thirty" },
            {40, "fourty" },
            {50, "fifty" },
            {60, "sixty" },
            {70, "seventy" },
            {80, "eighty" },
            {90, "ninety" }
        };
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number or a Roman numeral string: ");
            string input = Console.ReadLine();
            RomanNumeral romanNum;
            if (int.TryParse(input, out var number))
            {
                romanNum = new RomanNumeral(number);
            }
            else
            {
                if (!input.Except(string.Join("",RomanNumeral.Keys.ToArray())).Any())
                    romanNum = new RomanNumeral(input);
                else
                {
                    Console.WriteLine("Invalid input entered");
                    return;
                };
            }

            Console.WriteLine(romanNum.StringRepresentation);
            Console.WriteLine(romanNum.NumberRepresentation);
            Console.WriteLine(romanNum.TextRepresentation);
        }
    }
}
