using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class LoginTest
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string baseUrl = "https://localhost:7103/Identity/Account/Login";

        [TestInitialize]
        public void Setup()
        {
            // Configura automáticamente el ChromeDriver correcto
            new DriverManager().SetUpDriver(new ChromeConfig());

            // Especificar la ruta del ejecutable de Chrome si no está en la ubicación predeterminada
            var options = new ChromeOptions();
            options.BinaryLocation = @"C:\Users\HP\Downloads\chrome-win64\chrome-win64\chrome.exe"; // Cambia esta ruta si es necesario

            driver = new ChromeDriver(options);
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