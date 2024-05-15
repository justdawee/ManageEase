namespace ManageEase.Inputs;

public record LoginUserInput
{
    public string Username { get; set; }
    public string Password { get; set; }
}