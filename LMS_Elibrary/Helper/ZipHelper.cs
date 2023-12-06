using LMS_Elibrary.Data;
using System.IO.Compression;

namespace LMS_Elibrary.Helper
{
    public static class ZipHelper
    {
        //public static byte[] CreateZipFile(string fileName, byte[] fileContent)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            var entry = archive.CreateEntry($"{fileName}", CompressionLevel.Fastest);
        //            using (var entryStream = entry.Open())
        //            {
        //                entryStream.Write(fileContent, 0, fileContent.Length);
        //            }
        //        }

        //        return memoryStream.ToArray();
        //    }
        //}
        public static byte[] CreateZipFileFromDocuments(List<Data.File> documents, string zipFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in documents.Where(d => d.FileData != null && d.FileData.Length > 0))
                    {
                        var entry = archive.CreateEntry($"{document.FileName}", CompressionLevel.Fastest);
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(document.FileData, 0, document.FileData.Length);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
