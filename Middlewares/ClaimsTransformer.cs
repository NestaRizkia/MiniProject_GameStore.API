using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace GameStore.API.Middlewares;

public class ClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        if (identity == null)
        {
            return Task.FromResult(principal);
        }

        // 1. Extract Realm Roles
        var realmAccessClaim = identity.FindFirst("realm_access");
        if (realmAccessClaim != null && !string.IsNullOrWhiteSpace(realmAccessClaim.Value))
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(realmAccessClaim.Value);
                if (jsonDoc.RootElement.TryGetProperty("roles", out var rolesElement) && 
                    rolesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var roleName = role.GetString();
                        if (!string.IsNullOrEmpty(roleName) && !identity.HasClaim(ClaimTypes.Role, roleName))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        // 2. Extract Client Roles (Resource Access)
        var resourceAccessClaim = identity.FindFirst("resource_access");
        if (resourceAccessClaim != null && !string.IsNullOrWhiteSpace(resourceAccessClaim.Value))
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(resourceAccessClaim.Value);
                foreach (var clientProperty in jsonDoc.RootElement.EnumerateObject())
                {
                    if (clientProperty.Value.TryGetProperty("roles", out var clientRolesElement) && 
                        clientRolesElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in clientRolesElement.EnumerateArray())
                        {
                            var roleName = role.GetString();
                            if (!string.IsNullOrEmpty(roleName) && !identity.HasClaim(ClaimTypes.Role, roleName))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                            }
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        return Task.FromResult(principal);
    }
}
