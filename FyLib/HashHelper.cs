using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FyLib;
/// <summary>
/// 哈希和加密算法帮助类，提供各种常用的哈希计算和加密解密功能
/// </summary>
/// <remarks>
/// 该类包含以下主要功能模块：
/// 
/// 1. **MD5哈希算法**
///    - 计算MD5哈希值（注意：不推荐用于安全敏感场景）
///    - 支持字节数组和十六进制字符串输出
/// 
/// 2. **SHA系列哈希算法**
///    - SHA-1（已过时，不推荐安全使用）
///    - SHA-256（推荐用于安全应用）
///    - SHA-384
///    - SHA-512（最高安全级别）
/// 
/// 3. **HMAC签名算法**
///    - HMAC-SHA256（推荐）
///    - HMAC-SHA384
///    - HMAC-SHA512
/// 
/// 4. **AES对称加密**
///    - AES-256-CBC 加密/解密
///    - 自动生成IV或使用指定IV
/// 
/// 5. **RSA非对称加密**
///    - RSA公钥加密/私钥解密
///    - RSA私钥签名/公钥验签
/// 
/// 使用建议：
/// - 安全应用请使用SHA-256或SHA-512
/// - 敏感数据加密使用AES-256
/// - 数字签名使用RSA或HMAC-SHA256
/// </remarks>
public static class HashHelper
{
    #region MD5 哈希

    /// <summary>
    /// 计算字符串的MD5哈希值（大写）
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>MD5哈希值的十六进制字符串（大写）</returns>
    [Obsolete("MD5已不再安全，建议使用SHA256或更高级别的哈希算法")]
    public static string Md5(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Md5(bytes);
    }

    /// <summary>
    /// 计算字节数组的MD5哈希值（大写）
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>MD5哈希值的十六进制字符串（大写）</returns>
    [Obsolete("MD5已不再安全，建议使用SHA256或更高级别的哈希算法")]
    public static string Md5(byte[] input)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 计算字符串的MD5哈希值（小写）
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>MD5哈希值的十六进制字符串（小写）</returns>
    [Obsolete("MD5已不再安全，建议使用SHA256或更高级别的哈希算法")]
    public static string Md5Lower(string input)
    {
        return Md5(input).ToLower();
    }

    #endregion

    #region SHA 系列哈希

    /// <summary>
    /// 计算字符串的SHA1哈希值
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>SHA1哈希值的十六进制字符串（大写）</returns>
    [Obsolete("SHA1已不再安全，建议使用SHA256或更高级别的哈希算法")]
    public static string Sha1(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Sha1(bytes);
    }

