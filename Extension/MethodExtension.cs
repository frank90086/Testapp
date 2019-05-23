using System;
using Test.Service;

namespace Test.Extension{
    public static class MethodExtension{
        public static string DecryptToken(string str)
        {
            return ASEService.AESDecrypt(str);
        }

        public static string EncryptToken(string[] strArray)
        {
            return ASEService.AESEncrypt(String.Join(":", strArray));
        }

        public static long GetTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public static bool CheckTimestamp(string timestamp, long interval)
        {
            var now = GetTimestamp();
            var ts = long.Parse(timestamp);
            return now - ts <= interval;
        }
    }
}