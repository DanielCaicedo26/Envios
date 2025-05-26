using Entity.Dtos.Email;
using Microsoft.AspNetCore.Mvc;
using Utilities.Mail;
namespace Web.Controllers.Implements
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(EmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        // Aquí puedes agregar métodos para manejar las solicitudes HTTP relacionadas con el envío de correos electrónicos



        // Hacemos el Metodo POST

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var result = await _emailService.SendEmailAsync(request.To, request.Subject, request.Body, request.IsHtml);
            if (!result)
                return BadRequest("No se pudo enviar el correo.");

            return Ok("Correo enviado exitosamente.");
        }

        // POST: api/email/send-many
        [HttpPost("send-many")]
        public async Task<IActionResult> SendEmailToMany([FromBody] EmailToManyRequest request)
        {
            var result = await _emailService.SendEmailToManyAsync(request.To, request.Subject, request.Body, request.IsHtml);
            if (!result)
                return BadRequest("No se pudo enviar el correo a múltiples destinatarios.");

            return Ok("Correo enviado a múltiples destinatarios.");
        }

        // POST: api/email/send-copy
        [HttpPost("send-copy")]
        public async Task<IActionResult> SendEmailWithCopy([FromBody] EmailCopyRequest request)
        {
            var result = await _emailService.SendEmailWithCopyAsync(
                request.To, request.Subject, request.Body, request.Cc, request.Bcc, request.IsHtml);

            if (!result)
                return BadRequest("No se pudo enviar el correo con copia.");

            return Ok("Correo enviado con copia.");
        }
    }
}