    /// <summary>
    /// 计算字节数组的SHA1哈希值
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>SHA1哈希值的十六进制字符串（大写）</returns>
    [Obsolete("SHA1已不再安全，建议使用SHA256或更高级别的哈希算法")]
    public static string Sha1(byte[] input)
    {
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 计算字符串的SHA256哈希值
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>SHA256哈希值的十六进制字符串（大写）</returns>
    public static string Sha256(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Sha256(bytes);
    }

    /// <summary>
    /// 计算字节数组的SHA256哈希值
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>SHA256哈希值的十六进制字符串（大写）</returns>
    public static string Sha256(byte[] input)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 计算字符串的SHA384哈希值
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>SHA384哈希值的十六进制字符串（大写）</returns>
    public static string Sha384(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Sha384(bytes);
    }

    /// <summary>
    /// 计算字节数组的SHA384哈希值
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>SHA384哈希值的十六进制字符串（大写）</returns>
    public static string Sha384(byte[] input)
    {
        using var sha384 = SHA384.Create();
        var hash = sha384.ComputeHash(input);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 计算字符串的SHA512哈希值
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>SHA512哈希值的十六进制字符串（大写）</returns>
    public static string Sha512(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Sha512(bytes);
    }

    /// <summary>
    /// 计算字节数组的SHA512哈希值
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>SHA512哈希值的十六进制字符串（大写）</returns>
    public static string Sha512(byte[] input)
    {
        using var sha512 = SHA512.Create();
        var hash = sha512.ComputeHash(input);
        return Convert.ToHexString(hash);
    }

    #endregion

    #region HMAC 签名

    /// <summary>
    /// 使用HMAC-SHA256计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息</param>
    /// <param name="key">密钥</param>
    /// <returns>HMAC-SHA256签名的十六进制字符串（大写）</returns>
    public static string HmacSha256(string message, string key)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        return HmacSha256(messageBytes, keyBytes);
    }

    /// <summary>
    /// 使用HMAC-SHA256计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息字节数组</param>
    /// <param name="key">密钥字节数组</param>
    /// <returns>HMAC-SHA256签名的十六进制字符串（大写）</returns>
    public static string HmacSha256(byte[] message, byte[] key)
    {
        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(message);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 使用HMAC-SHA384计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息</param>
    /// <param name="key">密钥</param>
    /// <returns>HMAC-SHA384签名的十六进制字符串（大写）</returns>
    public static string HmacSha384(string message, string key)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        return HmacSha384(messageBytes, keyBytes);
    }

    /// <summary>
    /// 使用HMAC-SHA384计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息字节数组</param>
    /// <param name="key">密钥字节数组</param>
    /// <returns>HMAC-SHA384签名的十六进制字符串（大写）</returns>
    public static string HmacSha384(byte[] message, byte[] key)
    {
        using var hmac = new HMACSHA384(key);
        var hash = hmac.ComputeHash(message);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// 使用HMAC-SHA512计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息</param>
    /// <param name="key">密钥</param>
    /// <returns>HMAC-SHA512签名的十六进制字符串（大写）</returns>
    public static string HmacSha512(string message, string key)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        return HmacSha512(messageBytes, keyBytes);
    }

    /// <summary>
    /// 使用HMAC-SHA512计算消息签名
    /// </summary>
    /// <param name="message">要签名的消息字节数组</param>
    /// <param name="key">密钥字节数组</param>
    /// <returns>HMAC-SHA512签名的十六进制字符串（大写）</returns>
    public static string HmacSha512(byte[] message, byte[] key)
    {
        using var hmac = new HMACSHA512(key);
        var hash = hmac.ComputeHash(message);
        return Convert.ToHexString(hash);
    }

    #endregion

    #region AES 对称加密

    /// <summary>
    /// 使用AES-256-CBC加密字符串
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <param name="iv">初始化向量（16字节），如果为null则自动生成</param>
    /// <returns>加密后的Base64字符串（包含IV）</returns>
    public static string AesEncrypt(string plainText, string key, byte[]? iv = null)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length != 32)
        {
            // 自动调整密钥长度为32字节
            keyBytes = SHA256.HashData(keyBytes);
        }

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = AesEncrypt(plainBytes, keyBytes, iv);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// 使用AES-256-CBC加密字节数组
    /// </summary>
    /// <param name="plainBytes">明文字节数组</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <param name="iv">初始化向量（16字节），如果为null则自动生成</param>
    /// <returns>加密后的字节数组（包含IV）</returns>
    public static byte[] AesEncrypt(byte[] plainBytes, byte[] key, byte[]? iv = null)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;

        if (iv == null)
        {
            aes.GenerateIV();
            iv = aes.IV;
        }
        else
        {
            aes.IV = iv;
        }

        using var encryptor = aes.CreateEncryptor();
        var encryptedData = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // 将IV和加密数据组合在一起
        var result = new byte[iv.Length + encryptedData.Length];
        Array.Copy(iv, 0, result, 0, iv.Length);
        Array.Copy(encryptedData, 0, result, iv.Length, encryptedData.Length);

