namespace PadelApp.Application.DTOs;

public class PlayerDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }

    public string Gender { get; set; } = string.Empty;
}