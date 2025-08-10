using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly EmailService _emailService;

    public ContactController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ContactFormModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var fullName = $"{model.FirstName} {model.LastName}";
        var subject = $"New Contact Message from {fullName}";

        var body = $@"
            <p><strong>Name:</strong> {fullName}</p>
            <p><strong>Email:</strong> {model.Email}</p>
            <p><strong>Phone:</strong> {model.Phone}</p>
            <p><strong>Message:</strong></p>
            <p>{model.Message}</p>
        ";

        await _emailService.SendEmailAsync("ridesurecontact@gmail.com", subject, body);

        return Ok(new { message = "Message sent successfully" });
    }
}


public class ContactFormModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Message { get; set; }
}

