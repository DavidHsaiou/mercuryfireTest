using System.Text.Json.Serialization;

namespace mercuryfireTest.Model;

public class Myoffice
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
}