using ConsoleApplicationCancl.Services;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

var calculatorService = new CalculatorService();

while (true)
{
    Console.Write("Введите математическое выражение (например, '3.1 * (4 + 10)') или введите 'exit' для выхода:");
    string input = Console.ReadLine();

    if (input?.Trim().ToLower() == "exit")
    {
        break;
    }

    try
    {
        double result = calculatorService.Evaluate(input);
        Console.WriteLine($"Результат: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}
