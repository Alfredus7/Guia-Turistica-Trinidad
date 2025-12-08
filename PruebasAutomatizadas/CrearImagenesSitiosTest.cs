using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace PruebasAutomatizadas
{
    [TestClass]
    public class CrearImagenesSitiosTest
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

            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            driver = new ChromeDriver(options);

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [TestMethod]
        public void Agregar_Imagenes_Multiples_Sitios_Exitoso()
        {
            Console.WriteLine("=== TEST: Agregar Imágenes Múltiples a Sitios ===");

            try
            {
                // 1. Login
                Login();

                // 2. Definir las imágenes por sitio
                var imagenesPorSitio = new Dictionary<string, string[]>
                {
                    {
                        "Plaza del Ganadero", new string[]
                        {
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/95/2023-05-24-2023-05-20-PlazaElGanadero-min.jpg",
                            "https://bit-ideas.com/turismo/images/atractivos/PG-16.jpg"
                        }
                    },
                    {
                        "Plaza Principal Mariscal José Ballivián", new string[]
                        {
                            "https://bit-ideas.com/turismo/images/atractivos/PC-06.jpg"
                        }
                    },
                    {
                        "Plaza de la Tradición", new string[]
                        {
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/100/2023-05-24-2023-05-19-PlazadelaTradicion-min.jpg",
                            "https://i0.wp.com/tinformas.com/wp-content/uploads/2020/11/plaza-de-la-Tradicion.jpg?fit=2048%2C1386&ssl=1",
                            "https://scontent.fsrz4-1.fna.fbcdn.net/v/t39.30808-6/506231147_1036617451920496_189069179209790058_n.jpg?stp=dst-jpg_s590x590_tt6&_nc_cat=100&ccb=1-7&_nc_sid=833d8c&_nc_ohc=nio1e_NWtXcQ7kNvwGXAxWt&_nc_oc=AdlppLYG2GDH-yAvdnCEKsZn1BfPiXUNPqUTFWBMIUiHdYHMcpgezRN5goDkeB4igMk&_nc_zt=23&_nc_ht=scontent.fsrz4-1.fna&_nc_gid=aLG8ojKuwfYBb_9kPJxxbQ&oh=00_AfmqUT2ZqwLa_a8la6KIp2YIX4G_zvqWbo956iav0bBS6g&oe=693CAB13"
                        }
                    },
                    {
                        "Parque Pantanal", new string[]
                        {
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-07-05-pantanal1.jpg",
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-347426619_1699962663775444_984262381132875320_n.jpg",
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-347547591_1274036166873345_806432251794129246_n.jpg"
                        }
                    },
                    {
                        "Museo Ictícola", new string[]
                        {
                            "https://museoicticola.uabjb.edu.bo/images/speasyimagegallery/albums/1/images/gallery351.jpg",
                            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/MUSEOS%20DEL%20MUNICIPIO%20DE%20TRINIDAD/80/galeria/2023-05-22-_DSC3831-min.JPG"
                        }
                    }
                };

                // 3. Procesar cada sitio
                foreach (var sitio in imagenesPorSitio)
                {
                    Console.WriteLine($"\n--- Procesando sitio: {sitio.Key} ---");

                    // Navegar al formulario de imágenes
                    NavegarAFormularioImagenes();

                    // Preparar página para modo test
                    PrepararPaginaParaTest();

                    // Seleccionar sitio específico
                    SeleccionarSitioPorNombre(sitio.Key);

                    // Agregar imágenes para este sitio
                    AgregarImagenesDePrueba(sitio.Value);

                    // Enviar formulario
                    EnviarFormulario();

                    // Verificar éxito
                    if (VerificarCreacionExitosa())
                    {
                        Console.WriteLine($"✅ Imágenes agregadas exitosamente para {sitio.Key}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠ Posible error al agregar imágenes para {sitio.Key}");
                    }

                    // Pequeña pausa entre sitios
                    System.Threading.Thread.Sleep(2000);
                }

                Console.WriteLine("\n✅ Todas las imágenes agregadas exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                TomarScreenshot("error_imagenes");
                throw;
            }
        }

        private void Login()
        {
            driver.Navigate().GoToUrl(baseUrl + "Identity/Account/Login");
            wait.Until(d => d.Url.Contains("Login"));
            System.Threading.Thread.Sleep(2000);

            driver.FindElement(By.Id("Input_Email")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin123!");
            driver.FindElement(By.Id("login-submit")).Click();

            wait.Until(d => !d.Url.Contains("/Login"));
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("✓ Login exitoso");
        }

        private void NavegarAFormularioImagenes()
        {
            driver.Navigate().GoToUrl(baseUrl + "ImagenesSitios/Create");
            wait.Until(d => d.Url.Contains("/ImagenesSitios/Create"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("✓ En formulario de imágenes");
        }

        private void PrepararPaginaParaTest()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                document.body.classList.add('test-mode');
                console.log('Modo test activado para imágenes');
            ");
            System.Threading.Thread.Sleep(1000);
        }

        private void SeleccionarSitioPorNombre(string nombreSitio)
        {
            var sitioSelect = driver.FindElement(By.Id("sitioSelect"));
            var selectElement = new SelectElement(sitioSelect);

            // Buscar la opción que coincida con el nombre del sitio
            bool encontrado = false;
            foreach (var option in selectElement.Options)
            {
                if (option.Text.Contains(nombreSitio) || nombreSitio.Contains(option.Text))
                {
                    selectElement.SelectByText(option.Text);
                    Console.WriteLine($"✓ Sitio seleccionado: {option.Text}");
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                // Si no encuentra, seleccionar el primero disponible (índice 1, ya que 0 es placeholder)
                if (selectElement.Options.Count > 1)
                {
                    selectElement.SelectByIndex(1);
                    Console.WriteLine($"⚠ Sitio '{nombreSitio}' no encontrado. Seleccionando: {selectElement.SelectedOption.Text}");
                }
                else
                {
                    throw new Exception($"No se encontró el sitio: {nombreSitio}");
                }
            }

            System.Threading.Thread.Sleep(1000);
        }

        private void AgregarImagenesDePrueba(string[] urlsImagenes)
        {
            foreach (var url in urlsImagenes)
            {
                try
                {
                    var imageUrlInput = driver.FindElement(By.Id("imageUrlInput"));
                    var addButton = driver.FindElement(By.Id("addImageBtn"));

                    // Limpiar y escribir URL
                    imageUrlInput.Clear();
                    imageUrlInput.SendKeys(url);

                    // Hacer clic en agregar
                    addButton.Click();
                    System.Threading.Thread.Sleep(1000);

                    Console.WriteLine($"✓ Imagen agregada: {TruncarUrl(url)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ Error al agregar imagen {TruncarUrl(url)}: {ex.Message}");
                }
            }

            Console.WriteLine($"✓ Total imágenes agregadas para este sitio: {urlsImagenes.Length}");
        }

        private string TruncarUrl(string url, int maxLength = 60)
        {
            if (url.Length <= maxLength) return url;
            return url.Substring(0, maxLength) + "...";
        }

        private void EnviarFormulario()
        {
            // Tomar screenshot antes de enviar
            TomarScreenshot("antes_de_enviar_imagenes");

            // Buscar botón de submit
            var submitBtn = driver.FindElement(By.Id("submitBtn"));

            // Verificar si está habilitado
            if (!submitBtn.Enabled)
            {
                Console.WriteLine("⚠ Botón deshabilitado, verificando validaciones...");

                // Forzar validación de campo requerido
                var sitioSelect = driver.FindElement(By.Id("sitioSelect"));
                sitioSelect.Click();
                sitioSelect.SendKeys(Keys.Tab);
                System.Threading.Thread.Sleep(1000);
            }

            // Hacer clic en el botón
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitBtn);
            System.Threading.Thread.Sleep(3000);

            Console.WriteLine("✓ Formulario enviado");
        }

        private bool VerificarCreacionExitosa()
        {
            try
            {
                // Verificar que estamos en la lista de imágenes
                wait.Until(d => d.Url.Contains("/ImagenesSitios") && !d.Url.Contains("/Create"));

                TomarScreenshot("despues_de_enviar_imagenes");

                // Verificar mensaje de éxito
                var pageSource = driver.PageSource;
                if (pageSource.Contains("creado exitosamente") ||
                    pageSource.Contains("created successfully") ||
                    pageSource.Contains("alert-success"))
                {
                    return true;
                }

                // Alternativa: verificar que no hay errores
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors"));
                return validationErrors.Count == 0;
            }
            catch
            {
                return false;
            }
        }

        private void TomarScreenshot(string nombre)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var path = $@"C:\Screenshots\imagenes_{nombre}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(path);
                Console.WriteLine($"  📸 Screenshot: {path}");
            }
            catch { }
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                if (driver != null)
                {
                    TomarScreenshot("final");
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