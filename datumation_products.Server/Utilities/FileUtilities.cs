using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using datumation_products.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;

namespace datumation_products.Server.Utilities {
    public class FileHelpers {

        //private static async Task<string> ReadTextAsync(IFormFile formFile)
        //{
        //    var fileName = WebUtility.HtmlEncode(
        //     Path.GetFileName(formFile.FileName));
        //    using (BufferedStream bs = new BufferedStream(formFile.OpenReadStream(), 4096))
        //    {
        //        using (StreamReader sr = new StreamReader(bs))
        //        {
        //            string line;
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                byte[] buffer = new byte[0x1000];
        //                int numRead;
        //                while ((numRead = await sr.ReadAsync(buffer, 0, buffer.Length)) != 0)
        //                {
        //                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
        //                    sb.Append(text);
        //                }

        //                return sb.ToString();
        //            }
        //        }
        //    }
        //    using (var sourceStream = new StreamReader(formFile.OpenReadStream(),
        //        FileMode.Open, FileAccess.Read, FileShare.Read,
        //        bufferSize: 4096, useAsync: true))
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        byte[] buffer = new byte[0x1000];
        //        int numRead;
        //        while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        //        {
        //            string text = Encoding.Unicode.GetString(buffer, 0, numRead);
        //            sb.Append(text);
        //        }

        //        return sb.ToString();
        //    }
        //}
        public static async Task<string> ProcessFormFile (IFormFile formFile,
            ModelStateDictionary modelState) {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model 
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            MemberInfo property =
                typeof (FileUpload).GetProperty (
                    formFile.Name.Substring (formFile.Name.IndexOf (".") + 1));

            if (property != null) {
                var displayAttribute =
                    property.GetCustomAttribute (typeof (DisplayAttribute))
                as DisplayAttribute;

                if (displayAttribute != null) {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            var fileName = WebUtility.HtmlEncode (
                Path.GetFileName (formFile.FileName));

            if (formFile.ContentType.ToLower () != "text/plain") {
                modelState.AddModelError (formFile.Name,
                    $"The {fieldDisplayName}file ({fileName}) must be a text file.");
            }

            // Check the file length and don't bother attempting to
            // read it if the file contains no content. This check
            // doesn't catch files that only have a BOM as their
            // content, so a content length check is made later after 
            // reading the file's content to catch a file that only
            // contains a BOM.
            if (formFile.Length == 0) {
                modelState.AddModelError (formFile.Name,
                    $"The {fieldDisplayName}file ({fileName}) is empty.");
            } else if (formFile.Length > 10485760000000000) {
                modelState.AddModelError (formFile.Name,
                    $"The {fieldDisplayName}file ({fileName}) exceeds 1 MB.");
            } else {
                try {
                    string fileContents;

                    // The StreamReader is created to read files that are UTF-8 encoded. 
                    // If uploads require some other encoding, provide the encoding in the 
                    // using statement. To change to 32-bit encoding, change 
                    // new UTF8Encoding(...) to new UTF32Encoding().

                    var boundary = Extensions.FileExtensions.GetBoundary (formFile.ContentType);
                    var reader = new MultipartReader (boundary, formFile.OpenReadStream ());
                    var section = await reader.ReadNextSectionAsync ();

                    while (section != null) {
                        // process each image
                        const int chunkSize = 1024;
                        var buffer = new byte[chunkSize];
                        var bytesRead = 0;
                        // var fileName = Extensions.FileExtensions.GetFileName(section.ContentDisposition);

                        using (var stream = new FileStream (fileName, FileMode.Append)) {
                            do {
                                bytesRead = await section.Body.ReadAsync (buffer, 0, buffer.Length);
                                stream.Write (buffer, 0, bytesRead);

                            } while (bytesRead > 0);
                        }

                        section = await reader.ReadNextSectionAsync ();
                    }

                    using (
                        var reader2 =
                            new StreamReader (
                                formFile.OpenReadStream (),
                                new UTF8Encoding (encoderShouldEmitUTF8Identifier: false,
                                    throwOnInvalidBytes: true),
                                detectEncodingFromByteOrderMarks: true)) {
                        fileContents = await reader2.ReadToEndAsync ();

                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.
                        if (fileContents.Length > 0) {
                            return fileContents;
                        } else {
                            modelState.AddModelError (formFile.Name,
                                $"The {fieldDisplayName}file ({fileName}) is empty.");
                        }
                    }
                } catch (Exception ex) {
                    modelState.AddModelError (formFile.Name,
                        $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }

            return string.Empty;
        }
    }
}