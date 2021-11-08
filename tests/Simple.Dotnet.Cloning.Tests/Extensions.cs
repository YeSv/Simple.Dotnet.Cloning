using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Simple.Dotnet.Cloning.Tests
{
    public static class Extensions
    {
        public static bool HaveSameData<T>(this T instance, T clone) => JsonSerializer.Serialize(instance) == JsonSerializer.Serialize(clone);
    }
}
