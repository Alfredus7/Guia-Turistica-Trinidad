using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class CrearTipoTest
    {
        private IWebDriver driver = null!;
        private readonly string baseUrl = "https://localhost:7103/";

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void Crear_Tipo_Exitoso()
        {
            // --- LOGIN ---
            driver.Navigate().GoToUrl(baseUrl + "Identity/Account/Login");
            Thread.Sleep(1000);

            driver.FindElement(By.Id("Input_Email")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin123!");
            driver.FindElement(By.Id("login-submit")).Click();
            Thread.Sleep(2000);

            // --- VERIFICA LOGIN ---
            Assert.AreEqual(baseUrl, driver.Url, "No redirigió al inicio después del login.");

            // --- IR A ADMINISTRACIÓN > TIPOS ---
            IWebElement adminDropdown = driver.FindElement(By.XPath("//a[contains(.,'Administración')]"));
            adminDropdown.Click();
            Thread.Sleep(1000);

            IWebElement tipoLink = driver.FindElement(By.XPath("//a[contains(@href,'/Tipos')]"));
            tipoLink.Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("/Tipos"), "No se accedió correctamente a la página de Tipos.");

            // --- CLIC EN CREATE NEW ---
            IWebElement createLink = driver.FindElement(By.LinkText("Create New"));
            createLink.Click();
            Thread.Sleep(2000);

            Assert.IsTrue(driver.Url.Contains("/Tipos/Create"), "No se accedió al formulario de creación.");

            // --- LLENAR FORMULARIO ---
            driver.FindElement(By.Id("imageUrlInput")).SendKeys("https://pic2-cdn.creality.com/comp/model/20c37a5ddeaeb196355bd5036c394eb2.webp");

            driver.FindElement(By.Id("nombreInput")).SendKeys("Parque");

            

            // --- ENVIAR FORMULARIO ---
            //driver.FindElement(By.CssSelector("input[type='submit'][value='Create']")).Click();
            Thread.Sleep(3000);

            // --- VERIFICAR RESULTADO ---
            Assert.IsTrue(driver.Url.EndsWith("/Tipos"), "No regresó al listado después de crear el tipo.");

        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}