using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exeption.Implementations;

namespace SqlBuilder.Exeption.Abstractions
{
    public interface IErrorLocalizer
    {
        public void LoadLanguage(string languageCode);

        public string GetMessageValue(string key);
    }
}