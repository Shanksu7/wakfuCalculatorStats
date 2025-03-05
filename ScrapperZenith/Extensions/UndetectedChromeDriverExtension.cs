using HtmlAgilityPack;
using OpenQA.Selenium;
using SeleniumUndetectedChromeDriver;

namespace ScrapperZenith.Extensions
{
    public static class UndetectedChromeDriverExtension
    {
        public static HtmlDocument _htmlDoc;
        public static string Get(this UndetectedChromeDriver driver, string xpath) => driver.FindElement(By.XPath(xpath)).Text;
        public static string Get(this string xpath)
        {
            xpath = xpath.Replace("\r", "").Replace("\n", "");
            return _htmlDoc.DocumentNode.SelectSingleNode(xpath).InnerText;
        }
        public static HtmlNode GetNode(this string xpath)
        {
            xpath = xpath.Replace("\r", "").Replace("\n", "");
            return _htmlDoc.DocumentNode.SelectSingleNode(xpath);
        }
    }
}
