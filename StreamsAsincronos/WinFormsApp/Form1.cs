namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly string _apiUrl;
        private readonly HttpClient _httpClient;
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            _apiUrl = "https://localhost:7186";
            _httpClient = new HttpClient();
        }
        
        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            loadingGif.Visible = true;

            //foreach (var nombre in GenerarNombres())
            //{
            //    Console.WriteLine(nombre);
            //}

            await foreach (var nombre in GenerarNombresAsync())
            {
                Console.WriteLine(nombre);
            }

            loadingGif.Visible = false;
        }

        private IEnumerable<string> GenerarNombres()
        {
            yield return "Esteban";
            yield return "Alejandra";
        }

        private async IAsyncEnumerable<string> GenerarNombresAsync()
        {
            yield return "Esteban";
            await Task.Delay(2000);
            yield return "Alejandra";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
