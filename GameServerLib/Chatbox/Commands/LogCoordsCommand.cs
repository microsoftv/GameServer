using GameServerCore;
using LeagueSandbox.GameServer.Logging;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using log4net;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class LogCoordsCommand : ChatCommandBase
    {
        private readonly ILog _logger;
        private readonly IPlayerManager _playerManager;

        public override string Command => "logcoords";
        public override string Syntax => $"{Command} 0 (disable) / 1 (enable)";

        public bool logging = false;

        public LogCoordsCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _logger = LoggerProvider.GetLogger();
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var champion = _playerManager.GetPeerInfo((ulong)userId).Champion;

            var split = arguments.ToLower().Split(' ');

            if (split.Length < 2 || !byte.TryParse(split[1], out var input) || input > 1)
            {
                ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                ShowSyntax();
            }
            else
            {
                logging = input != 0;
                LogInfo("Is Logging?: " + logging);
            }
            if (logging == true)
            {
                LogInfo("Logging current Position to chat!");
                for (var i = 0.0f; i < 86400.00f && logging == true; i += 0.10f) //logs for 24 hours
                {
                    CreateTimer(i, () => {
                        if (logging == false)
                        {
                            return;
                        }
                        else
                        {
                            _logger.Debug($"At {champion.X}; {champion.Y}");
                            var msg = $"At Coords - X: {champion.X} Y: {champion.Y} Z: {champion.GetZ()}";
                            ChatCommandManager.SendDebugMsgFormatted(DebugMsgType.NORMAL, msg);
                        }
                    });
                }
            }
        }
    }
}