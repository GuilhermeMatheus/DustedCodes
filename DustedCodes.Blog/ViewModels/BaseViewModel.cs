﻿using DustedCodes.Blog.Configuration;
using DustedCodes.Blog.Helpers;

namespace DustedCodes.Blog.ViewModels
{
    public abstract class BaseViewModel
    {
        private readonly string _pageTitle;
        private const int CssVersion = 20;
        private const int JavaScriptVersion = 5;

        protected BaseViewModel(
            string pageTitle = null)
        {
            _pageTitle = pageTitle;
        }

        protected virtual IAppConfig AppConfig => new AppConfig();
        protected virtual IUrlGenerator UrlGenerator => new UrlGenerator();

        public virtual string PageTitle => $"{_pageTitle} - {BlogTitle}";
        public string BaseUrl => UrlGenerator.GetBaseUrl();
        public string BlogTitle => AppConfig.BlogTitle;
        public string BlogDescription => AppConfig.BlogDescription;
        public string DisqusShortname => AppConfig.DisqusShortname;
        public bool IsProductionEnvironment => AppConfig.IsProductionEnvironment;

        public string CssFilePath => UrlGenerator.GenerateContentUrl(
            $"~/Assets/Css/site{(IsDebugMode() ? "" : ".min")}.css?v={CssVersion}");

        public string JavaScriptFilePath => UrlGenerator.GenerateContentUrl(
            $"~/Assets/Scripts/main{(IsDebugMode() ? "" : ".min")}.js?v={JavaScriptVersion}");

        public string FullQualifiedContentUrl(string relativePath)
            => UrlGenerator.GenerateFullQualifiedContentUrl(relativePath);

        private static bool IsDebugMode()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}