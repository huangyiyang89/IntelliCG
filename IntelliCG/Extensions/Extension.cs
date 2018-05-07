using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace IntelliCG.Extensions
{
    public static class Extensions
    {
        public static string To62String(this int num)
        {

            var temp = num;
            var sb = new StringBuilder();
            var charArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


            if (temp < 63)
            {
                return charArray[temp].ToString();
            }

            do
            {
                long remainder = temp % 62;
                sb.Append(charArray[remainder]);
                temp = temp / 62;
            }
            while (temp > 62 - 1);
            sb.Append(charArray[temp]);
            var chars = sb.ToString().ToCharArray();
            Array.Reverse(chars);
            var result = new string(chars);
            return result;


        }
    }
}
