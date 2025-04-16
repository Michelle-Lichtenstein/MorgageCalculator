using System;
using System.Collections.Generic;
using System.Globalization;

namespace MortgageCalculatorProgram
{
    public class MortgageCalculator
    {
        private const int MonthsInYear = 12;

        //This method calculates the monthly repayment amount based on the loan amount, the annual interest rate, as well as the loan term.
        public double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears)
        {
        //Convert annual interest rate to monthly interest rate
            double monthlyInterestRate = annualInterestRate / 100 / MonthsInYear;

        //This calculates the number of payments
            int totalPayments = loanTermYears * MonthsInYear;

        //This calculates the monthly repayment by using the formula for an amortizing loan
            double monthlyRepayment = loanAmount * (monthlyInterestRate / (1 - Math.Pow(1 + monthlyInterestRate, -totalPayments)));
            return monthlyRepayment;
        }

        //This method calculates the total amount of interest paid over the life of the loan.
        public double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears)
        {
            //Calculates monthly repayment
            double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
            
            //Calculates the total payments that was made
            int totalPayments = loanTermYears * MonthsInYear;
            
            //Calculates thebtotal amount paid over the life of the loan
            double totalPaid = monthlyRepayment * totalPayments;
            
            //Calculates the total interest paid by subtracting the loan amount from the total amount paid
            double totalInterestPaid = totalPaid - loanAmount;
            return totalInterestPaid;
        }

        // This method calculates the total amount paid over the life of the loan (this includes the principal as well as the interest).
        public double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears)
        {
            double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
            int totalPayments = loanTermYears * MonthsInYear;
            double totalPaid = monthlyRepayment * totalPayments;
            return totalPaid;
        }

        // The method in this section is used to generate an amortization schedule detailing each month's payment
        public List<AmortizationEntry> GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears)
        {
            List<AmortizationEntry> amortizationSchedule = new List<AmortizationEntry>();
            double monthlyInterestRate = annualInterestRate / 100 / MonthsInYear;
            int totalPayments = loanTermYears * MonthsInYear;
            double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
            double balance = loanAmount;

        // The for loop loops through each payment month to calculate the interest, the principal, and the remaining balance
            for (int i = 1; i <= totalPayments; i++)
            {
                double interestPayment = balance * monthlyInterestRate;
                double principalPayment = monthlyRepayment - interestPayment;
                balance -= principalPayment;

        // Create an amortization entry for the current month as well as add it to the schedule.
                AmortizationEntry entry = new AmortizationEntry
                {
                    PaymentNumber = i,
                    PaymentAmount = monthlyRepayment,
                    InterestPaid = interestPayment,
                    PrincipalPaid = principalPayment,
                    RemainingBalance = balance
                };

                amortizationSchedule.Add(entry);
            }

            return amortizationSchedule;
        }
    }

    // This class represents an entry in the amortization schedule.
    public class AmortizationEntry
    {
        public int PaymentNumber { get; set; }
        public double PaymentAmount { get; set; }
        public double InterestPaid { get; set; }
        public double PrincipalPaid { get; set; }
        public double RemainingBalance { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our Mortgage Calculator ");
            Console.WriteLine("Please provide us with the following details:");

            Console.Write("Your Loan Amount: R ");
            double loanAmount = double.Parse(Console.ReadLine());

            double annualInterestRate;
            bool isValidAnnualInterestRate = false;
            do
            {
                Console.Write("The Annual Interest Rate (% or decimal): ");
                string input = Console.ReadLine().Replace(',', '.');
                isValidAnnualInterestRate = double.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, 
                    out annualInterestRate);
                if (!isValidAnnualInterestRate)
                {
                    Console.WriteLine("Your input was invalid. Please enter a valid annual interest rate.");
                }
            } while (!isValidAnnualInterestRate);

            Console.Write("Loan Term (in years):  ");
            int loanTermYears = int.Parse(Console.ReadLine());

            MortgageCalculator calculator = new MortgageCalculator();

            // Generates the amortization schedule.
            List<AmortizationEntry> amortizationSchedule = calculator.GenerateAmortizationSchedule(loanAmount, annualInterestRate, loanTermYears);

            // Outputs the amortization schedule.
            Console.WriteLine("\nAmortization Schedule:");
            Console.WriteLine("Payment Month\tPayment Amount\tInterest Paid\tPrincipal Paid\tRemaining Balance");
            foreach (var entry in amortizationSchedule)
            {
                Console.WriteLine($"{entry.PaymentNumber}\t\tR {entry.PaymentAmount,-12:N2}\tR {entry.InterestPaid,-12:N2}\tR {entry.PrincipalPaid,-12:N2}\tR " +
                    $"{entry.RemainingBalance,-12:N2}");

            }

            double monthlyRepayment = calculator.CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
            double totalInterestPaid = calculator.CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
            double totalAmountPaid = calculator.CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);

            // Calculates and outputs the mortgage summary.
            Console.WriteLine("\nMortgage Summary:");
            Console.WriteLine($"Monthly Repayment: R {monthlyRepayment:N2}");
            Console.WriteLine($"Total Interest Paid: R {totalInterestPaid:N2}");
            Console.WriteLine($"Total Amount Paid: R {totalAmountPaid:N2}");
        }
    }
}