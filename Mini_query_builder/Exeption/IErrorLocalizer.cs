using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SqlBuilder
{
    public interface IErrorLocalizer
    {
        public void LoadLanguage(string languageCode);

        public string GetMessageValue(string key);
    }
}