using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class LoginTest
    {
        private IWebDriver driver = null!;
        private readonly string baseUrl = "https://localhost:7103/Identity/Account/Login";

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void Login_Exitoso()
        {
            driver.Navigate().GoToUrl(baseUrl);

            Thread.Sleep(1000); // Espera a que cargue

            driver.FindElement(By.Id("Input_Email")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin123!");
            driver.FindElement(By.Id("login-submit")).Click();

            Thread.Sleep(2000); // Espera redirección

            Assert.AreEqual("https://localhost:7103/", driver.Url, "El login no redirigió correctamente al inicio.");
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}