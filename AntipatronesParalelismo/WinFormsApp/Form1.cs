using System.Diagnostics;

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

            var valorSinInterlocked = 0;
            Parallel.For(0, 1000000, i =>
            {
                valorSinInterlocked++;
            });
            Console.WriteLine($"Sumatoria sin interlocked: {valorSinInterlocked}");


            Console.WriteLine("Fin");
            loadingGif.Visible = false;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var stopwatch = new Stopwatch();
        //    var max = int.MaxValue / 3;
        //    var numeros = Enumerable.Range(0, max);

        //    stopwatch.Start();
        //    await Task.Run(() =>
        //    {
        //        foreach (var numero in numeros)
        //        {
        //            var resultado = numero + numero;
        //        }
        //    });
        //    Console.WriteLine($"Tiempo transcurrido secuencial: {stopwatch.Elapsed.TotalSeconds} segundos.");

        //    stopwatch.Restart();
        //    await Task.Run(() =>
        //    {
        //        Parallel.ForEach(numeros, numero =>
        //        {
        //            var resultado = numero + numero;
        //        });
        //    });
        //    Console.WriteLine($"Tiempo transcurrido paralelo: {stopwatch.Elapsed.TotalSeconds} segundos.");


        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
