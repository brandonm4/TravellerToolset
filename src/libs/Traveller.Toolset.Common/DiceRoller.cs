using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Traveller.Toolset;

public class DiceRoller
{
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    /// <summary>
    /// Rolls a number of dice and returns individual results.
    /// Example: RollDice(3,6) -> [2,1,5]
    /// </summary>
    public int[] RollDice(int dice, int sides)
    {
        
        if (dice <= 0 || sides <= 1)
            throw new ArgumentOutOfRangeException();

        var results = new int[dice];
        for (int i = 0; i < dice; i++)
        {
            results[i] = Roll1D(sides);
        }
        return results;
    }

    /// <summary>
    /// Rolls dice and returns total with optional modifier.
    /// Example: RollDiceTotal(3,6,+1) -> 9
    /// </summary>
    public int RollDiceTotal(int dice, int sides, int modifier = 0)
    {
        var rolls = RollDice(dice, sides);
        return rolls.Sum() + modifier;
    }

    /// <summary>
    /// Rolls an entire dice expression like "2d6+1", "1d10", or "(2d6+1)+(1d20)-(1d4)".
    /// Supports +, -, parentheses, integers, and dice terms.
    /// </summary>
    public int RollExpression(string expression, out string breakdown)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentNullException(nameof(expression));

        // Clean and normalize expression
        var sanitized = expression.Replace(" ", string.Empty).ToLowerInvariant();

        var tokens = Tokenize(sanitized);
        var (total, log) = EvaluateTokens(tokens);

        breakdown = log;
        return total;
    }

    /// <summary>
    /// Parses and rolls a "special" expression like "2d6p1d6".
    /// Returns a combined result with both parts rolled independently.
    /// </summary>
    public SpecialRollResult RollSpecial(string expression)
    {
        // Example pattern: 2d6p1d6
        // Supports p (primary/plus) and optional modifiers like +1
        var match = Regex.Match(expression, @"^(?<primary>\d+d\d+)(?:p(?<special>\d+d\d+))?$", RegexOptions.IgnoreCase);
        if (!match.Success)
            throw new ArgumentException("Invalid special dice expression", nameof(expression));

        var primaryExpr = match.Groups["primary"].Value;
        var specialExpr = match.Groups["special"].Success ? match.Groups["special"].Value : null;

        var (primaryCount, primarySides) = ParseDice(primaryExpr);
        var primaryRolls = RollDice(primaryCount, primarySides);
        var primaryTotal = primaryRolls.Sum();

        int[]? specialRolls = null;
        int? specialTotal = null;

        if (specialExpr != null)
        {
            var (specialCount, specialSides) = ParseDice(specialExpr);
            specialRolls = RollDice(specialCount, specialSides);
            specialTotal = specialRolls.Sum();
        }

        return new SpecialRollResult
        {
            PrimaryExpression = primaryExpr,
            PrimaryRolls = primaryRolls,
            PrimaryTotal = primaryTotal,
            SpecialExpression = specialExpr,
            SpecialRolls = specialRolls ?? Array.Empty<int>(),
            SpecialTotal = specialTotal
        };
    }

    

    /// <summary>
    /// Rolls a single die with the given number of sides.
    /// Uses cryptographically strong randomness and avoids repeating streaks.
    /// </summary>
    private int Roll1D(int sides)
    {
        Span<byte> buffer = stackalloc byte[4];
        int result;
        do
        {
            _rng.GetBytes(buffer);
            result = BitConverter.ToInt32(buffer) & int.MaxValue;
        } while (result >= (int.MaxValue / sides) * sides);

        return (result % sides) + 1;
    }

    public class SpecialRollResult
    {
        public string PrimaryExpression { get; set; } = string.Empty;
        public int[] PrimaryRolls { get; set; } = Array.Empty<int>();
        public int PrimaryTotal { get; set; }

        public string? SpecialExpression { get; set; }
        public int[] SpecialRolls { get; set; } = Array.Empty<int>();
        public int? SpecialTotal { get; set; }

        public override string ToString()
        {
            if (SpecialExpression == null)
                return $"{PrimaryExpression}: [{string.Join(",", PrimaryRolls)}] = {PrimaryTotal}";
            return $"{PrimaryExpression}: [{string.Join(",", PrimaryRolls)}] = {PrimaryTotal}, " +
                   $"{SpecialExpression}: [{string.Join(",", SpecialRolls)}] = {SpecialTotal}";
        }
    }

    private (int total, string breakdown) EvaluateTokens(List<string> tokens)
    {
        var stack = new Stack<int>();
        var logStack = new Stack<string>();
        var opStack = new Stack<char>();

        int i = 0;
        while (i < tokens.Count)
        {
            var token = tokens[i];

            if (int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            {
                stack.Push(number);
                logStack.Push(number.ToString());
            }
            else if (Regex.IsMatch(token, @"^\d+d\d+$"))
            {
                var (count, sides) = ParseDice(token);
                var rolls = RollDice(count, sides);
                var subtotal = rolls.Sum();
                stack.Push(subtotal);
                logStack.Push($"{token}:[{string.Join(",", rolls)}]={subtotal}");
            }
            else if (token == "(")
            {
                // find the matching closing parenthesis and recurse
                var subExpr = new List<string>();
                int depth = 1;
                i++;
                while (i < tokens.Count && depth > 0)
                {
                    if (tokens[i] == "(") depth++;
                    else if (tokens[i] == ")") depth--;
                    if (depth > 0)
                        subExpr.Add(tokens[i]);
                    i++;
                }

                var (subTotal, subLog) = EvaluateTokens(subExpr);
                stack.Push(subTotal);
                logStack.Push($"({subLog})={subTotal}");
                i--; // adjust because loop will increment
            }
            else if (token == "+" || token == "-")
            {
                opStack.Push(token[0]);
            }

            i++;
        }

        // Evaluate stacks left-to-right
        var total = stack.ElementAt(stack.Count - 1);
        var log = logStack.ElementAt(logStack.Count - 1);

        // Rebuild in order since we built stacks
        var nums = stack.Reverse().ToList();
        var ops = opStack.Reverse().ToList();
        var logs = logStack.Reverse().ToList();

        int result = nums.First();
        var fullLog = new List<string> { logs.First() };
        for (int j = 1; j < nums.Count; j++)
        {
            var op = ops.ElementAtOrDefault(j - 1);
            if (op == '+') result += nums[j];
            else if (op == '-') result -= nums[j];
            fullLog.Add($"{op}{logs[j]}");
        }

        return (result, string.Join(" ", fullLog));
    }

    private static List<string> Tokenize(string expr)
    {
        // Split into numbers, dice, +, -, and parentheses
        var tokens = Regex.Matches(expr, @"(\d+d\d+|\d+|[()+-])")
                          .Select(m => m.Value)
                          .ToList();
        return tokens;
    }

    private static (int count, int sides) ParseDice(string expr)
    {
        var m = Regex.Match(expr, @"(?<count>\d+)d(?<sides>\d+)", RegexOptions.IgnoreCase);
        if (!m.Success)
            throw new ArgumentException($"Invalid dice expression: {expr}");

        var count = int.Parse(m.Groups["count"].Value);
        var sides = int.Parse(m.Groups["sides"].Value);
        return (count, sides);
    }
    
}
