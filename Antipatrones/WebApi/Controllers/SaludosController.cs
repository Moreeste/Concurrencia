using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaludosController : ControllerBase
    {
        [HttpGet("{nombre}")]
        public async Task<ActionResult<string>> ObtenerSaludo(string nombre)
        {
            Console.WriteLine($"Hilo antes del await: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine($"Hilo después del await: {Thread.CurrentThread.ManagedThreadId}");

            return $"Hola, {nombre}!";
        }

        [HttpGet("Delay/{nombre}")]
        public async Task<ActionResult<string>> ObtenerSaludoConDelay(string nombre)
        {
            //try
            //{
            //    OperacionVoidAsync();
            //}
            //catch (Exception ex)
            //{

            //}

            //OperacionTaskAsync();

            OperacionVoidSync();

            return $"Hola, {nombre}!";
        }

        //Antipatron: async void
        private async void OperacionVoidAsync()
        {
            await Task.Delay(1);
            throw new ApplicationException();
        }

        private void OperacionVoidSync()
        {
            throw new ApplicationException();
        }

        private async Task OperacionTaskAsync()
        {
            await Task.Delay(1);
            throw new ApplicationException();
        }
    }
}
