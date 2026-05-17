using System;

namespace Payroll
{
    // Класс для расчета зарплаты сотрудника за месяц.
    public class Payment
    {
        private const decimal PensionFundPercent = 1m;
        private const decimal IncomeTaxPercent = 13m;

        public string FullName { get; set; }

        public decimal Salary { get; set; }

        public int HireYear { get; set; }

        public decimal AllowancePercent { get; set; }

        public decimal IncomeTax { get; private set; }

        public int DaysWorked { get; set; }

        public int WorkingDaysInMonth { get; set; }

        public decimal AccruedAmount { get; private set; }

        public decimal WithheldAmount { get; private set; }

        public Payment(
            string fullName,
            decimal salary,
            int hireYear,
            decimal allowancePercent,
            int daysWorked,
            int workingDaysInMonth)
        {
            FullName = fullName;
            Salary = salary;
            HireYear = hireYear;
            AllowancePercent = allowancePercent;
            DaysWorked = daysWorked;
            WorkingDaysInMonth = workingDaysInMonth;
        }

        public decimal CalculateAccrued()
        {
            Validate();

            decimal basePay = Salary * DaysWorked / (decimal)WorkingDaysInMonth;
            decimal allowance = basePay * AllowancePercent / 100m;

            AccruedAmount = Math.Round(basePay + allowance, 2);
            return AccruedAmount;
        }

        public decimal CalculateWithheld()
        {
            decimal accrued = CalculateAccrued();
            decimal pensionFund = accrued * PensionFundPercent / 100m;

            IncomeTax = Math.Round(accrued * IncomeTaxPercent / 100m, 2);
            WithheldAmount = Math.Round(pensionFund + IncomeTax, 2);
            return WithheldAmount;
        }

        public decimal CalculateNet()
        {
            CalculateWithheld();
            return Math.Round(AccruedAmount - WithheldAmount, 2);
        }

        public int CalculateExperience()
        {
            int currentYear = DateTime.Now.Year;
            int experience = currentYear - HireYear;

            return experience < 0 ? 0 : experience;
        }

        public override string ToString()
        {
            decimal netAmount = CalculateNet();

            return
                $"Сотрудник: {FullName}\n" +
                $"Оклад: {Salary:F2}\n" +
                $"Год поступления: {HireYear}\n" +
                $"Стаж: {CalculateExperience()} лет\n" +
                $"Отработано дней: {DaysWorked} из {WorkingDaysInMonth}\n" +
                $"Процент надбавки: {AllowancePercent}%\n" +
                $"Начислено: {AccruedAmount:F2}\n" +
                $"Подоходный налог: {IncomeTax:F2}\n" +
                $"Удержано: {WithheldAmount:F2}\n" +
                $"К выдаче: {netAmount:F2}";
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(FullName))
                throw new ArgumentException("ФИО не может быть пустым.");

            if (Salary < 0)
                throw new ArgumentException("Оклад не может быть отрицательным.");

            if (AllowancePercent < 0)
                throw new ArgumentException("Процент надбавки не может быть отрицательным.");

            if (WorkingDaysInMonth <= 0)
                throw new ArgumentException("Количество рабочих дней в месяце должно быть больше нуля.");

            if (DaysWorked < 0 || DaysWorked > WorkingDaysInMonth)
                throw new ArgumentException("Количество отработанных дней должно быть от 0 до количества рабочих дней.");
        }
    }
}
