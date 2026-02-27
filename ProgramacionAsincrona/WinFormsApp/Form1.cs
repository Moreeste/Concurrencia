using Newtonsoft.Json;
using System.Diagnostics;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly string _apiUrl;
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();
            _apiUrl = "https://localhost:7186";
            _httpClient = new HttpClient();
        }

        //Bloquear la interfaz de usuario
        //private void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    Thread.Sleep(5000);
        //}

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            loadingGif.Visible = true;

            var tarjetas = ObtenerTarjetasDeCredito(25000);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await ProcesarTarjetas(tarjetas);
            }
            catch(HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }

            stopwatch.Stop();
            MessageBox.Show($"Operación finalizada en: {stopwatch.Elapsed.TotalSeconds} segundos");

            loadingGif.Visible = false;
        }

        private async Task ProcesarTarjetas(List<string> tarjetas)
        {
            var tareas = new List<Task>();

            foreach (var tarjeta in tarjetas)
            {
                var json = JsonConvert.SerializeObject(tarjeta);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var respuestaTask = _httpClient.PostAsync($"{_apiUrl}/api/Tarjetas", content);
                tareas.Add(respuestaTask);
            }

            await Task.WhenAll(tareas);
        }

        private List<string> ObtenerTarjetasDeCredito(int cantidadDeTarjetas)
        {
            var tarjetas = new List<string>();

            for (int i = 0; i < cantidadDeTarjetas; i++)
            {
                tarjetas.Add(i.ToString().PadLeft(16, '0'));
            }

            return tarjetas;
        }

        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        private async Task<string> ObtenerSaludo(string nombre)
        {
            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos2/{nombre}"))
            {
                respuesta.EnsureSuccessStatusCode();
                var saludo = await respuesta.Content.ReadAsStringAsync();
                return saludo;
            }
        }
    }
}
