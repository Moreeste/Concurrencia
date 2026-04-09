using System.Collections.Concurrent;
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

            var mutexA = new object();
            var mutexB = new object();

            var tarea1 = Task.Run(() =>
            {
                Parallel.For(1, 100000, i =>
                {
                    lock (mutexA)
                    {
                        lock (mutexB)
                        {
                            var valor = i;
                        }
                    }
                });
            });

            var tarea2 = Task.Run(() =>
            {
                Parallel.For(1, 100000, i =>
                {
                    lock (mutexB)
                    {
                        lock (mutexA)
                        {
                            var valor = i;
                        }
                    }
                });
            });

            await Task.WhenAll(tarea1, tarea2);

            Console.WriteLine("Fin");
            loadingGif.Visible = false;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var mutex = new object();
        //    Random random = new Random();
        //    var diccionarioConcurrente = new ConcurrentDictionary<double, int>();

        //    Parallel.For(1, 1000000, i =>
        //    {
        //        double llave;
        //        lock (mutex)
        //        {
        //            llave = random.NextDouble();
        //        }
        //        diccionarioConcurrente.AddOrUpdate(llave, 1, (llave, valorAnterior) => valorAnterior + 1);
        //    });

        //    var masFrecuentes = diccionarioConcurrente.OrderByDescending(x => x.Value).Take(5).ToList();

        //    foreach (var item in masFrecuentes)
        //    {
        //        Console.WriteLine($"Valor: {item.Key}, Frecuencia: {item.Value}");
        //    }

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var stopwatch = new Stopwatch();

        //    stopwatch.Start();
        //    var matrices = Enumerable.Range(1, 1000).AsParallel().Select(x => Matrices.InicializarMatriz(750, 750)).ToList();

        //    var tiempoParalelismo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Paralelismo: {tiempoParalelismo} segundos.");

        //    stopwatch.Restart();
        //    var matrices2 = Enumerable.Range(1, 1000).AsParallel().Select(x => Matrices.InicializarMatrizSaturado(750, 750)).ToList();

        //    var tiempoSobreSaturacion = stopwatch.Elapsed.TotalSeconds;

        //    Console.WriteLine($"Sobre saturación: {tiempoSobreSaturacion} segundos.");
        //    EscribirComparacion(tiempoParalelismo, tiempoSobreSaturacion);

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        private void EscribirComparacion(double tiempo1, double tiempo2)
        {
            var diferencia = tiempo2 - tiempo1;
            diferencia = Math.Round(diferencia, 2);
            var incrementoPorcentual = ((tiempo2 - tiempo1) / tiempo1) * 100;
            incrementoPorcentual = Math.Round(incrementoPorcentual, 2);
            Console.WriteLine($"Diferencia: {diferencia}, {incrementoPorcentual}%");
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var valorSinInterlocked = 0;
        //    Parallel.For(0, 1000000, i =>
        //    {
        //        valorSinInterlocked++;
        //    });
        //    Console.WriteLine($"Sumatoria sin interlocked: {valorSinInterlocked}");


        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

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
