using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter car number code (e.g., AA, BB, CC): ");
        string code = Console.ReadLine();

        // Підключення до сервера
        TcpClient client = new TcpClient("127.0.0.1", 6463);
        NetworkStream stream = client.GetStream();

        // Відправка коду номера на сервер
        byte[] data = Encoding.UTF8.GetBytes(code);
        stream.Write(data, 0, data.Length);
        Console.WriteLine($"Sent code: {code}");

        // Отримання відповіді від сервера
        data = new byte[256];
        int bytesRead = stream.Read(data, 0, data.Length);
        string region = Encoding.UTF8.GetString(data, 0, bytesRead);

        Console.WriteLine($"The region is: {region}");

        // Закриття з'єднання
        client.Close();
    }
}
