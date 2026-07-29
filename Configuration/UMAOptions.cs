namespace GameStore.API.Configuration;

public class UMAOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Realm { get; set; } = string.Empty;
    public string AuthServerUrl { get; set; } = string.Empty;
    public List<ResourceMapping> ResourceMappings { get; set; } = [];
}
