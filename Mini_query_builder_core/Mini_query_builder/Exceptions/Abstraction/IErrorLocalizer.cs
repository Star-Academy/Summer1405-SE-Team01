using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exceptions.Implementations;

namespace SqlBuilder.Exceptions.Abstractions
{
    public interface IErrorLocalizer
    {
        public void LoadLanguage(string languageCode);

        public string GetMessageValue(string key);
    }
}