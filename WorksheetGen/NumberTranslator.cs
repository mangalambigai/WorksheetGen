using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorksheetGen
{
    class NumberTranslator
    {
    
        static string[] numbers ={
                            "one",
                            "two",
                            "three",
                            "four",
                            "five",
                            "six",
                            "seven",
                            "eight",
                            "nine",
                            "ten",
                            "eleven",
                            "twelve",
                            "thirteen",
                            "fourteen",
                            "fifteen",
                            "sixteen",
                            "seventeen",
                            "eighteen",
                            "nineteen",
                            "twenty"
                            };

        static string[] tens ={
                                 "ten",
                                 "twenty",
                                 "thirty",
                                 "forty",
                                 "fifty",
                                 "sixty",
                                 "seventy",
                                 "eighty",
                                 "ninety",
                                 "hundred"
                             };
        public static int ToNumber(string words) 
        {
            int num = 0;
            if (words.Contains("thousand"))
            {
                int loc = words.IndexOf("thousand");
                num += ToNumber(words.Substring(0, loc))*1000;
                words = words.Substring(loc + 1);
            }
            if (words.Contains("hundred"))
            {
                int loc = words.IndexOf("hundred");
                num += ToNumber(words.Substring(0, loc)) * 100;
                words = words.Substring(loc + 1);
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                if (words.Contains(numbers[i]))
                {
                    num += i + 1;
                }
            }

            return num;
        }

        public static string ToWords(int num, bool bRecur=false)
        {
            string words= "";
            if (num > 1000)
            {
                words = ToWords(num / 1000, true)+" thousand";
                num %= 1000;
            }
            if (num > 100)
            {
                words += ToWords(num / 100, true) + " hundred";
                num %= 100;
            }
            if (num > 0 && bRecur==false && words.Length>0)
                words += " and";
            if (num > 20) 
            {
                words += " "+tens[num / 10 - 1];
                num %= 10;
            }
            if (num > 0)
            {
                words += " "+ numbers[num - 1];
            }

            words.Trim();
            return words;
        }
    }
}
