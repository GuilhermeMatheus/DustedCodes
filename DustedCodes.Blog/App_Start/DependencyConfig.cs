using DustedCodes.Blog.Caching;
using DustedCodes.Blog.Config;
using DustedCodes.Blog.Controllers;
using DustedCodes.Blog.Data;
using DustedCodes.Blog.Data.LocalStorage;
using DustedCodes.Blog.Feeds;
using DustedCodes.Blog.Helpers;
using DustedCodes.Blog.IO;
using DustedCodes.Blog.Services;
using DustedCodes.Blog.ViewModels;
using Ninject;

namespace DustedCodes.Blog
{
    public static class DependencyConfig
    {
        public static void Setup(IKernel kernel)
        {
            IAppConfig appConfig = new AppConfig();
            kernel.Bind<IAppConfig>().To<AppConfig>();

            kernel.Bind<ITextReaderFactory>().To<TextReaderFactory>();

            kernel.Bind<IArticleCache>().ToConstant(MemoryArticleCache.Instance);
            kernel.Bind<IArticleParser>().To<ArticleParser>();
            kernel.Bind<IArticleRepository>().To<StaticFileArticleRepository>()
                .WithConstructorArgument("articleDirectoryPath", appConfig.ArticlesDirectoryPath);

            kernel.Bind<IFeedBuilder>().To<FeedBuilder>();
            kernel.Bind<IFeedItemConverter>().To<FeedItemConverter>();

            kernel.Bind<IUrlEncoder>().To<UrlEncoder>();
            kernel.Bind<IUrlGenerator>().To<UrlGenerator>();

            kernel.Bind<IDirectoryReader>().To<DirectoryReader>();

            kernel.Bind<IArticleService>().To<ArticleService>();
            kernel.Bind<IFeedService>().To<FeedService>();

            kernel.Bind<IViewModelFactory>().To<ViewModelFactory>();

            kernel.Bind<BlogController>().ToSelf().WithConstructorArgument("pageSize", appConfig.BlogPageSize);
        }
    }
}