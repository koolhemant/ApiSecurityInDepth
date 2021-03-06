﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace ApiSecurityInDepth.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1.fullaccess", "Full access"),
                new ApiScope("api2.fullaccess", "Full access")
            };

        public static IEnumerable<ApiResource> ApiResources => 
            new List<ApiResource>
           {
                #region JWT
                // allows full access (via scope) to the api1 resource (which will end up as audience value)
                new ApiResource("api1", "API1", new[] { "profile", "email" })
                {
                    Scopes = { "api1.fullaccess" }
                },
                #endregion

                #region Reference token
                // // allows full access (via scope) to the api1 resource (which will end up as audience value)
                //new ApiResource("api1", "API1", new[] { "profile", "email" })
                //{
                //    Scopes = { "api1.fullaccess" },
                //    ApiSecrets = { new Secret("apisecret".Sha256()) }
                //},
                #endregion

                 // allows full access (via scope) to the api2 resource (which will end up as audience value)
                new ApiResource("api2", "API2", new[] { "profile", "email" })
                {
                    Scopes = { "api2.fullaccess" }
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "api1client",
                    ClientName = "API1 Client",
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    RequireConsent = false,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         "api2.fullaccess" }
                },
                new Client
                {
                    ClientId = "webclient",
                    ClientName = "API Security in Depth Web Client",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = { "https://localhost:44375/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44375/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44375/signout-callback-oidc" },
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         "api1.fullaccess" }
                },
               new Client
               {
                    ClientId = "api1jwtclient",
                    ClientSecrets =
                    {
                        new Secret
                        {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                            Value = "MIIDIDCCAgigAwIBAgIQf16IitnXMJtF3QAqTP2+uDANBgkqhkiG9w0BAQUFADAjMSEwHwYDVQQDDBhEZW1vIFNpZ25pbmcgQ2VydGlmaWNhdGUwHhcNMjAwODE4MDg1MjU3WhcNMjEwODE4MDkxMjU3WjAjMSEwHwYDVQQDDBhEZW1vIFNpZ25pbmcgQ2VydGlmaWNhdGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCzfBtM2Wj40oyVBYLfwHpiPEPqQ1Yc0qbZzDjggZgZ26hI50r+ZpYu097wp6IQqpdsG6659EYqwKiUnnYujtSt3HqxOl3aZp2vJFPKdmjOIncOj3SXjj++iRzwl6w3u5yiUtssQ4G4DzYDtRp0U/KcdNCv6jE+cNQc80a5zsXtJ9JhI5th9+cp1guICJrEOse4Uto4n2N2o6wUFgCwnvtILp7SjWKQo2yhv2mw+kEzvaoLkvVj0aDOgg1Jak+yGDjHHw46Gx7TmrugTSL/gwwD6qDVHtEBxQzFgu79tRKiYJP1GMD/L5Lhuvj4oqU1JGfVuT+OimrvxiZ8AlyUIOIpAgMBAAGjUDBOMA4GA1UdDwEB/wQEAwIFoDAdBgNVHSUEFjAUBggrBgEFBQcDAgYIKwYBBQUHAwEwHQYDVR0OBBYEFArzUT+ir9oRhMn0AaGvC+Ok3QXOMA0GCSqGSIb3DQEBBQUAA4IBAQBFpZa4CKSnmyZtRB7j3FKY782yuoskh2n7QtNEV3OYESuhOTGjWJr2W3gDItdLGXxjFwZwN7IbkMXW843h8J9hBQ2Y6D7J3c+0VfqsCOzxXQp0IPBWV2nslQdTMM1uD6CCGaur0xaq/OvWATwMijQHD+ZM4IOGBuMqwRR3UrBYlbQs0XdzPsO65MUt6mVSryBdJJAtVCzRD1sOteLOGqXtWJFBxmrFmYqh9gzm9ShHE5re1MSSEm1f3FOnUs89FTj5TNWxEhBiZ9NN/4rsZQmjmAEmLoDfay2FRkhpQElMG8JCL/q6/x3ZYAkb+cFUSVueQo8AUDpC0PiX71lq/yR9"
                        }
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.fullaccess" }
                },
                new Client
                {
                    ClientId = "webclientjwt",
                    ClientName = "API Security in Depth Web Client with Private Key JWT Client Authentication",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret
                        {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                            Value = "MIIDIDCCAgigAwIBAgIQf16IitnXMJtF3QAqTP2+uDANBgkqhkiG9w0BAQUFADAjMSEwHwYDVQQDDBhEZW1vIFNpZ25pbmcgQ2VydGlmaWNhdGUwHhcNMjAwODE4MDg1MjU3WhcNMjEwODE4MDkxMjU3WjAjMSEwHwYDVQQDDBhEZW1vIFNpZ25pbmcgQ2VydGlmaWNhdGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCzfBtM2Wj40oyVBYLfwHpiPEPqQ1Yc0qbZzDjggZgZ26hI50r+ZpYu097wp6IQqpdsG6659EYqwKiUnnYujtSt3HqxOl3aZp2vJFPKdmjOIncOj3SXjj++iRzwl6w3u5yiUtssQ4G4DzYDtRp0U/KcdNCv6jE+cNQc80a5zsXtJ9JhI5th9+cp1guICJrEOse4Uto4n2N2o6wUFgCwnvtILp7SjWKQo2yhv2mw+kEzvaoLkvVj0aDOgg1Jak+yGDjHHw46Gx7TmrugTSL/gwwD6qDVHtEBxQzFgu79tRKiYJP1GMD/L5Lhuvj4oqU1JGfVuT+OimrvxiZ8AlyUIOIpAgMBAAGjUDBOMA4GA1UdDwEB/wQEAwIFoDAdBgNVHSUEFjAUBggrBgEFBQcDAgYIKwYBBQUHAwEwHQYDVR0OBBYEFArzUT+ir9oRhMn0AaGvC+Ok3QXOMA0GCSqGSIb3DQEBBQUAA4IBAQBFpZa4CKSnmyZtRB7j3FKY782yuoskh2n7QtNEV3OYESuhOTGjWJr2W3gDItdLGXxjFwZwN7IbkMXW843h8J9hBQ2Y6D7J3c+0VfqsCOzxXQp0IPBWV2nslQdTMM1uD6CCGaur0xaq/OvWATwMijQHD+ZM4IOGBuMqwRR3UrBYlbQs0XdzPsO65MUt6mVSryBdJJAtVCzRD1sOteLOGqXtWJFBxmrFmYqh9gzm9ShHE5re1MSSEm1f3FOnUs89FTj5TNWxEhBiZ9NN/4rsZQmjmAEmLoDfay2FRkhpQElMG8JCL/q6/x3ZYAkb+cFUSVueQo8AUDpC0PiX71lq/yR9"
                        }
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = { "https://localhost:44375/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44375/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44375/signout-callback-oidc" },
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         "api1.fullaccess" }
                },
            };
    }
}