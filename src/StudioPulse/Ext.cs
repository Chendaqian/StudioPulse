using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Media
{
    /// <summary>
    /// 提供颜色渐变辅助方法
    /// </summary>
    public static class ColorExtensions
    {
        public static Color FadeTo(this Color color, Color toColor, float percent)
        {
            return Color.FromArgb((byte)Lerp(color.A, toColor.A, percent), (byte)Lerp(color.R, toColor.R, percent), (byte)Lerp(color.G, toColor.G, percent), (byte)Lerp(color.B, toColor.B, percent));
        }

        private static float Lerp(float v0, float v1, float t)
        {
            return v0 + (v1 - v0) * t;
        }
    }
}

namespace System
{
    /// <summary>
    /// 提供字节数的可读格式化方法
    /// </summary>
    public static class ByteSizeExtensions
    {
        public static string ToReadableByteSize(this long @long)
        {
            return @long.ToReadableByteSize("0.## ");
        }

        public static string ToReadableByteSize(this long @long, string format)
        {
            string str;
            string sign = @long < 0 ? "-" : string.Empty;
            double num;
            if (@long >= 1152921504606846976L)
            {
                str = "EB";
                num = @long >> 50;
            }
            else if (@long >= 1125899906842624L)
            {
                str = "PB";
                num = @long >> 40;
            }
            else if (@long >= 1099511627776L)
            {
                str = "TB";
                num = @long >> 30;
            }
            else if (@long >= 1073741824)
            {
                str = "GB";
                num = @long >> 20;
            }
            else if (@long < 1048576)
            {
                if (@long < 1024)
                {
                    return @long.ToString(string.Concat(sign, "0 B"));
                }
                str = "KB";
                num = @long;
            }
            else
            {
                str = "MB";
                num = @long >> 10;
            }
            num = num / 1024;
            return string.Concat(sign, num.ToString(format), str);
        }
    }

    /// <summary>
    /// 提供格式占位符查找方法
    /// </summary>
    public static class FormatExtensions
    {
        public static int IndexOfAny(this string @string, char[] anyOf, out char? foundChar)
        {
            return @string.IndexOfAny(anyOf, 0, out foundChar);
        }

        public static int IndexOfAny(this string @string, char[] anyOf, int startIndex, out char? foundChar)
        {
            return @string.IndexOfAny(anyOf, startIndex, @string.Length - startIndex, out foundChar);
        }

        public static int IndexOfAny(this string @string, char[] anyOf, int startIndex, int count, out char? foundChar)
        {
            foundChar = null;
            int num = @string.IndexOfAny(anyOf, startIndex, count);
            if (num > -1)
            {
                bool flag = false;
                for (int i = 0; i < anyOf.Length && !flag; i++)
                {
                    char? nullable = anyOf[i];
                    char? nullable1 = nullable;
                    foundChar = nullable;
                    char? nullable2 = nullable1;
                    int str = @string[num];
                    flag = nullable2.GetValueOrDefault() == (char)str;
                }
            }
            return num;
        }

        public static int IndexOfAny(this string @string, string[] anyOf)
        {
            return @string.IndexOfAny(anyOf, 0);
        }

        public static int IndexOfAny(this string @string, string[] anyOf, out string foundString)
        {
            return @string.IndexOfAny(anyOf, 0, out foundString);
        }

        public static int IndexOfAny(this string @string, string[] anyOf, int startIndex)
        {
            return @string.IndexOfAny(anyOf, startIndex, @string.Length - startIndex);
        }

        public static int IndexOfAny(this string @string, string[] anyOf, int startIndex, out string foundString)
        {
            return @string.IndexOfAny(anyOf, startIndex, @string.Length - startIndex, out foundString);
        }

        public static int IndexOfAny(this string @string, string[] anyOf, int startIndex, int count)
        {
            return @string.IndexOfAny(anyOf, startIndex, count, out string _);
        }

