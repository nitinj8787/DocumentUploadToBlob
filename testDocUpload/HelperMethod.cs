using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testDocUpload
{
    public static class HelperMethod
    {
        // add more extention if required
        public static readonly string[] PERMITTED_FILE_EXTENSION = { ".pdf" };
        
        
        
        /// <summary>
        /// return if file has allowed file type 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsPermittedFile(string filePath)
        {
            return PERMITTED_FILE_EXTENSION.Any(filePath.Contains);
        }
        
        /// <summary>
        /// get the full file name & path from gievn filename and path
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="downloadPath"></param>
        /// <returns></returns>
        public static string GetFileNamePath(string fileName, string downloadPath)
        {
            var a = downloadPath.EndsWith('/') ? downloadPath : downloadPath.Append('/');
            string downloadfile = string.Concat(a, fileName);
            return downloadfile;
        }

    }
}
