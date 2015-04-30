using System.IO;
using System.Threading.Tasks;

namespace DustedCodes.Blog.IO
{
    public sealed class FilesReader : IFilesReader
    {
        public async Task<string> ReadAllTextAsync(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}