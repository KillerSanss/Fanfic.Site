namespace Application.Dto.EmailDto;

public class CreateEmailRequest
{
    public string SenderName { get; init; }
    public string SenderEmail { get; init; }
    public string ToEmail { get; init; }
    public string Subject { get; init; }
    public string Message { get; init; }
}