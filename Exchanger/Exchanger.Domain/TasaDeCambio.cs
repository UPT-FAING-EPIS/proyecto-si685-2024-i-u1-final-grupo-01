namespace Exchanger.Domain;
public class TasaDeCambio
{
    private static TasaDeCambio? instance = null;
    private Dictionary<string, double> tasasDeCambio = new Dictionary<string, double>();

    private TasaDeCambio()
    {
        // Obtenemos las monedas desde Moneda.cs y establecemos los valores de las tasas
        tasasDeCambio.Add(Moneda.USD.Nombre, 1.0); // Dólar estadounidense como base
        tasasDeCambio.Add(Moneda.EUR.Nombre, 0.85); // Euro
        tasasDeCambio.Add(Moneda.GBP.Nombre, 0.73); // Libra esterlina
        tasasDeCambio.Add(Moneda.PEN.Nombre, 3.65); // Nuevo Sol Peruano
    }

    public static TasaDeCambio GetInstance()
    {
        if (instance == null)
        {
            instance = new TasaDeCambio();
        }
        return instance!;
    }

    public Dictionary<string, double> ObtenerTasasDeCambio()
    {
        return tasasDeCambio;
    }
}