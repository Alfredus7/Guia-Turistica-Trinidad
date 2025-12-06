using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class CrearTipoTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7103/";

        [TestInitialize]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.BinaryLocation = @"C:\Users\HP\Downloads\chrome-win64\chrome-win64\chrome.exe";

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TestMethod]
        public void Crear_Tipo_Exitoso()
        {
            // --- LOGIN ---
            driver.Navigate().GoToUrl(baseUrl + "Identity/Account/Login");

            wait.Until(d => d.FindElement(By.Id("Input_Email"))).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin123!");
            driver.FindElement(By.Id("login-submit")).Click();

            wait.Until(d => d.Url == baseUrl);

            // --- ADMINISTRACIÓN > TIPOS ---
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/Tipos')]"))).Click();

            wait.Until(d => d.Url.Contains("/Tipos"));

            // --- CREATE NEW ---
            wait.Until(d => d.FindElement(By.LinkText("Create New"))).Click();
            wait.Until(d => d.Url.Contains("/Tipos/Create"));

            // Hacer scroll para asegurar visibilidad
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // --- LLENAR FORMULARIO ---
            IWebElement imagenInput = wait.Until(d => d.FindElement(By.Id("ImagenUrl")));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", imagenInput);
            imagenInput.Clear();
            imagenInput.SendKeys("https://pic2-cdn.creality.com/comp/model/20c37a5ddeaeb196355bd5036c394eb2.webp");

            IWebElement nombreInput = wait.Until(d => d.FindElement(By.Id("Nombre")));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", nombreInput);
            nombreInput.Clear();
            nombreInput.SendKeys("Parque");

            // --- SUBMIT ---
            IWebElement submitBtn = wait.Until(d => d.FindElement(By.Id("submitBtn")));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", submitBtn);
            submitBtn.Click();

            // --- VERIFICAR ---
            wait.Until(d => d.Url.Contains("/Tipos"));
            Assert.IsTrue(driver.Url.Contains("/Tipos"), "No retornó al listado después de crear el Tipo.");
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
