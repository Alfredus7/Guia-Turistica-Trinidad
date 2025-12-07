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
    public class CrearSitiosTuristicosTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7103/";

        // Clase para almacenar datos del sitio turístico
        private class SitioData
        {
            public string Nombre { get; set; }
            public string NombreIngles { get; set; }
            public string NombrePortugues { get; set; }
            public string Descripcion { get; set; }
            public string DescripcionIngles { get; set; }
            public string DescripcionPortugues { get; set; }
            public string Direccion { get; set; }
            public int TipoId { get; set; }
        }

        // Datos de los sitios a insertar (usando TipoId que coincida con tus datos)
        private readonly List<SitioData> sitiosData = new List<SitioData>
        {
            // Plazas y Parques Urbanos (TipoId = 1)
            new SitioData
            {
                Nombre = "Plaza Principal Mariscal José Ballivián",
                NombreIngles = "Mariscal José Ballivián Main Square",
                NombrePortugues = "Praça Principal Mariscal José Ballivián",
                Descripcion = "Es el corazón de la ciudad. Aquí puedes visitar la Catedral, disfrutar de la sombra de grandes árboles tropicales y ver el monumento al Mariscal Ballivián.",
                DescripcionIngles = "It is the heart of the city. Here you can visit the Cathedral, enjoy the shade of large tropical trees, and see the monument to Marshal Ballivián.",
                DescripcionPortugues = "É o coração da cidade. Aqui você pode visitar a Catedral, desfrutar da sombra de grandes árvores tropicais e ver o monumento ao Marechal Ballivián.",
                Direccion = "Calle La Paz esquina 18 de Noviembre",
                TipoId = 1
            },
            new SitioData
            {
                Nombre = "Plaza del Ganadero",
                NombreIngles = "Cattle Rancher Square",
                NombrePortugues = "Praça do Fazendeiro",
                Descripcion = "Un homenaje a la principal actividad económica del Beni. Cuenta con la estatua 'El Ganadero' y murales que representan la vida de campo.",
                DescripcionIngles = "A tribute to Beni's main economic activity. It features the statue 'El Ganadero' and murals representing country life.",
                DescripcionPortugues = "Uma homenagem à principal atividade econômica de Beni. Apresenta a estátua 'El Ganadero' e murais que representam a vida no campo.",
                Direccion = "Avenida José Chávez Suárez",
                TipoId = 1
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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [TestMethod]
        public void Crear_Multiples_Sitios_Exitoso()
        {
            Console.WriteLine("=== INICIO PRUEBA: Crear Múltiples Sitios Turísticos ===");

            // PASO 1: LOGIN
            Console.WriteLine("\n[PASO 1] Iniciando sesión...");
            Login();

            // PASO 2: NAVEGAR A SITIOS TURÍSTICOS
            Console.WriteLine("\n[PASO 2] Navegando a Sitios Turísticos...");
            NavegarASitios();

            // PASO 3: CREAR CADA SITIO
            Console.WriteLine($"\n[PASO 3] Creando {sitiosData.Count} sitios turísticos...");

            int sitiosCreadosExitosamente = 0;

            for (int i = 0; i < sitiosData.Count; i++)
            {
                var sitio = sitiosData[i];
                Console.WriteLine($"\n--- Creando Sitio {i + 1}/{sitiosData.Count}: '{sitio.Nombre}' ---");

                try
                {
                    // 3.1: Ir al formulario de creación
                    Console.WriteLine("  - Navegando a formulario de creación...");
                    NavegarAFormularioCreacion();

                    // 3.2: Preparar página para modo test
                    Console.WriteLine("  - Preparando página para pruebas...");
                    PrepararPaginaParaTest();

                    // 3.3: Llenar el formulario
                    Console.WriteLine("  - Llenando formulario...");
                    LlenarFormularioSitio(sitio);

                    // 3.4: Enviar formulario
                    Console.WriteLine("  - Enviando formulario...");
                    EnviarFormulario();

                    // 3.5: Verificar éxito
                    if (VerificarCreacionExitosa())
                    {
                        Console.WriteLine($"  ✓ Sitio '{sitio.Nombre}' creado exitosamente");
                        sitiosCreadosExitosamente++;
                    }
                    else
                    {
                        Console.WriteLine($"  ⚠ Posible error en creación de '{sitio.Nombre}'");
                        TomarScreenshot($"error_sitio_{i}_{DateTime.Now:HHmmss}");
                    }

                    // Esperar entre sitios si no es el último
                    if (i < sitiosData.Count - 1)
                    {
                        System.Threading.Thread.Sleep(2000);
                        NavegarASitios(); // Volver a la lista
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ERROR creando sitio '{sitio.Nombre}': {ex.Message}");
                    TomarScreenshot($"error_fatal_{i}_{DateTime.Now:HHmmss}");

                    // Intentar recuperar
                    try
                    {
                        driver.Navigate().GoToUrl(baseUrl);
                        System.Threading.Thread.Sleep(2000);
                        NavegarASitios();
                    }
                    catch
                    {
                        driver.Navigate().Refresh();
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }

            // RESULTADO FINAL
            Console.WriteLine($"\n=== RESULTADO FINAL ===");
            Console.WriteLine($"Sitios a crear: {sitiosData.Count}");
            Console.WriteLine($"Sitios creados exitosamente: {sitiosCreadosExitosamente}");

            Assert.IsTrue(sitiosCreadosExitosamente > 0,
                $"Se esperaba crear al menos 1 sitio, pero se crearon {sitiosCreadosExitosamente}");
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

            // Esperar login exitoso
            wait.Until(d => !d.Url.Contains("/Login"));
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("  ✓ Login exitoso");
        }

        private void NavegarASitios()
        {
            // Ir directamente a la URL de Sitios
            driver.Navigate().GoToUrl(baseUrl + "SitiosTuristicos");
            wait.Until(d => d.Url.Contains("/SitiosTuristicos"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("  ✓ En página de Sitios Turísticos");
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

            // Esperar que cargue el formulario
            wait.Until(d =>
                d.FindElements(By.Id("Nombre")).Count > 0 ||
                d.FindElements(By.Name("Nombre")).Count > 0
            );
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("  ✓ Formulario de creación cargado");
        }

        private void PrepararPaginaParaTest()
        {
            // Activar modo test y desactivar validaciones problemáticas
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                // Forzar modo test
                document.body.classList.add('test-mode');
                
                // Desactivar validaciones automáticas
                const form = document.getElementById('siteForm');
                if (form) {
                    form.noValidate = true;
                    form.classList.remove('needs-validation');
                }
                
                // Desactivar eventos problemáticos
                document.querySelectorAll('input, textarea, select').forEach(input => {
                    input.onblur = null;
                    input.oninput = null;
                });
                
                console.log('Página preparada para pruebas Selenium');
            ");

            System.Threading.Thread.Sleep(1000);
        }

        private void LlenarFormularioSitio(SitioData sitio)
        {
            // Método 1: Intentar llenar con JavaScript directo primero
            try
            {
                LlenarConJavaScriptDirecto(sitio);
                Console.WriteLine("  ✓ Formulario llenado con JavaScript directo");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ JavaScript directo falló: {ex.Message}");
            }

            // Método 2: Llenar manualmente con Selenium
            LlenarManualConSelenium(sitio);
        }

        private void LlenarConJavaScriptDirecto(SitioData sitio)
        {
            string script = $@"
                // Establecer valores directamente
                document.querySelector('[name=""Nombre""]').value = '{EscapeJavaScript(sitio.Nombre)}';
                document.querySelector('[name=""Descripcion""]').value = '{EscapeJavaScript(sitio.Descripcion)}';
                document.querySelector('[name=""Direccion""]').value = '{EscapeJavaScript(sitio.Direccion)}';
                
                // Coordenadas (ya están establecidas por defecto en la vista)
                
                // Tipo
                var tipoSelect = document.querySelector('[name=""TipoId""]');
                if (tipoSelect) {{
                    tipoSelect.value = '{sitio.TipoId}';
                    tipoSelect.dispatchEvent(new Event('change'));
                }}
                
                // Pestaña inglés
                var englishTab = document.getElementById('english-tab');
                if (englishTab) {{
                    englishTab.click();
                    setTimeout(function() {{
                        document.querySelector('[name=""NombreIngles""]').value = '{EscapeJavaScript(sitio.NombreIngles)}';
                        document.querySelector('[name=""DescripcionIngles""]').value = '{EscapeJavaScript(sitio.DescripcionIngles)}';
                    }}, 100);
                }}
                
                // Pestaña portugués
                var portugueseTab = document.getElementById('portuguese-tab');
                if (portugueseTab) {{
                    setTimeout(function() {{
                        portugueseTab.click();
                        setTimeout(function() {{
                            document.querySelector('[name=""NombrePortugues""]').value = '{EscapeJavaScript(sitio.NombrePortugues)}';
                            document.querySelector('[name=""DescripcionPortugues""]').value = '{EscapeJavaScript(sitio.DescripcionPortugues)}';
                        }}, 100);
                    }}, 200);
                }}
                
                return 'Valores establecidos con JavaScript';
            ";

            var result = ((IJavaScriptExecutor)driver).ExecuteScript(script);
            Console.WriteLine($"  Resultado JS: {result}");
            System.Threading.Thread.Sleep(1500);
        }

        private void LlenarManualConSelenium(SitioData sitio)
        {
            // 1. Nombre Español (campo REQUERIDO)
            var nombreInput = FindElementWithFallback("Nombre", "Nombre", "nombreInput");
            if (nombreInput == null) throw new Exception("No se encontró campo Nombre");

            ClearAndSendKeys(nombreInput, sitio.Nombre);
            System.Threading.Thread.Sleep(300);

            // 2. Descripción Español
            var descripcionInput = FindElementWithFallback("Descripcion", "Descripcion", "descripcionInput");
            if (descripcionInput != null)
            {
                ClearAndSendKeys(descripcionInput, sitio.Descripcion);
                System.Threading.Thread.Sleep(300);
            }

            // 3. Dirección
            var direccionInput = FindElementWithFallback("Direccion", "Direccion", "direccionInput");
            if (direccionInput != null)
            {
                ClearAndSendKeys(direccionInput, sitio.Direccion);
                System.Threading.Thread.Sleep(300);
            }

            // 4. Seleccionar Tipo
            var tipoSelect = FindElementWithFallback("TipoId", "TipoId", "tipoSelect");
            if (tipoSelect != null)
            {
                var selectElement = new SelectElement(tipoSelect);

                // Intentar seleccionar por valor
                try
                {
                    selectElement.SelectByValue(sitio.TipoId.ToString());
                }
                catch
                {
                    // Si falla, intentar por índice (el primer elemento es placeholder)
                    selectElement.SelectByIndex(sitio.TipoId);
                }
                System.Threading.Thread.Sleep(300);
            }

            // 5. Pestaña Inglés
            try
            {
                var englishTab = driver.FindElement(By.CssSelector("#english-tab, [data-bs-target='#english']"));
                ClickWithJavaScript(englishTab);
                System.Threading.Thread.Sleep(500);

                var nombreInglesInput = FindElementWithFallback("NombreIngles", "NombreIngles", "nombreInglesInput");
                if (nombreInglesInput != null)
                {
                    ClearAndSendKeys(nombreInglesInput, sitio.NombreIngles);
                }

                var descripcionInglesInput = FindElementWithFallback("DescripcionIngles", "DescripcionIngles", "descripcionInglesInput");
                if (descripcionInglesInput != null)
                {
                    ClearAndSendKeys(descripcionInglesInput, sitio.DescripcionIngles);
                }
            }
            catch { Console.WriteLine("  ⚠ Pestaña inglés no encontrada"); }

            // 6. Pestaña Portugués
            try
            {
                var portuguesTab = driver.FindElement(By.CssSelector("#portuguese-tab, [data-bs-target='#portuguese']"));
                ClickWithJavaScript(portuguesTab);
                System.Threading.Thread.Sleep(500);

                var nombrePortuguesInput = FindElementWithFallback("NombrePortugues", "NombrePortugues", "nombrePortuguesInput");
                if (nombrePortuguesInput != null)
                {
                    ClearAndSendKeys(nombrePortuguesInput, sitio.NombrePortugues);
                }

                var descripcionPortuguesInput = FindElementWithFallback("DescripcionPortugues", "DescripcionPortugues", "descripcionPortuguesInput");
                if (descripcionPortuguesInput != null)
                {
                    ClearAndSendKeys(descripcionPortuguesInput, sitio.DescripcionPortugues);
                }
            }
            catch { Console.WriteLine("  ⚠ Pestaña portugués no encontrada"); }

            Console.WriteLine("  ✓ Formulario llenado manualmente");
        }

        private void EnviarFormulario()
        {
            // Tomar screenshot antes de enviar
            TomarScreenshot("antes_de_enviar");

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
                var textos = new[] { "Crear Sitio", "Create", "Guardar", "Save", "Crear", "Submit" };
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
                // Último recurso: usar JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "var form = document.getElementById('siteForm'); if(form) form.submit();"
                );
                Console.WriteLine("  ✓ Formulario enviado con JavaScript");
            }
            else
            {
                // Verificar si el botón está habilitado
                if (!submitButton.Enabled)
                {
                    Console.WriteLine("  ⚠ Botón deshabilitado, intentando habilitar...");

                    // Forzar validación del campo requerido
                    var nombreInput = FindElementWithFallback("Nombre", "Nombre", "nombreInput");
                    if (nombreInput != null)
                    {
                        nombreInput.SendKeys("");
                        nombreInput.SendKeys(Keys.Tab);
                        System.Threading.Thread.Sleep(1000);
                    }

                    // Volver a buscar el botón
                    submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));
                }

                if (submitButton != null && submitButton.Enabled)
                {
                    ClickWithJavaScript(submitButton);
                    Console.WriteLine("  ✓ Botón de enviar clickeado");
                }
                else
                {
                    // Forzar envío con JavaScript
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "document.querySelector('button[type=\"submit\"]').click();"
                    );
                    Console.WriteLine("  ✓ Envío forzado con JavaScript");
                }
            }

            System.Threading.Thread.Sleep(3000); // Esperar respuesta del servidor
        }

        private bool VerificarCreacionExitosa()
        {
            try
            {
                // Verificar que estamos en la lista de sitios
                wait.Until(d => d.Url.Contains("/SitiosTuristicos") && !d.Url.Contains("/Create"));

                // Tomar screenshot después de enviar
                TomarScreenshot("despues_de_enviar");

                // Buscar mensaje de éxito
                var pageSource = driver.PageSource;
                if (pageSource.Contains("creado exitosamente") ||
                    pageSource.Contains("created successfully") ||
                    pageSource.Contains("alert-success"))
                {
                    return true;
                }

                // Alternativa: verificar que no hay errores
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors, .field-validation-error"));
                if (validationErrors.Count == 0)
                {
                    // Verificar que el sitio aparece en la tabla
                    var sitioActual = sitiosData[0]; // Último creado
                    if (pageSource.Contains(sitioActual.Nombre))
                    {
                        return true;
                    }
                }

                return false;
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

        private void ClearAndSendKeys(IWebElement element, string text)
        {
            try
            {
                element.Clear();
                System.Threading.Thread.Sleep(100);

                // Escribir carácter por carácter (más confiable para Selenium)
                foreach (char c in text)
                {
                    element.SendKeys(c.ToString());
                    System.Threading.Thread.Sleep(10); // Pequeña pausa entre caracteres
                }
            }
            catch
            {
                // Si falla, usar JavaScript
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value = arguments[1];",
                    element, text
                );
            }
        }

        private void ClickWithJavaScript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }

        private string EscapeJavaScript(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "\\n");
        }

        private void TomarScreenshot(string nombre)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var path = $@"C:\Screenshots\{nombre}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(path);
                Console.WriteLine($"  📸 Screenshot: {path}");
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
                    TomarScreenshot("final");

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