using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Starting client...");

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5271/api/v1/quick-auth-hub")
    .Build();

connection.On<object>("OnStatusChanged", (data) =>
{
    Console.WriteLine("Received update:");
    Console.WriteLine(data);
});

await connection.StartAsync();
Console.WriteLine("Connected to SignalR hub.");

// Подпишемся на группу (sessionId)
var sessionId = "1c9a9e44-8305-4fe7-8b48-15dd7ae7d424"; // ← сюда свой актуальный sessionId из CreateSession
await connection.InvokeAsync("SubscribeToSession", sessionId);
Console.WriteLine("Subscribed to session group. Waiting for updates...");

Console.ReadLine();