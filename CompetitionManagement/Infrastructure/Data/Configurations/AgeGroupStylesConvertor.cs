using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class AgeGroupStylesConvertor()
    : ValueConverter<List<string>, string>(
        v => JsonSerializer.Serialize(
            v,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }),
        v => string.IsNullOrEmpty(v)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(
                v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<string>()
    );
