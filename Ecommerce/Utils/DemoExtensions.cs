using System;

namespace Ecommerce.Utils
{
    public static class DemoExtensions
    {
        public static string ToShortDateString(this DateTime input)
        {
            return input.ToString("d");
        }
    }
}