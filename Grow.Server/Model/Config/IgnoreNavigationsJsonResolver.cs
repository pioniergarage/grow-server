using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Grow.Server.Model.Config
{
    public class IgnoreNavigationsJsonResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
            {
                if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal)
                {
                    prop.ShouldSerialize = _ => false;
                }
            }
            return prop;
        }
    }
}
