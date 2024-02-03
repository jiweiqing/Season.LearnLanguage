using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Domain
{
    public static class EncryptHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            string result = string.Empty;
            //32位大写
            using (var md5 = MD5.Create())
            {
                var arr = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(arr);
                result = strResult.Replace("-", "");
            }
            return result;
        }
    }
}
