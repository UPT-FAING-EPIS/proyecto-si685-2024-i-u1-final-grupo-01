using System;
using System.Collections.Generic;
//Main
namespace Exchanger.Domain;
class Program
{
    static void Main(string[] args)
    {
        // Lista de clientes registrados
        List<Cliente> clientes = new List<Cliente>();

        Transaccion transaccion = Transaccion.Factory.CrearTransaccion(TasaDeCambio.GetInstance().ObtenerTasasDeCambio());

        bool loggedIn = false;
        Cliente? clienteActual = null;

        while (!loggedIn)
        {
            Console.WriteLine("*************************");
            Console.WriteLine("          Menu:          ");
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Registrarse");
            Console.WriteLine("3. Salir");
            Console.WriteLine("*************************");
            Console.Write("Escoge una opción: ");
            string? eleccion = Console.ReadLine();

            switch (eleccion)
            {
                case "1":
                    Console.Write(" Nombre de usuario: ");
                    string? nombreUsuario = Console.ReadLine();
                    Console.Write(" Contraseña: ");
                    string? contraseña = Console.ReadLine();
                    Console.Write(" Token: ");
                    string? token = Console.ReadLine();

                    // Verificamos credenciales
                    if (nombreUsuario != null && contraseña != null)
                    {
                        foreach (var cliente in clientes)
                        {
                            if (cliente.VerificarCredenciales(nombreUsuario, contraseña, token))
                            {
                                loggedIn = true;
                                clienteActual = cliente;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Manejo de la situación en la que nombreUsuario es nulo.
                    }

                    if (!loggedIn)
                    {
                        Console.WriteLine("Usuario, contraseña o token inválido. Inténtalo de nuevo");
                    }
                    break;
                case "2":
                    Console.Write(" Nuevo nombre de usuario: ");
                    string? nuevoNombreUsuario = Console.ReadLine();
                    Console.Write(" Nueva contraseña: ");
                    string? nuevaContraseña = Console.ReadLine();

                    // Generamos un token único
                    Token.TokenBuilder constructorToken = new Token.TokenBuilder();
                    Token nuevoToken = constructorToken.AddRandomChars(5).Build();
                    
                    if (nuevoNombreUsuario != null && nuevaContraseña != null)
                    {
                        clientes.Add(new Cliente(nuevoNombreUsuario, nuevaContraseña, nuevoToken.Value));
                        Console.WriteLine($"Usuario creado exitosamente. Tu token único es: {nuevoToken.Value}");
                    }
                    else
                    {
                        Console.WriteLine($"No se aceptan valores nulos");
                    }
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }
        }

        // Si llega aquí, significa que el cliente ha iniciado sesión correctamente
        Console.WriteLine($"¡Bienvenido, {clienteActual!.NombreUsuario}!");

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("*************************");
            Console.WriteLine("        Opciones        ");
            Console.WriteLine("1. Convertir moneda");
            Console.WriteLine("2. Ver historial");
            Console.WriteLine("3. Depositar");
            Console.WriteLine("4. Retirar");
            Console.WriteLine("5. Ver saldo actual");
            Console.WriteLine("6. Salir");
            Console.WriteLine("*************************");
            Console.Write("Escoge una opción: ");
            string? eleccion = Console.ReadLine();

            switch (eleccion)
            {
                case "1":
                    Console.Write(" Cantidad a convertir: ");
                    string? inputCantidadAConvertir = Console.ReadLine();
                    if (inputCantidadAConvertir != null)
                    {
                        if (double.TryParse(inputCantidadAConvertir, out double cantidadAConvertir))
                        {
                            Console.Write(" Moneda de origen (USD, EUR, GBP, PEN): ");
                            string? monedaOrigen = Console.ReadLine()?.ToUpper();
                            Console.Write(" Moneda de destino (USD, EUR, GBP, PEN): ");
                            string? monedaDestino = Console.ReadLine()?.ToUpper();
                            if(monedaOrigen != null && monedaDestino != null){
                                double cantidadConvertida = transaccion.RealizarConversion(cantidadAConvertir, monedaOrigen, monedaDestino);
                                clienteActual!.RealizarTransaccion(cantidadAConvertir, monedaOrigen, monedaDestino, cantidadConvertida);
                                Console.WriteLine($"Cantidad convertida: {cantidadConvertida} {monedaDestino}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("La cantidad ingresada no es válida.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se ingresó ninguna cantidad.");
                    }
                    break;
                case "2":
                    clienteActual!.VerHistorial();
                    break;
                case "3":
                    Console.Write(" Monto a depositar: ");
                    string? inputMontoDeposito = Console.ReadLine();
                    if (inputMontoDeposito != null)
                    {
                        if (double.TryParse(inputMontoDeposito, out double montoDeposito))
                        {
                            Console.Write(" Tipo de moneda (USD, EUR, GBP, PEN): ");
                            string? tipoMonedaDeposito = Console.ReadLine()?.ToUpper();
                            if(tipoMonedaDeposito != null){
                                clienteActual!.Depositar(montoDeposito, tipoMonedaDeposito);
                            }
                        }
                        else
                        {
                            Console.WriteLine("El monto ingresado no es válido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se ingresó ningún monto.");
                    }
                    break;
                case "4":
                    Console.Write(" Monto a retirar: ");
                    string? inputMontoRetiro = Console.ReadLine();
                    if (inputMontoRetiro != null)
                    {
                        if (double.TryParse(inputMontoRetiro, out double montoRetiro))
                        {
                            Console.Write(" Tipo de moneda (USD, EUR, GBP, PEN): ");
                            string? tipoMonedaRetiro = Console.ReadLine()?.ToUpper();
                            if(tipoMonedaRetiro != null){
                                clienteActual!.Retirar(montoRetiro, tipoMonedaRetiro);
                            }
                        }
                        else
                        {
                            Console.WriteLine("El monto ingresado no es válido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se ingresó ningún monto.");
                    }
                    break;
                case "5":
                    clienteActual!.VerSaldoActual();
                    break;
                case "6":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }
        }
    }
}
