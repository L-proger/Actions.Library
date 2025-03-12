using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Tar;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace Actions {
    public static class Archive {

        public static void UnpackTarGz(string archiveFileName, string outDirectory) {
            using var fileStream = File.OpenRead(archiveFileName);
            using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            TarFile.ExtractToDirectory(gzipStream, outDirectory, true);
        }

        public static void UnpackTarBz2(string archiveFileName, string outDirectory) {
            using var fileStream = File.OpenRead(archiveFileName);
            using var bz2Stream = new BZip2Stream(fileStream, SharpCompress.Compressors.CompressionMode.Decompress, false);
            TarFile.ExtractToDirectory(bz2Stream, outDirectory, true);
        }

        public static void UnpackZip(string archiveFileName, string outDirectory) {
            using var fileStream = File.OpenRead(archiveFileName);

            using var zipArchive = SharpCompress.Archives.Zip.ZipArchive.Open(fileStream);
            foreach (var entry in zipArchive.Entries.Where(entry => !entry.IsDirectory)) {
                entry.WriteToDirectory(outDirectory, new ExtractionOptions() {
                    ExtractFullPath = true,
                    Overwrite = true
                });
            }
        }
    }
}
