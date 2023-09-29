

// HashHelper
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class HashHelper
{
    private class CRC32
    {
        private static uint[] crcTable;

        public CRC32()
        {
            crcTable = new uint[256]
            {
                0u, 1996959894u, 3993919788u, 2567524794u, 124634137u, 1886057615u, 3915621685u, 2657392035u, 249268274u, 2044508324u,
                3772115230u, 2547177864u, 162941995u, 2125561021u, 3887607047u, 2428444049u, 498536548u, 1789927666u, 4089016648u, 2227061214u,
                450548861u, 1843258603u, 4107580753u, 2211677639u, 325883990u, 1684777152u, 4251122042u, 2321926636u, 335633487u, 1661365465u,
                4195302755u, 2366115317u, 997073096u, 1281953886u, 3579855332u, 2724688242u, 1006888145u, 1258607687u, 3524101629u, 2768942443u,
                901097722u, 1119000684u, 3686517206u, 2898065728u, 853044451u, 1172266101u, 3705015759u, 2882616665u, 651767980u, 1373503546u,
                3369554304u, 3218104598u, 565507253u, 1454621731u, 3485111705u, 3099436303u, 671266974u, 1594198024u, 3322730930u, 2970347812u,
                795835527u, 1483230225u, 3244367275u, 3060149565u, 1994146192u, 31158534u, 2563907772u, 4023717930u, 1907459465u, 112637215u,
                2680153253u, 3904427059u, 2013776290u, 251722036u, 2517215374u, 3775830040u, 2137656763u, 141376813u, 2439277719u, 3865271297u,
                1802195444u, 476864866u, 2238001368u, 4066508878u, 1812370925u, 453092731u, 2181625025u, 4111451223u, 1706088902u, 314042704u,
                2344532202u, 4240017532u, 1658658271u, 366619977u, 2362670323u, 4224994405u, 1303535960u, 984961486u, 2747007092u, 3569037538u,
                1256170817u, 1037604311u, 2765210733u, 3554079995u, 1131014506u, 879679996u, 2909243462u, 3663771856u, 1141124467u, 855842277u,
                2852801631u, 3708648649u, 1342533948u, 654459306u, 3188396048u, 3373015174u, 1466479909u, 544179635u, 3110523913u, 3462522015u,
                1591671054u, 702138776u, 2966460450u, 3352799412u, 1504918807u, 783551873u, 3082640443u, 3233442989u, 3988292384u, 2596254646u,
                62317068u, 1957810842u, 3939845945u, 2647816111u, 81470997u, 1943803523u, 3814918930u, 2489596804u, 225274430u, 2053790376u,
                3826175755u, 2466906013u, 167816743u, 2097651377u, 4027552580u, 2265490386u, 503444072u, 1762050814u, 4150417245u, 2154129355u,
                426522225u, 1852507879u, 4275313526u, 2312317920u, 282753626u, 1742555852u, 4189708143u, 2394877945u, 397917763u, 1622183637u,
                3604390888u, 2714866558u, 953729732u, 1340076626u, 3518719985u, 2797360999u, 1068828381u, 1219638859u, 3624741850u, 2936675148u,
                906185462u, 1090812512u, 3747672003u, 2825379669u, 829329135u, 1181335161u, 3412177804u, 3160834842u, 628085408u, 1382605366u,
                3423369109u, 3138078467u, 570562233u, 1426400815u, 3317316542u, 2998733608u, 733239954u, 1555261956u, 3268935591u, 3050360625u,
                752459403u, 1541320221u, 2607071920u, 3965973030u, 1969922972u, 40735498u, 2617837225u, 3943577151u, 1913087877u, 83908371u,
                2512341634u, 3803740692u, 2075208622u, 213261112u, 2463272603u, 3855990285u, 2094854071u, 198958881u, 2262029012u, 4057260610u,
                1759359992u, 534414190u, 2176718541u, 4139329115u, 1873836001u, 414664567u, 2282248934u, 4279200368u, 1711684554u, 285281116u,
                2405801727u, 4167216745u, 1634467795u, 376229701u, 2685067896u, 3608007406u, 1308918612u, 956543938u, 2808555105u, 3495958263u,
                1231636301u, 1047427035u, 2932959818u, 3654703836u, 1088359270u, 936918000u, 2847714899u, 3736837829u, 1202900863u, 817233897u,
                3183342108u, 3401237130u, 1404277552u, 615818150u, 3134207493u, 3453421203u, 1423857449u, 601450431u, 3009837614u, 3294710456u,
                1567103746u, 711928724u, 3020668471u, 3272380065u, 1510334235u, 755167117u
            };
        }

        public int GetCRC32(byte[] bytes)
        {
            int num = bytes.Length;
            uint num2 = uint.MaxValue;
            checked
            {
                for (int i = 0; i < num; i++)
                {
                    num2 = ((num2 >> 8) & 0xFFFFFFu) ^ crcTable[(int)(IntPtr)((num2 ^ bytes[i]) & 0xFF)];
                }
                return (int)(num2 ^ 0xFFFFFFFFu);
            }
        }

        public int GetCRC32(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            int num = bytes.Length;
            uint num2 = uint.MaxValue;
            checked
            {
                for (int i = 0; i < num; i++)
                {
                    num2 = ((num2 >> 8) & 0xFFFFFFu) ^ crcTable[(int)(IntPtr)((num2 ^ bytes[i]) & 0xFF)];
                }
                return (int)(num2 ^ 0xFFFFFFFFu);
            }
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
            int cRC = new CRC32().GetCRC32(array5);
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
