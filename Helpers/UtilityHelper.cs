using HIDAeroService.Constants;
using HIDAeroService.Entity;
using HIDAeroService.Logging;
using HIDAeroService.Model;
using System.Net;
using System.Text;
using static HIDAeroService.AeroLibrary.Description;


namespace HIDAeroService.Utility
{
    public sealed class UtilityHelper
    {
        

        public static string ByteToHexStr(byte[] data)
        {
            var reversed = data.Reverse().ToArray(); // Reverse the byte array
            return BitConverter.ToString(reversed);
        }

        public static string ByteToHex(byte[] data)
        {
            if (data == null || data.Length == 0) return string.Empty;

            byte[] trimmed = data.Reverse()
                                 .SkipWhile(b => b == 0x00)
                                 .Reverse()
                                 .ToArray();

            return BitConverter.ToString(trimmed).Replace("-", "");
        }

        public static string ByteToText(byte[] data)
        {
            return System.Text.Encoding.Default.GetString(data);
        }

        public static int HexStrToInt(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }

        public static byte HexStrToByte(string hex)
        {
            return Convert.ToByte(hex, 16);
        }

        public static string ByteToTextThai(byte[] data)
        {
            Encoding thaiEncoding = Encoding.GetEncoding("windows-874"); // or "TIS-620" //windows-874
            return thaiEncoding.GetString(data);
        }

        public static byte[] TrimTrailingSpaces(byte[] data)
        {
            int i = data.Length - 1;
            while (i >= 0 && data[i] == 0x20) i--;
            return data.Take(i + 1).ToArray();
        }

        public static string CharToString(char[] chars)
        {
            return new string(chars);
        }

        public static DateTimeOffset UnixToDateTime(int timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        }

        public static string[] UnixToDateTimeParts(int timestamp)
        {
            string[] d = new string[2];

            //var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            var dateTime = new DateTime(1970, 1, 1).AddSeconds(timestamp).ToLocalTime(); ;

            d[0] = dateTime.Date.ToString("yyyy-MM-dd");             // Only the date part (00:00:00 time)
            d[1] = dateTime.TimeOfDay.ToString(@"hh\:mm\:ss");        // Only the time part

            return d;

        }

        public static string ParseFirmware(short major,short minor)
        {
            string s = minor.ToString();

            return $"{major}.{s.Substring(0, s.Length - 1)}.{s[^1]}";
        }


        public static short[] GetSCPComponent(short model)
        {
            short nInput, nOutput, nReaders;
            switch (model)
            {
                case 196:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 4;
                    break;
                case 193:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 4;
                    break;
                case 194:
                    nInput = 19;
                    nOutput = 2;
                    nReaders = 0;
                    break;
                case 195:
                    nInput = 5;
                    nOutput = 12;
                    nReaders = 0;
                    break;
                case 190:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 2;
                    break;
                case 191:
                    nInput = 19;
                    nOutput = 2;
                    nReaders = 0;
                    break;
                case 192:
                    nInput = 5;
                    nOutput = 12;
                    nReaders = 0;
                    break;
                default:
                    nInput = 0;
                    nOutput = 0;
                    nReaders = 0;
                    break;
            }

            return [nInput, nOutput, nReaders];
        }

        public static void SetAllTz(AccessLevelDoorTimeZone entity, short value)
        {
            for (int i = 1; i <= 64; i++)
            {
                var prop = typeof(AccessLevelDoorTimeZone).GetProperty($"TzAcr{i}");
                if (prop != null && prop.CanWrite)
                    prop.SetValue(entity, value);
            }
        }

        public static string IntegerToIp(int ip)
        {
            //int ipAsInt = 3232235777; // Example: 192.168.1.1
            byte[] b = BitConverter.GetBytes(ip);
            Array.Reverse(b);
            IPAddress ipAddress = new IPAddress(b);

            // Convert to string (IPv4 format)
            string ipString = ipAddress.ToString();
            return ipString;  // Strike: 192.168.1.1
        }

        public static int IpToInteger(string ipAdress)
        {
            var ip = IPAddress.Parse(ipAdress);
            byte[] b = ip.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(b);
            }

            return BitConverter.ToInt32(b, 0);
        }

        public static bool IsBitSet(int value, int bit)
        {
            return (value & (1 << bit)) != 0;
        }

    }
}
