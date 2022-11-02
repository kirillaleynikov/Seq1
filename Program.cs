using System;
using Serilog;
using Serilog.Configuration;

namespace exchange
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding=System.Text.Encoding.Unicode;
            var tryParseResult = false;
            var kurs = 60.47d;
            var com037 = 0.37d;
            var com500 = 500d;
            double sum1 = 0, sum2 = 0, value=0;
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Курс доллара:", kurs)
                .WriteTo.Seq("http://localhost:5341/", apiKey: "xSQNmUCDzyRmyt7Vkg0x")
                .CreateLogger();
     

            while (!tryParseResult)
            {
                Console.Write("Введите значение в долларах: ");
                var userInput = Console.ReadLine();
                tryParseResult = double.TryParse(userInput, out value);
            
                if(!tryParseResult)
                {
                    Log.Error("Введено некорректное значение");
                }
            }
            Log.Information($"Введено корректное значение: {value}");
            sum1 += value * kurs - 8;
            sum2 += value * kurs * 0.9963;
            if (value <= com500 / kurs)
            {
                Console.WriteLine("Курс доллара: " + kurs + " ₽");
                Console.WriteLine("Вы выводите: {0:#.##} ₽", (kurs * value));
                Console.WriteLine("Комиссия банка составляет 8 ₽");
                Console.WriteLine("Итого: {0:#.##} ₽", sum1);
                Log.Information($"К выдаче: {sum1:C2}");
            }
            else
            {
                Console.WriteLine("Курс доллара: " + kurs + " ₽");
                Console.WriteLine("Вы выводите: {0:#.##} ₽", (value * kurs));
                Console.WriteLine("Комиссия банка составляет: " + com037 + " %");
                Console.WriteLine("Итого: {0:#.##} ₽", sum2);
                Log.Information($"К выдаче: {sum2:C2}");
            }
            Log.Information("Конвертирование прошло успешно");
            Log.CloseAndFlush();
        }
        
    }
}