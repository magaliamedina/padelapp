namespace PadelApp.Application.DTOs;

public class MatchResponse
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxPlayers { get; set; }
    public string MatchType { get; set; } = string.Empty;
    public List<PlayerDto> ActivePlayers { get; set; } = new();
    public List<PlayerDto> WaitingReplacement { get; set; } = new();
    public List<PlayerDto> CancelledPlayers { get; set; } = new();
}