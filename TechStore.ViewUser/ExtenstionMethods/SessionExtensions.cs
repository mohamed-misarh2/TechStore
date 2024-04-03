﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace TechStore.ViewUser.ExtenstionMethods
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}