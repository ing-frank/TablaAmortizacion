using System.Globalization;
using Spectre.Console;

// Formato numérico de República Dominicana: 10,000.00
CultureInfo.CurrentCulture = new CultureInfo("es-DO");

AnsiConsole.Write(new Rule("[yellow]Tabla de amortización de préstamo[/]"));
AnsiConsole.WriteLine();

// ---------- Entrada de datos ----------
decimal monto = AnsiConsole.Prompt(
    new TextPrompt<decimal>("Monto del préstamo:")
        .ValidationErrorMessage("[red]Debe escribir un número válido[/]")
        .Validate(v => v > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("[red]El monto debe ser mayor que cero[/]")));

decimal tasaAnual = AnsiConsole.Prompt(
    new TextPrompt<decimal>("Tasa de interés anual (%):")
        .ValidationErrorMessage("[red]Debe escribir un número válido[/]")
        .Validate(v => v >= 0
            ? ValidationResult.Success()
            : ValidationResult.Error("[red]La tasa no puede ser negativa[/]")));

int plazo = AnsiConsole.Prompt(
    new TextPrompt<int>("Plazo del préstamo (en meses):")
        .ValidationErrorMessage("[red]Debe escribir un número entero[/]")
        .Validate(v => v > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("[red]El plazo debe ser mayor que cero[/]")));

// ---------- Cálculos ----------
// Tasa mensual: i = (I / 12) / 100
decimal tasaMensual = tasaAnual / 12 / 100;

// (1 + i)^n calculado con un for para mantener la precisión de decimal
decimal factor = 1;
for (int k = 0; k < plazo; k++)
{
    factor *= 1 + tasaMensual;
}

// Cuota fija: C = M * i * (1+i)^n / ((1+i)^n - 1)
decimal cuota = tasaMensual == 0
    ? monto / plazo
    : monto * tasaMensual * factor / (factor - 1);

// ---------- Tabla ----------
var tabla = new Table()
    .Border(TableBorder.Rounded)
    .Title("[bold]Tabla de amortización[/]");

tabla.AddColumn(new TableColumn("[bold]No. de cuota[/]").Centered());
tabla.AddColumn(new TableColumn("[bold]Pago de cuota[/]").RightAligned());
tabla.AddColumn(new TableColumn("[bold]Interés a pagar[/]").RightAligned());
tabla.AddColumn(new TableColumn("[bold]Abono al capital[/]").RightAligned());
tabla.AddColumn(new TableColumn("[bold]Saldo[/]").RightAligned());

decimal saldo = monto;
decimal totalIntereses = 0;
decimal totalPagado = 0;

for (int numero = 1; numero <= plazo; numero++)
{
    decimal interes = saldo * tasaMensual;   // interés del período
    decimal abono = cuota - interes;         // abono al capital
    saldo -= abono;                          // M = M - abono al capital

    // Evita que quede un residuo de centavos en la última cuota
    if (numero == plazo && Math.Abs(saldo) < 0.01m)
    {
        saldo = 0;
    }

    totalIntereses += interes;
    totalPagado += cuota;

    tabla.AddRow(
        numero.ToString(),
        cuota.ToString("N2"),
        interes.ToString("N2"),
        abono.ToString("N2"),
        saldo.ToString("N2"));
}

// ---------- Salida ----------
AnsiConsole.WriteLine();
AnsiConsole.MarkupLine($"Tasa de interés mensual: [green]{tasaMensual * 100:N4} %[/]");
AnsiConsole.MarkupLine($"Cuota fija mensual: [green]{cuota:N2}[/]");
AnsiConsole.WriteLine();
AnsiConsole.Write(tabla);
AnsiConsole.MarkupLine($"Total de intereses: [yellow]{totalIntereses:N2}[/]");
AnsiConsole.MarkupLine($"Total pagado: [yellow]{totalPagado:N2}[/]");
