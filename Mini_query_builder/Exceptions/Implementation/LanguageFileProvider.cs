using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exceptions.Abstractions;

namespace SqlBuilder.Exceptions.Implementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class LanguageFileProvider : ILanguageFileProvider
    {
        public bool Exists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
    }

}
