namespace ManageEase.Inputs;

public record RegisterUserInput
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}