using System;
using System.Collections.Generic;
namespace Exchanger.Domain;
public class Cliente
{
    public string? NombreUsuario { get; }
    private string? Contraseña { get; }
    private string? Token { get; }
    public List<HistorialTransaccion> historialTransacciones; // Nueva lista para almacenar el historial
    public Dictionary<string, double> saldoPorMoneda; // Nuevo diccionario para mantener el saldo por tipo de moneda
    public MedioDePago? MedioDePago { get; set; }

    public Cliente(string nombreUsuario, string contraseña, string? token = null)
    {
        NombreUsuario = nombreUsuario;
        Contraseña = contraseña;
        Token = token;
        historialTransacciones = new List<HistorialTransaccion>(); // Inicializamos la lista
        saldoPorMoneda = new Dictionary<string, double>(); // Inicializamos el diccionario
        // Inicializamos el saldo para cada tipo de moneda
        saldoPorMoneda.Add(Moneda.USD.Nombre, 500.0);
        saldoPorMoneda.Add(Moneda.EUR.Nombre, 500.0);
        saldoPorMoneda.Add(Moneda.GBP.Nombre, 500.0);
        saldoPorMoneda.Add(Moneda.PEN.Nombre, 500.0);
    }

    // Método para realizar una transacción y agregarla al historial
    public bool RealizarTransaccion(double cantidad, string monedaOrigen, string monedaDestino, double cantidadConvertida)
    {
        // Verificar si el cliente tiene suficiente saldo en la moneda de origen
        if (saldoPorMoneda.ContainsKey(monedaOrigen) && saldoPorMoneda[monedaOrigen] >= cantidad && saldoPorMoneda.ContainsKey(monedaDestino))
        {
            // Realizar la transacción
            saldoPorMoneda[monedaOrigen] -= cantidad; // Restar el saldo de la moneda de origen
            saldoPorMoneda[monedaDestino] += cantidadConvertida; // Añadir el saldo convertido a la moneda de destino

            // Crear el registro de la transacción en el historial
            HistorialTransaccion transaccion = new HistorialTransaccion(DateTime.Now, cantidad, monedaOrigen, monedaDestino, cantidadConvertida);
            historialTransacciones.Add(transaccion);
            Console.WriteLine("Transacción realizada con éxito.");
            return true; 
        }
        else
        {
            Console.WriteLine("Saldo insuficiente en la moneda de origen.");
            return false;
        }
    }

    public void VerSaldoActual()
    {
        Console.WriteLine("Saldo actual por tipo de moneda:");
        foreach (var kvp in saldoPorMoneda)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    // Método para ver el historial de transacciones
    public string VerHistorial()
    {
        string historialString = "Historial de Transacciones:\n";
        foreach (var transaccion in historialTransacciones)
        {
            historialString += $"Fecha: {transaccion.Fecha}, Cantidad: {transaccion.Cantidad} {transaccion.MonedaOrigen} a {transaccion.CantidadConvertida} {transaccion.MonedaDestino}\n";
        }
        Console.WriteLine(historialString); // Imprimir el historial en la consola
        return historialString; // Devolver el historial como una cadena de texto
    }

    public void LimpiarHistorialTransacciones()
    {
        historialTransacciones.Clear();
    }

    // Método para depositar dinero en una moneda específica
    public bool Depositar(double cantidad, string tipoMoneda)
    {
        if (saldoPorMoneda.ContainsKey(tipoMoneda))
        {
            saldoPorMoneda[tipoMoneda] += cantidad;
            Console.WriteLine($"Depósito exitoso de {cantidad} {tipoMoneda}. Nuevo saldo: {saldoPorMoneda[tipoMoneda]} {tipoMoneda}");
            return true;
        }
        else
        {
            Console.WriteLine("Tipo de moneda inválido.");
            return false;
        }
    }

    // Método para retirar dinero de una moneda específica
    public bool Retirar(double cantidad, string tipoMoneda)
    {
        if (MedioDePago == null)
        {
            Console.WriteLine("No hay una cuenta CCI asociada. Ingrese una nueva:");
            string? nuevaCCI = Console.ReadLine();
            if (nuevaCCI != null)
            {
                MedioDePago = new MedioDePago(nuevaCCI);
            }
            else
            {
                // Manejo de la situación en la que la entrada es nula.
            }
        }

        if (saldoPorMoneda.ContainsKey(tipoMoneda))
        {
            if (cantidad <= saldoPorMoneda[tipoMoneda])
            {
                saldoPorMoneda[tipoMoneda] -= cantidad;
                Console.WriteLine($"Retiro exitoso de {cantidad} {tipoMoneda}. Nuevo saldo: {saldoPorMoneda[tipoMoneda]} {tipoMoneda}");
                return true;
            }
            else
            {
                Console.WriteLine("Saldo insuficiente para realizar el retiro.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Tipo de moneda inválido.");
            return false;
        }
    }

    public bool VerificarCredenciales(string nombreUsuario, string contraseña, string? token = null)
    {
        return NombreUsuario == nombreUsuario && Contraseña == contraseña && (token == null || Token == token);
    }
}

// Clase para almacenar detalles de la transacción
public class HistorialTransaccion
{
    public DateTime Fecha { get; }
    public double Cantidad { get; }
    public string MonedaOrigen { get; }
    public string MonedaDestino { get; }
    public double CantidadConvertida { get; }

    public HistorialTransaccion(DateTime fecha, double cantidad, string monedaOrigen, string monedaDestino, double cantidadConvertida)
    {
        Fecha = fecha;
        Cantidad = cantidad;
        MonedaOrigen = monedaOrigen;
        MonedaDestino = monedaDestino;
        CantidadConvertida = cantidadConvertida;
    }
}
