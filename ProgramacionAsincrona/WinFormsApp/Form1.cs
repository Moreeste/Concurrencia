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
            await Esperar();
            var nombre = txtInput.Text;

            try
            {
                var saludo = await ObtenerSaludo(nombre);
                MessageBox.Show(saludo);
            }
            catch(HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
            loadingGif.Visible = false;
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
