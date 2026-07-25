using System.Text.Json;
using System.Text.Json.Serialization;
using GameStore.API.Services;

namespace GameStore.API.Helpers;

public class HashidsJsonConverterFactory : JsonConverterFactory
{
    private readonly HashidService _hashidService;

    public HashidsJsonConverterFactory(HashidService hashidService)
    {
        _hashidService = hashidService;
    }

    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(int) || typeToConvert == typeof(int?);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(int))
            return new IntConverter(_hashidService);
        return new NullableIntConverter(_hashidService);
    }

    private class IntConverter : JsonConverter<int>
    {
        private readonly HashidService _hashidService;
        public IntConverter(HashidService hashidService) => _hashidService = hashidService;

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var hashid = reader.GetString();
            return hashid is not null ? _hashidService.Decode(hashid) : 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(_hashidService.Encode(value));
        }
    }

    private class NullableIntConverter : JsonConverter<int?>
    {
        private readonly HashidService _hashidService;
        public NullableIntConverter(HashidService hashidService) => _hashidService = hashidService;

        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            var hashid = reader.GetString();
            return hashid is not null ? _hashidService.Decode(hashid) : null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(_hashidService.Encode(value.Value));
            else
                writer.WriteNullValue();
        }
    }
}