using System.Text;

namespace Eucyon_Tribes.Models
{
    public static class Hash
    {
        private const string Key = "superSecureSecredKey!";
        
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string DecryptPassword(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            return result[..^Key.Length];
        }
    }
}
