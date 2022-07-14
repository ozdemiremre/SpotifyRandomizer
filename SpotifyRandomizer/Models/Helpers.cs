using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpotifyRandomizer.Models
{
    public static class Helpers
    {
        public static string GenerateRandomString(int length)
        {
            string text = string.Empty;
            string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < length; i++)
            {
                text += possible[random.Next(possible.Length)];
            }

            return text;
        }

        public static void ExecuteOnUIThread(Action actionToExecute)
        {
            Application.Current.Dispatcher.Dispatch(actionToExecute);
        }

        public static string Base64Encode(string text)
        {
            var textInBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textInBytes);
        }

        public static string Base64Decode(string base64Data)
        {
            var base64DataInBytes = System.Convert.FromBase64String(base64Data);
            return System.Text.Encoding.UTF8.GetString(base64DataInBytes);
        }

        public static string ComputeSHA256(string rawData)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                byte[] byteVal = RandomNumberGenerator.GetBytes(1);
                int k = (byteVal[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
