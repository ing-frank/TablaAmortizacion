# Tabla de amortización - Actividad 2

Aplicación de consola en C# que calcula y muestra la tabla de amortización de un préstamo con cuotas fijas, usando la librería Spectre.Console.

**Estudiante:** Frank Alberto Sherivé
**Matrícula:** LR-2025-03023
**Universidad:** UFHEC, La Romana

## Qué hace

Solicita el monto del préstamo, la tasa de interés anual (%) y el plazo en meses. Calcula la cuota fija mensual y muestra, para cada mes, el pago de cuota, el interés a pagar, el abono al capital y el saldo.

## Fórmulas

- Tasa mensual: `i = (I / 12) / 100`
- Cuota fija: `C = M * i * (1+i)^n / ((1+i)^n - 1)`
- Interés del período: `saldo * i`
- Abono al capital: `C - interés`
- Saldo: `saldo - abono al capital`

## Cómo ejecutar

```
dotnet restore
dotnet run
```

Requiere .NET 8 SDK.
