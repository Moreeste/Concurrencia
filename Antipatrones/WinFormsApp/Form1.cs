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

            //Antipatron: Sincrono dento de asincrono
            var valor = ObtenerValor().Result;

            Console.WriteLine(valor);

            loadingGif.Visible = false;
        }

        private async Task<string> ObtenerValor()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return "Esteban";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
