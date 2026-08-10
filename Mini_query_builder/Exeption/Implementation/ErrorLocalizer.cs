using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exeption.Abstractions;

namespace SqlBuilder.Exeption.Implementations
{
    internal sealed class ErrorLocalizer : IErrorLocalizer
    {
        private readonly ILanguageFileProvider _fileProvider;
        private Dictionary<string, string> _messages = [];

        public ErrorLocalizer(ILanguageFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public void LoadLanguage(string languageCode)
        {
            var filePath = $"Exeption/Json/errors.{languageCode}.json";

            if (!_fileProvider.Exists(filePath))
            {
                Console.WriteLine($"[Warning] Language file not found: {filePath}");
                return;
            }
            try
            {
                var jsonString = _fileProvider.ReadAllText(filePath);
                _messages = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)
                            ?? new Dictionary<string, string>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Error] Invalid JSON format in '{filePath}': {ex.Message}");
                _messages = new Dictionary<string, string>();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[Error] IO error while reading language file: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"[Error] Permission denied reading language file: {ex.Message}");
            }
        }

        public string GetMessageValue(string key)
        {
            return _messages.TryGetValue(key, out string? message) ? message : $"[{key}]";
        }
    }
}