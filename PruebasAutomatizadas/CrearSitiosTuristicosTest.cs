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
            public string Latitud { get; set; }
            public string Longitud { get; set; }
            public int TipoId { get; set; }
        }

        // Datos de los sitios a insertar
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
                Latitud = "-14.839810",
                Longitud = "-64.903800",
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
                Latitud = "-14.832900",
                Longitud = "-64.915500",
                TipoId = 1
            },
            new SitioData
            {
                Nombre = "Plaza de la Tradición",
                NombreIngles = "Tradition Square",
                NombrePortugues = "Praça da Tradição",
                Descripcion = "Un espacio cultural que tiene murales y esculturas sobre las costumbres locales, como el 'jocheo de toros' y el 'palo encebao'.",
                DescripcionIngles = "A cultural space with murals and sculptures about local customs, such as 'jocheo de toros' and 'palo encebao'.",
                DescripcionPortugues = "Um espaço cultural com murais e esculturas sobre costumes locais, como o 'jocheo de toros' e o 'palo encebao'.",
                Direccion = "Av. 6 de Agosto",
                Latitud = "-14.845000",
                Longitud = "-64.901500",
                TipoId = 1
            },
            new SitioData
            {
                Nombre = "Plazuela Doris Natusch",
                NombreIngles = "Doris Natusch Plaza",
                NombrePortugues = "Praça Doris Natusch",
                Descripcion = "Un rincón histórico y artístico que rinde homenaje a la cultura beniana, ideal para una pausa tranquila.",
                DescripcionIngles = "A historical and artistic corner that pays homage to the Benian culture, ideal for a quiet break.",
                DescripcionPortugues = "Um canto histórico e artístico que homenageia a cultura beniana, ideal para uma pausa tranquila.",
                Direccion = "Cerca de la Av. Germán Busch",
                Latitud = "-14.835500",
                Longitud = "-64.908000",
                TipoId = 1
            },
            new SitioData
            {
                Nombre = "Parque Pantanal",
                NombreIngles = "Pantanal Park",
                NombrePortugues = "Parque Pantanal",
                Descripcion = "Un parque tipo zoológico urbano donde se pueden observar animales de la región (capibaras, caimanes) en un entorno semi-natural.",
                DescripcionIngles = "An urban zoo-like park where you can observe regional animals (capybaras, caimans) in a semi-natural environment.",
                DescripcionPortugues = "Um parque urbano tipo zoológico onde você pode observar animais da região (capivaras, jacarés) em um ambiente semi-natural.",
                Direccion = "Cruce Av. Ganadero y Av. Circunvalación",
                Latitud = "-14.825000",
                Longitud = "-64.920000",
                TipoId = 1
            },
            // Puertos Turísticos y Balnearios (TipoId = 2)
            new SitioData
            {
                Nombre = "Puerto Almacén",
                NombreIngles = "Almacén Port",
                NombrePortugues = "Porto Almacén",
                Descripcion = "Famoso por sus restaurantes rústicos donde sirven deliciosos platos de pescado de río y por los paseos en canoa entre la selva de galería.",
                DescripcionIngles = "Famous for its rustic restaurants serving delicious river fish dishes and for canoe trips through the gallery forest.",
                DescripcionPortugues = "Famoso por seus restaurantes rústicos que servem deliciosos pratos de peixe de rio e pelos passeios de canoa pela mata ciliar.",
                Direccion = "A orillas del Río Ibare, a 8km de Trinidad",
                Latitud = "-14.870000",
                Longitud = "-64.880000",
                TipoId = 2
            },
            new SitioData
            {
                Nombre = "Puerto Ballivián",
                NombreIngles = "Ballivián Port",
                NombrePortugues = "Porto Ballivián",
                Descripcion = "Lugar histórico y muy tranquilo a orillas del río Ibare, ideal para caminar por su costanera turística y refrescarse. Cerca de Loma Suárez.",
                DescripcionIngles = "A quiet and historic spot on the banks of the Ibare River, ideal for walking along its tourist promenade and cooling off. Near Loma Suárez.",
                DescripcionPortugues = "Um lugar tranquilo e histórico às margens do Rio Ibare, ideal para passear pela sua orla turística e se refrescar. Perto de Loma Suárez.",
                Direccion = "A orillas del Río Ibare, cerca de Loma Suárez",
                Latitud = "-14.855000",
                Longitud = "-64.885000",
                TipoId = 2
            },
            new SitioData
            {
                Nombre = "Puerto Varador",
                NombreIngles = "Varador Port",
                NombrePortugues = "Porto Varador",
                Descripcion = "Puerto comercial y turístico a orillas del majestuoso río Mamoré. En época seca, se forman playas de arena donde la gente va a bañarse.",
                DescripcionIngles = "Commercial and tourist port on the banks of the majestic Mamoré River. In the dry season, sand beaches form where people go swimming.",
                DescripcionPortugues = "Porto comercial e turístico às margens do majestoso Rio Mamoré. Na estação seca, formam-se praias de areia onde as pessoas vão nadar.",
                Direccion = "A orillas del Río Mamoré, a 13km de Trinidad",
                Latitud = "-14.800000",
                Longitud = "-64.980000",
                TipoId = 2
            },
            new SitioData
            {
                Nombre = "Laguna Suárez",
                NombreIngles = "Suárez Lagoon",
                NombrePortugues = "Lagoa Suárez",
                Descripcion = "Laguna artificial construida por la antigua cultura de los Moxos. Es el balneario principal de la ciudad, con restaurantes a la orilla del agua.",
                DescripcionIngles = "Artificial lagoon built by the ancient Moxos culture. It is the city's main spa, with restaurants on the water's edge.",
                DescripcionPortugues = "Lagoa artificial construída pela antiga cultura Moxos. É o principal balneário da cidade, com restaurantes à beira d'água.",
                Direccion = "Zona Oeste de Trinidad",
                Latitud = "-14.850000",
                Longitud = "-64.915000",
                TipoId = 2
            },
            // Museos y Cultura (TipoId = 3)
            new SitioData
            {
                Nombre = "Museo Ictícola",
                NombreIngles = "Ichthyological Museum (Fish Museum)",
                NombrePortugues = "Museu Ictícola (Museu de Peixes)",
                Descripcion = "Ubicado en el campus de la UAB. Tiene una de las colecciones de peces de agua dulce más grandes de Sudamérica, incluyendo el famoso bufeo disecado.",
                DescripcionIngles = "Located on the UAB campus. It has one of the largest freshwater fish collections in South America, including the famous stuffed pink dolphin.",
                DescripcionPortugues = "Localizado no campus da UAB. Possui uma das maiores coleções de peixes de água doce da América do Sul, incluindo o famoso golfinho rosa dissecado.",
                Direccion = "Campus UAB, Av. Ganadero",
                Latitud = "-14.815000",
                Longitud = "-64.905000",
                TipoId = 3
            },
            new SitioData
            {
                Nombre = "Museo Etno-Arqueológico 'Kenneth Lee'",
                NombreIngles = "Kenneth Lee Ethno-Archaeological Museum",
                NombrePortugues = "Museu Etno-Arqueológico 'Kenneth Lee'",
                Descripcion = "Imprescindible para conocer la 'Cultura Hidráulica de Moxos'. Explica cómo los antiguos habitantes manejaban las inundaciones con lomas y camellones.",
                DescripcionIngles = "Essential for understanding the 'Hydraulic Culture of Moxos'. It explains how ancient inhabitants managed floods with mounds and raised fields.",
                DescripcionPortugues = "Essencial para entender a 'Cultura Hidráulica de Moxos'. Explica como os antigos habitantes gerenciavam as inundações com montes e campos elevados.",
                Direccion = "Cerca de la Plazuela Doris Natusch",
                Latitud = "-14.840000",
                Longitud = "-64.905000",
                TipoId = 3
            },
            // Naturaleza y Ecoturismo (TipoId = 4)
            new SitioData
            {
                Nombre = "Santuario Ecológico Chuchini",
                NombreIngles = "Chuchini Ecological Sanctuary",
                NombrePortugues = "Santuário Ecológico Chuchini",
                Descripcion = "Refugio de vida silvestre y sitio arqueológico donde puedes hacer caminatas por la selva, ver lagartos y aves, y navegar por los canales.",
                DescripcionIngles = "Wildlife refuge and archaeological site where you can hike through the jungle, see alligators and birds, and navigate the channels.",
                DescripcionPortugues = "Refúgio de vida selvagem e sítio arqueológico onde você pode fazer caminhadas na selva, ver jacarés e pássaros, e navegar pelos canais.",
                Direccion = "A 14 km de Trinidad por el río Ibare",
                Latitud = "-14.890000",
                Longitud = "-64.870000",
                TipoId = 4
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
        public void Crear_Multiples_Sitios_Exitoso()
        {
            Console.WriteLine("=== INICIO PRUEBA: Crear Múltiples Sitios Turísticos ===");

            // PASO 1: LOGIN
            Console.WriteLine("\n[PASO 1] Iniciando sesión...");
            Login();

            // PASO 2: NAVEGAR A SITIOS TURÍSTICOS
            Console.WriteLine("\n[PASO 2] Navegando por el navbar a Sitios Turísticos...");
            NavegarASitiosViaNavbar();

            // PASO 3: CREAR CADA SITIO
            Console.WriteLine($"\n[PASO 3] Creando {sitiosData.Count} sitios turísticos...");

            int sitiosCreadosExitosamente = 0;

            for (int i = 0; i < sitiosData.Count; i++)
            {
                var sitio = sitiosData[i];
                Console.WriteLine($"\n--- Creando Sitio {i + 1}/{sitiosData.Count}: '{sitio.Nombre}' ---");

                try
                {
                    // 3.1: Hacer clic en "Create New" desde el index de Sitios
                    Console.WriteLine("  - Haciendo clic en 'Create New'...");
                    var createNewLink = wait.Until(d => d.FindElement(By.LinkText("Create New")));
                    createNewLink.Click();

                    // Esperar que el formulario cargue
                    wait.Until(d => d.FindElement(By.Id("nombreInput")));
                    Console.WriteLine("  ✓ Formulario cargado");

                    // 3.2: Llenar el formulario
                    Console.WriteLine("  - Llenando formulario...");
                    LlenarFormularioSitio(sitio);

                    // 3.3: Dar clic en "Crear" (Submit)
                    Console.WriteLine("  - Enviando formulario...");
                    HacerClicEnCrearSitio();

                    // 3.4: Verificar que regresa a la lista (index de Sitios)
                    wait.Until(d => d.Url.Contains("/SitiosTuristicos") && !d.Url.Contains("/Create"));
                    Console.WriteLine("  ✓ Redireccionado a lista de Sitios");

                    // 3.5: Verificar que el sitio aparece en la tabla
                    System.Threading.Thread.Sleep(1500);
                    if (VerificarSitioEnTabla(sitio.Nombre))
                    {
                        Console.WriteLine($"  ✓ Sitio '{sitio.Nombre}' creado exitosamente");
                        sitiosCreadosExitosamente++;
                    }
                    else
                    {
                        Console.WriteLine($"  ⚠ Sitio '{sitio.Nombre}' creado pero no aparece en la tabla");
                    }

                    // Solo esperar un momento si no es el último sitio
                    if (i < sitiosData.Count - 1)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ERROR creando sitio '{sitio.Nombre}': {ex.Message}");
                    Console.WriteLine($"  Detalles del error: {ex.StackTrace}");

                    // Si hay error, intentar regresar a la lista de sitios
                    try
                    {
                        driver.Navigate().GoToUrl(baseUrl + "SitiosTuristicos");
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch
                    {
                        // Si falla, recargar y navegar de nuevo
                        driver.Navigate().Refresh();
                        System.Threading.Thread.Sleep(2000);
                        NavegarASitiosViaNavbar();
                    }
                }
            }

            // RESULTADO FINAL
            Console.WriteLine($"\n=== RESULTADO FINAL ===");
            Console.WriteLine($"Sitios a crear: {sitiosData.Count}");
            Console.WriteLine($"Sitios creados exitosamente: {sitiosCreadosExitosamente}");

            if (sitiosCreadosExitosamente == sitiosData.Count)
            {
                Console.WriteLine("✅ ¡TODOS los sitios fueron creados exitosamente!");
            }
            else if (sitiosCreadosExitosamente > 0)
            {
                Console.WriteLine($"⚠ {sitiosData.Count - sitiosCreadosExitosamente} sitios no fueron creados");
            }
            else
            {
                Console.WriteLine("❌ No se pudo crear ningún sitio");
            }

            Assert.IsTrue(sitiosCreadosExitosamente >= sitiosData.Count - 2,
                $"Se esperaba crear al menos {sitiosData.Count - 2} sitios");
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

        private void NavegarASitiosViaNavbar()
        {
            Console.WriteLine("[2] Navegando a SitiosTuristicos...");
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/SitiosTuristicos')]"))).Click();

            wait.Until(d => d.Url.Contains("/SitiosTuristicos"));
            Console.WriteLine("✓ En página de Tipos");
        }

        private void LlenarFormularioSitio(SitioData sitio)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Esperar a que el formulario cargue completamente
            System.Threading.Thread.Sleep(1500);

            // 1. Coordenadas (mapa simplificado - inputs directos)
            Console.WriteLine("    - Llenando coordenadas...");

            // Latitud
            var latInput = wait.Until(d => d.FindElement(By.Id("latInput")));
            ScrollToElement(latInput);
            latInput.Clear();
            latInput.SendKeys(sitio.Latitud);

            // Longitud
            var lngInput = driver.FindElement(By.Id("lngInput"));
            ScrollToElement(lngInput);
            lngInput.Clear();
            lngInput.SendKeys(sitio.Longitud);

            // 2. Información en Español (pestaña activa por defecto)
            Console.WriteLine("    - Llenando información en español...");

            // Nombre Español
            var nombreInput = wait.Until(d => d.FindElement(By.Id("nombreInput")));
            ScrollToElement(nombreInput);
            nombreInput.Clear();
            nombreInput.SendKeys(sitio.Nombre);

            // Descripción Español
            var descripcionInput = driver.FindElement(By.Id("descripcionInput"));
            ScrollToElement(descripcionInput);
            descripcionInput.Clear();
            descripcionInput.SendKeys(sitio.Descripcion);

            // Dirección
            var direccionInput = driver.FindElement(By.Id("direccionInput"));
            ScrollToElement(direccionInput);
            direccionInput.Clear();
            direccionInput.SendKeys(sitio.Direccion);

            // 3. Cambiar a pestaña Inglés
            Console.WriteLine("    - Llenando información en inglés...");
            try
            {
                var englishTab = driver.FindElement(By.Id("english-tab"));
                ScrollAndClick(englishTab);
                System.Threading.Thread.Sleep(500);

                // Nombre Inglés
                var nombreInglesInput = wait.Until(d => d.FindElement(By.Id("nombreInglesInput")));
                ScrollToElement(nombreInglesInput);
                nombreInglesInput.Clear();
                nombreInglesInput.SendKeys(sitio.NombreIngles);

                // Descripción Inglés
                var descripcionInglesInput = driver.FindElement(By.Id("descripcionInglesInput"));
                ScrollToElement(descripcionInglesInput);
                descripcionInglesInput.Clear();
                descripcionInglesInput.SendKeys(sitio.DescripcionIngles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ⚠ Error en pestaña inglés: {ex.Message}");
            }

            // 4. Cambiar a pestaña Portugués
            Console.WriteLine("    - Llenando información en portugués...");
            try
            {
                var portuguesTab = driver.FindElement(By.Id("portuguese-tab"));
                ScrollAndClick(portuguesTab);
                System.Threading.Thread.Sleep(500);

                // Nombre Portugués
                var nombrePortuguesInput = wait.Until(d => d.FindElement(By.Id("nombrePortuguesInput")));
                ScrollToElement(nombrePortuguesInput);
                nombrePortuguesInput.Clear();
                nombrePortuguesInput.SendKeys(sitio.NombrePortugues);

                // Descripción Portugués
                var descripcionPortuguesInput = driver.FindElement(By.Id("descripcionPortuguesInput"));
                ScrollToElement(descripcionPortuguesInput);
                descripcionPortuguesInput.Clear();
                descripcionPortuguesInput.SendKeys(sitio.DescripcionPortugues);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ⚠ Error en pestaña portugués: {ex.Message}");
            }

            // 5. Seleccionar Tipo de Sitio
            Console.WriteLine("    - Seleccionando tipo de sitio...");
            try
            {
                var tipoSelect = driver.FindElement(By.Id("tipoSelect"));
                ScrollToElement(tipoSelect);

                // Crear SelectElement
                var selectElement = new SelectElement(tipoSelect);

                // Intentar seleccionar por valor (TipoId)
                try
                {
                    selectElement.SelectByValue(sitio.TipoId.ToString());
                    Console.WriteLine($"    ✓ Tipo seleccionado: {sitio.TipoId}");
                }
                catch
                {
                    // Si falla, seleccionar por índice (el primer elemento es el placeholder)
                    selectElement.SelectByIndex(sitio.TipoId); // TipoId 1, 2, 3, 4
                    Console.WriteLine($"    ✓ Tipo seleccionado por índice: {sitio.TipoId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ⚠ Error seleccionando tipo: {ex.Message}");
            }

            // 6. Opcional: Usar ubicación por defecto de Trinidad
            Console.WriteLine("    - Usando ubicación por defecto...");
            try
            {
                var defaultLocationBtn = driver.FindElement(By.XPath("//button[contains(text(),'Usar ubicación de Trinidad')]"));
                ScrollAndClick(defaultLocationBtn);
                System.Threading.Thread.Sleep(500);
            }
            catch
            {
                // Ignorar si no existe el botón
            }
        }

        private void HacerClicEnCrearSitio()
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
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Crear Sitio')]")); }
                catch { }
                try { submitButton = driver.FindElement(By.XPath("//button[contains(text(),'Create Site')]")); }
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

        private bool VerificarSitioEnTabla(string nombreSitio)
        {
            try
            {
                // Buscar en el contenido de la página
                return driver.PageSource.Contains(nombreSitio);
            }
            catch
            {
                return false;
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
                    string screenshotPath = $@"C:\Screenshots\sitios_test_{timestamp}.png";
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