using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
