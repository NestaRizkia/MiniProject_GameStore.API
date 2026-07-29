using System.Net.Http.Json;
using System.Text.Json;
using GameStore.API.Configuration;
using Microsoft.Extensions.Options;

namespace GameStore.API.Middlewares;

public class KeycloakAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UMAOptions _options;
    private static readonly HttpClient _httpClient = new();

    private static readonly Dictionary<string, string> MethodToScope = new(StringComparer.OrdinalIgnoreCase)
    {
        ["GET"] = "Read",
        ["POST"] = "Create",
        ["PATCH"] = "Update",
        ["DELETE"] = "Delete"
    };

    private static readonly Dictionary<string, string> ScopeOverrides = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/details"] = "Read",
        ["/remove"] = "Delete",
        ["/update"] = "Update"
    };

    public KeycloakAuthorizationMiddleware(RequestDelegate next, IOptions<UMAOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;

        if (path == "/" || path == null)
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = 401;
            return;
        }

        var mapping = _options.ResourceMappings
            .FirstOrDefault(m => path.StartsWith(m.PathPrefix, StringComparison.OrdinalIgnoreCase));

        if (mapping == null)
        {
            context.Response.StatusCode = 403;
            return;
        }

        var scope = ResolveScope(context.Request.Method, path, mapping.PathPrefix);
        if (scope == null)
        {
            context.Response.StatusCode = 405;
            return;
        }

        var authorized = await CheckUmaPermissionAsync(context, mapping.ResourceName, scope);

        if (!authorized)
        {
            context.Response.StatusCode = 403;
            return;
        }

        await _next(context);
    }

    private static string? ResolveScope(string method, string path, string pathPrefix)
    {
        var suffix = path[pathPrefix.Length..];

        if (ScopeOverrides.TryGetValue(suffix, out var scope))
            return scope;

        return MethodToScope.GetValueOrDefault(method);
    }

    private async Task<bool> CheckUmaPermissionAsync(HttpContext context, string resourceName, string scope)
    {
        try
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
                return false;

            var formData = new Dictionary<string, string>
            {
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:uma-ticket",
                ["audience"] = _options.ClientId,
                ["permission"] = $"{resourceName}#{scope}",
                ["response_mode"] = "decision",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret
            };

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_options.AuthServerUrl}/realms/{_options.Realm}/protocol/openid-connect/token");

            request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.TryGetProperty("result", out var result) && result.GetBoolean();
        }
        catch
        {
            return false;
        }
    }
}
