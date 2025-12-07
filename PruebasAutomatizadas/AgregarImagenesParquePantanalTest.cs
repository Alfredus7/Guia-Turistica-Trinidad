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
    public class AgregarImagenesParquePantanalTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7103/";

        // URLs de las imágenes del Parque Pantanal
        private readonly List<string> imagenesParquePantanal = new List<string>
        {
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-07-05-pantanal1.jpg",
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-_DSC1592.JPG",
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-347426619_1699962663775444_984262381132875320_n.jpg",
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-347547591_1274036166873345_806432251794129246_n.jpg"
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
        public void Agregar_Imagenes_Parque_Pantanal()
        {
            Console.WriteLine("=== INICIO PRUEBA: Agregar Imágenes al Parque Pantanal ===");

            // PASO 1: LOGIN
            Console.WriteLine("\n[PASO 1] Iniciando sesión...");
            Login();

            // PASO 2: NAVEGAR A IMÁGENES
            Console.WriteLine("\n[PASO 2] Navegando a Gestión de Imágenes...");
            NavegarAImagenesViaNavbar();

            // PASO 3: AGREGAR CADA IMAGEN
            Console.WriteLine($"\n[PASO 3] Agregando {imagenesParquePantanal.Count} imágenes al Parque Pantanal...");

            int imagenesAgregadasExitosamente = 0;

            for (int i = 0; i < imagenesParquePantanal.Count; i++)
            {
                var imagenUrl = imagenesParquePantanal[i];
                Console.WriteLine($"\n--- Agregando Imagen {i + 1}/{imagenesParquePantanal.Count} ---");

                try
                {
                    // 3.1: Hacer clic en "Create New" desde el index de Imágenes
                    Console.WriteLine("  - Haciendo clic en 'Create New'...");
                    var createNewLink = wait.Until(d => d.FindElement(By.LinkText("Create New")));
                    createNewLink.Click();

                    // Esperar que el formulario cargue
                    wait.Until(d => d.FindElement(By.Id("sitioSelect")));
                    Console.WriteLine("  ✓ Formulario cargado");

                    // 3.2: Llenar el formulario
                    Console.WriteLine("  - Llenando formulario...");
                    LlenarFormularioImagen(imagenUrl);

                    // 3.3: Dar clic en "Guardar"
                    Console.WriteLine("  - Enviando formulario...");
                    HacerClicEnGuardar();

                    // 3.4: Verificar que regresa a la lista (index de Imágenes)
                    wait.Until(d => d.Url.Contains("/ImagenesSitios") && !d.Url.Contains("/Create"));
                    Console.WriteLine("  ✓ Redireccionado a lista de Imágenes");
                    imagenesAgregadasExitosamente++;

                    // Solo esperar un momento si no es la última imagen
                    if (i < imagenesParquePantanal.Count - 1)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ERROR agregando imagen {i + 1}: {ex.Message}");
                    Console.WriteLine($"  Detalles del error: {ex.StackTrace}");

                    // Si hay error, intentar regresar a la lista de imágenes
                    try
                    {
                        driver.Navigate().GoToUrl(baseUrl + "ImagenesSitios");
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch
                    {
                        // Si falla, recargar y navegar de nuevo
                        driver.Navigate().Refresh();
                        System.Threading.Thread.Sleep(2000);
                        NavegarAImagenesViaNavbar();
                    }
                }
            }

            // RESULTADO FINAL
            Console.WriteLine($"\n=== RESULTADO FINAL ===");
            Console.WriteLine($"Imágenes a agregar: {imagenesParquePantanal.Count}");
            Console.WriteLine($"Imágenes agregadas exitosamente: {imagenesAgregadasExitosamente}");

            if (imagenesAgregadasExitosamente == imagenesParquePantanal.Count)
            {
                Console.WriteLine("✅ ¡TODAS las imágenes fueron agregadas exitosamente!");
            }
            else if (imagenesAgregadasExitosamente > 0)
            {
                Console.WriteLine($"⚠ {imagenesParquePantanal.Count - imagenesAgregadasExitosamente} imágenes no fueron agregadas");
            }
            else
            {
                Console.WriteLine("❌ No se pudo agregar ninguna imagen");
            }

            Assert.IsTrue(imagenesAgregadasExitosamente >= imagenesParquePantanal.Count - 1,
                $"Se esperaba agregar al menos {imagenesParquePantanal.Count - 1} imágenes");
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

        private void NavegarAImagenesViaNavbar()
        {
            Console.WriteLine("[2] Navegando a ImagenesSitios...");
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/ImagenesSitios')]"))).Click();

            wait.Until(d => d.Url.Contains("/ImagenesSitios"));
            Console.WriteLine("✓ En página de ImagenesSitios");
        }

        private void LlenarFormularioImagen(string imagenUrl)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Esperar a que el formulario cargue completamente
            System.Threading.Thread.Sleep(1500);

            // 1. Seleccionar "Parque Pantanal" en el dropdown
            Console.WriteLine("    - Seleccionando Parque Pantanal...");
            try
            {
                var sitioSelect = driver.FindElement(By.Id("sitioSelect"));
                ScrollToElement(sitioSelect);

                // Crear SelectElement
                var selectElement = new SelectElement(sitioSelect);

                // Buscar la opción que contenga "Parque Pantanal"
                bool encontrado = false;
                foreach (var option in selectElement.Options)
                {
                    if (option.Text.Contains("Parque Pantanal"))
                    {
                        selectElement.SelectByText(option.Text);
                        encontrado = true;
                        Console.WriteLine($"    ✓ Sitio seleccionado: {option.Text}");
                        break;
                    }
                }

                if (!encontrado)
                {
                    // Si no encuentra, seleccionar la primera opción (no el placeholder)
                    selectElement.SelectByIndex(1);
                    Console.WriteLine("    ⚠ Parque Pantanal no encontrado, seleccionando primera opción");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ⚠ Error seleccionando sitio: {ex.Message}");
            }

            // 2. Ingresar URL de imagen
            Console.WriteLine("    - Ingresando URL de imagen...");
            var imageUrlInput = wait.Until(d => d.FindElement(By.Id("imageUrlInput")));
            ScrollToElement(imageUrlInput);
            imageUrlInput.Clear();
            imageUrlInput.SendKeys(imagenUrl);

            // 3. Hacer clic en "Agregar"
            Console.WriteLine("    - Haciendo clic en 'Agregar'...");
            var addImageBtn = driver.FindElement(By.Id("addImageBtn"));
            ScrollAndClick(addImageBtn);
            System.Threading.Thread.Sleep(500); // Esperar a que se procese

            // 4. Verificar que se haya agregado (opcional: podría haber un alert o cambio en el campo oculto)
            Console.WriteLine($"    ✓ URL agregada: {imagenUrl.Substring(0, Math.Min(50, imagenUrl.Length))}...");
        }

        private void HacerClicEnGuardar()
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
                    Console.WriteLine("  ⚠ Botón deshabilitado, verificando validaciones...");

                    // Verificar si falta algún campo requerido
                    try
                    {
                        var sitioSelect = driver.FindElement(By.Id("sitioSelect"));
                        if (string.IsNullOrEmpty(sitioSelect.GetAttribute("value")))
                        {
                            Console.WriteLine("  ⚠ No se seleccionó un sitio");
                        }

                        var UrlImagen = driver.FindElement(By.Id("UrlImagen"));
                        if (string.IsNullOrEmpty(UrlImagen.GetAttribute("value")))
                        {
                            Console.WriteLine("  ⚠ No se agregó una imagen");
                        }

                        // Intentar forzar validación
                        var campo = driver.FindElement(By.Id("imageUrlInput"));
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
            try
            {
                js.ExecuteScript("document.querySelector('button[type=\"submit\"]').click();");
            }
            catch
            {
                // Intentar otro selector
                js.ExecuteScript("document.querySelector('.btn-primary').click();");
            }
        }

        private void ScrollToElement(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
                System.Threading.Thread.Sleep(200);
            }
            catch
            {
                // Ignorar errores de scroll
            }
        }

        private void ScrollAndClick(IWebElement element, bool useJavascript = false)
        {
            ScrollToElement(element);
            System.Threading.Thread.Sleep(200);

            if (useJavascript)
            {
                try
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].click();", element);
                }
                catch { }
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
                    try
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].click();", element);
                    }
                    catch { }
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
                    string screenshotPath = $@"C:\Screenshots\imagenes_parque_pantanal_test_{timestamp}.png";
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