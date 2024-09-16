using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Servers
{
    private static Servers instance;
    private readonly List<string> _servers;
    private readonly object _lock = new object(); // Объект для синхронизации в многопоточном режиме

    private Servers()
    {
        _servers = new List<string>();
    }

    public static Servers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Servers();
            }
            return instance;
        }
    }

    public bool AddServer(string serverAddress)
    {
        lock (_lock) // Блокировка для многопоточного доступа
        {
            if (IsValidServerAddress(serverAddress) && !_servers.Contains(serverAddress))
            {
                _servers.Add(serverAddress);
                return true;
            }
            return false;
        }
    }

    public List<string> GetHttpServers()
    {
        lock (_lock) // Блокировка для многопоточного доступа
        {
            return _servers.Where(s => s.StartsWith("http")).ToList();
        }
    }

    public List<string> GetHttpsServers()
    {
        lock (_lock) // Блокировка для многопоточного доступа
        {
            return _servers.Where(s => s.StartsWith("https")).ToList();
        }
    }

    private bool IsValidServerAddress(string serverAddress)
    {
        return serverAddress.StartsWith("http") || serverAddress.StartsWith("https");
    }
}

// Пример использования:

public class Example
{
    public static void Main(string[] args)
    {
        // Добавление серверов
        Servers.Instance.AddServer("http://www.google.com");
        Servers.Instance.AddServer("https://www.microsoft.com");
        Servers.Instance.AddServer("http://www.example.com");

        // Получение списков серверов
        List<string> httpServers = Servers.Instance.GetHttpServers();
        List<string> httpsServers = Servers.Instance.GetHttpsServers();

        // Вывод результатов
        Console.WriteLine("HTTP серверы:");
        foreach (string server in httpServers)
        {
            Console.WriteLine(server);
        }

        Console.WriteLine("HTTPS серверы:");
        foreach (string server in httpsServers)
        {
            Console.WriteLine(server);
        }

        Console.ReadKey();
    }
}