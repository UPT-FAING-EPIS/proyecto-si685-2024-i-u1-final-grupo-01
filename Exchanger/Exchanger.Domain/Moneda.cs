namespace Exchanger.Domain;
public class Moneda
{
    public string Nombre { get; }

    public Moneda(string nombre)
    {
        Nombre = nombre;
    }

    // Definimos las monedas aquí
    public static Moneda USD = new Moneda("USD"); // Dólar estadounidense como base
    public static Moneda EUR = new Moneda("EUR"); // Euro
    public static Moneda GBP = new Moneda("GBP"); // Libra esterlina
    public static Moneda PEN = new Moneda("PEN"); // Nuevo Sol Peruano
}