/*
 * 数据的加密和解密
 */
using System.IO;
using System.Text;
using System.Security.Cryptography;

public static class Crypto
{
    #region Variables
    private static RijndaelManaged m_RijndaelMgr = null;
    //加密的密钥,随便设置
    private static string m_szRijndaelKey = "qazwsxedcrfvtgbyhnujmiklop102938";
    #endregion

    #region Properties
    private static RijndaelManaged RijndaelMgr
    {
        get
        {
            if (null == m_RijndaelMgr)
            {
                m_RijndaelMgr = new RijndaelManaged();
                m_RijndaelMgr.Key = UTF8Encoding.UTF8.GetBytes(m_szRijndaelKey);
                m_RijndaelMgr.Mode = CipherMode.ECB;
                m_RijndaelMgr.Padding = PaddingMode.PKCS7;
            }
            return m_RijndaelMgr;
        }
    }
    #endregion

    #region Functions
    /// <summary>
    /// 加密文件
    /// </summary>
	public static void Encrypt(string szInFilePathName, string szOutFilePathName)
    {
        // 读取文件
        FileStream InFileStream = new FileStream(szInFilePathName, FileMode.Open, FileAccess.Read);
        byte[] InBytes = new byte[InFileStream.Length];
        InFileStream.Read(InBytes, 0, InBytes.Length);
        InFileStream.Close();

        // 加密
        byte[] OutBytes;
        Encrypt(InBytes, out OutBytes);

        // 写入文件
        FileStream OutFileStream = new FileStream(szOutFilePathName, FileMode.Create, FileAccess.Write);
        OutFileStream.Write(OutBytes, 0, OutBytes.Length);
        OutFileStream.Close();
    }

    /// <summary>
    /// 加密：bytes-->bytes
    /// </summary>
	public static void Encrypt(byte[] InBytes, out byte[] OutBytes)
    {
        // 加密
        ICryptoTransform CryptoTransform = RijndaelMgr.CreateEncryptor();
        OutBytes = CryptoTransform.TransformFinalBlock(InBytes, 0, InBytes.Length);
    }

    /// <summary>
    /// 加密：bytes-->string
    /// </summary>
    public static void Encrypt(byte[] InBytes, out string szOutput)
    {
        byte[] OutBytes;
        Encrypt(InBytes, out OutBytes);
        szOutput = System.Convert.ToBase64String(OutBytes);
    }

    /// <summary>
    /// 加密：string-->string
    /// </summary>
    public static void Encrypt(string szInput, out string szOutput)
    {
        byte[] InBytes = UTF8Encoding.UTF8.GetBytes(szInput);
        Encrypt(InBytes, out szOutput);
    }

    /// <summary>
    /// 加密：string-->string
    /// </summary>
    public static string Encrypt(string szInput)
    {
        string szOutput = string.Empty;
        Encrypt(szInput, out szOutput);
        return szOutput;
    }

    /// <summary>
    /// 解密文件
    /// </summary>
	public static void Decrypt(string szInFilePathName, string szOutFilePathName, ref string szOutput)
    {
        // 读取文件
        FileStream InFileStream = new FileStream(szInFilePathName, FileMode.Open, FileAccess.Read);
        byte[] InBytes = new byte[InFileStream.Length];
        InFileStream.Read(InBytes, 0, InBytes.Length);
        InFileStream.Close();

        // 解密
        byte[] OutBytes;
        Decrypt(InBytes, out OutBytes);

        // 输出到字符串
        if (null != szOutput)
        {
            szOutput = UTF8Encoding.UTF8.GetString(OutBytes);
        }

        // 输出到文件
        if (null != szOutFilePathName)
        {
            FileStream OutFileStream = new FileStream(szOutFilePathName, FileMode.OpenOrCreate, FileAccess.Write);
            OutFileStream.Write(OutBytes, 0, OutBytes.Length);
            OutFileStream.Close();
        }
    }

    /// <summary>
    /// 解密bytes-->bytes
    /// </summary>
    public static void Decrypt(byte[] InBytes, out byte[] OutBytes)
    {
        // 解密
        ICryptoTransform CryptoTransform = RijndaelMgr.CreateDecryptor();
        OutBytes = CryptoTransform.TransformFinalBlock(InBytes, 0, InBytes.Length);
    }

    /// <summary>
    /// 解密：bytes->string
    /// </summary>
    public static void Decrypt(byte[] InBytes, out string szOutput)
    {
        byte[] OutBytes;
        Decrypt(InBytes, out OutBytes);
        szOutput = UTF8Encoding.UTF8.GetString(OutBytes);
    }

    /// <summary>
    /// 解密：string-->string
    /// </summary>
    public static void Decrypt(string szInput, out string szOutput)
    {
        byte[] InBytes = System.Convert.FromBase64String(szInput);
        Decrypt(InBytes, out szOutput);
    }

    /// <summary>
    /// 解密：string-->string
    /// </summary>
    public static string Decrypt(string szInput)
    {
        string szOutput = string.Empty;
        Decrypt(szInput, out szOutput);
        return szOutput;
    }
    #endregion
}
