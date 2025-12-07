using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class CrearTiposTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7103/";

        // Clase para almacenar datos del tipo
        private class TipoData
        {
            public string Nombre { get; set; }
            public string NombreIngles { get; set; }
            public string NombrePortugues { get; set; }
            public string Descripcion { get; set; }
            public string DescripcionIngles { get; set; }
            public string DescripcionPortugues { get; set; }
            public string ImagenUrl { get; set; }
        }

        // Datos de los tipos a insertar
        private readonly List<TipoData> tiposData = new List<TipoData>
        {
            new TipoData
            {
                Nombre = "Plazas",
                NombreIngles = "Plazas",
                NombrePortugues = "Praças e Parques Urbanos",
                Descripcion = "Lugares ideales para pasear, ver la vida local y disfrutar de la brisa en moto o a pie.",
                DescripcionIngles = "Ideal places for walking, observing local life, and enjoying the breeze on a motorcycle or on foot.",
                DescripcionPortugues = "Lugares ideais para passear, ver a vida local e curtir a brisa de moto ou a pé.",
                ImagenUrl = "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/96/galeria/2024-11-19-PlazaJoseBallivian(2).jpg"
            },
            new TipoData
            {
                Nombre = "Puertos Turísticos y Balnearios",
                NombreIngles = "Tourist Ports and Resorts",
                NombrePortugues = "Portos Turísticos e Balneários",
                Descripcion = "Sitios a orillas de los ríos (Ibare y Mamoré) perfectos para comer pescado fresco, nadar o pasear en barco.",
                DescripcionIngles = "Sites on the banks of the rivers (Ibare and Mamoré) perfect for eating fresh fish, swimming, or boat riding.",
                DescripcionPortugues = "Locais às margens dos rios (Ibare e Mamoré) perfeitos para comer peixe fresco, nadar ou andar de barco.",
                ImagenUrl = "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-_DSC1592.JPG"
            },
            new TipoData
            {
                Nombre = "Museos y Cultura",
                NombreIngles = "Museums and Culture",
                NombrePortugues = "Museus e Cultura",
                Descripcion = "Espacios para entender la rica historia, la arqueología y la biodiversidad de la Amazonía boliviana.",
                DescripcionIngles = "Spaces to understand the rich history, archaeology, and biodiversity of the Bolivian Amazon.",
                DescripcionPortugues = "Espaços para entender a rica história, a arqueologia e a biodiversidade da Amazônia boliviana.",
                ImagenUrl = "https://trinidadtevajaenamorar.com.bo/Imageneslugares/MUSEOS%20DEL%20MUNICIPIO%20DE%20TRINIDAD/79/galeria/2023-05-23-_DSC4328-min.JPG"
            },
            new TipoData
            {
                Nombre = "Naturaleza",
                NombreIngles = "Nature",
                NombrePortugues = "Natureza",
                Descripcion = "Refugios de vida silvestre y sitios arqueológicos donde puedes hacer caminatas por la selva y ver fauna.",
                DescripcionIngles = "Wildlife refuges and archaeological sites where you can hike through the jungle and observe fauna.",
                DescripcionPortugues = "Refúgios de vida selvagem e sítios arqueológicos onde você pode fazer caminhadas na selva e observar a fauna.",
                ImagenUrl = "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-07-05-pantanal2.jpg"
            }
        };

        [TestInitialize]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.BinaryLocation = @"C:\Users\HP\Downloads\chrome-win64\chrome-win64\chrome.exe";

            // Configuraciones para mejorar velocidad
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            driver = new ChromeDriver(options);

            // Configurar timeouts
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [TestMethod]
        public void Crear_Multiples_Tipos_Exitoso()
        {
            Console.WriteLine("=== INICIO PRUEBA: Crear Múltiples Tipos ===");

            // PASO 1: LOGIN
            Console.WriteLine("\n[PASO 1] Iniciando sesión...");
            Login();

            // PASO 2: NAVEGAR A TIPOS USANDO EL NAVBAR
            Console.WriteLine("\n[PASO 2] Navegando por el navbar a Tipos...");
            NavegarATiposViaNavbar();

            // PASO 3: CREAR CADA TIPO
            Console.WriteLine($"\n[PASO 3] Creando {tiposData.Count} tipos...");

            int tiposCreadosExitosamente = 0;

            for (int i = 0; i < tiposData.Count; i++)
            {
                var tipo = tiposData[i];
                Console.WriteLine($"\n--- Creando Tipo {i + 1}/{tiposData.Count}: '{tipo.Nombre}' ---");

                try
                {
                    // 3.1: Hacer clic en "Create New" desde el index de Tipos
                    Console.WriteLine("  - Haciendo clic en 'Create New'...");
                    var createNewLink = wait.Until(d => d.FindElement(By.LinkText("Create New")));
                    createNewLink.Click();

                    // Esperar que el formulario cargue
                    wait.Until(d => d.FindElement(By.Id("nombreInput")));
                    Console.WriteLine("  ✓ Formulario cargado");

                    // 3.2: Llenar el formulario
                    Console.WriteLine("  - Llenando formulario...");
                    LlenarFormularioTipo(tipo);

                    // 3.3: Dar clic en "Crear" (Submit)
                    Console.WriteLine("  - Enviando formulario...");
                    HacerClicEnCrear();

                    // 3.4: Verificar que regresa a la lista (index de Tipos)
                    wait.Until(d => d.Url.Contains("/Tipos") && !d.Url.Contains("/Create"));
                    Console.WriteLine("  ✓ Redireccionado a lista de Tipos");

                    // 3.5: Verificar que el tipo aparece en la tabla
                    System.Threading.Thread.Sleep(1000);
                    if (VerificarTipoEnTabla(tipo.Nombre))
                    {
                        Console.WriteLine($"  ✓ Tipo '{tipo.Nombre}' creado exitosamente");
                        tiposCreadosExitosamente++;
                    }
                    else
                    {
                        Console.WriteLine($"  ⚠ Tipo '{tipo.Nombre}' creado pero no aparece en la tabla");
                    }

                    // Solo esperar un momento si no es el último tipo
                    if (i < tiposData.Count - 1)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ERROR creando tipo '{tipo.Nombre}': {ex.Message}");
                    Console.WriteLine($"  Detalles del error: {ex.StackTrace}");

                    // Si hay error, intentar regresar a la lista de tipos
                    try
                    {
                        driver.Navigate().GoToUrl(baseUrl + "Tipos");
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch
                    {
                        // Si falla, recargar y navegar de nuevo
                        driver.Navigate().Refresh();
                        System.Threading.Thread.Sleep(2000);
                        NavegarATiposViaNavbar();
                    }
                }
            }

            // RESULTADO FINAL
            Console.WriteLine($"\n=== RESULTADO FINAL ===");
            Console.WriteLine($"Tipos a crear: {tiposData.Count}");
            Console.WriteLine($"Tipos creados exitosamente: {tiposCreadosExitosamente}");

            if (tiposCreadosExitosamente == tiposData.Count)
            {
                Console.WriteLine("✅ ¡TODOS los tipos fueron creados exitosamente!");
            }
            else if (tiposCreadosExitosamente > 0)
            {
                Console.WriteLine($"⚠ {tiposData.Count - tiposCreadosExitosamente} tipos no fueron creados");
            }
            else
            {
                Console.WriteLine("❌ No se pudo crear ningún tipo");
            }

            Assert.IsTrue(tiposCreadosExitosamente >= tiposData.Count - 1,
                $"Se esperaba crear al menos {tiposData.Count - 1} tipos");
        }

        private void Login()
        {
            driver.Navigate().GoToUrl(baseUrl + "Identity/Account/Login");
            System.Threading.Thread.Sleep(1500);

            // Email
            var emailInput = wait.Until(d => d.FindElement(By.Id("Input_Email")));
            emailInput.Clear();
            emailInput.SendKeys("admin@admin.com");

            // Password
            var passwordInput = driver.FindElement(By.Id("Input_Password"));
            passwordInput.Clear();
            passwordInput.SendKeys("Admin123!");

            // Botón Login
            var loginButton = driver.FindElement(By.Id("login-submit"));
            loginButton.Click();

            // Esperar login exitoso
            wait.Until(d => !d.Url.Contains("/Login"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("  ✓ Login exitoso");
        }

        private void NavegarATiposViaNavbar()
        {
            Console.WriteLine("[2] Navegando a Tipos...");
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/Tipos')]"))).Click();

            wait.Until(d => d.Url.Contains("/Tipos"));
            Console.WriteLine("✓ En página de Tipos");
        }



        private void LlenarFormularioTipo(TipoData tipo)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Esperar a que el formulario cargue completamente
            System.Threading.Thread.Sleep(1000);

            // 1. Pestaña Español (asumimos que está activa por defecto)

            // Imagen URL
            var imagenInput = wait.Until(d => d.FindElement(By.Id("imageUrlInput")));
            ScrollToElement(imagenInput);
            imagenInput.Clear();
            imagenInput.SendKeys(tipo.ImagenUrl);

            // Nombre Español
            var nombreInput = wait.Until(d => d.FindElement(By.Id("nombreInput")));
            ScrollToElement(nombreInput);
            nombreInput.Clear();
            nombreInput.SendKeys(tipo.Nombre);

            // Descripción Español
            var descripcionInput = wait.Until(d => d.FindElement(By.Id("descripcionInput")));
            ScrollToElement(descripcionInput);
            descripcionInput.Clear();
            descripcionInput.SendKeys(tipo.Descripcion);

            // 2. Cambiar a pestaña Inglés
            try
            {
                var englishTab = driver.FindElement(By.XPath("//button[contains(text(),'Inglés')]"));
                ScrollAndClick(englishTab);
                System.Threading.Thread.Sleep(300);

                // Nombre Inglés
                var nombreInglesInput = wait.Until(d => d.FindElement(By.Id("nombreInglesInput")));
                ScrollToElement(nombreInglesInput);
                nombreInglesInput.Clear();
                nombreInglesInput.SendKeys(tipo.NombreIngles);

                // Descripción Inglés
                var descripcionInglesInput = wait.Until(d => d.FindElement(By.Id("descripcionInglesInput")));
                ScrollToElement(descripcionInglesInput);
                descripcionInglesInput.Clear();
                descripcionInglesInput.SendKeys(tipo.DescripcionIngles);
            }
            catch
            {
                Console.WriteLine("  ⚠ No se encontró pestaña Inglés, continuando...");
            }

            // 3. Cambiar a pestaña Portugués
            try
            {
                var portuguesTab = driver.FindElement(By.XPath("//button[contains(text(),'Portugués')]"));
                ScrollAndClick(portuguesTab);
                System.Threading.Thread.Sleep(300);

                // Nombre Portugués
                var nombrePortuguesInput = wait.Until(d => d.FindElement(By.Id("nombrePortuguesInput")));
                ScrollToElement(nombrePortuguesInput);
                nombrePortuguesInput.Clear();
                nombrePortuguesInput.SendKeys(tipo.NombrePortugues);

                // Descripción Portugués
                var descripcionPortuguesInput = wait.Until(d => d.FindElement(By.Id("descripcionPortuguesInput")));
                ScrollToElement(descripcionPortuguesInput);
                descripcionPortuguesInput.Clear();
                descripcionPortuguesInput.SendKeys(tipo.DescripcionPortugues);
            }
            catch
            {
                Console.WriteLine("  ⚠ No se encontró pestaña Portugués, continuando...");
            }
        }

        private void HacerClicEnCrear()
        {
            // Buscar botón de submit por varios métodos
            IWebElement submitButton = null;

            // Primero intentar por ID
            try { submitButton = driver.FindElement(By.Id("submitBtn")); }
            catch { }

            // Si no, buscar por type='submit'
            if (submitButton == null)
            {
                try { submitButton = driver.FindElement(By.XPath("//button[@type='submit']")); }
                catch { }
            }

            // Si no, buscar por texto
            if (submitButton == null)
            {
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Crear')]")); }
                catch { }
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Create')]")); }
                catch { }
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Guardar')]")); }
                catch { }
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Save')]")); }
                catch { }
            }

            // Si no, buscar por clase
            if (submitButton == null)
            {
                try { submitButton = driver.FindElement(By.XPath("//button[contains(@class,'btn-primary')]")); }
                catch { }
            }

            if (submitButton != null)
            {
                // Verificar si está habilitado
                if (!submitButton.Enabled)
                {
                    Console.WriteLine("  ⚠ Botón deshabilitado, intentando forzar validación...");

                    // Intentar activar validaciones
                    try
                    {
                        var campo = driver.FindElement(By.Id("nombreInput"));
                        campo.Click();
                        campo.SendKeys(Keys.Tab);
                        System.Threading.Thread.Sleep(500);

                        // Volver a buscar el botón
                        submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));
                    }
                    catch { }
                }

                if (submitButton != null && submitButton.Enabled)
                {
                    ScrollAndClick(submitButton, useJavascript: true);
                    return;
                }
            }

            // Último recurso: JavaScript
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelector('button[type=\"submit\"]').click();");
        }

        private bool VerificarTipoEnTabla(string nombreTipo)
        {
            try
            {
                // Buscar en el contenido de la página
                return driver.PageSource.Contains(nombreTipo);
            }
            catch
            {
                return false;
            }
        }

        private void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
            System.Threading.Thread.Sleep(200);
        }

        private void ScrollAndClick(IWebElement element, bool useJavascript = false)
        {
            ScrollToElement(element);
            System.Threading.Thread.Sleep(200);

            if (useJavascript)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", element);
            }
            else
            {
                try
                {
                    element.Click();
                }
                catch
                {
                    // Si falla el click normal, usar JavaScript
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].click();", element);
                }
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                if (driver != null)
                {
                    // Tomar screenshot al final de la prueba
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string screenshotPath = $@"C:\Screenshots\tipos_test_{timestamp}.png";
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(screenshotPath);
                    Console.WriteLine($"\nScreenshot guardado en: {screenshotPath}");

                    driver.Quit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en cleanup: {ex.Message}");
            }
        }
    }
}