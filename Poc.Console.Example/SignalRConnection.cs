using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace Poc.ConsoleExample;

public class SignalRConnection
{
    private Guid _myID;
    private Guid _myTennantId;
    private string _urlHub;
    private IConfiguration _iconfiguration;

    public SignalRConnection(Guid myID, Guid tennantId, string urlHub, IConfiguration iconfiguration)
    {
        this._myTennantId = tennantId;
        this._myID = myID;
        _urlHub = urlHub;
        _iconfiguration = iconfiguration;

    }

    public async Task Start()
    {
        try
        {
            var connection = new HubConnectionBuilder()
                 .WithUrl(_urlHub, options =>
                 {
                     options.Headers["ClientID"] = _myID.ToString();
                 })
                 .WithAutomaticReconnect()
                 .Build();

            // receive a message from the hub
            connection.On<string, string, string>("callMessage", OnReceiveMessage);

            Console.WriteLine($"Connecting to the hub - {connection.ConnectionId}");

            await connection.StartAsync();

            // send a message to the hub
            await connection.InvokeAsync("callMessage", _myTennantId, _myID.ToString(), connection.ConnectionId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void OnReceiveMessage(string tennantId, string user, string message)
    {
        try
        {
            if (user.Equals(_myID.ToString(), StringComparison.CurrentCultureIgnoreCase) || (tennantId.Equals(_myTennantId.ToString(), StringComparison.CurrentCultureIgnoreCase) && user.ToLower() == ""))
            {
                Console.WriteLine($"Message to me or Everyone from my tenant  {message}, totemId: {user}, tennantID: {tennantId}");
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

}