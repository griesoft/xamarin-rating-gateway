using System;
using Newtonsoft.Json;

namespace Griesoft.Xamarin.RatingGateway.Cache
{
    internal sealed class PrimitiveJsonConverter : JsonConverter
    {
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanConvert(Type objectType) => objectType.IsPrimitive;
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (serializer.TypeNameHandling != TypeNameHandling.All)
            {
                writer.WriteValue(value);
            }

            writer.WriteStartObject();
            writer.WritePropertyName("$type", false);

            if (serializer.TypeNameAssemblyFormatHandling == TypeNameAssemblyFormatHandling.Full)
            {
                writer.WriteValue(value?.GetType().AssemblyQualifiedName);
            }
            else
            {
                writer.WriteValue(value?.GetType().FullName);
            }

            writer.WritePropertyName("$value", false);
            writer.WriteValue(value);
            writer.WriteEndObject();
        }
    }
}
