using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using DustedCodes.Blog.Config;
using DustedCodes.Blog.Data;
using DustedCodes.Blog.Feeds;
using DustedCodes.Blog.Services;
using NSubstitute;
using NUnit.Framework;

namespace DustedCodes.Blog.Tests
{
    [TestFixture]
    public class FeedServiceTests
    {
        private IArticleRepository _articleRepository;
        private IFeedBuilder _feedBuilder;
        private IAppConfig _appConfig;
        private IFeedService _sut;

        [SetUp]
        public void Setup()
        {
            _articleRepository = Substitute.For<IArticleRepository>();
            _feedBuilder = Substitute.For<IFeedBuilder>();
            _appConfig = Substitute.For<IAppConfig>();
            _sut = new FeedService(_articleRepository, _feedBuilder, _appConfig);
        }

        [TearDown]
        public void TearDown()
        {
            _articleRepository = null;
            _feedBuilder = null;
            _appConfig = null;
            _sut = null;
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Gets_Article_From_Repository_Only_Once()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 10;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _articleRepository.ReceivedWithAnyArgs(1).GetMostRecentAsync(0);
        }

        [Test]
        public void GetFeed__With_Item_Count_5__Gets_5_Article_From_Repository()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _articleRepository.Received().GetMostRecentAsync(5);
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Sets_Feed_Url_Once()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.ReceivedWithAnyArgs(1).SetFeedUrl("");
        }

        [Test]
        public void GetFeed__With_Feed_Url__Sets_Given_Url_In_Feed_Builder()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.Received().SetFeedUrl(feedUrl);
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Sets_Feed_Title_Once()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.ReceivedWithAnyArgs(1).SetFeedTitle("");
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Sets_Given_Title_In_Feed_Builder()
        {
            const string feedUrl = "feedUrl";
            const string feedTitle = "any title comes here";
            const int itemCount = 5;
            _appConfig.BlogTitle.Returns(feedTitle);

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.Received().SetFeedTitle(feedTitle);
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Sets_Feed_Description_Once()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.ReceivedWithAnyArgs(1).SetFeedDescription("");
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Sets_Given_Description_In_Feed_Builder()
        {
            const string feedUrl = "feedUrl";
            const string feedDescription = "feedDescription";
            const int itemCount = 5;
            _appConfig.BlogDescription.Returns(feedDescription);

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.Received().SetFeedDescription(feedDescription);
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Builds_Feed_Once()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.ReceivedWithAnyArgs(1).Build(null);
        }

        [Test]
        public void GetFeed__With_Any_Arguments__Builds_Feed_With_Article_From_Repository()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;
            IEnumerable<Article> articles = new List<Article>();
            _articleRepository.GetMostRecentAsync(0).ReturnsForAnyArgs(Task.FromResult(articles));

            _sut.GetFeedAsync(feedUrl, itemCount);

            _feedBuilder.Received().Build(articles);
        }

        [Test]
        public async Task GetFeed__With_Any_Arguments__Returns_Feed_From_Builder()
        {
            const string feedUrl = "feedUrl";
            const int itemCount = 5;
            var feed = new SyndicationFeed();
            _feedBuilder.Build(null).ReturnsForAnyArgs(feed);

            var actual = await _sut.GetFeedAsync(feedUrl, itemCount);

            Assert.AreSame(feed, actual);
        }
    }
}