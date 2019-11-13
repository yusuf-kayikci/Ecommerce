using Newtonsoft.Json;

namespace Ecommerce.Utils
{
    public static class DemoUtils
    {
        public static string Encode(object input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}