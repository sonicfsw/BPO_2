using System;
using Payroll;

class Program
{
    static void Main()
    {
        var payment = new Payment(
            fullName: "Иванов Иван Иванович",
            salary: 50000m,
            hireYear: 2018,
            allowancePercent: 10m,
            daysWorked: 20,
            workingDaysInMonth: 22);

        payment.CalculateAccrued();
        payment.CalculateWithheld();

        Console.WriteLine(payment);
    }
}
