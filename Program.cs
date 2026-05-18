using System;
using System.Globalization;
using Payroll;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Введите данные для расчета заработной платы.");

            // Создаем объект Payment на основе данных, введенных пользователем с клавиатуры.
            var payment = new Payment(
                fullName: ReadRequiredString("ФИО сотрудника: "),
                salary: ReadDecimal("Оклад: "),
                hireYear: ReadInt("Год поступления на работу: "),
                allowancePercent: ReadDecimal("Процент надбавки: "),
                daysWorked: ReadInt("Количество отработанных дней: "),
                workingDaysInMonth: ReadInt("Количество рабочих дней в месяце: "));

            // Выполняем расчет начислений и удержаний.
            payment.CalculateAccrued();
            payment.CalculateWithheld();

            Console.WriteLine();
            Console.WriteLine(payment);
        }
        catch (ArgumentException exception)
        {
            // Обработка ошибок, связанных с некорректными значениями полей.
            Console.WriteLine($"Ошибка в данных: {exception.Message}");
        }
        catch (Exception exception)
        {
            // Общий обработчик для непредвиденных ошибок программы.
            Console.WriteLine($"Непредвиденная ошибка: {exception.Message}");
        }
    }

    // Метод считывает непустую строку с клавиатуры.
    private static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string? value = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("значение не может быть пустым.");

                return value.Trim();
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Ошибка ввода: {exception.Message}");
            }
        }
    }

    // Метод считывает целое число и повторяет ввод при ошибке.
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string? value = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(value))
                    throw new FormatException("введите целое число.");

                return int.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода: нужно ввести целое число.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода: число выходит за допустимый диапазон.");
            }
        }
    }

    // Метод считывает дробное число и повторяет ввод при ошибке.
    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string? value = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(value))
                    throw new FormatException("введите число.");

                // Позволяем пользователю вводить дробную часть через запятую или точку.
                value = value.Replace(',', '.');
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода: нужно ввести число.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка ввода: число выходит за допустимый диапазон.");
            }
        }
    }
}
