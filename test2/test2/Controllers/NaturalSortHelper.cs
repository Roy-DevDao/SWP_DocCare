using System.Text.RegularExpressions;

namespace test2.Controllers
{
    public static class NaturalSortHelper
    {
        public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> source, Func<T, string> selector, bool descending = false)
        {
            int maxDigits = source
                .SelectMany(item => Regex.Matches(selector(item), @"\d+").Cast<Match>())
                .Max(m => m.Value.Length);

            Func<T, IEnumerable<object>> projection = item => Regex
                .Split(selector(item), @"(\d+)")
                .Select(part => int.TryParse(part, out var number) ? (object)number : part.PadLeft(maxDigits, ' '));

            return descending ? source.OrderByDescending(projection) : source.OrderBy(projection);
        }
    }
}