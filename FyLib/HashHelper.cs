

// HashHelper
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class HashHelper
{
    public class CRC32
    {
        private static uint[] crcTable;

        public CRC32()
        {
            const uint polynomial = 0xedb88320;
            crcTable = new uint[256];

            for (int i = 0; i < 256; i++)
            {
                uint crc = (uint)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                crcTable[i] = crc;
            }
        }

        public uint GetCRC32(byte[] bytes)
        {
            uint crcValue = 0xffffffff;

            for (int i = 0; i < bytes.Length; i++)
            {
                byte index = (byte)(((crcValue) & 0xff) ^ bytes[i]);
                crcValue = crcTable[index] ^ (crcValue >> 8);
            }
            return ~crcValue;
        }

        public uint GetCRC32(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return GetCRC32(bytes);
        }
    }

    private class QQCrypter
    {
        private long contextStart;

        private long Crypt;

        private bool Header;

        private byte[] Key;

        private byte[] Out;

        private long padding;

        private byte[] Plain;

        private long Pos;

        private long preCrypt;

        private byte[] prePlain;

        private Random rd;

        public QQCrypter()
        {
            Key = new byte[16];
            rd = new Random();
        }

        public byte[] QQTeanDecipher(byte[] arrayIn, List<byte[]> Keys)
        {
            byte[] array = new byte[0];
            for (int i = 0; i < Keys.Count; i = checked(i + 1))
            {
                array = TeanDecipher(arrayIn, Keys[i]);
                if (array.Length != 0)
                {
                    return array;
                }
            }
            return array;
        }

        public byte[] TeanDecipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Decrypt(arrayIn, arrayKey, 0L, 2654435769u, 16u);
        }

        public byte[] TeanEncipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Encrypt(arrayIn, arrayKey, 0L, 2654435769u, 16u);
        }

        public byte[] XTeanEncipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Encrypt(arrayIn, arrayKey, 0L, 1474859335u, 32u);
        }

        public byte[] XTeanDecipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Decrypt(arrayIn, arrayKey, 0L, 1474859335u, 32u);
        }

        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, uint delta, uint round)
        {
            return Decipher(arrayIn, arrayKey, 0L, delta, round);
        }

        private long getUnsignedInt(byte[] arrayIn, int offset, int len)
        {
            long num = 0L;
            int num2 = 0;
            checked
            {
                num2 = ((len <= 8) ? (offset + len) : (offset + 8));
                for (int i = offset; i < num2; i++)
                {
                    num <<= 8;
                    num |= (ushort)(arrayIn[i] & 0xFF);
                }
                return (num & 0xFFFFFFFFu) | (num >> 32);
            }
        }

        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            byte[] arr = new byte[24];
            byte[] array = new byte[8];
            checked
            {
                if (arrayIn.Length >= 8)
                {
                    if (arrayKey.Length < 16)
                    {
                        return array;
                    }
                    delta &= 0xFFFFFFFFu;
                    long num = delta * round;
                    num &= 0xFFFFFFFFu;
                    long num2 = getUnsignedInt(arrayIn, (int)offset, 4);
                    long num3 = getUnsignedInt(arrayIn, (int)offset + 4, 4);
                    long unsignedInt = getUnsignedInt(arrayKey, 0, 4);
                    long unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
                    long unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
                    long unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
                    for (int i = 1; i <= round; i++)
                    {
                        num3 -= ((num2 << 4) + unsignedInt3) ^ (num2 + num) ^ ((num2 >> 5) + unsignedInt4);
                        num3 &= 0xFFFFFFFFu;
                        num2 -= ((num3 << 4) + unsignedInt) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt2);
                        num2 &= 0xFFFFFFFFu;
                        num -= unchecked((long)delta);
                        num &= 0xFFFFFFFFu;
                    }
                    arr = CopyMemory(arr, 0, num2);
                    arr = CopyMemory(arr, 4, num3);
                    array[0] = arr[3];
                    array[1] = arr[2];
                    array[2] = arr[1];
                    array[3] = arr[0];
                    array[4] = arr[7];
                    array[5] = arr[6];
                    array[6] = arr[5];
                    array[7] = arr[4];
                }
                return array;
            }
        }

        private byte[] CopyMemory(byte[] arr, int arr_index, long input)
        {
            checked
            {
                if (arr_index + 4 <= arr.Length)
                {
                    arr[arr_index + 3] = (byte)((input & 0xFF000000u) >> 24);
                    arr[arr_index + 2] = (byte)((input & 0xFF0000) >> 16);
                    arr[arr_index + 1] = (byte)((input & 0xFF00) >> 8);
                    arr[arr_index] = (byte)(input & 0xFF);
                    arr[arr_index] = (byte)(arr[arr_index] & 0xFF);
                    arr[arr_index + 1] = (byte)(arr[arr_index + 1] & 0xFF);
                    arr[arr_index + 2] = (byte)(arr[arr_index + 2] & 0xFF);
                    arr[arr_index + 3] = (byte)(arr[arr_index + 3] & 0xFF);
                }
                return arr;
            }
        }

        private byte[] QQ_Decrypt(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            byte[] result = new byte[0];
            if (arrayIn.Length < 16 || arrayIn.Length % 8 != 0)
            {
                return result;
            }
            if (arrayKey.Length != 16)
            {
                return result;
            }
            checked
            {
                byte[] array = new byte[offset + 8];
                arrayKey.CopyTo(Key, 0);
                preCrypt = 0L;
                Crypt = 0L;
                prePlain = Decipher(arrayIn, arrayKey, offset, delta, round);
                Pos = prePlain[0] & 7;
                long num = arrayIn.Length - Pos - 10;
                if (num <= 0)
                {
                    return result;
                }
                Out = new byte[num];
                preCrypt = 0L;
                Crypt = 8L;
                contextStart = 8L;
                Pos++;
                padding = 1L;
                while (padding < 3)
                {
                    if (Pos < 8)
                    {
                        Pos++;
                        padding++;
                    }
                    else if (Pos == 8)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = arrayIn[i];
                        }
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                long num2 = 0L;
                while (num != 0L)
                {
                    if (Pos < 8)
                    {
                        Out[(int)(IntPtr)num2] = (byte)(array[(int)(IntPtr)(offset + preCrypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]);
                        num2++;
                        num--;
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        array = arrayIn;
                        preCrypt = Crypt - 8;
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                for (padding = 1L; padding <= 7; padding++)
                {
                    if (Pos < 8)
                    {
                        if ((array[(int)(IntPtr)(offset + preCrypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]) != 0)
                        {
                            return result;
                        }
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            array[j] = arrayIn[j];
                        }
                        preCrypt = Crypt;
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                return Out;
            }
        }

        private bool Decrypt8Bytes(byte[] arrayIn, uint delta, uint round)
        {
            return Decrypt8Bytes(arrayIn, 0L, delta, round);
        }

        private bool Decrypt8Bytes(byte[] arrayIn, long offset, uint delta, uint round)
        {
            checked
            {
                for (Pos = 0L; Pos <= 7; Pos++)
                {
                    if (contextStart + Pos > arrayIn.Length - 1)
                    {
                        return true;
                    }
                    prePlain[(int)(IntPtr)Pos] = (byte)(prePlain[(int)(IntPtr)Pos] ^ arrayIn[(int)(IntPtr)(offset + Crypt + Pos)]);
                }
                try
                {
                    prePlain = Decipher(prePlain, Key, delta, round);
                    contextStart += 8L;
                    Crypt += 8L;
                    Pos = 0L;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private long Rand()
        {
            checked
            {
                return rd.Next() + unchecked(rd.Next() % 1024);
            }
        }

        private void Encrypt8Bytes(uint delta, uint round)
        {
            checked
            {
                try
                {
                    for (Pos = 0L; Pos <= 7; Pos++)
                    {
                        if (Header)
                        {
                            Plain[(int)(IntPtr)Pos] = (byte)(Plain[(int)(IntPtr)Pos] ^ prePlain[(int)(IntPtr)Pos]);
                        }
                        else
                        {
                            Plain[(int)(IntPtr)Pos] = (byte)(Plain[(int)(IntPtr)Pos] ^ Out[(int)(IntPtr)(preCrypt + Pos)]);
                        }
                    }
                    byte[] array = Encipher(Plain, Key, delta, round);
                    for (int i = 0; i <= 7; i++)
                    {
                        Out[(int)(IntPtr)(Crypt + i)] = array[i];
                    }
                    for (Pos = 0L; Pos <= 7; Pos++)
                    {
                        Out[(int)(IntPtr)(Crypt + Pos)] = (byte)(Out[(int)(IntPtr)(Crypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]);
                    }
                    Plain.CopyTo(prePlain, 0);
                    preCrypt = Crypt;
                    Crypt += 8L;
                    Pos = 0L;
                    Header = false;
                }
                catch
                {
                }
            }
        }

        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, uint delta, uint round)
        {
            return Encipher(arrayIn, arrayKey, 0L, delta, round);
        }

        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            byte[] array = new byte[8];
            byte[] arr = new byte[24];
            checked
            {
                if (arrayIn.Length >= 8)
                {
                    if (arrayKey.Length < 16)
                    {
                        return array;
                    }
                    long num = 0L;
                    delta &= 0xFFFFFFFFu;
                    long num2 = getUnsignedInt(arrayIn, (int)offset, 4);
                    long num3 = getUnsignedInt(arrayIn, (int)offset + 4, 4);
                    long unsignedInt = getUnsignedInt(arrayKey, 0, 4);
                    long unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
                    long unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
                    long unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
                    for (int i = 1; i <= round; i++)
                    {
                        num += unchecked((long)delta);
                        num &= 0xFFFFFFFFu;
                        num2 += ((num3 << 4) + unsignedInt) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt2);
                        num2 &= 0xFFFFFFFFu;
                        num3 += ((num2 << 4) + unsignedInt3) ^ (num2 + num) ^ ((num2 >> 5) + unsignedInt4);
                        num3 &= 0xFFFFFFFFu;
                    }
                    arr = CopyMemory(arr, 0, num2);
                    arr = CopyMemory(arr, 4, num3);
                    array[0] = arr[3];
                    array[1] = arr[2];
                    array[2] = arr[1];
                    array[3] = arr[0];
                    array[4] = arr[7];
                    array[5] = arr[6];
                    array[6] = arr[5];
                    array[7] = arr[4];
                }
                return array;
            }
        }

        private byte[] QQ_Encrypt(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            Plain = new byte[8];
            prePlain = new byte[8];
            Pos = 1L;
            padding = 0L;
            preCrypt = 0L;
            Crypt = 0L;
            arrayKey.CopyTo(Key, 0);
            Header = true;
            Pos = 2L;
            Pos = checked(arrayIn.Length + 10) % 8;
            checked
            {
                if (Pos != 0L)
                {
                    Pos = 8 - Pos;
                }
                Out = new byte[arrayIn.Length + Pos + 10];
                Plain[0] = (byte)((Rand() & 0xF8) | Pos);
                for (int i = 1; i <= Pos; i++)
                {
                    Plain[i] = (byte)(Rand() & 0xFF);
                }
                Pos++;
                padding = 1L;
                while (padding < 3)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = (byte)(Rand() & 0xFF);
                        padding++;
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                int num = (int)offset;
                long num2 = 0L;
                num2 = arrayIn.Length;
                while (num2 > 0)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = arrayIn[num];
                        num++;
                        Pos++;
                        num2--;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                padding = 1L;
                while (padding < 9)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = 0;
                        Pos++;
                        padding++;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                return Out;
            }
        }

        public static byte[] TeaEncipher(byte[] encrypt_data, byte[] key)
        {
            List<byte> list = new List<byte>(encrypt_data);
            List<byte> list2 = new List<byte>(key);
            list.Reverse(0, 4);
            list.Reverse(4, 4);
            uint num = smethod_0(list.GetRange(0, 4).ToArray());
            uint num2 = smethod_0(list.GetRange(4, 4).ToArray());
            uint num3 = smethod_0(list2.GetRange(0, 4).ToArray());
            uint num4 = smethod_0(list2.GetRange(4, 4).ToArray());
            uint num5 = smethod_0(list2.GetRange(8, 4).ToArray());
            uint num6 = smethod_0(list2.GetRange(12, 4).ToArray());
            uint num7 = 16u;
            uint num8 = 0u;
            uint num9 = 2654435769u;
            checked
            {
                while (num7-- != 0)
                {
                    num8 += num9;
                    num += ((num2 << 4) + num3) ^ (num2 + num8) ^ ((num2 >> 5) + num4);
                    num2 += ((num << 4) + num5) ^ (num + num8) ^ ((num >> 5) + num6);
                }
                return ToByteArray(new uint[2]
                {
                    byteswap_ulong(num),
                    byteswap_ulong(num2)
                }, IncludeLength: false);
            }
        }

        private static uint byteswap_ulong(uint i)
        {
            return checked((i << 24) + ((i << 8) & 0xFF0000) + ((i >> 8) & 0xFF00) + (i >> 24));
        }

        private static byte[] ToByteArray(uint[] Data, bool IncludeLength)
        {
            checked
            {
                int num = ((!IncludeLength) ? (Data.Length << 2) : ((int)Data[Data.Length - 1]));
                byte[] array = new byte[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = (byte)(Data[i >> 2] >> ((i & 3) << 3));
                }
                return array;
            }
        }

        private static uint smethod_0(byte[] data)
        {
            try
            {
                return checked((uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)));
            }
            catch
            {
                return 0u;
            }
        }
    }

    public static byte[] MD5(string str)
    {
        MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
        byte[] bytes = Encoding.Default.GetBytes(str);
        return mD5CryptoServiceProvider.ComputeHash(bytes);
    }

    public static string MD5_(string str)
    {
        MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
        byte[] bytes = Encoding.Default.GetBytes(str);
        byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
        string text = "";
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            text += array[i].ToString("x").PadLeft(2, '0');
        }
        return text;
    }

    public static string MD5_(byte[] value)
    {
        byte[] array = new MD5CryptoServiceProvider().ComputeHash(value);
        string text = "";
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            text += array[i].ToString("x").PadLeft(2, '0');
        }
        return text;
    }

    public static byte[] MD5(byte[] value)
    {
        return new MD5CryptoServiceProvider().ComputeHash(value);
    }

    public static byte[] QQTEAEncrypt(byte[] value, byte[] key)
    {
        byte[] result = null;
        if (key.Length != 16)
        {
            return result;
        }
        return new QQCrypter().TeanEncipher(value, key);
    }

    public static byte[] QQTEADecrypt(byte[] value, byte[] key)
    {
        byte[] result = null;
        if (key.Length != 16)
        {
            return result;
        }
        return new QQCrypter().TeanDecipher(value, key);
    }

    public static byte[] Crc32(byte[] value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value));
    }

    public static byte[] Crc32(string value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value));
    }

    public static string Crc32_(string value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value)).ToHex().ToUpper();
    }

    public static string Crc32_(byte[] value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value)).ToHex().ToUpper();
    }

    public static byte[] XTeaEncrypt(byte[] value, byte[] key)
    {
        return new QQCrypter().XTeanEncipher(value, key);
    }

    public static byte[] XTeaDecrypt(byte[] value, byte[] key)
    {
        return new QQCrypter().XTeanDecipher(value, key);
    }

    public static byte[] CreateOfficial(int tzm, byte[] OffKey, byte[] bufSigPicNew, byte[] bufTGTGT)
    {
        int num = 4;
        int num2 = 256;
        byte[] collection = OffKey.Md5();
        byte[] collection2 = MD5(bufSigPicNew);
        List<byte> list = new List<byte>(collection);
        list.AddRange(collection2);
        byte[] array = list.ToArray();
        checked
        {
            int num3 = unchecked(tzm % 19) + 5;
            byte[] array2 = new byte[256];
            byte[] array3 = new byte[256];
            for (int i = 0; i < num2; i++)
            {
                array2[i] = (byte)i;
                int num4 = 16 + unchecked(i % 16);
                array3[i] = array[num4];
            }
            int num5 = 0;
            byte b = 0;
            for (int j = 0; j < num2; j++)
            {
                unchecked
                {
                    num5 = checked(num5 + unchecked((int)array2[j]) + unchecked((int)array3[j])) % num2;
                    b = array2[num5];
                    array2[num5] = array2[j];
                    array2[j] = b;
                }
            }
            num5 = 0;
            for (int k = 0; k < 16; k++)
            {
                unchecked
                {
                    num5 = checked(num5 + unchecked((int)array2[checked(k + 1)])) % num2;
                    b = array2[num5];
                }
                array2[num5] = array2[k + 1];
                array2[k + 1] = b;
                int num6;
                unchecked
                {
                    num6 = checked(unchecked((int)array2[num5]) + unchecked((int)array2[checked(k + 1)])) % num2;
                }
                list.Add((byte)(array2[num6] ^ array[k]));
            }
            list.AddRange(MD5(bufTGTGT));
            List<byte> list2 = new List<byte>(MD5(list.ToArray()));
            byte[] array4 = list2.ToArray();
            for (int l = 0; l < num3; l++)
            {
                array4 = MD5(array4);
            }
            list.RemoveRange(0, 16);
            list.InsertRange(0, array4);
            byte[] encrypt_data = list2.GetRange(0, 8).ToArray();
            byte[] encrypt_data2 = list2.GetRange(8, 8).ToArray();
            byte[] array5 = new byte[16];
            for (int m = 0; m < num; m++)
            {
                int index = m * 16;
                List<byte> range = list.GetRange(index, 16);
                range.Reverse(0, 4);
                range.Reverse(4, 4);
                range.Reverse(8, 4);
                range.Reverse(12, 4);
                List<byte> list3 = new List<byte>(QQCrypter.TeaEncipher(encrypt_data, range.ToArray()));
                list3.AddRange(QQCrypter.TeaEncipher(encrypt_data2, range.ToArray()));
                for (int n = m; n < 16; n++)
                {
                    ref byte reference = ref array5[n];
                    reference = (byte)(reference ^ list3[n]);
                }
            }
            array5 = MD5(array5);
            int cRC = (int)new CRC32().GetCRC32(array5);
            List<byte> list4 = new List<byte>(array5);
            list4.AddRange(BitConverter.GetBytes(cRC));
            return list4.ToArray();
        }
    }

    public static string sha256(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] array = SHA256.Create().ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static string sha256(byte[] data)
    {
        byte[] array = SHA256.Create().ComputeHash(data);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static string sha1(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] array = SHA1.Create().ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static string sha1(byte[] data)
    {
        byte[] array = SHA1.Create().ComputeHash(data);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static string sha512(byte[] data)
    {
        byte[] array = SHA512.Create().ComputeHash(data);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    public static string sha512(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] array = SHA512.Create().ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }
}
