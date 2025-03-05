using Domain.Enums;
using Domain.Models.Stats;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ScrapperZenith.Extensions;
using SeleniumUndetectedChromeDriver;
using System.Reflection;


namespace ScrapperZenith
{

    public class Scrapper
    {
        static UndetectedChromeDriver driver;
        static HashSet<string> asd = new HashSet<string>();
        public static async Task Get()
        {

            using (driver = UndetectedChromeDriver.Create(driverExecutablePath: await new ChromeDriverInstaller().Auto(force: true), headless: true))
                try
                {
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var options = new ChromeOptions();
                    options.AddArguments(new string[]
                    {
       "--headless=new",
        "--whitelisted-ips=190.143.44.188",
        "--disable-gpu",
        "--no-sandbox",
        "--disable-dev-shm-usage",
        "--disable-extensions",
        "disable-infobars"
                });

                    await GetZenithData("https://zenithwakfu.com/builder/6e185");
                    Console.ReadKey();
                    driver.Quit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    driver?.Quit();
                    driver?.Dispose();
                }
        }


        public static async Task<StatsCollection> GetZenithData(string url)
        {
            //https://zenithwakfu.com/builder/e5b6b
            //driver.GoToUrl("https://zenithwakfu.com/builder/6e185");
            using (driver = UndetectedChromeDriver.Create(driverExecutablePath: await new ChromeDriverInstaller().Auto(force: true), headless: true))
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var options = new ChromeOptions();
                options.AddArguments(new string[]
                {
       "--headless=new",
        "--whitelisted-ips=190.143.44.188",
        "--disable-gpu",
        "--no-sandbox",
        "--disable-dev-shm-usage",
        "--disable-extensions",
        "disable-infobars"
            });
                driver.GoToUrl(url);
                driver.Manage().Cookies.AddCookie(new Cookie(name: "lang", value: "es", domain: ".zenithwakfu.com", path: "/", expiry: DateTime.Now.AddDays(1)));
                driver.Navigate().Refresh();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                // Wait until the element contains some text
                IWebElement element = wait.Until(driver =>
                {
                    try
                    {
                        IWebElement el = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]"));
                        if (!string.IsNullOrWhiteSpace(el.Text))
                        {
                            return el;
                        }
                        return null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });

                var htmlContent = driver.ExecuteScript("return document.documentElement.outerHTML;") as string;

                //var path = @"D:\User\Downloads\untitled.html";
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(htmlContent);
                //doc.LoadHtml(File.ReadAllText(path));
                UndetectedChromeDriverExtension._htmlDoc = doc;

                HtmlNode parentNode = doc.DocumentNode.SelectSingleNode("//div[@id=\"vertical-tabpanel-4\"]/div[1]/div[2]/div[2]");
                Console.WriteLine("*****************************************************");
                var title = "/html/body/div[1]/div/div/div/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]\r\n".Get();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(title);
                Console.ResetColor();
                var helmSubli = parentNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var necklaceSubli = parentNode.ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var armorSubli = parentNode.ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var ring1 = parentNode.ChildNodes[3].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var ring2 = parentNode.ChildNodes[4].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var bootsSubli = parentNode.ChildNodes[5].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var capeSubli = parentNode.ChildNodes[6].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var shouldersSubli = parentNode.ChildNodes[7].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var beltSubli = parentNode.ChildNodes[8].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                var wep = parentNode.ChildNodes[9].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.GetStats(title);
                Console.WriteLine("*****************************************************");
                var subliStats = helmSubli + necklaceSubli + armorSubli + ring1 + ring2 + bootsSubli + capeSubli + shouldersSubli + beltSubli + wep;

                var epic = "/html/body/div[1]/div/div/div/div[3]/div[2]/div[5]/div/div[2]/div[2]/div[11]/div[1]/div/div[2]/div/div\r\n".Get();
                var relic = "/html/body/div[1]/div/div/div/div[3]/div[2]/div[5]/div/div[2]/div[2]/div[11]/div[2]/div/div[2]/div/div\r\n".Get();

                var lvl = "/html/body/div[1]/div/div/div/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]\r\n".Get();
                var pdv = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div".Get();
                var ap = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[1]/div[2]/div[2]/div[2]/div".Get();
                var mp = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[1]/div[2]/div[3]/div[2]/div".Get();
                var receivedarmor = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[2]/div[2]/div[1]/div[2]/div".Get();
                var givenArmor = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[2]/div[2]/div[2]/div[2]/div".Get();

                var water = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[1]/div[1]/div[2]/div".Get();
                var air = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[3]/div[1]/div[2]/div".Get();
                var earth = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[2]/div[1]/div[2]/div\r\n".Get();
                var fire = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[4]/div[1]/div[2]/div\r\n".Get();
                var waterResist = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[1]/div[2]/div[2]/div\r\n".Get();
                var airResist = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[3]/div[2]/div[2]/div".Get();
                var earthResist = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[2]/div[2]/div[2]/div\r\n".Get();
                var fireResist = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[3]/div[2]/div[4]/div[2]/div[2]/div\r\n".Get();
                var di = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[1]/div[2]/div\r\n".Get();
                var critrate = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[3]/div[2]/div\r\n".Get();
                var finalHeal = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[2]/div[2]/div".Get();
                var block = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[4]/div[2]/div\r\n".Get();
                var init = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[5]/div[2]/div\r\n".Get();
                var range = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[6]/div[2]/div\r\n".Get();
                var dodge = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[7]/div[2]/div\r\n".Get();
                var tackle = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[8]/div[2]/div\r\n".Get();
                var wisdom = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[9]/div[2]/div\r\n".Get();
                var pp = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[10]/div[2]/div\r\n".Get();
                var controle = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[11]/div[2]/div\r\n".Get();
                var will = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[4]/div[2]/div[12]/div[2]/div\r\n".Get();
                var domainCrit = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[1]/div[2]/div\r\n".Get();
                var rear = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[3]/div[2]/div".Get();
                var mele = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[5]/div[2]/div".Get();
                var heal = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[7]/div[2]/div\r\n".Get();
                var resistCrit = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[2]/div[2]/div\r\n".Get();
                var rearResit = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[4]/div[2]/div\r\n".Get();
                var dist = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[6]/div[2]/div\r\n".Get();
                var berserker = "/html/body/div[1]/div/div/div/div[3]/div[1]/div[5]/div[2]/div[8]/div[2]/div\r\n".Get();

                StatsCollection result = new StatsCollection();

                result[StatsEnum.INFLICTED_DAMAGE] = double.Parse(di);
                result[StatsEnum.CRIT_HIT] = double.Parse(critrate);
                result[StatsEnum.WATER_DOMAIN] = double.Parse(water);
                result[StatsEnum.EARTH_DOMAIN] = double.Parse(earth);
                result[StatsEnum.AIR_DOMAIN] = double.Parse(air);
                result[StatsEnum.FIRE_DOMAIN] = double.Parse(fire);

                result[StatsEnum.WATER_RESIST] = double.Parse(waterResist);
                result[StatsEnum.EARTH_RESIST] = double.Parse(earthResist);
                result[StatsEnum.AIR_RESIST] = double.Parse(airResist);
                result[StatsEnum.FIRE_RESIST] = double.Parse(fireResist);
                result[StatsEnum.FINAL_HEAL] = double.Parse(finalHeal);
                result[StatsEnum.CRIT_DOMAIN] = double.Parse(domainCrit);
                result[StatsEnum.REAR_DOMAIN] = double.Parse(rear);
                result[StatsEnum.MELE_DOMAIN] = double.Parse(mele);
                result[StatsEnum.HEAL_DOMAIN] = double.Parse(heal);
                result[StatsEnum.CRIT_RESISTANCE] = double.Parse(resistCrit);
                result[StatsEnum.DISTANCE_DOMAIN] = double.Parse(dist);
                result[StatsEnum.BERSERKER_DOMAIN] = double.Parse(berserker);
                var to_return = result + subliStats;
                return to_return;
            }
        }

        static async Task LoadPage(string source, ChromeDriver driver)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(source);

            }
            catch (Exception ex)
            {
                driver?.Quit();
                driver?.Dispose();
            }
        }

    }
}
