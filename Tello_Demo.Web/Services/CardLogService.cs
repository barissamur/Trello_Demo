using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using Tello_Demo.Web.Enums;
using Tello_Demo.Web.Models;

namespace Tello_Demo.Web.Services;


public class CardLogService
{
    private readonly string _logFilePath;

    public CardLogService(string baseDirectory)
    {
        string logDirectory = Path.Combine(baseDirectory, "log");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        _logFilePath = Path.Combine(logDirectory, "CardOperationsLog.log");
    }


    public async Task LogCardEventAsync(string details)
    {
        CardLog logEntry = new()
        {
            EventTime = DateTime.Now,
            Details = details
        };

        string logJson = JsonConvert.SerializeObject(logEntry) + ","; 

        using (StreamWriter sw = new StreamWriter(_logFilePath, append: true, Encoding.UTF8))
        {
            await sw.WriteLineAsync(logJson);
        }
    }

}