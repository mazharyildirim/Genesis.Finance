namespace Genesis.CoreApi.Shared.Cryptography
{
    public interface ISymmetricCryptographer
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
