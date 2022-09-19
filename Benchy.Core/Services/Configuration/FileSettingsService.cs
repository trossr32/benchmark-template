using Benchy.Core.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Benchy.Core.Services.Configuration
{
    public interface IFileSettingsService
    {
        FileSettings GetSettings();
    }

    public class FileSettingsService : IFileSettingsService
    {
        private readonly ILogger<FileSettingsService> _logger;
        private readonly FileSettings _fileSettings;

        public FileSettingsService(ILogger<FileSettingsService> logger, IOptions<FileSettings> fileSettings)
        {
            _logger = logger;
            _fileSettings = fileSettings.Value;
        }

        public FileSettings GetSettings()
        {
            return _fileSettings;
        }
    }
}
