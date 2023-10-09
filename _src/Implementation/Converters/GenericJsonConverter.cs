using System;
using Newtonsoft.Json;

namespace Implementation.Converters
{
    public class GenericJsonConverter<TInterface, TConcreteType> : JsonConverter 
        where TInterface : class 
        where TConcreteType : class, TInterface
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TInterface));
        }
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(TConcreteType));
        }
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(TConcreteType));
        }
    }
}
