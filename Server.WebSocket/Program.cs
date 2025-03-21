using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

app.Map("/", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {

        //Aceitando a conexão
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        
        while (true)
        {
            //Sending message from Server
            var data = Encoding.ASCII.GetBytes($"Hello World -> {DateTime.Now}");

            await webSocket.SendAsync(
                data,
                System.Net.WebSockets.WebSocketMessageType.Text,
                true, CancellationToken.None);

            //Receving message from Client
            await webSocket.ReceiveAsync(data, CancellationToken.None);
            Console.WriteLine(Encoding.ASCII.GetString(data));

            await Task.Delay(1000);
        }
    }

});

await app.RunAsync();
