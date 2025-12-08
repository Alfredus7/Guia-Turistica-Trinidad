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
                NombrePortugues = "Praças",
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
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [TestMethod]
        public void Crear_Multiples_Tipos_Exitoso()
        {
            Console.WriteLine("=== INICIO PRUEBA: Crear Múltiples Tipos ===");

            // PASO 1: LOGIN
            Console.WriteLine("\n[PASO 1] Iniciando sesión...");
            Login();

            // PASO 2: NAVEGAR A TIPOS
            Console.WriteLine("\n[PASO 2] Navegando a Tipos...");
            NavegarATipos();

            // PASO 3: CREAR CADA TIPO
            Console.WriteLine($"\n[PASO 3] Creando {tiposData.Count} tipos...");

            int tiposCreadosExitosamente = 0;

            for (int i = 0; i < tiposData.Count; i++)
            {
                var tipo = tiposData[i];
                Console.WriteLine($"\n--- Creando Tipo {i + 1}/{tiposData.Count}: '{tipo.Nombre}' ---");

                try
                {
                    // 3.1: Hacer clic en "Create New"
                    Console.WriteLine("  - Navegando a formulario de creación...");
                    NavegarAFormularioCreacion();

                    // 3.2: Llenar el formulario con IDs CORRECTOS
                    Console.WriteLine("  - Llenando formulario...");
                    LlenarFormularioTipo(tipo);

                    // 3.3: Enviar formulario
                    Console.WriteLine("  - Enviando formulario...");
                    EnviarFormulario();

                    // 3.4: Verificar éxito
                    if (VerificarCreacionExitosa())
                    {
                        Console.WriteLine($"  ✓ Tipo '{tipo.Nombre}' creado exitosamente");
                        tiposCreadosExitosamente++;
                    }
                    else
                    {
                        Console.WriteLine($"  ⚠ Posible error en creación de '{tipo.Nombre}'");
                        TomarScreenshot($"error_tipo_{i}_{DateTime.Now:HHmmss}");
                    }

                    // Esperar entre tipos
                    if (i < tiposData.Count - 1)
                    {
                        System.Threading.Thread.Sleep(2000);
                        NavegarATipos(); // Volver a la lista
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ERROR creando tipo '{tipo.Nombre}': {ex.Message}");
                    TomarScreenshot($"error_fatal_{i}_{DateTime.Now:HHmmss}");

                    // Intentar recuperar navegando a home
                    try
                    {
                        driver.Navigate().GoToUrl(baseUrl);
                        System.Threading.Thread.Sleep(2000);
                        NavegarATipos();
                    }
                    catch
                    {
                        // Si todo falla, recargar
                        driver.Navigate().Refresh();
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }

            // RESULTADO FINAL
            Console.WriteLine($"\n=== RESULTADO FINAL ===");
            Console.WriteLine($"Tipos a crear: {tiposData.Count}");
            Console.WriteLine($"Tipos creados exitosamente: {tiposCreadosExitosamente}");

            Assert.IsTrue(tiposCreadosExitosamente > 0,
                $"Se esperaba crear al menos 1 tipo, pero se crearon {tiposCreadosExitosamente}");
        }

        private void Login()
        {
            driver.Navigate().GoToUrl(baseUrl + "Identity/Account/Login");
            wait.Until(d => d.Url.Contains("Login"));
            System.Threading.Thread.Sleep(2000);

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

            // Esperar login exitoso - verificar que estamos en home
            wait.Until(d => !d.Url.Contains("/Login"));
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("  ✓ Login exitoso");
        }

        private void NavegarATipos()
        {
            Console.WriteLine("[2] Navegando a Tipos...");
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/Tipos')]"))).Click();

            wait.Until(d => d.Url.Contains("/Tipos"));
            Console.WriteLine("✓ En página de Tipos");
        }

        private void NavegarAFormularioCreacion()
        {
            // Buscar enlace de creación
            var createLink = wait.Until(d =>
                d.FindElement(By.LinkText("Create New")) ??
                d.FindElement(By.XPath("//a[contains(text(),'Nuevo')]")) ??
                d.FindElement(By.XPath("//a[contains(@href,'/Create')]"))
            );

            createLink.Click();

            // Esperar que cargue el formulario (verificar campo Nombre)
            wait.Until(d =>
                d.FindElements(By.Id("Nombre")).Count > 0 ||
                d.FindElements(By.Name("Nombre")).Count > 0
            );
            System.Threading.Thread.Sleep(1500);
            Console.WriteLine("  ✓ Formulario de creación cargado");
        }

        private void LlenarFormularioTipo(TipoData tipo)
        {
            // Activar modo test explícitamente
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
        // Forzar modo test
        document.body.classList.add('test-mode');
        console.log('Modo test forzado desde Selenium');
        
        // Desactivar todas las validaciones automáticas
        const form = document.getElementById('typeForm');
        if (form) {
            form.noValidate = true;
            form.classList.remove('needs-validation');
        }
        
        // Desactivar eventos problemáticos
        document.querySelectorAll('input, textarea').forEach(input => {
            // Guardar valores pero desactivar validaciones
            input.setAttribute('data-original-onblur', input.onblur);
            input.setAttribute('data-original-oninput', input.oninput);
            input.onblur = null;
            input.oninput = null;
        });
    ");

            System.Threading.Thread.Sleep(1000);

            // 1. Nombre Español (campo REQUERIDO)
            var nombreInput = driver.FindElement(By.Name("Nombre"));
            nombreInput.Clear();
            System.Threading.Thread.Sleep(200);

            // Escribir carácter por carácter (más confiable)
            foreach (char c in tipo.Nombre)
            {
                nombreInput.SendKeys(c.ToString());
                System.Threading.Thread.Sleep(50);
            }

            // Verificar que se escribió
            Console.WriteLine($"  Nombre escrito: {nombreInput.GetAttribute("value")}");

            // 2. Descripción Español
            var descripcionInput = driver.FindElement(By.Name("Descripcion"));
            descripcionInput.Clear();
            descripcionInput.SendKeys(tipo.Descripcion);

            // 3. Imagen URL
            var imagenInput = driver.FindElement(By.Id("imageUrlInput"));
            imagenInput.Clear();
            imagenInput.SendKeys(tipo.ImagenUrl);

            // 4. Pestaña Inglés
            try
            {
                var englishTab = driver.FindElement(By.CssSelector("button[data-bs-target='#english']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", englishTab);
                System.Threading.Thread.Sleep(300);

                // Nombre Inglés
                var nombreInglesInput = driver.FindElement(By.Name("NombreIngles"));
                nombreInglesInput.Clear();
                nombreInglesInput.SendKeys(tipo.NombreIngles);

                // Descripción Inglés
                var descripcionInglesInput = driver.FindElement(By.Name("DescripcionIngles"));
                descripcionInglesInput.Clear();
                descripcionInglesInput.SendKeys(tipo.DescripcionIngles);
            }
            catch { Console.WriteLine("  Pestaña inglés no encontrada"); }

            // 5. Pestaña Portugués
            try
            {
                var portuguesTab = driver.FindElement(By.CssSelector("button[data-bs-target='#portuguese']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", portuguesTab);
                System.Threading.Thread.Sleep(300);

                // Nombre Portugués
                var nombrePortuguesInput = driver.FindElement(By.Name("NombrePortugues"));
                nombrePortuguesInput.Clear();
                nombrePortuguesInput.SendKeys(tipo.NombrePortugues);

                // Descripción Portugués
                var descripcionPortuguesInput = driver.FindElement(By.Name("DescripcionPortugues"));
                descripcionPortuguesInput.Clear();
                descripcionPortuguesInput.SendKeys(tipo.DescripcionPortugues);
            }
            catch { Console.WriteLine("  Pestaña portugués no encontrada"); }

            // 6. Tomar screenshot del formulario lleno
            TomarScreenshot("formulario_llenado");

            Console.WriteLine("  ✓ Formulario llenado");
        }

        private void EnviarFormulario()
        {
            // Buscar botón de submit por múltiples métodos
            IWebElement submitButton = null;

            // Método 1: Por ID específico
            try { submitButton = driver.FindElement(By.Id("submitBtn")); }
            catch { }

            // Método 2: Por type submit
            if (submitButton == null)
            {
                try { submitButton = driver.FindElement(By.XPath("//button[@type='submit']")); }
                catch { }
            }

            // Método 3: Por texto
            if (submitButton == null)
            {
                var textos = new[] { "Crear", "Create", "Guardar", "Save", "Enviar", "Submit" };
                foreach (var texto in textos)
                {
                    try
                    {
                        submitButton = driver.FindElement(By.XPath($"//button[contains(text(),'{texto}')]"));
                        if (submitButton != null) break;
                    }
                    catch { }
                }
            }

            // Método 4: Por clase
            if (submitButton == null)
            {
                try { submitButton = driver.FindElement(By.CssSelector("button.btn-success, button.btn-primary")); }
                catch { }
            }

            if (submitButton == null)
            {
                throw new Exception("No se pudo encontrar el botón de envío");
            }

            // Verificar si el botón está habilitado
            if (!submitButton.Enabled)
            {
                Console.WriteLine("  ⚠ Botón deshabilitado, verificando validaciones...");

                // Forzar focus/blur en campo requerido
                var nombreInput = FindElementWithFallback("Nombre", "Nombre", "nombreInput");
                nombreInput.SendKeys("");
                nombreInput.SendKeys(Keys.Tab);
                System.Threading.Thread.Sleep(1000);

                // Volver a buscar el botón
                submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            }

            // Usar JavaScript para hacer clic (más confiable)
            ClickWithJavaScript(submitButton);
            System.Threading.Thread.Sleep(2000);
        }

        private bool VerificarCreacionExitosa()
        {
            try
            {
                // Verificar que estamos en la lista de tipos
                wait.Until(d => d.Url.Contains("/Tipos") && !d.Url.Contains("/Create"));

                // Buscar mensaje de éxito (puede ser alert, toast, o texto en página)
                var pageSource = driver.PageSource;
                if (pageSource.Contains("creado exitosamente") ||
                    pageSource.Contains("created successfully") ||
                    pageSource.Contains("succesfully") ||
                    pageSource.Contains("alert-success"))
                {
                    return true;
                }

                // Alternativa: verificar que no hay errores de validación
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors, .field-validation-error"));
                return validationErrors.Count == 0;
            }
            catch
            {
                return false;
            }
        }

        // ===== MÉTODOS AUXILIARES =====

        private IWebElement FindElementWithFallback(string id, string name, string oldId = null)
        {
            try
            {
                // Intentar por ID
                return driver.FindElement(By.Id(id));
            }
            catch
            {
                try
                {
                    // Intentar por name
                    return driver.FindElement(By.Name(name));
                }
                catch
                {
                    // Intentar por old ID (para compatibilidad)
                    if (oldId != null)
                    {
                        return driver.FindElement(By.Id(oldId));
                    }
                    throw;
                }
            }
        }

        private void ScrollAndClear(IWebElement element)
        {
            ScrollToElement(element);
            element.Clear();
            System.Threading.Thread.Sleep(200);
        }

        private void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});",
                element
            );
            System.Threading.Thread.Sleep(200);
        }

        private void ClickWithJavaScript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }

        private void TomarScreenshot(string nombre)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var path = $@"C:\Screenshots\{nombre}.png";
                screenshot.SaveAsFile(path);
                Console.WriteLine($"  📸 Screenshot guardado: {path}");
            }
            catch
            {
                // Ignorar errores de screenshot
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                if (driver != null)
                {
                    // Tomar screenshot final
                    TomarScreenshot($"final_{DateTime.Now:yyyyMMdd_HHmmss}");

                    // Cerrar navegador
                    driver.Quit();
                    driver = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en cleanup: {ex.Message}");
            }
        }
    }
}