using Microsoft.AspNetCore.Http;
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
            var esperar = RandomGen.NextDouble() * 10 + 1;
            await Task.Delay(TimeSpan.FromSeconds((int)esperar));
            return $"Hola, {nombre}!";
        }

        [HttpGet("Despedida/{nombre}")]
        public async Task<ActionResult<string>> ObtenerDespedidaConDelay(string nombre)
        {
            var esperar = RandomGen.NextDouble() * 10 + 1;
            await Task.Delay(TimeSpan.FromSeconds((int)esperar));
            return $"Bye, {nombre}!";
        }
    }
}
