﻿using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;
using System.Collections.Generic;

namespace PaypalCore.Models
{
    public static class PaypalConfigurationDto
    {

        static PaypalConfigurationDto()
        {
            
        }

        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode",mode}
            };
        }

        private static string GetAccessToken(string ClientId, string ClientSecret, string mode)
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, new Dictionary<string, string>()
            {
                {"mode",mode}
            }).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            APIContext apiContext = new APIContext(GetAccessToken(clientId, clientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }
    }
}
