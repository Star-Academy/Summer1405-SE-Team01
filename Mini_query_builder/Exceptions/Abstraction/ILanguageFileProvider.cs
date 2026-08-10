using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SqlBuilder.Exceptions.Implementations;

namespace SqlBuilder.Exceptions.Abstractions
{
    public interface ILanguageFileProvider
    {
        bool Exists(string path);
        string ReadAllText(string path);
    }

}