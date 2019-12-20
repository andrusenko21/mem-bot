using MemBotModels.Models;
using MemBotModels.ServicePrototypes;
using System.IO;
using System.Text.RegularExpressions;

namespace MemBot
{
    // TODO: Remove this class
    public class LocalStorageToDatabaseMigrator
    {
        private readonly IMemService _memService;

        public LocalStorageToDatabaseMigrator(IMemService memService)
        {
            _memService = memService;
        }

        public void Migrate()
        {
            const string filesLocation = @"E:\audio\";
            DirectoryInfo directoryInfo = new DirectoryInfo(filesLocation);
            var fileInfos = directoryInfo.GetFiles();

            foreach (var fileInfo in fileInfos)
            {
                using var reader = fileInfo.OpenRead();
                byte[] audioBytes = new byte[fileInfo.Length];
                reader.Read(audioBytes, 0, (int)fileInfo.Length);

                _memService.Add(new Mem
                {
                    Command = $"/{ParseCommand(fileInfo.Name)}",
                    Content = audioBytes,
                    FileName = fileInfo.Name
                });
            }
        }

        private string ParseCommand(string fileName)
        {
            return new Regex("(?<name>.+).mp3").Match(fileName).Groups["name"].Value.Replace(".", "");
        }
    }
}
