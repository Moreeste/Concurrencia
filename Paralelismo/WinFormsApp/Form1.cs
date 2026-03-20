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

            var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
            var directorioBaseSecuencial = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
            var directorioBaseParalelo = Path.Combine(directorioActual, @"Imagenes\resultado-paralelo");
            PrepararEjecucion(directorioBaseParalelo, directorioBaseSecuencial);

            Console.WriteLine("Inicio");

            var imagenes = ObtenerImagenes();

            //Secuencial
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var imagen in imagenes)
            {
                await ProcesarImagen(directorioBaseSecuencial, imagen);
            }
            stopwatch.Stop();
            var tiempoSecuencial = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"Secuencial: {tiempoSecuencial}");

            //Paralelo
            stopwatch.Restart();
            var tareasEnumerable = imagenes.Select(async imagen => await ProcesarImagen(directorioBaseParalelo, imagen));
            await Task.WhenAll(tareasEnumerable);
            stopwatch.Stop();
            var tiempoParalelo = stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"Paralelo: {tiempoParalelo}");

            EscribirComparacion(tiempoSecuencial, tiempoParalelo);

            Console.WriteLine("Fin");

            loadingGif.Visible = false;
        }

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
