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

            var queryParalelo = Enumerable.Range(1, 10).AsParallel().WithDegreeOfParallelism(2)
                .Select(x => Matrices.InicializarMatriz(100, 100));

            //foreach (var matriz in queryParalelo)
            //{
            //    Console.WriteLine({matriz[0, 0]});
            //}

            queryParalelo.ForAll(matriz =>
            {
                Console.WriteLine(matriz[0, 0]);
            });

            Console.WriteLine("Fin");
            loadingGif.Visible = false;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    //var fuente = Enumerable.Range(1, 1000);
        //    //var suma = fuente.AsParallel().Sum();
        //    //var promedio = fuente.AsParallel().Average();
        //    //Console.WriteLine($"La suma es: {suma}");
        //    //Console.WriteLine($"El promedio es: {promedio}");

        //    var matrices = Enumerable.Range(1, 500).Select(x => Matrices.InicializarMatriz(1000, 1000)).ToList();
        //    Console.WriteLine("Matrices generadas");

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var sumaMatricesSecuencial = matrices.Aggregate(Matrices.SumaMatricesSecuencial);
        //    var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Tiempo secuencial: {tiempoSecuencial} seg.");
        //    stopwatch.Restart();
        //    var sumaMatricesParalelo = matrices.AsParallel().Aggregate(Matrices.SumaMatricesSecuencial);
        //    var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Tiempo paralelo: {tiempoParalelo}");
        //    EscribirComparacion(tiempoSecuencial, tiempoParalelo);

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    _cancellationTokenSource = new CancellationTokenSource();
        //    var fuente = Enumerable.Range(1, 20);
        //    var elementosPares = fuente
        //        .AsParallel().WithDegreeOfParallelism(2).WithCancellation(_cancellationTokenSource.Token)
        //        .AsOrdered().Where(x => x % 2 == 0).ToList();
        //    foreach (var elemento in elementosPares)
        //    {
        //        Console.WriteLine(elemento);
        //    }

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var valorIncrementado = 0;
        //    var valorSumado = 0;
        //    var mutex = new object();

        //    Parallel.For(0, 10000, i =>
        //    {
        //        //Interlocked.Increment(ref valorIncrementado);
        //        //Interlocked.Add(ref valorSumado, valorIncrementado);

        //        lock (mutex)
        //        {
        //            valorIncrementado++;
        //            valorSumado += valorIncrementado;
        //        }
        //    });
        //    Console.WriteLine($"Valor incrementado: {valorIncrementado}");
        //    Console.WriteLine($"Valor sumado: {valorSumado}");

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    var valorSinInterLocked = 0;
        //    Parallel.For(0, 1000000, i =>
        //    {
        //        valorSinInterLocked++;
        //    });
        //    Console.WriteLine($"Sumatoria sin interlocked: {valorSinInterLocked}");

        //    var valorConInterLocked = 0;
        //    Parallel.For(0, 1000000, i =>
        //    {
        //        Interlocked.Increment(ref valorConInterLocked);
        //    });
        //    Console.WriteLine($"Sumatoria con interlocked: {valorConInterLocked}");

        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;
        //    Console.WriteLine("Inicio");

        //    for (int i = 1; i < 13; i++)
        //    {
        //        await RealizarPruebaMatrices(i);
        //    }

        //    _cancellationTokenSource = null;
        //    Console.WriteLine("Fin");
        //    loadingGif.Visible = false;
        //}

        private async Task RealizarPruebaMatrices(int maximoGradoParalelismo)
        {
            int colCount = 2508;
            int rowCount = 1300;
            int colCount2 = 1850;
            double[,] m1 = Matrices.InicializarMatriz(rowCount, colCount);
            double[,] m2 = Matrices.InicializarMatriz(colCount, colCount2);
            double[,] result = new double[rowCount, colCount2];

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await Task.Run(() =>
                {
                    Matrices.MultiplicarMatricesParalelo(m1, m2, result, _cancellationTokenSource.Token, maximoGradoParalelismo);
                });

                stopwatch.Stop();
                Console.WriteLine($"Máximo grado: {maximoGradoParalelismo}, tiempo {stopwatch.Elapsed.TotalSeconds} seg.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operación cancelada");
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
        //    var carpetaOrigen = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
        //    var carpetaDestinoSecuencial = Path.Combine(directorioActual, @"Imagenes\foreach-secuencial");
        //    var carpetaDestinoParalelo = Path.Combine(directorioActual, @"Imagenes\foreach-paralelo");
        //    PrepararEjecucion(carpetaDestinoParalelo, carpetaDestinoSecuencial);
        //    var archivos = Directory.EnumerateFiles(carpetaOrigen);

        //    var columasMatrizA = 208;
        //    var filas = 1240;
        //    var colimasMatrizB = 750;
        //    var matrizA = Matrices.InicializarMatriz(filas, columasMatrizA);
        //    var matrizB = Matrices.InicializarMatriz(columasMatrizA, colimasMatrizB);
        //    var resultado = new double[filas, colimasMatrizB];

        //    Action multiplicarMatrices = () => Matrices.MultiplicarMatricesSecuencial(matrizA, matrizB, resultado);
        //    Action VoltearImagenes = () =>
        //    {
        //        foreach (var archivo in archivos)
        //        {
        //            VoltearImagen(archivo, carpetaDestinoSecuencial);
        //        }
        //    };
        //    Action[] acciones = new Action[] { multiplicarMatrices, VoltearImagenes };

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    foreach (var accion in acciones)
        //    {
        //        accion();
        //    }
        //    stopwatch.Stop();
        //    var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Secuencial: {tiempoSecuencial} seg.");

        //    stopwatch.Restart();
        //    Parallel.Invoke(acciones);
        //    stopwatch.Stop();
        //    var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Paralelo: {tiempoParalelo} seg.");

        //    EscribirComparacion(tiempoSecuencial, tiempoParalelo);

        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
        //    var carpetaOrigen = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
        //    var carpetaDestinoSecuencial = Path.Combine(directorioActual, @"Imagenes\foreach-secuencial");
        //    var carpetaDestinoParalelo = Path.Combine(directorioActual, @"Imagenes\foreach-paralelo");
        //    PrepararEjecucion(carpetaDestinoParalelo, carpetaDestinoSecuencial);
        //    var archivos = Directory.EnumerateFiles(carpetaOrigen);

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    foreach (var archivo in archivos)
        //    {
        //        VoltearImagen(archivo, carpetaDestinoSecuencial);
        //    }
        //    stopwatch.Stop();
        //    var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Secuencial: {tiempoSecuencial} seg.");

        //    stopwatch.Restart();
        //    Parallel.ForEach(archivos, archivo =>
        //    {
        //        VoltearImagen(archivo, carpetaDestinoParalelo);
        //    });
        //    stopwatch.Stop();
        //    var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Paralelo: {tiempoParalelo} seg.");

        //    EscribirComparacion(tiempoSecuencial, tiempoParalelo);

        //    loadingGif.Visible = false;
        //}

        private void VoltearImagen(string archivo, string carpetaDestino)
        {
            using (var image = new Bitmap(archivo))
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                var nombreArchivo = Path.GetFileName(archivo);
                var destino = Path.Combine(carpetaDestino, nombreArchivo);
                image.Save(destino);
            }
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    var columasMatrizA = 1100;
        //    var filas = 1000;
        //    var colimasMatrizB = 1750;

        //    var matrizA = Matrices.InicializarMatriz(filas, columasMatrizA);
        //    var matrizB = Matrices.InicializarMatriz(columasMatrizA, colimasMatrizB);
        //    var resultado = new double[filas, colimasMatrizB];

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    await Task.Run(() =>
        //    {
        //        Matrices.MultiplicarMatricesSecuencial(matrizA, matrizB, resultado);
        //    });
        //    stopwatch.Stop();
        //    var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Secuencial: {tiempoSecuencial} seg.");

        //    resultado = new double[filas, colimasMatrizB];
        //    stopwatch.Restart();
        //    await Task.Run(() =>
        //    {
        //        Matrices.MultiplicarMatricesParalelo(matrizA, matrizB, resultado);
        //    });
        //    stopwatch.Stop();
        //    var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Paralelo: {tiempoParalelo} seg.");

        //    EscribirComparacion(tiempoSecuencial, tiempoParalelo);

        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
        //    var directorioBaseSecuencial = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
        //    var directorioBaseParalelo = Path.Combine(directorioActual, @"Imagenes\resultado-paralelo");
        //    PrepararEjecucion(directorioBaseParalelo, directorioBaseSecuencial);

        //    Console.WriteLine("Inicio");

        //    var imagenes = ObtenerImagenes();

        //    //Secuencial
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    foreach (var imagen in imagenes)
        //    {
        //        await ProcesarImagen(directorioBaseSecuencial, imagen);
        //    }
        //    stopwatch.Stop();
        //    var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Secuencial: {tiempoSecuencial}");

        //    //Paralelo
        //    stopwatch.Restart();
        //    var tareasEnumerable = imagenes.Select(async imagen => await ProcesarImagen(directorioBaseParalelo, imagen));
        //    await Task.WhenAll(tareasEnumerable);
        //    stopwatch.Stop();
        //    var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
        //    Console.WriteLine($"Paralelo: {tiempoParalelo}");

        //    EscribirComparacion(tiempoSecuencial, tiempoParalelo);

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

        private async Task ProcesarImagen(string directorio, Imagen imagen)
        {
            var response = await _httpClient.GetAsync(imagen.Url);
            var content = await response.Content.ReadAsByteArrayAsync();

            Bitmap bitmap;
            using (var ms = new MemoryStream(content))
            {
                bitmap = new Bitmap(ms);
            }

            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var destino = Path.Combine(directorio, imagen.Nombre);
            bitmap.Save(destino);
        }

        private static List<Imagen> ObtenerImagenes()
        {
            var imagenes = new List<Imagen>();

            for (int i = 0; i < 3; i++)
            {
                imagenes.Add(new Imagen
                {
                    Nombre = $"Rick {i}.jpeg",
                    Url = "https://rickandmortyapi.com/api/character/avatar/1.jpeg"
                });

                imagenes.Add(new Imagen
                {
                    Nombre = $"Morty {i}.jpeg",
                    Url = "https://rickandmortyapi.com/api/character/avatar/2.jpeg"
                });

                imagenes.Add(new Imagen
                {
                    Nombre = $"Summer {i}.jpeg",
                    Url = "https://rickandmortyapi.com/api/character/avatar/3.jpeg"
                });

                imagenes.Add(new Imagen
                {
                    Nombre = $"Beth {i}.jpeg",
                    Url = "https://rickandmortyapi.com/api/character/avatar/4.jpeg"
                });

                imagenes.Add(new Imagen
                {
                    Nombre = $"Jerry {i}.jpeg",
                    Url = "https://rickandmortyapi.com/api/character/avatar/5.jpeg"
                });
            }

            return imagenes;
        }

        private void PrepararEjecucion(string destinoBaseParalelo, string destinoBaseSecuencial)
        {
            if (!Directory.Exists(destinoBaseParalelo))
            {
                Directory.CreateDirectory(destinoBaseParalelo);
            }

            if (!Directory.Exists(destinoBaseSecuencial))
            {
                Directory.CreateDirectory(destinoBaseSecuencial);
            }

            BorrarArchivos(destinoBaseSecuencial);
            BorrarArchivos(destinoBaseParalelo);
        }

        private void BorrarArchivos(string directorio)
        {
            var archivos = Directory.EnumerateFiles(directorio);
            foreach (var archivo in archivos)
            {
                File.Delete(archivo);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
