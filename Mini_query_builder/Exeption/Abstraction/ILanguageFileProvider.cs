using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exeption.Implementations;

namespace SqlBuilder.Exeption.Abstractions
{
    public interface ILanguageFileProvider
    {
        bool Exists(string path);
        string ReadAllText(string path);
    }

}