using System;
using System.Collections.Generic;
using System.Text;


// Patrón creacional: Singleton
public class CurrencyExchangeService
{
    private static CurrencyExchangeService? instance = null;
    private Dictionary<string, double> exchangeRates = new Dictionary<string, double>();

    private CurrencyExchangeService()
    {
        // Inicialización de tasas de cambio
        exchangeRates.Add("USD", 1.0); // Dólar estadounidense como base
        exchangeRates.Add("EUR", 0.85); // Euro
        exchangeRates.Add("GBP", 0.73); // Libra esterlina
        // Agregar más tasas de cambio según tus necesidades
    }

    public static CurrencyExchangeService GetInstance()
    {
        if (instance == null)
        {
            instance = new CurrencyExchangeService();
        }
        return instance!;
    }

    public double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
    {
        if (!exchangeRates.ContainsKey(fromCurrency) || !exchangeRates.ContainsKey(toCurrency))
        {
            throw new ArgumentException("Moneda no compatible.");
        }

        double fromRate = exchangeRates[fromCurrency];
        double toRate = exchangeRates[toCurrency];

        return amount * (toRate / fromRate);
    }
}

// Patrón creacional: Factory Method
public abstract class UserFactory
{
    public abstract User CreateUser(string username, string password, string? token = null);
}

public class OnlineCurrencyUserFactory : UserFactory
{
    public override User CreateUser(string username, string password, string? token = null)
    {
        return new User(username, password, token);
    }
}

public class TokenUserFactory : UserFactory
{
    public override User CreateUser(string username, string password, string? token = null)
    {
        return new TokenUser(username, token ?? "default-token");
    }
}

// Clases y definiciones de usuarios permanecen igual

public class User
{
    public string Username { get; }
    private string Password { get; }
    private string? Token { get; }

    public User(string username, string password, string? token = null)
    {
        Username = username;
        Password = password;
        Token = token;
    }

    public bool CheckCredentials(string username, string password, string? token = null)
    {
        return Username == username && Password == password && (token == null || Token == token);
    }
}

public class TokenUser : User
{
    public TokenUser(string username, string token) : base(username, "", token) { }
}

// Patrón creacional: Builder
public class Token
{
    public string Value { get; }

    private Token(string value)
    {
        Value = value;
    }

    public class TokenBuilder
    {
        private StringBuilder tokenBuilder;

        public TokenBuilder()
        {
            tokenBuilder = new StringBuilder();
        }

        public TokenBuilder AddRandomChars(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                tokenBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return this;
        }

        public Token Build()
        {
            return new Token(tokenBuilder.ToString());
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Lista de usuarios registrados
        List<User> users = new List<User>();

        CurrencyExchangeService exchangeService = CurrencyExchangeService.GetInstance();
        UserFactory userFactory = new OnlineCurrencyUserFactory(); // Cambiar a TokenUserFactory si se desea usar token

        bool loggedIn = false;
        User? currentUser = null;

        while (!loggedIn)
        {
            Console.WriteLine("=========================");
            Console.WriteLine("          Menu:          ");
            Console.WriteLine("1. Log in");
            Console.WriteLine("2. Sign up");
            Console.WriteLine("3. Salir");
            Console.WriteLine("=========================");
            Console.Write("Escoge una opción: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write(">>> Username: ");
                    string username = Console.ReadLine();
                    Console.Write(">>> Password: ");
                    string password = Console.ReadLine();
                    Console.Write(">>> Token: ");
                    string token = Console.ReadLine();

                    // Verificar credenciales
                    foreach (var user in users)
                    {
                        if (user.CheckCredentials(username, password, token))
                        {
                            loggedIn = true;
                            currentUser = user;
                            break;
                        }
                    }

                    if (!loggedIn)
                    {
                        Console.WriteLine("Usuario, Contraseña o Token inválido. Inténtalo de nuevo");
                    }
                    break;
                case "2":
                    Console.Write(">>> Nuevo username: ");
                    string newUsername = Console.ReadLine();
                    Console.Write(">>> Nuevo password: ");
                    string newPassword = Console.ReadLine();
                    
                    // Generar un token único
                    Token.TokenBuilder tokenBuilder = new Token.TokenBuilder();
                    Token newToken = tokenBuilder.AddRandomChars(5).Build();
                    
                    users.Add(userFactory.CreateUser(newUsername, newPassword, newToken.Value));
                    Console.WriteLine($"Usuario creado exitosamente. Tu token único es: {newToken.Value}");
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }
        }

        // Si llega aquí, significa que el usuario ha iniciado sesión correctamente
        Console.WriteLine($"¡Bienvenido, {currentUser!.Username}!");

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("=========================");
            Console.WriteLine("        Opciones        ");
            Console.WriteLine("1. Convertir moneda");
            Console.WriteLine("2. Salir");
            Console.WriteLine("=========================");
            Console.Write("Escoge una opción: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write(">>> Cantidad a convertir: ");
                    double amountToConvert = double.Parse(Console.ReadLine());
                    Console.Write(">>> Moneda de origen (ej. USD): ");
                    string fromCurrency = Console.ReadLine().ToUpper();
                    Console.Write(">>> Moneda de destino (ej. EUR): ");
                    string toCurrency = Console.ReadLine().ToUpper();
                    double convertedAmount = exchangeService.ConvertCurrency(amountToConvert, fromCurrency, toCurrency);
                    Console.WriteLine($"Cantidad convertida: {convertedAmount} {toCurrency}");
                    break;
                case "2":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }
        }
    }
}
