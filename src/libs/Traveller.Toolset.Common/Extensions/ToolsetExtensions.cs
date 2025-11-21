using System;

namespace Traveller.Toolset.Extensions;

public static class ToolsetExtensions
{
    public static string RollToCode(this int roll)
    {
        return roll >= 10 ? ((char)('A' + (roll - 10))).ToString() : roll.ToString();
    }
    public static int CodeToInt(this string code)
    {
        //1=1, A = 10, B = 11, etc
        if (string.IsNullOrEmpty(code))
            return 0;

        code = code.ToUpper().Trim();

        // Try to parse as a digit
        if (int.TryParse(code, out int result))
            return result;

        // Handle A-Z (A=10, B=11, etc.)
        if (code.Length == 1 && char.IsLetter(code[0]))
            return code[0] - 'A' + 10;

        return 0;
    }
}
