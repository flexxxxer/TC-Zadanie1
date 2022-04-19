using System.Net;
using System.Net.Sockets;
using System.Globalization;

var port = args.Length switch
{
    not 1 => 4000,
    1 => int.TryParse(args[0], out var parsedPort) ? parsedPort : 4000,
};

var tcpListener = new TcpListener(IPAddress.Any, port);
tcpListener.Start();

Console.WriteLine($"Server uruchomił się: {DateTime.Now}");
Console.WriteLine($"Autor serveru: Aleksandr Kovalyov");
Console.WriteLine($"Port TCP serveru: {port}");

while (true)
{
    var client = await tcpListener.AcceptTcpClientAsync();

    await using var stream = client.GetStream();
    await using var sw = new StreamWriter(stream);

    var ipAdress = client.Client.RemoteEndPoint switch
    {
        IPEndPoint ip when IPAddress.IsLoopback(ip.Address) => null,
        IPEndPoint ip => ip.Address,
        _ => null
    };

    var ipDateTime = await GetDateTimeFromIpAdress(ipAdress ?? IPAddress.Any);

    var contentToSend = $@"
Hello world!
Your ip: {ipAdress?.ToString() ?? "Undefined"}
Your date time (using ip): {ipDateTime?.ToString() ?? "Undefined"}";

    await sw.WriteLineAsync($@"
HTTP/1.1 200 OK
Content-Type: text/plain
Content-Length: {contentToSend.Length}
{contentToSend}");

    await sw.FlushAsync();

    client.Client.Disconnect(true);
}

static async Task<DateTime?> GetDateTimeFromIpAdress(IPAddress ipAdress)
{
    using var client = new HttpClient();

    var respone = await client.GetAsync($"https://ipapi.co/{ipAdress}/utc_offset");
    var content = await respone.Content.ReadAsStringAsync();

    return content switch
    {
        "Undefined" => null,
        var other => DateTime.UtcNow.Add(
            DateTimeOffset.ParseExact(other.Insert(3, ":"), "zzz", CultureInfo.CurrentCulture).Offset
        ),
    };
}