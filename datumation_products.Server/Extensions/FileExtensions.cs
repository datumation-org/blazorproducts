using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Server.Extensions {
    public class FileExtensions {
        public static bool IsMultipartContentType (string contentType) {
            return !string.IsNullOrEmpty (contentType) &&
                contentType.IndexOf ("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string GetBoundary (string contentType) {
            var elements = contentType.Split (' ');
            var element = elements.Where (entry => entry.StartsWith ("boundary=")).First ();
            var boundary = element.Substring ("boundary=".Length);
            // Remove quotes
            if (boundary.Length >= 2 && boundary[0] == '"' &&
                boundary[boundary.Length - 1] == '"') {
                boundary = boundary.Substring (1, boundary.Length - 2);
            }
            return boundary;
        }

        public static string GetFileName (string contentDisposition) {
            return contentDisposition
                .Split (';')
                .SingleOrDefault (part => part.Contains ("filename"))
                .Split ('=')
                .Last ()
                .Trim ('"');
        }
    }
}