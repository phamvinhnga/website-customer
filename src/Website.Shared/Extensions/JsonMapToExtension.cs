﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Website.Shared.Extensions
{
    public static class JsonMapToExtension
    {
        public static TOutput JsonMapTo<TOutput>(this object input)
        {
            var value = ConvertToJson(input);
            return value.ConvertFromJson<TOutput>();
        }

        public static string ConvertToJson<TEntity>(this TEntity input)
        {
            return JsonConvert.SerializeObject(input, Formatting.None);
        }

        public static TEntity ConvertFromJson<TEntity>(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default(TEntity);
            }
            try
            {
                return JsonConvert.DeserializeObject<TEntity>(input);
            }
            catch
            {
                return default(TEntity);
            }
        }
    }
}
