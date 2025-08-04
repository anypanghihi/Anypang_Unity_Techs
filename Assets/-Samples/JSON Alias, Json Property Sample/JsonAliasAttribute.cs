using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sample.JsonAliasing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonAliasAttribute : Attribute
    {
        public string[] Aliases { get; }

        public JsonAliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}