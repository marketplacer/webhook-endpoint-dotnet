using System.Security.Cryptography;
using System.Text;

public static class HmacHash
{
    public static string GenerateHmacHash(string key, string body)
    {
        string hash = String.Empty;
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] bodyBytes = Encoding.UTF8.GetBytes(body);

        using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
        {
            // Compute the hash
            byte[] hashBytes = hmac.ComputeHash(bodyBytes);

            hash = Convert.ToBase64String(hashBytes);
        }
        return hash;
        
    }
}