﻿using OpenQA.Selenium;

namespace DustedCodes.Automation.Framework.Abstractions
{
    public static class AboutPage
    {
        public static bool IsAt()
        {
            var title = Driver.Instance.FindElement(By.CssSelector("#about > h1"));
            return title.Text == "About";
        }

        public static void GoToAtomFeed()
        {
            Driver.Instance.FindElement(By.LinkText("Atom")).Click();
        }
    }
}
