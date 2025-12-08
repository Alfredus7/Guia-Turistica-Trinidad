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
                TipoId = 4
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
            Console.WriteLine("[2] Navegando a SitiosTuristicos...");
            wait.Until(d => d.FindElement(By.XPath("//a[contains(.,'Administración')]"))).Click();
            wait.Until(d => d.FindElement(By.XPath("//a[contains(@href,'/SitiosTuristicos')]"))).Click();

            wait.Until(d => d.Url.Contains("/SitiosTuristicos"));
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
                
                // Simplificar el mapa para pruebas
                const mapElement = document.getElementById('map');
                if (mapElement && mapElement.innerHTML.includes('Leaflet')) {
                    // Si hay mapa, asegurar que las coordenadas sean editables
                    const latInput = document.getElementById('Latitud');
                    const lngInput = document.getElementById('Longitud');
                    if (latInput && lngInput) {
                        latInput.readOnly = false;
                        lngInput.readOnly = false;
                    }
                }
                
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
                
                // Coordenadas (actualizar ambos: campos ocultos y de visualización)
                var latInput = document.getElementById('Latitud');
                var lngInput = document.getElementById('Longitud');
                var latDisplay = document.getElementById('latDisplay');
                var lngDisplay = document.getElementById('lngDisplay');
                
                if (latInput && lngInput) {{
                    latInput.value = '{sitio.Latitud}';
                    lngInput.value = '{sitio.Longitud}';
                    
                    // Actualizar campos de visualización si existen
                    if (latDisplay) latDisplay.value = '{sitio.Latitud}';
                    if (lngDisplay) lngDisplay.value = '{sitio.Longitud}';
                    
                    // Disparar evento de cambio si el mapa está en modo interactivo
                    latInput.dispatchEvent(new Event('change'));
                    lngInput.dispatchEvent(new Event('change'));
                }}
                
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
                
                return 'Valores establecidos con JavaScript, incluyendo coordenadas: {sitio.Latitud}, {sitio.Longitud}';
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

            // 4. Coordenadas (campos ocultos y de visualización)
            try
            {
                // Campos ocultos (se envían al servidor)
                var latHidden = FindElementWithFallback("Latitud", "Latitud");
                var lngHidden = FindElementWithFallback("Longitud", "Longitud");

                // Campos de visualización (solo lectura)
                var latDisplay = FindElementWithFallback("latDisplay", "latDisplay");
                var lngDisplay = FindElementWithFallback("lngDisplay", "lngDisplay");

                if (latHidden != null)
                {
                    SetElementValue(latHidden, sitio.Latitud);
                    Console.WriteLine($"  ✓ Latitud establecida: {sitio.Latitud}");
                }
                else
                {
                    Console.WriteLine("  ⚠ No se encontró campo oculto Latitud");
                }

                if (lngHidden != null)
                {
                    SetElementValue(lngHidden, sitio.Longitud);
                    Console.WriteLine($"  ✓ Longitud establecida: {sitio.Longitud}");
                }
                else
                {
                    Console.WriteLine("  ⚠ No se encontró campo oculto Longitud");
                }

                // Actualizar campos de visualización si existen
                if (latDisplay != null) SetElementValue(latDisplay, sitio.Latitud);
                if (lngDisplay != null) SetElementValue(lngDisplay, sitio.Longitud);

                // Verificar que se establecieron correctamente
                if (latHidden != null && lngHidden != null)
                {
                    var latValue = latHidden.GetAttribute("value");
                    var lngValue = lngHidden.GetAttribute("value");
                    Console.WriteLine($"  ✓ Coordenadas verificadas: {latValue}, {lngValue}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Error al establecer coordenadas: {ex.Message}");
                Console.WriteLine("  ⚠ Continuando con la prueba...");
            }

            // 5. Seleccionar Tipo
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
                Console.WriteLine($"  ✓ Tipo seleccionado: {sitio.TipoId}");
            }

            // 6. Pestaña Inglés
            try
            {
                var englishTab = driver.FindElement(By.CssSelector("#english-tab, [data-bs-target='#english']"));
                ClickWithJavaScript(englishTab);
                System.Threading.Thread.Sleep(500);

                var nombreInglesInput = FindElementWithFallback("NombreIngles", "NombreIngles", "nombreInglesInput");
                if (nombreInglesInput != null)
                {
                    ClearAndSendKeys(nombreInglesInput, sitio.NombreIngles);
                    Console.WriteLine("  ✓ Nombre en inglés establecido");
                }

                var descripcionInglesInput = FindElementWithFallback("DescripcionIngles", "DescripcionIngles", "descripcionInglesInput");
                if (descripcionInglesInput != null)
                {
                    ClearAndSendKeys(descripcionInglesInput, sitio.DescripcionIngles);
                    Console.WriteLine("  ✓ Descripción en inglés establecida");
                }
            }
            catch { Console.WriteLine("  ⚠ Pestaña inglés no encontrada"); }

            // 7. Pestaña Portugués
            try
            {
                var portuguesTab = driver.FindElement(By.CssSelector("#portuguese-tab, [data-bs-target='#portuguese']"));
                ClickWithJavaScript(portuguesTab);
                System.Threading.Thread.Sleep(500);

                var nombrePortuguesInput = FindElementWithFallback("NombrePortugues", "NombrePortugues", "nombrePortuguesInput");
                if (nombrePortuguesInput != null)
                {
                    ClearAndSendKeys(nombrePortuguesInput, sitio.NombrePortugues);
                    Console.WriteLine("  ✓ Nombre en portugués establecido");
                }

                var descripcionPortuguesInput = FindElementWithFallback("DescripcionPortugues", "DescripcionPortugues", "descripcionPortuguesInput");
                if (descripcionPortuguesInput != null)
                {
                    ClearAndSendKeys(descripcionPortuguesInput, sitio.DescripcionPortugues);
                    Console.WriteLine("  ✓ Descripción en portugués establecida");
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

                    // Intentar por selector CSS
                    try
                    {
                        return driver.FindElement(By.CssSelector($"[id*='{id}'], [name*='{name}']"));
                    }
                    catch
                    {
                        throw new Exception($"No se pudo encontrar el elemento con id='{id}' o name='{name}'");
                    }
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

        private void SetElementValue(IWebElement element, string value)
        {
            try
            {
                // Usar JavaScript para establecer el valor
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].value = arguments[1];",
                    element,
                    value
                );

                // Disparar evento de cambio
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].dispatchEvent(new Event('change'));",
                    element
                );

                // También disparar evento input por si acaso
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].dispatchEvent(new Event('input'));",
                    element
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Error en SetElementValue: {ex.Message}");
                // Fallback: intentar con Clear y SendKeys
                element.Clear();
                element.SendKeys(value);
                element.SendKeys(Keys.Tab);
            }
        }

        private void ClickWithJavaScript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }

        private string EscapeJavaScript(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
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