        return result;
    }

    /// <summary>
    /// 使用AES-256-CBC解密字符串
    /// </summary>
    /// <param name="cipherText">密文（Base64字符串，包含IV）</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>解密后的明文</returns>
    public static string AesDecrypt(string cipherText, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length != 32)
        {
            // 自动调整密钥长度为32字节
            keyBytes = SHA256.HashData(keyBytes);
        }

        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = AesDecrypt(cipherBytes, keyBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 使用AES-256-CBC解密字节数组
    /// </summary>
    /// <param name="cipherBytes">密文字节数组（包含IV）</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>解密后的字节数组</returns>
    public static byte[] AesDecrypt(byte[] cipherBytes, byte[] key)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;

        // 提取IV（前16字节）
        var iv = new byte[16];
        Array.Copy(cipherBytes, 0, iv, 0, 16);
        aes.IV = iv;

        // 提取加密数据（剩余字节）
        var encryptedData = new byte[cipherBytes.Length - 16];
        Array.Copy(cipherBytes, 16, encryptedData, 0, encryptedData.Length);

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
    }

    /// <summary>
    /// 使用AES-256-ECB加密字符串
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>加密后的Base64字符串</returns>
    /// <remarks>
    /// 注意：ECB模式不使用IV，相同的明文块会产生相同的密文块，安全性较低。
    /// 建议仅在特定场景下使用，一般情况下应使用CBC模式。
    /// </remarks>
    public static string AesEncryptEcb(string plainText, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length != 32)
        {
            // 自动调整密钥长度为32字节
            keyBytes = SHA256.HashData(keyBytes);
        }

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = AesEncryptEcb(plainBytes, keyBytes);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// 使用AES-256-ECB加密字节数组
    /// </summary>
    /// <param name="plainBytes">明文字节数组</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>加密后的字节数组</returns>
    /// <remarks>
    /// 注意：ECB模式不使用IV，相同的明文块会产生相同的密文块，安全性较低。
    /// 建议仅在特定场景下使用，一般情况下应使用CBC模式。
    /// </remarks>
    public static byte[] AesEncryptEcb(byte[] plainBytes, byte[] key)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;

        using var encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }

    /// <summary>
    /// 使用AES-256-ECB解密字符串
    /// </summary>
    /// <param name="cipherText">密文（Base64字符串）</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>解密后的明文</returns>
    public static string AesDecryptEcb(string cipherText, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length != 32)
        {
            // 自动调整密钥长度为32字节
            keyBytes = SHA256.HashData(keyBytes);
        }

        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = AesDecryptEcb(cipherBytes, keyBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 使用AES-256-ECB解密字节数组
    /// </summary>
    /// <param name="cipherBytes">密文字节数组</param>
    /// <param name="key">密钥（32字节/256位）</param>
    /// <returns>解密后的字节数组</returns>
    public static byte[] AesDecryptEcb(byte[] cipherBytes, byte[] key)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
    }

    #endregion

    #region RSA 非对称加密

    /// <summary>
    /// 生成RSA密钥对
    /// </summary>
    /// <param name="keySize">密钥长度（位），默认2048，可选1024、2048、4096</param>
    /// <returns>元组（公钥XML，私钥XML）</returns>
    public static (string PublicKey, string PrivateKey) GenerateRsaKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        var publicKey = rsa.ToXmlString(false);
        var privateKey = rsa.ToXmlString(true);
        return (publicKey, privateKey);
    }

    /// <summary>
    /// 使用RSA公钥加密
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="publicKeyXml">公钥（XML格式）</param>
    /// <returns>加密后的Base64字符串</returns>
    public static string RsaEncrypt(string plainText, string publicKeyXml)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = RsaEncrypt(plainBytes, publicKeyXml);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// 使用RSA公钥加密
    /// </summary>
    /// <param name="plainBytes">明文字节数组</param>
    /// <param name="publicKeyXml">公钥（XML格式）</param>
    /// <returns>加密后的字节数组</returns>
    public static byte[] RsaEncrypt(byte[] plainBytes, string publicKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(publicKeyXml);
        return rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用RSA私钥解密
    /// </summary>
    /// <param name="cipherText">密文（Base64字符串）</param>
    /// <param name="privateKeyXml">私钥（XML格式）</param>
    /// <returns>解密后的明文</returns>
    public static string RsaDecrypt(string cipherText, string privateKeyXml)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = RsaDecrypt(cipherBytes, privateKeyXml);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 使用RSA私钥解密
    /// </summary>
    /// <param name="cipherBytes">密文字节数组</param>
    /// <param name="privateKeyXml">私钥（XML格式）</param>
    /// <returns>解密后的字节数组</returns>
    public static byte[] RsaDecrypt(byte[] cipherBytes, string privateKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(privateKeyXml);
        return rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用RSA私钥签名
    /// </summary>
    /// <param name="data">要签名的数据</param>
    /// <param name="privateKeyXml">私钥（XML格式）</param>
    /// <returns>签名的Base64字符串</returns>
    public static string RsaSign(string data, string privateKeyXml)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = RsaSign(dataBytes, privateKeyXml);
        return Convert.ToBase64String(signatureBytes);
    }

    /// <summary>
    /// 使用RSA私钥签名
    /// </summary>
    /// <param name="data">要签名的数据字节数组</param>
    /// <param name="privateKeyXml">私钥（XML格式）</param>
    /// <returns>签名的字节数组</returns>
    public static byte[] RsaSign(byte[] data, string privateKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(privateKeyXml);
        return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用RSA公钥验证签名
    /// </summary>
    /// <param name="data">原始数据</param>
    /// <param name="signature">签名（Base64字符串）</param>
    /// <param name="publicKeyXml">公钥（XML格式）</param>
    /// <returns>验证是否通过</returns>
    public static bool RsaVerify(string data, string signature, string publicKeyXml)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(signature);
        return RsaVerify(dataBytes, signatureBytes, publicKeyXml);
    }

    /// <summary>
    /// 使用RSA公钥验证签名
    /// </summary>
    /// <param name="data">原始数据字节数组</param>
    /// <param name="signature">签名字节数组</param>
    /// <param name="publicKeyXml">公钥（XML格式）</param>
    /// <returns>验证是否通过</returns>
    public static bool RsaVerify(byte[] data, byte[] signature, string publicKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(publicKeyXml);
        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    #endregion

    #region Base64 编码

    /// <summary>
    /// Base64编码
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>Base64编码后的字符串</returns>
    public static string Base64Encode(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64编码
    /// </summary>
    /// <param name="input">输入字节数组</param>
    /// <returns>Base64编码后的字符串</returns>
    public static string Base64Encode(byte[] input)
    {
        return Convert.ToBase64String(input);
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    /// <param name="input">Base64字符串</param>
    /// <returns>解码后的字符串</returns>
    public static string Base64Decode(string input)
    {
        var bytes = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Base64解码为字节数组
    /// </summary>
    /// <param name="input">Base64字符串</param>
    /// <returns>解码后的字节数组</returns>
    public static byte[] Base64DecodeToBytes(string input)
    {
        return Convert.FromBase64String(input);
    }

    #endregion

    #region PBKDF2 密钥派生

    /// <summary>
    /// 使用PBKDF2派生密钥
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="salt">盐值（建议至少16字节）</param>
    /// <param name="iterations">迭代次数（建议至少10000）</param>
    /// <param name="keyLength">派生密钥长度（字节）</param>
    /// <returns>派生的密钥字节数组</returns>
    public static byte[] Pbkdf2(string password, byte[] salt, int iterations = 10000, int keyLength = 32)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        return Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations, HashAlgorithmName.SHA256, keyLength);
    }

    /// <summary>
    /// 使用PBKDF2派生密钥（自动生成盐值）
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="iterations">迭代次数（建议至少10000）</param>
    /// <param name="keyLength">派生密钥长度（字节）</param>
    /// <param name="saltLength">盐值长度（字节）</param>
    /// <returns>元组（派生的密钥，使用的盐值）</returns>
    public static (byte[] Key, byte[] Salt) Pbkdf2WithSalt(string password, int iterations = 10000, int keyLength = 32, int saltLength = 16)
    {
        var salt = RandomNumberGenerator.GetBytes(saltLength);
        var key = Pbkdf2(password, salt, iterations, keyLength);
        return (key, salt);
    }

    #endregion

    #region 密码哈希（带盐值）

    /// <summary>
    /// 对密码进行哈希处理（SHA256 + 盐值）
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="salt">盐值（如果为null则自动生成）</param>
    /// <returns>元组（哈希值，盐值）</returns>
    public static (string Hash, string Salt) HashPassword(string password, string? salt = null)
    {
        byte[] saltBytes;
        if (salt == null)
        {
            saltBytes = RandomNumberGenerator.GetBytes(16);
            salt = Convert.ToBase64String(saltBytes);
        }
        else
        {
            saltBytes = Convert.FromBase64String(salt);
        }

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combined = new byte[saltBytes.Length + passwordBytes.Length];
        Array.Copy(saltBytes, 0, combined, 0, saltBytes.Length);
        Array.Copy(passwordBytes, 0, combined, saltBytes.Length, passwordBytes.Length);

        var hash = Sha256(combined);
        return (hash, salt);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password">要验证的密码</param>
    /// <param name="hash">存储的哈希值</param>
    /// <param name="salt">存储的盐值</param>
    /// <returns>密码是否正确</returns>
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var (computedHash, _) = HashPassword(password, salt);
        return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
    /// <summary>
    /// 检查密码强度
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}
