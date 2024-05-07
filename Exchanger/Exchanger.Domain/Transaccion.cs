namespace Exchanger.Domain;
// Patron Creacional: Factory Method
public abstract class Transaccion
{
    public abstract double RealizarConversion(double cantidad, string monedaOrigen, string monedaDestino);

    public static TransaccionFactory Factory { get; } = new TransaccionFactory();
}

public class TransaccionMoneda : Transaccion
{
    private readonly Dictionary<string, double> tasasDeCambio;

    public TransaccionMoneda(Dictionary<string, double> tasasDeCambio)
    {
        this.tasasDeCambio = tasasDeCambio;
    }

    public override double RealizarConversion(double cantidad, string monedaOrigen, string monedaDestino)
    {
        if (!tasasDeCambio.ContainsKey(monedaOrigen) || !tasasDeCambio.ContainsKey(monedaDestino))
        {
            throw new ArgumentException("Moneda no compatible.");
        }

        double tasaOrigen = tasasDeCambio[monedaOrigen];
        double tasaDestino = tasasDeCambio[monedaDestino];

        return cantidad * (tasaDestino / tasaOrigen);
    }
}

public class TransaccionFactory
{
    public Transaccion CrearTransaccion(Dictionary<string, double> tasasDeCambio)
    {
        return new TransaccionMoneda(tasasDeCambio);
    }
}