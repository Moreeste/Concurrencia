using Newtonsoft.Json;
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

        //Bloquear la interfaz de usuario
        //private void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    Thread.Sleep(5000);
        //}

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            loadingGif.Visible = true;

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var resultado = await Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    return 7;

                }).WithCancellation(_cancellationTokenSource.Token);

                Console.WriteLine(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }

            loadingGif.Visible = false;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    var tarea = EvaluaValor(txtInput.Text);

        //    Console.WriteLine("Inicio");
        //    Console.WriteLine($"Is Completed: {tarea.IsCompleted}");
        //    Console.WriteLine($"Is Canceled: {tarea.IsCanceled}");
        //    Console.WriteLine($"Is Faulted: {tarea.IsFaulted}");

        //    try
        //    {
        //        await tarea;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Excepción: {ex.Message}");
        //    }

        //    Console.WriteLine("Fin");
        //    Console.WriteLine("");

        //    loadingGif.Visible = false;
        //}

        public Task EvaluaValor(string valor)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (valor == "1")
            {
                tcs.SetResult(null);
            }
            else if (valor == "2")
            {
                tcs.SetCanceled();
            }
            else
            {
                tcs.SetException(new ApplicationException($"Valor inválido: {valor}"));
            }

            return tcs.Task;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    _cancellationTokenSource = new CancellationTokenSource();
        //    var token = _cancellationTokenSource.Token;
        //    var nombres = new string[] { "Esteban", "María", "Juan", "Ana", "Luis" };

        //    //var tareasHttp = nombres.Select(x => ObtenerSaludoConDelay(x, token));
        //    //var tarea = await Task.WhenAny(tareasHttp);
        //    //var contenido = await tarea;
        //    //Console.WriteLine(contenido.ToUpper());
        //    //_cancellationTokenSource?.Cancel();

        //    //var tareasHttp = nombres.Select(x =>
        //    //{
        //    //    Func<CancellationToken, Task<string>> funcion = (cancellationToken) => ObtenerSaludoConDelay(x, cancellationToken);
        //    //    return funcion;
        //    //});

        //    //var contenido = await EjecutarUno(tareasHttp);
        //    //Console.WriteLine(contenido.ToUpper());

        //    var contenido = await EjecutarUno(
        //        (ct) => ObtenerSaludoConDelay("Esteban", ct),
        //        (ct) => ObtenerDespedida("Esteban", ct));

        //    Console.WriteLine(contenido.ToUpper());

        //    loadingGif.Visible = false;
        //}

        private async Task<T> EjecutarUno<T>(IEnumerable<Func<CancellationToken, Task<T>>> funciones)
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            var tarea = await Task.WhenAny(tareas);
            cts.Cancel();
            return await tarea;
        }

        private async Task<T> EjecutarUno<T>(params Func<CancellationToken, Task<T>>[] funciones)
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            var tarea = await Task.WhenAny(tareas);
            cts.Cancel();
            return await tarea;
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    loadingGif.Visible = true;

        //    //await Reintentar(ProcesarSaludo);

        //    try
        //    {
        //        var contenido = await Reintentar(async () =>
        //        {
        //            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos2/Esteban"))
        //            {
        //                respuesta.EnsureSuccessStatusCode();
        //                return await respuesta.Content.ReadAsStringAsync();
        //            }
        //        });

        //        Console.WriteLine(contenido);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Excepción atrapada");
        //    }


        //    loadingGif.Visible = false;
        //}

        private async Task ProcesarSaludo()
        {
            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos2/Esteban"))
            {
                respuesta.EnsureSuccessStatusCode();
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
            }
        }

        private async Task Reintentar(Func<Task> f, int reintentos = 3, int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos; i++)
            {
                try
                {
                    await f();
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }
        }

        private async Task<T> Reintentar<T>(Func<Task<T>> f, int reintentos = 3, int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos - 1; i++)
            {
                try
                {
                    return await f();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }

            return await f();
        }

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    CheckForIllegalCrossThreadCalls = true;

        //    loadingGif.Visible = true;

        //    btnCancelar.Text = "Antes";
        //    await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(continueOnCapturedContext: false);
        //    btnCancelar.Text = "Después";

        //    loadingGif.Visible = false;
        //}

        //private async void btnIniciar_Click(object sender, EventArgs e)
        //{
        //    _cancellationTokenSource = new CancellationTokenSource();
        //    _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
        //    loadingGif.Visible = true;
        //    pgProcesamiento.Visible = true;
        //    var reportarProgreso = new Progress<int>(ReportarProgresoTarjetas);

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    try
        //    {
        //        var tarjetas = await ObtenerTarjetasDeCredito(20, _cancellationTokenSource.Token);
        //        await ProcesarTarjetas(tarjetas, reportarProgreso, _cancellationTokenSource.Token);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    catch (TaskCanceledException ex)
        //    {
        //        MessageBox.Show("Operación cancelada");
        //    }

        //    stopwatch.Stop();
        //    MessageBox.Show($"Operación finalizada en: {stopwatch.Elapsed.TotalSeconds} segundos");

        //    loadingGif.Visible = false;
        //    pgProcesamiento.Visible = false;
        //    pgProcesamiento.Value = 0;
        //}

        private void ReportarProgresoTarjetas(int porcentaje)
        {
            pgProcesamiento.Value = porcentaje;
        }

        private Task ProcesarTarjetasMock(List<string> tarjetas, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        private async Task ProcesarTarjetas(List<string> tarjetas, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            using var semaforo = new SemaphoreSlim(2);

            var tareas = new List<Task<HttpResponseMessage>>();

            var indice = 0;

            tareas = tarjetas.Select(async tarjeta =>
            {
                var json = JsonConvert.SerializeObject(tarjeta);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await semaforo.WaitAsync();
                try
                {
                    var tareaInterna = await _httpClient.PostAsync($"{_apiUrl}/api/Tarjetas", content, cancellationToken);

                    //if (progress != null)
                    //{
                    //    indice++;
                    //    var porcentaje = (double)indice / tarjetas.Count;
                    //    porcentaje = porcentaje * 100;
                    //    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                    //    progress.Report(porcentajeInt);
                    //}

                    return tareaInterna;
                }
                finally
                {
                    semaforo.Release();
                }
            }).ToList();

            var respuestasTareas = Task.WhenAll(tareas);

            if (progress != null)
            {
                while (await Task.WhenAny(respuestasTareas, Task.Delay(1000)) != respuestasTareas)
                {
                    var tareasCompletadas = tareas.Where(x => x.IsCompleted).Count();
                    var porcentaje = (double)tareasCompletadas / tarjetas.Count;
                    porcentaje = porcentaje * 100;
                    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                    progress.Report(porcentajeInt);
                }
            }

            var respuestas = await respuestasTareas;

            var tarjetasRechazadas = new List<string>();

            foreach (var respuesta in respuestas)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var respuestaTarjeta = JsonConvert.DeserializeObject<RespuestaTarjeta>(contenido);

                if (!respuestaTarjeta.Aprobada)
                {
                    tarjetasRechazadas.Add(respuestaTarjeta.Tarjeta);
                }
            }

            foreach (var tarjeta in tarjetasRechazadas)
            {
                Console.WriteLine(tarjeta);
            }
        }

        private Task<List<string>> ObtenerTarjetasDeCreditoMock(int cantidadDeTarjetas, CancellationToken cancellationToken = default)
        {
            var tarjetas = new List<string>();
            tarjetas.Add("0000000000000000");

            return Task.FromResult(tarjetas);
        }

        private Task ObtenerTareaConError()
        {
            return Task.FromException(new ApplicationException());
        }

        private Task ObtenerTareaCancelada()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return Task.FromCanceled(_cancellationTokenSource.Token);
        }

        private async Task<List<string>> ObtenerTarjetasDeCredito(int cantidadDeTarjetas, CancellationToken cancellationToken = default)
        {
            return await Task.Run(async () =>
            {
                var tarjetas = new List<string>();

                for (int i = 0; i < cantidadDeTarjetas; i++)
                {
                    //await Task.Delay(1000);
                    tarjetas.Add(i.ToString().PadLeft(16, '0'));

                    Console.WriteLine($"Han sido generadas {tarjetas.Count} tarjetas");

                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                }

                return tarjetas;
            });
        }

        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        private async Task<string> ObtenerSaludo(string nombre)
        {
            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos/{nombre}"))
            {
                respuesta.EnsureSuccessStatusCode();
                var saludo = await respuesta.Content.ReadAsStringAsync();
                return saludo;
            }
        }

        private async Task<string> ObtenerSaludoConDelay(string nombre, CancellationToken cancellationToken)
        {
            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos/Delay/{nombre}", cancellationToken))
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                return contenido;
            }
        }

        private async Task<string> ObtenerDespedida(string nombre, CancellationToken cancellationToken)
        {
            using (var respuesta = await _httpClient.GetAsync($"{_apiUrl}/api/Saludos/Despedida/{nombre}", cancellationToken))
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                return contenido;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
