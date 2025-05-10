using System.Text.Json;
using System.Text.Json.Serialization;

namespace netapi.Services
{
    /// <summary>
    /// Custom JSON converter to handle nullable integers that might come as strings from the external API
    /// </summary>
    public class NullableIntJsonConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }
                
                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
            
            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
