using System.Security.Cryptography;

//Generate a secure key with the required size for HMAC-SHA512
byte[] keyBytes = GenerateSecureKey(64); // 64 bytes = 512 bits

//Convert the byte array to a base64-encoded string (for storage or transmission)
string base64Key = Convert.ToBase64String(keyBytes);

Console.WriteLine(base64Key);
Console.ReadLine();


static byte[] GenerateSecureKey(int keySizeInBytes)
{
    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
    {
        byte[] keyBytes = new byte[keySizeInBytes];
        randomNumberGenerator.GetBytes(keyBytes);
        return keyBytes;
    }
}
