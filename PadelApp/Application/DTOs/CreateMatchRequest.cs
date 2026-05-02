namespace PadelApp.Application.DTOs;

public class CreateMatchRequest
{
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxPlayers { get; set; }
}