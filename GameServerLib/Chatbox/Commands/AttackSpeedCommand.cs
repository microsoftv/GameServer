using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class AttackSpeedCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;
        private StatsModifier _statMod;

        public override string Command => "as";
        public override string Syntax => $"{Command} AttackSpeedBonus or AttackSpeedFlat #";

        public AttackSpeedCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var split = arguments.ToLower().Split(' ');
            if (split.Length < 3)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else if (split[1].Equals("attackspeedbonus"))
            {
                if (float.TryParse(split[2], out var attackspeed))
                {
                    _statMod = new StatsModifier();
                    _statMod.AttackSpeed.FlatBonus += attackspeed;
                    _playerManager.GetPeerInfo((ulong)userId).Champion.AddStatModifier(_statMod);
                }
            }
            else if (split[1].Equals("attackspeedflat"))
            {
                if (float.TryParse(split[2], out var attackspeed))
                {
                    _playerManager.GetPeerInfo((ulong)userId).Champion.Stats.AttackSpeedFlat = attackspeed;
                }
            }
            else
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
        }
    }
}
