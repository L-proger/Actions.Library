using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Actions.Library.SystemProcess;

namespace Actions {
    public static class Download {
        public static async Task<byte[]?> BytesFromUrl(string url) {
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(url))
                return result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;
        }

        public static async Task<string?> FileFromUrl(string url, string outputFilePath) {
            using var client = new HttpClient();
            using var result = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if(!result.IsSuccessStatusCode) {
                    return null;
            }
            using var responseStream = result.Content.ReadAsStream();
            using var destinationFileStream = new FileStream(outputFilePath, FileMode.CreateNew);
            responseStream.CopyTo(destinationFileStream);
            return outputFilePath;
        }
    }
}
