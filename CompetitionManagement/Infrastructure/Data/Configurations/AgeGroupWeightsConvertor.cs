using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class AgeGroupWeightsConvertor()
    : ValueConverter<List<int>, string>(
        v => JsonSerializer.Serialize(v,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }),
        v => string.IsNullOrEmpty(v)
            ? new List<int>()
            : JsonSerializer.Deserialize<List<int>>(
                v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<int>()
    );
