using MemBot.Parsers;
using MemBotModels.Models;
using MemBotModels.ServicePrototypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace MemBotWorker
{
    public class MemBotClient
    {
        #region Fields
        private readonly TelegramBotClient _bot;

        // TODO: Maybe it is better to create ServiceFactory
        private readonly IMemService _memService;
        private Dictionary<MessageType, Func<Message, Task>> _messageHandlers;
        // TODO: Maybe it is better to extract command handlers to separate class
        private Dictionary<ServiceCommand, Func<Message, string[], Task>> _serviceCommandHandlers;
        #endregion

        #region MemBot initialization

        public MemBotClient(IMemService memService)
        {
            string token = Environment.GetEnvironmentVariable("MEM_BOT_TOKEN");
            _bot = new TelegramBotClient(token);
            _memService = memService;
            _bot.OnMessage += OnMessage;
            InitializeMessageHandlers();
            InitializeServiceCommandHandlers();
        }

        private void InitializeMessageHandlers()
        {
            _messageHandlers = new Dictionary<MessageType, Func<Message, Task>>
            {
                [MessageType.Text] = TextMessageAsyncHandler,
                [MessageType.Audio] = AudioMessageAsyncHandler
            };
        }

        private void InitializeServiceCommandHandlers()
        {
            _serviceCommandHandlers = new Dictionary<ServiceCommand, Func<Message, string[], Task>>
            {
                [ServiceCommand.Add] = AddAudioAsyncHandler,
                [ServiceCommand.Remove] = RemoveAudioAsyncHandler,
                [ServiceCommand.Help] = GetHelpAsyncHandler
            };
        }

        #endregion

        public void Start()
        {
            _bot.StartReceiving();
        }

        public void Stop()
        {
            _bot.StopReceiving();
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            await _messageHandlers[e.Message.Type](e.Message);
        }

        #region Message handlers

        public async Task TextMessageAsyncHandler(Message message)
        {
            if (await TryExecuteServiceCommandAsync(message, message.Text))
                return;

            try
            {
                foreach (var command in CommandParser.ParseAudioCommandChain(message.Text))
                {
                    // TODO: Check that command exists before trying to get it from the database.
                    Mem mem = _memService.Get(command);
                    if (mem == null) continue;

                    using var audioStream = new MemoryStream(mem.Content);
                    InputOnlineFile audioFile = new InputOnlineFile(audioStream);

                    await _bot.SendAudioAsync(
                            chatId: message.Chat.Id,
                            title: mem.FileName,
                            audio: audioFile);
                }
            }
            // TODO: Catch more specific exceptions. If it is neccessary create your own ones.
            // Do it for the rest of try-catch blocks.
            catch (Exception ex)
            {
                await SendTextAsync(message, ex.Message);
            }
        }

        public async Task AudioMessageAsyncHandler(Message message)
        {
            if (!await TryExecuteServiceCommandAsync(message, message.Caption))
                // TODO: Update this message
                await SendTextAsync(message, "Invalid command format. Please, " +
                    "enter file name separated by one white space after command.");
        }

        #endregion

        #region Service command handlers

        public async Task AddAudioAsyncHandler(Message message, string[] args)
        {
            try
            {
                string commandName = args[0];
                string fileName = $"{(args.Length > 1 ? args[1] : commandName)}.mp3";

                using MemoryStream audioStream = new MemoryStream();
                var file = await _bot.GetInfoAndDownloadFileAsync(message.Audio.FileId, audioStream);

                _memService.Add(new Mem
                {
                    Command = $"/{commandName}",
                    FileName = fileName,
                    Content = audioStream.ToArray()
                });

                await SendTextAsync(message, "Record was successfully added.");
            }
            catch (Exception ex)
            {
                await SendTextAsync(message, ex.Message);
            }
        }

        public async Task RemoveAudioAsyncHandler(Message message, string[] args)
        {
            try
            {
                string command = $"/{args[0]}";

                _memService.Delete(new Mem { Command = command });
                await SendTextAsync(message, $"The {command} was successfully removed.");
            }
            catch(Exception ex)
            {
                await SendTextAsync(message, ex.Message);
            }
        }

        // TODO: Add description of the Service Commands (such as add, remove, etc)
        // TODO: Update help with maned attributes. For example: '-b' is stands for begins with
        public async Task GetHelpAsyncHandler(Message message, string[] args)
        {
            try
            {
                string pattern = (args != null && args.Length != 0) ? args[0] : string.Empty;
                var commands = _memService.GetHelp(pattern);

                await _bot.SendTextMessageAsync(message.Chat.Id, string.Join('\n', commands));
            }
            catch (Exception ex)
            {
                await SendTextAsync(message, ex.Message);
            }
        }

        #endregion

        #region Auxiliary methods

        private async Task<bool> TryExecuteServiceCommandAsync(Message message, string rawCommand)
        {
            if (CommandParser.TryParseServiceCommand(rawCommand, out var serviceCommand, out var args))
            {
                await _serviceCommandHandlers[serviceCommand](message, args);
                return true;
            }

            return false;
        }

        private async Task SendTextAsync(Message message, string text)
        {
            await _bot.SendTextMessageAsync(message.Chat.Id, text);
        }

        #endregion
    }
}
