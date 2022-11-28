// Dovydas Stanislauskas IFF-2/2;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace K2_Practice_Excercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.GetEncoding(1257);
            const string Data = "Tekstas.txt";
            const string Result = "RedTekstas.txt";
            PerformTask(Data, Result);
        }
        public static bool NoDigits(string line)
        {
            char[] chars = line.ToCharArray();
            foreach(char ch in chars)
            {
                if(Char.IsDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }
        public static int NumberDifferentVowelsInLine(string line)
        {
            int result = 0;
            string vowels = "aąeęėiyouū";
            line = line.ToLower();
            foreach(char ch in vowels.ToCharArray())
            {
                if(line.Contains(ch.ToString()))
                {
                    result++;
                }
            }
            return result;
        }
        public static string FindWord1Line(string line, string punctuation)
        {
            Match longestWord = null;
            int length = 0;
            foreach (Match match in Regex.Matches(line, $"([^{punctuation}]+)([{punctuation}]*)"))
            {                
                if (NumberDifferentVowelsInLine(match.Value)==3 && (longestWord==null || length < match.Groups[1].Value.Length))
                {                   
                        longestWord = match;
                        length = longestWord.Length;                   
                }
            }
            if (longestWord != null)
            {
                return longestWord.Value;
            }
            else
            {
                return "";
            }
        }
        public static string EditLine(string line, string punctuation, string word)
        {
            Match match = Regex.Match(line, $@"(^|[{punctuation}]+)({word})");

            if (match.Success)
            {
                line = match.Groups[2] + line.Remove(line.IndexOf(match.Groups[2].Value), match.Groups[2].Length);
            }
            return line;
        }
        public static string FindWord2Line(string line, string punctuation)
        {
            Match numWord = null;            
            foreach(Match match in Regex.Matches(line, $"([^{punctuation}]+)[{punctuation}]*"))
            {
                if(NoDigits(match.Value))
                {
                    numWord = match;
                }
            }
            if(numWord!=null)
            {
                return numWord.Value;
            }
            else
            {
                return "";
            }            
        }
        public static void PerformTask(string fd, string fr)
        {
            string NoNumbers=String.Empty;
            using (StreamReader sr = new StreamReader(fd))
            {
                string line;
                string punctuation = sr.ReadLine();
                using (StreamWriter writer = new StreamWriter(fr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string longestWord = FindWord1Line(line, punctuation);
                        string editedLine = EditLine(line, punctuation, longestWord);
                        writer.WriteLine(editedLine);
                        string noNumberWord = FindWord2Line(editedLine, punctuation);
                        noNumberWord = Regex.Match(noNumberWord, @"\w+").Value;
                        NoNumbers += $"{noNumberWord}\r\n";
                    }
                    writer.WriteLine();
                    writer.WriteLine(NoNumbers);
                }
            }
        }
    }
}
