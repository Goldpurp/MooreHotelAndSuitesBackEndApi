namespace MooreHotelAndSuites.Application.Common
{
    public static class NameHelper
    {
        public static string Normalize(string input)
        {
            return string.Join(" ",
                input
                    .Trim()
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
