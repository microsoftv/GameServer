using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class ManaRegenCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "manaregen";
        public override string Syntax => $"{Command} regenAmount";

        public ManaRegenCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            if (split.Length < 2)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else if (float.TryParse(split[1], out var manaregen))
            {
                _playerManager.GetPeerInfo((ulong)userId).Champion.Stats.ManaRegeneration.FlatBonus += manaregen;
            }
        }
    }
}
