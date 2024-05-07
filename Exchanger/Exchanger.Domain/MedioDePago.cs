namespace Exchanger.Domain;
public class MedioDePago
{
    public string CCI { get; set; }

    public MedioDePago(string cci)
    {
        CCI = cci;
    }
}