using Game.Application.Commands.Response;
using Shared.CQRS;

namespace Game.Application.Commands;

public class MatchmakeCommand : ICommand<MatchmakeResponse>
{
    public string PlayerId { get; set; }
    public int Elo { get; set; }
}