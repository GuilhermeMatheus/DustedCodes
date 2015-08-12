using System.Runtime.Caching;
using DustedCodes.Blog.Config;
using DustedCodes.Blog.Controllers;
using DustedCodes.Blog.Feeds;
using DustedCodes.Blog.Helpers;
using DustedCodes.Blog.ViewModels;
using DustedCodes.Core.Caching;
using DustedCodes.Core.Data;
using DustedCodes.Core.Data.LocalStorage;
using DustedCodes.Core.IO;
using DustedCodes.Core.Services;
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

            kernel.Bind<IArticleParser>().To<ArticleParser>();
            kernel.Bind<IArticleRepository>().To<StaticFileArticleRepository>()
                .WithConstructorArgument("articleDirectoryPath", appConfig.ArticlesDirectoryPath);

            kernel.Bind<IFeedItemConverter>().To<FeedItemConverter>();
            kernel.Bind<IFeedFactory>().To<FeedFactory>().WithConstructorArgument("maxItemCount", appConfig.FeedMaxItemCount);

            kernel.Bind<IUrlEncoder>().To<UrlEncoder>();
            kernel.Bind<IUrlGenerator>().To<UrlGenerator>();

            kernel.Bind<IDirectoryReader>().To<DirectoryReader>();

            if (appConfig.UseCache)
            {
                kernel.Bind<ICache>()
                    .To<ObjectCacheWrapper>()
                    .WithConstructorArgument("objectCache", MemoryCache.Default)
                    .WithConstructorArgument("defaultCacheItemPolicy", new CacheItemPolicy());
            }
            else
            {
                kernel.Bind<ICache>().To<NullCache>();
            }

            kernel.Bind<IArticleService>().To<ArticleService>();

            kernel.Bind<IViewModelFactory>().To<ViewModelFactory>();

            kernel.Bind<BlogController>().ToSelf().WithConstructorArgument("pageSize", appConfig.BlogPageSize);
        }
    }
}