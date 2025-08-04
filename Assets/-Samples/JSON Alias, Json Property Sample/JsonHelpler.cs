using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Sample.JsonAliasing
{
    public static class JsonHelper
    {
        public static T DeserializeWithAlias<T>(string json) where T : new()
        {
            JObject jsonObject = JObject.Parse(json);
            T obj = new T();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                // JsonAlias 어트리뷰트 확인
                var aliasAttr = property.GetCustomAttribute<JsonAliasAttribute>();
                string[] keys = aliasAttr?.Aliases ?? new string[] { property.Name }; // 없으면 기본 속성명 사용

                // JSON에서 해당 키 찾기
                foreach (string key in keys)
                {
                    if (jsonObject.TryGetValue(key, out JToken value))
                    {
                        property.SetValue(obj, value.ToObject(property.PropertyType));
                        break;
                    }
                }
            }

            return obj;
        }
    }
}