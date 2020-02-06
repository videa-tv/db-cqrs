using System;
using System.IO;
using System.Reflection;

namespace Videa.Data.CQRS.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetEmbeddedQuery(this Assembly assembly, string sqlQueryResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (string.IsNullOrWhiteSpace(sqlQueryResourceName))
            {
                throw new ArgumentNullException(nameof(sqlQueryResourceName));
            }

            using (var stream = assembly.GetManifestResourceStream(sqlQueryResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException(
                        $"Assembly ({assembly}) doesn't contain resource {sqlQueryResourceName}. " +
                        $"Existing resources : {string.Join(", ", assembly.GetManifestResourceNames())}");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

        }
    }
}