using MemBotModels.Models;
using System;
using System.Text.RegularExpressions;

namespace MemBot.Parsers
{
    public static class CommandParser
    {
        public static bool TryParseServiceCommand(string command, out ServiceCommand serviceCommand, out string[] args)
        {
            serviceCommand = ServiceCommand.Unknown;
            args = null;

            Regex regex = new Regex(@"\/(?<command>\w+)(?<args>(.|\s)*)", RegexOptions.Singleline);
            var match = regex.Match(command);

            if (match.Success && Enum.TryParse(match.Groups["command"].Value, true, out serviceCommand))
            {
                if (!match.Groups["args"].Success && serviceCommand == ServiceCommand.Help)
                    return true;

                args = match.Groups["args"].Value.Split(' ', '\n', StringSplitOptions.RemoveEmptyEntries);
                return true;
            }

            return false;
        }

        // TODO: Cover this method with unit tests
        public static string[] ParseAudioCommandChain(string commandChain)
        {
            // TODO: Refactor this bullshit. It looks horible
            return commandChain.Replace("/", "").Replace(".", "").ToLower().Split(' ', '\n', '@');
        }
    }
}
