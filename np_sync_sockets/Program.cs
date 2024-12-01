using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

class Program
{
    private static Dictionary<string, string> carRegions;

    static void Main(string[] args)
    {
        // Ініціалізація словника для збереження кодів номерів та областей
        carRegions = new Dictionary<string, string>
        {
            { "AA", "Kyiv" },
            { "BB", "Odessa" },
            { "CC", "Lviv" },
            { "DD", "Kharkiv" }
        };

        TcpListener server = new TcpListener(IPAddress.Any, 6463); // Порт сервера
        server.Start();

        Console.WriteLine("Server is listening on port 6463...");

        while (true)
        {
            // Прийом клієнта
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Створення потоку для взаємодії з клієнтом
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];

            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                // Отримуємо код номеру від клієнта
                string code = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received code: {code}");

                // Визначення області по коду
                string region = carRegions.ContainsKey(code) ? carRegions[code] : "Unknown";

                // Відправка відповіді назад клієнту
                byte[] response = Encoding.UTF8.GetBytes(region);
                stream.Write(response, 0, response.Length);
                Console.WriteLine($"Sent region: {region}");
            }

            client.Close();
        }
    }
}
