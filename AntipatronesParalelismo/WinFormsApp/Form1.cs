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
            Console.WriteLine("Inicio");



            Console.WriteLine("Fin");
            loadingGif.Visible = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
