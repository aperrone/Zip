using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Compression
{
    public class Zip
    {
        public static Task<Stream> CompressAsync(IEnumerable<string> paths)
        {
            var streamInfos = paths.Select(name => ((Stream)new FileStream(name, FileMode.Open), name));
            return CompressAsync(streamInfos);
        }

        public static Task<Stream> CompressAsync(Stream stream, string name)
        {
            return CompressAsync(new (Stream Stream, string Name)[] { (stream, name) });
        }

        public static async Task<Stream> CompressAsync(IEnumerable<(Stream Stream, string Name)> streamInfos)
        {
            var output = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(output, ZipArchiveMode.Create, true))
            {
                foreach (var item in streamInfos)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(item.Name);
                    using (Stream stream = entry.Open())
                    {
                        await item.Stream.CopyToAsync(stream);
                    }
                }
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public static Stream Compress(IEnumerable<string> paths)
        {
            var streamInfos = paths.Select(name => ((Stream)new FileStream(name, FileMode.Open), name));
            return Compress(streamInfos);
        }

        public static Stream Compress(Stream stream, string name)
        {
            return Compress(new (Stream Stream, string Name)[] { (stream, name) });
        }

        public static Stream Compress(IEnumerable<(Stream Stream, string Name)> streamInfos)
        {
            var output = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(output, ZipArchiveMode.Create, true))
            {
                foreach (var item in streamInfos)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(item.Name);
                    using (Stream stream = entry.Open())
                    {
                        item.Stream.CopyTo(stream);
                    }
                }
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }
}
