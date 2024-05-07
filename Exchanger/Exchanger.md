```mermaid
classDiagram

class CasaDeCambio {
    - nombre: String
    - direccion: String
    - listaDeMoneda: List<Moneda>
    + Dar_alta(Moneda: Moneda): void
    + Dar_baja(Moneda: Moneda): void
}

class Moneda {
    - codigo: String
    - nombre: String
    + AgregarMoneda(Moneda: Moneda): void 
    + buscarMoneda(codigo: String): Moneda
}

class Cliente {
    - nombre: String
    - direccion: String
    - saldo: double
    - listaDeTransacciones: List<Transaccion>
    + Cliente(nombre: String, direccion: String, saldo: double)
    + depositar(monto: double): void
    + retirar(monto: double): boolean
    + getSaldo(): double
    + agregarTransaccion(transaccion: Transaccion): void
}

class MediodePago {
    - Cliente : String
    - CCI : String
    
    + Cuenta(Cliente: String, CCI: String)
    + getCliente(): String
    + setCliente(nuevoCliente: String): void
    + getCCI(): String
    + setCCI(nuevaCCI: String): void
}

abstract class Transaccion {
    - Cliente : String
    - TasadeCambio : String
    - fecha: date
    - monto : double
    + Transaccion( fecha: date, tipo: String, monto: double, descripcion: String)
    + getFecha(): date
    + getTipo(): String
    + getMonto(): double
    + getDescripcion(): String
}

class TasadeCambio {
    
    - Fecha : Date
    - MonedaOrigen: String
    - MonedaDestino: String
    + TasadeCambio(TipoDeConversion: String, MonedaOrigen: String, MonedaDestino: String, Tasa: double)
    + ObtenerTasadeCambio(): double
    + ActualizarTasadeCambio(nuevaTasa: double): void
    + implementaCambio(monto: double, divisaOrigen: Divisa, divisaDestino: Divisa): double
}

CasaDeCambio --> Moneda : Contiene
Moneda --> TasadeCambio : Tiene
TasadeCambio --> Transaccion : Realiza
Transaccion --> Cliente : Realiza
CasaDeCambio --> Cliente : Tiene
Transaccion --> MediodePago : Realiza

```
