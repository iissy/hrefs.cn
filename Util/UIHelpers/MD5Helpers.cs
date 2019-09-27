using System.Security.Cryptography;
using System.Text;

namespace ASY.Hrefs.Util.UIHelpers
{
    public static class MD5Helpers
    {
        public static string ComputeHash(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            password = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                password = password + s[i].ToString("x2");
            }

            return password;
        }
    }
}