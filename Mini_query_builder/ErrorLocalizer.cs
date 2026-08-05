using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SqlBuilder
{
    public class ErrorLocalizer
    {
        private Dictionary<string, string> _messages = new();

        public void LoadLanguage(string languageCode)
        {
            string filePath = $"Locales/errors.{languageCode}.json";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Warning] Language file not found: {filePath}");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                _messages = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString) 
                            ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to load language file: {ex.Message}");
            }
        }

        public string Get(string key)
        {
            return _messages.TryGetValue(key, out string? message) ? message : $"[{key}]";
        }
    }
}