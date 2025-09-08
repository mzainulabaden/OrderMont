using Abp.Reflection.Extensions;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP;

public static class ExtensionMethods
{
    public static void VerifyDateRange(DateTime start_date, DateTime end_date)
    {
        if (end_date < start_date)
            throw new ArgumentException($"Invalid date range: Start Date: {start_date.ToShortDateString()} cannot be after End Date: {end_date.ToShortDateString()}.");
    }

    public static async Task<string> SavePdfFileAsync(this byte[] file_bytes, string file_name)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_hhmmss");
        var unique_file_name = $"{Path.GetFileNameWithoutExtension(file_name)}_{timestamp}.pdf";
        var report_directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports");

        if (!Directory.Exists(report_directory))
            Directory.CreateDirectory(report_directory);

        var fullPath = Path.Combine(report_directory, unique_file_name);
        await File.WriteAllBytesAsync(fullPath, file_bytes);

        return Path.Combine("/Reports", unique_file_name).Replace("\\", "/");
    }

    public static TimeOnly ToTimeOnly(this DateTime datetime)
    {
        return TimeOnly.FromDateTime(datetime);
    }

    public static DateOnly ToDateOnly(this DateTime datetime)
    {
        return DateOnly.FromDateTime(datetime.Date);
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static string GetVoucherPrefix(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        var parts = value.Split('-');
        return parts[0] ?? "";
    }

    public static int GetVoucherIndex(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        var parts = value.Split('-');
        if (parts.Length > 3 && int.TryParse(parts[3], out var index))
            return index;

        return default;
    }

    public static string CalculateJobDuration(this DateTime date_of_joining)
    {
        var currentDate = DateTime.Now;

        var years = currentDate.Year - date_of_joining.Year;
        var months = currentDate.Month - date_of_joining.Month;
        var days = currentDate.Day - date_of_joining.Day;

        if (months < 0)
        {
            months += 12;
            years--;
        }

        if (days < 0)
        {
            var previousMonth = currentDate.AddMonths(-1);
            days += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            months--;
        }

        return $"{years} Y, {months} M, {days} D";
    }

    public static bool IsAlphanumeric(this string str, bool allow_spaces = true)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                if (allow_spaces && c == ' ')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool IsAlphabetic(this string str, bool allow_spaces = true)
    {
        if (IsNullOrEmpty(str))
            return false;

        foreach (char c in str)
        {
            if (!char.IsLetter(c))
            {
                if (allow_spaces && c == ' ')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static decimal SetPrecision(this decimal value, int decimal_places)
    {
        return Math.Round(value, decimal_places);
    }

    public static bool IsAllDigit(this string str)
    {
        bool isTrue = true;
        foreach (var ch in str)
            if (!char.IsDigit(ch) && ch != '.')
                isTrue = false;
        return isTrue;
    }

    public static string GetIntDigits(this string str, bool includeMinusSign = true)
    {
        string output = "";
        foreach (var ch in str)
            if (char.IsDigit(ch) || includeMinusSign && ch == '-')
                output += ch;
        return output;
    }

    public static string ToPascalCase(this string value)
    {
        string output = "";
        if (string.IsNullOrWhiteSpace(value))
            return value;

        foreach (var split in value.Split(' '))
        {
            if (!string.IsNullOrWhiteSpace(split))
            {
                var str = split;
                var first = str[0].ToString().ToUpper();
                str = str.Remove(0, 1);
                output += first + str + " ";
            }
        }

        return output.RemoveLastChar();
    }

    public static TKey GetValueOrDefault<T, TKey>(this T obj, Func<T, TKey> selector)
    {
        if (obj == null)
            return default;

        return selector(obj);
    }

    public static string FirstToUpper(this string value)
    {
        string output = "";
        if (string.IsNullOrWhiteSpace(value))
            return output;

        string firstLetter = value[0].ToString().ToUpper();
        output = value.Remove(0, 1);
        output = firstLetter + output;
        return output;
    }

    public static string GetDoubleDigits(this string str)
    {
        string output = "";
        if (string.IsNullOrWhiteSpace(str))
            return output;

        foreach (var ch in str)
            if (char.IsDigit(ch) || ch == '.')
                output += ch;
        return output;
    }

    public static string GetChars(this string str)
    {
        string output = "";
        foreach (var ch in str)
            if (char.IsLetter(ch))
                output += ch;
        return output;
    }

    public static int TryToInt(this string str_num, bool exact)
    {
        if (!exact)
        {
            string digits = str_num.GetIntDigits();
            int.TryParse(digits, out int num);
            return num;
        }
        else
        {
            int.TryParse(str_num, out int num);
            return num;
        }
    }

    public static int TryToInt(this string str_num, string str_to_remove)
    {
        if (str_num == null)
            return 0;

        int.TryParse(str_num.Replace(str_to_remove, string.Empty), out int num);
        return num;
    }

    public static int TryToInt(this string str_num)
    {
        int.TryParse(str_num, out int num);
        return num;
    }

    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }

    public static double TryToDouble(this string str_num, string str_to_remove = null)
    {
        if (!string.IsNullOrWhiteSpace(str_to_remove))
            str_num = str_num.Replace(str_to_remove, string.Empty);

        double.TryParse(str_num, out double num);
        return num;
    }

    public static string TryToCommaNumeric(this string str_num)
    {
        string digits = str_num.GetIntDigits();
        if (digits == "-")
            return digits;
        else
        {
            int.TryParse(digits, out int num);
            if (num == 0)
                return "";
            else return num.ToString("#,##0");
        }
    }

    public static string RemoveLastChar(this string str, int count = 1)
    {
        if (str.Length > 0)
            return str.Remove(str.Length - count, count);
        else return str;
    }

    public static List<string> SeparateBy(this string str, string braces, bool return_input = false)
    {
        if (string.IsNullOrWhiteSpace(str))
            return new List<string>();

        if (braces.Length != 2)
            throw new Exception("'Braces' must be of length '2'.");

        List<string> output = new List<string>();
        Regex r = new Regex($@"(?<=\{braces[0]})[^{braces[1]}]*(?=\{braces[1]})");
        var matches = r.Matches(str);
        if (return_input)
        {
            if (matches.Count == 0)
                return new List<string> { str };
            else foreach (Match m in matches)
                    output.Add(m.Value);
        }
        else foreach (Match m in matches)
                output.Add(m.Value);

        return output;
    }

    public static IEnumerable<T> DistinctSuccessive<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        Func<T, T, bool> equals = (left, right) => null == comparer
          ? Equals(left, right)
          : comparer.Equals(left, right);

        bool first = true;
        T prior = default;

        foreach (var item in source)
        {
            if (first || !equals(item, prior))
                yield return item;

            first = false;
            prior = item;
        }
    }

    public static bool[] ToBinary(this string data)
    {
        bool[] buffer = new bool[(data.Length * 8 + (false ? data.Length - 1 : 0))];
        int index = 0;
        for (int i = 0; i < data.Length; i++)
        {
            string binary = Convert.ToString(data[i], 2).PadLeft(8, '0');
            for (int j = 0; j < 8; j++)
            {
                buffer[index] = binary[j] != '0';
                index++;
            }
        }

        return buffer;
    }

    public static bool IsEmail(this string value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        var regex = new Regex(EmailRegex);
        return regex.IsMatch(value);
    }

    public static bool IsValidFilePath(this string file_path)
    {
        // Check if the path is empty
        if (string.IsNullOrWhiteSpace(file_path))
        {
            return false;
        }

        // Check for invalid characters
        var invalid_chars = Path.GetInvalidPathChars();
        if (file_path.IndexOfAny(invalid_chars) >= 0)
        {
            return false;
        }

        // Check if the path is too long
        if (file_path.Length > 255)
        {
            return false;
        }

        // Check if the path format is correct
        try
        {
            string full_path = Path.GetFullPath(file_path);

            // Check if the path exists
            if (!File.Exists(full_path) && !Directory.Exists(full_path))
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool IsValidFileName(this string file_name)
    {
        // Check if the file name is empty
        if (string.IsNullOrWhiteSpace(file_name))
        {
            return false;
        }

        // Check for invalid characters
        var invalid_chars = Path.GetInvalidFileNameChars();
        if (file_name.IndexOfAny(invalid_chars) >= 0)
        {
            return false;
        }

        // Check if the file name is too long
        if (file_name.Length > 255)
        {
            return false;
        }

        return true;
    }

    public static int ToNumber(this char alphabet)
    {
        return char.ToUpper(alphabet) - 64;
    }

    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is not MemberExpression member)
        {
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a field, not a property.",
                propertyLambda.ToString()));
        }

        Type type = typeof(TSource);
        if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
        {
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(),
                type));
        }

        return propInfo;
    }

    public static long TryToLong(this string number)
    {
        long.TryParse(number, out long result);
        return result;
    }

    public static bool IsAnonymousType(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
            && type.IsGenericType && type.Name.Contains("AnonymousType")
            && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
            && type.Attributes.HasFlag(TypeAttributes.NotPublic);
    }

    public static List<string> InFileDuplication<T>(this Dictionary<int, T> dict, Func<T, object> validator)
    {
        var output = new List<string>();

        if (dict.Count == 0)
            return output;

        var groups = dict.GroupBy(i => validator(i.Value));
        foreach (var group in groups)
            if (group.Count() > 1)
            {
                output.Add($"Found {group.Count()} duplicates listed below:");
                foreach (var unit in group)
                    output.Add($"Duplicate Data at Row: '{unit.Key}'.");
            }

        return output;
    }

    public static string ListStringToString(this IEnumerable<string> list, char seprator = ',')
    {
        try
        {
            var output = "";
            foreach (var item in list)
                output += $"{item}{seprator}";
            return output != "" ? output.Remove(output.Length - 1, 1) : "";
        }
        catch { return null; }
    }

    public static List<string> StringToListString(this string str, char seprator = ',')
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
                return new List<string>();

            var output = new List<string>();
            var splits = str.Split(seprator);
            foreach (var s in splits)
                output.Add(s);
            return output;
        }
        catch { return null; }
    }

    public static string LongsToString(this IEnumerable<long> list, char seprator = ',')
    {
        var output = "";
        foreach (var item in list)
            output += $"{item}{seprator}";
        return output != "" ? output.Remove(output.Length - 1, 1) : "";
    }

    public static List<long> ToLongsList(this string str, char separator = ',')
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
                return new List<long>();

            var longs = new List<long>();
            var splits = str.Split(separator);
            foreach (var s in splits)
                longs.Add(long.Parse(s));
            return longs;
        }
        catch { return null; }
    }

    public static void ShowUserFriendlyException(this List<string> errors)
    {
        var str = "";
        foreach (var item in errors)
            str += $"{item}\n";
        throw new UserFriendlyException(str);
    }

    public static List<string> SeprateBy(this string str, string braces, bool return_input = false)
    {
        if (string.IsNullOrWhiteSpace(str))
            return new List<string>();

        if (braces.Length != 2)
            throw new Exception("'Braces' must be of length '2'.");

        List<string> output = new List<string>();
        Regex r = new Regex($@"(?<=\{braces[0]})[^{braces[1]}]*(?=\{braces[1]})");
        var matches = r.Matches(str);
        if (return_input)
        {
            if (matches.Count == 0)
                return new List<string> { str };
            else foreach (Match m in matches)
                    output.Add(m.Value);
        }
        else foreach (Match m in matches)
                output.Add(m.Value);

        return output;
    }

    public static string SaveImage(this string base64_image, string folder_name)
    {
        try
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\{folder_name}\");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filename = Guid.NewGuid().ToString() + ".jpg";
            var filepath = Path.Combine(folder, filename);
            var bytes = Convert.FromBase64String(base64_image);
            File.WriteAllBytes(filepath, bytes);
            return $@"\{folder_name}\{filename}";
        }
        catch { return null; }
    }

    public static bool TryConvertToType(string value, Type targetType, out object result)
    {
        result = null!;

        try
        {
            if (targetType == typeof(string))
            {
                result = value;
            }
            else if (targetType == typeof(int))
            {
                result = int.Parse(value);
            }
            else if (targetType == typeof(bool))
            {
                result = bool.Parse(value);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void DeleteFile(this string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static string CalculateContentRootFolder()
    {
        var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(ERPCoreModule).GetAssembly().Location);
        if (coreAssemblyDirectoryPath == null)
        {
            throw new Exception("Could not find location of ERP.Core assembly!");
        }

        var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
        while (!DirectoryContains(directoryInfo.FullName, "ERP.sln"))
        {
            if (directoryInfo.Parent == null)
            {
                throw new Exception("Could not find content root folder!");
            }

            directoryInfo = directoryInfo.Parent;
        }

        var webMvcFolder = Path.Combine(directoryInfo.FullName, "src", "ERP.Web.Mvc");
        if (Directory.Exists(webMvcFolder))
        {
            return webMvcFolder;
        }

        var webHostFolder = Path.Combine(directoryInfo.FullName, "src", "ERP.Web.Host");
        if (Directory.Exists(webHostFolder))
        {
            return webHostFolder;
        }

        throw new Exception("Could not find root folder of the web project!");
    }

    private static bool DirectoryContains(string directory, string fileName)
    {
        return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
    }

    public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
}
