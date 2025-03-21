using System.Net.WebSockets;
using System.Text;

using var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri("ws://localhost:5177"), CancellationToken.None);

var buffer = new byte[256];

while (ws.State == WebSocketState.Open)
{
    //Sending the message
    var message = Encoding.ASCII.GetBytes("Hello World, I'm using WebSockets");
    await ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);

    //Receiving the message
    var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

    //Verifing closing connection
    if (result.MessageType == WebSocketMessageType.Close)
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
    else
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
}