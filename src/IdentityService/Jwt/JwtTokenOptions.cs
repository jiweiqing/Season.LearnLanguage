﻿namespace IdentityService.Host
{
    public class JwtTokenOptions
    {
        public const string SectionName = "JwtToken";
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
