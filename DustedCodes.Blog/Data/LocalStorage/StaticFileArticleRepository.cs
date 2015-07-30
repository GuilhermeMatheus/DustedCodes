using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DustedCodes.Blog.IO;

namespace DustedCodes.Blog.Data.LocalStorage
{
    public sealed class StaticFileArticleRepository : IArticleRepository
    {
        private readonly string _articleDirectoryPath;
        private readonly IArticleParser _articleParser;
        private readonly IDirectoryReader _directoryReader;

        public StaticFileArticleRepository(
            string articleDirectoryPath, 
            IArticleParser articleParser, 
            IDirectoryReader directoryReader)
        {
            _articleDirectoryPath = articleDirectoryPath;
            _articleParser = articleParser;
            _directoryReader = directoryReader;
        }

        public async Task<Article> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("id cannot be null or empty.");

            var filePath = string.Format("{0}\\{1}.html", _articleDirectoryPath, id);
            var fileInfo = new FileInfo(filePath);

            return await _articleParser.ParseAsync(fileInfo);
        }

        public async Task<ICollection<Article>> GetAllOrderedByDateAsync()
        {
            const string searchPattern = "*.html";
            var files = _directoryReader.GetFiles(_articleDirectoryPath, searchPattern);

            if (files == null || files.Length == 0)
                return null;

            var articles = new List<Article>();

            foreach (var file in files)
            {
                var article = await _articleParser.ParseAsync(file);
                articles.Add(article);
            }

            return articles.OrderByDescending(a => a.Metadata.PublishDateTime).ToList();
        }
    }
}