using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Smalec.Lib.Shared.Helpers
{
    public static class EnumHelper
    {
        public static T GetByEnumMemberValue<T>(string value)
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (string.Equals(GetEnumMemberAttrValue(enumValue), value))
                {
                    return enumValue;
                }
            }

            return default;
        }

        public static string GetEnumMemberAttrValue<T>(T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if (attr != null)
            {
                return attr.Value;
            }

            return null;
        }
    }
}