        public static int IndexOfAny(this string @string, string[] anyOf, int startIndex, int count, out string foundString)
        {
            Dictionary<string, int> strs = new Dictionary<string, int>();
            string[] strArrays = anyOf;

            foreach (string str in strArrays)
            {
                int num = @string.IndexOf(str, startIndex, count, StringComparison.Ordinal);
                if (num > -1)
                {
                    strs[str] = num;
                }
            }
            if (!strs.Any())
            {
                foundString = null;
                return -1;
            }
            KeyValuePair<string, int> keyValuePair = strs.ElementAt(0);
            foundString = keyValuePair.Key;
            return keyValuePair.Value;
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, out char? foundChar)
        {
            return @string.LastIndexOfAny(anyOf, @string.Length - 1, out foundChar);
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, int startIndex, out char? foundChar)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, startIndex + 1, out foundChar);
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, int startIndex, int count, out char? foundChar)
        {
            foundChar = null;
            int num = @string.LastIndexOfAny(anyOf, startIndex, count);
            if (num > -1)
            {
                bool flag = false;
                for (int i = 0; i < anyOf.Length && !flag; i++)
                {
                    char? nullable = anyOf[i];
                    char? nullable1 = nullable;
                    foundChar = nullable;
                    char? nullable2 = nullable1;
                    int str = @string[num];
                    flag = (nullable2.GetValueOrDefault() == (char)str);
                }
            }
            return num;
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf)
        {
            return @string.LastIndexOfAny(anyOf, @string.Length - 1);
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf, out string foundString)
        {
            return @string.LastIndexOfAny(anyOf, @string.Length - 1, out foundString);
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf, int startIndex)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, out string _);
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf, int startIndex, out string foundString)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, startIndex + 1, out foundString);
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf, int startIndex, int count)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, count, out string _);
        }

        public static int LastIndexOfAny(this string @string, string[] anyOf, int startIndex, int count, out string foundString)
        {
            Dictionary<string, int> strs = new Dictionary<string, int>();
            string[] strArrays = anyOf;
            foreach (string str in strArrays)
            {
                int num = @string.LastIndexOf(str, startIndex, count, StringComparison.Ordinal);
                if (num > -1)
                {
                    strs[str] = num;
                }
            }
            if (!strs.Any())
            {
                foundString = null;
                return -1;
            }
            KeyValuePair<string, int> keyValuePair = strs.ElementAt(0);
            foundString = keyValuePair.Key;
            return keyValuePair.Value;
        }
    }
}

namespace System.Diagnostics
{
    /// <summary>
    /// 根据进程 CPU 时间增量计算占用率
    /// </summary>
    public static class ProcessUsageExtensions
    {
        private static readonly Dictionary<int, DateTime> CpuCheckTimes;

        private static readonly Dictionary<int, TimeSpan> CpuTimes;

        static ProcessUsageExtensions()
        {
            CpuTimes = new Dictionary<int, TimeSpan>();
            CpuCheckTimes = new Dictionary<int, DateTime>();
        }

        public static double GetCpuUsage(this Process process)
        {
            // 使用两次采样的处理器时间差计算当前进程占用率。
            process.Refresh();
            if (!CpuTimes.ContainsKey(process.Id))
            {
                process.InitCpuUsage();
            }
            TimeSpan totalProcessorTime = process.TotalProcessorTime;
            DateTime utcNow = DateTime.UtcNow;
            TimeSpan item = totalProcessorTime - CpuTimes[process.Id];
            double totalSeconds = item.TotalSeconds;
            TimeSpan timeSpan = utcNow - CpuCheckTimes[process.Id];
            double num = timeSpan.TotalSeconds;
            double processorCount = totalSeconds / (Environment.ProcessorCount * num);
            CpuTimes[process.Id] = totalProcessorTime;
            CpuCheckTimes[process.Id] = utcNow;
            return processorCount;
        }

        public static void InitCpuUsage(this Process process)
        {
            CpuTimes[process.Id] = process.TotalProcessorTime;
            CpuCheckTimes[process.Id] = DateTime.UtcNow;
        }
    }
}