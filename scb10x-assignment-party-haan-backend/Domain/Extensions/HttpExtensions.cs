using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace scb10x_assignment_party_haan_backend.Domain.Extensions
{
    public static class HttpExtensions
    {
        private const string Authorization = "Authorization";
        private const string Bearer = "Bearer ";

        public static string GetUserId(this HttpContext httpContext)
        {
            var user = httpContext.User;
            var id = user?.Claims?.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sub)?.Value;

            return id;
        }

        public static bool HasAuthorization(this HttpContext httpContext) =>
            !string.IsNullOrWhiteSpace(GetJwt(httpContext));

        private static string GetAuthorization(this HttpContext httpContext)
        {
            var authorization = string.Empty;
            if (httpContext.Request.Headers.ContainsKey(Authorization))
                authorization = httpContext.Request.Headers[Authorization];

            return authorization;
        }

        private static string GetJwt(this HttpContext httpContext)
        {
            var authorization = GetAuthorization(httpContext);
            return !string.IsNullOrWhiteSpace(authorization) ? authorization.Substring(Bearer.Length)?.Trim() : null;
        }
    }
}
