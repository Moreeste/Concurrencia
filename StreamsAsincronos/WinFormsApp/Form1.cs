using System.Runtime.CompilerServices;

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

            //_cancellationTokenSource = new CancellationTokenSource();

            //foreach (var nombre in GenerarNombres())
            //{
            //    Console.WriteLine(nombre);
            //}

            //try
            //{
            //    await foreach (var nombre in GenerarNombresAsync(_cancellationTokenSource.Token))
            //    {
            //        Console.WriteLine(nombre);
            //    }
            //}
            //catch (TaskCanceledException ex)
            //{
            //    Console.WriteLine("Operación Cancelada");
            //}
            //finally
            //{
            //    _cancellationTokenSource?.Dispose();
            //}

            var nombresEnumerable = GenerarNombresAsync();
            await ProcesarNombres(nombresEnumerable);

            Console.WriteLine("Fin");

            loadingGif.Visible = false;
        }

        private IEnumerable<string> GenerarNombres()
        {
            yield return "Esteban";
            yield return "Alejandra";
        }

        private async IAsyncEnumerable<string> GenerarNombresAsync([EnumeratorCancellation] CancellationToken token = default)
        {
            yield return "Esteban";
            await Task.Delay(2000, token);
            yield return "Alejandra";
            await Task.Delay(2000, token);
            yield return "Carol";
        }

        private async Task ProcesarNombres(IAsyncEnumerable<string> nombresEnumerable)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await foreach (var nombre in nombresEnumerable.WithCancellation(_cancellationTokenSource.Token))
                {
                    Console.WriteLine(nombre);
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Operación Cancelada");
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
