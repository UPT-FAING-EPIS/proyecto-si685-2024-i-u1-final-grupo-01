using System;
using System.Collections.Generic;
namespace Exchanger.Domain;
public class CasaDeCambio
{
    private static CasaDeCambio? instance = null;

    public static CasaDeCambio GetInstance()
    {
        if (instance == null)
        {
            instance = new CasaDeCambio();
        }
        return instance!;
    }
}