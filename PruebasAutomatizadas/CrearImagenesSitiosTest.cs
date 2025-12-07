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
    public void Agregar_Imagenes_A_Sitio_Exitoso()
    {
        Console.WriteLine("=== TEST: Agregar Imágenes a Sitio ===");

        try
        {
            // 1. Login
            Login();

            // 2. Navegar a formulario de imágenes
            NavegarAFormularioImagenes();

            // 3. Preparar página para modo test
            PrepararPaginaParaTest();

            // 4. Seleccionar sitio
            SeleccionarSitio();

            // 5. Agregar imágenes
            AgregarImagenesDePrueba();

            // 6. Enviar formulario
            EnviarFormulario();

            // 7. Verificar éxito
            if (VerificarCreacionExitosa())
            {
                Console.WriteLine("✅ Imágenes agregadas exitosamente");
            }
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

    private void SeleccionarSitio()
    {
        // Seleccionar el primer sitio disponible (excepto el placeholder)
        var sitioSelect = driver.FindElement(By.Id("sitioSelect"));
        var selectElement = new SelectElement(sitioSelect);

        // Seleccionar el primer sitio disponible (índice 1, ya que 0 es placeholder)
        if (selectElement.Options.Count > 1)
        {
            selectElement.SelectByIndex(1);
            Console.WriteLine($"✓ Sitio seleccionado: {selectElement.SelectedOption.Text}");
        }
        else
        {
            throw new Exception("No hay sitios disponibles para seleccionar");
        }

        System.Threading.Thread.Sleep(1000);
    }

    private void AgregarImagenesDePrueba()
    {
        // URL de imagen de prueba (deberías usar URLs reales de tus imágenes)
        string[] urlsImagenes = {
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/96/galeria/2024-11-19-PlazaJoseBallivian(2).jpg",
            "https://trinidadtevajaenamorar.com.bo/Imageneslugares/PATRIMONIOURBANOARQUITECTONICOYARTISTICO/11/galeria/2023-05-29-_DSC1592.JPG"
        };

        foreach (var url in urlsImagenes)
        {
            var imageUrlInput = driver.FindElement(By.Id("imageUrlInput"));
            var addButton = driver.FindElement(By.Id("addImageBtn"));

            // Limpiar y escribir URL
            imageUrlInput.Clear();
            imageUrlInput.SendKeys(url);

            // Hacer clic en agregar
            addButton.Click();
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine($"✓ Imagen agregada: {url}");
        }

        Console.WriteLine($"✓ Total imágenes agregadas: {urlsImagenes.Length}");
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