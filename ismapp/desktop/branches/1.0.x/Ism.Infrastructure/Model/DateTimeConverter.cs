using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ism.Infrastructure.Model
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter() : this(null) {}

        public DateTimeConverter(string format)
        {
            //yyyyMMddHHmmss.FFFFFFFK
            //format = "yyyyMMddHHmmss";
            if (string.IsNullOrEmpty(format))
                format = "yyyyMMddHHmmss";
            base.DateTimeFormat = format;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var r  = base.ReadJson(reader, objectType, existingValue, serializer);
                return r;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}
