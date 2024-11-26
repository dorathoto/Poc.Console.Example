using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Poc.ConsoleExample
{
    internal class Program
    {
        internal static IConfiguration _iconfiguration;

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json",
               optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();


            //variables
            var MyID = Guid.NewGuid();
            var MyTennantId = _iconfiguration.GetValue<Guid>("TennantId");
            var urlHub = _iconfiguration.GetValue<string>("SignalR:UrlHub");


            #region SignalR
            var signalRConnection = new SignalRConnection(MyID, MyTennantId, urlHub, _iconfiguration);  // #progress
            await signalRConnection.Start();
            #endregion
            await Task.Delay(Timeout.Infinite);

        }


    }
}
