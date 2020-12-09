using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ism.Infrastructure.Model
{
    public class UnixTimestampJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime dateTime = DateTime.MinValue;

            if (reader.TokenType != JsonToken.Null)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    JToken token = JToken.Load(reader);
                    List<string> items = token.ToObject<List<string>>();
                    
                }
                else
                {
                    JValue jValue = new JValue(reader.Value);
                    switch (reader.TokenType)
                    {
                        case JsonToken.String:
                            //myCustomType = new MyCustomType((string)jValue);
                            break;
                        case JsonToken.Date:
                            //myCustomType = new MyCustomType((DateTime)jValue);
                            break;
                        case JsonToken.Boolean:
                            //myCustomType = new MyCustomType((bool)jValue);
                            break;
                        case JsonToken.Integer:
                            int i = (int)jValue;
                            //myCustomType = new MyCustomType(i);
                            break;
                        default:
                            Console.WriteLine("Default case");
                            Console.WriteLine(reader.TokenType.ToString());
                            break;
                    }
                }
            }

            return dateTime;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(BaseModel).IsAssignableFrom(objectType); ;
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
