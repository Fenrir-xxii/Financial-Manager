using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ModelsForWpfOnly;

public struct LoanPayback
{
    public LoanPayback(DateTime date, decimal amount, int paymentMethodId, string? description, int currencyCodeNumber)
    {
        Date = date;
        Amount = amount;
        PaymentMethodId = paymentMethodId;
        Description = description;
        CurrencyCodeNumber = currencyCodeNumber;
    }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public string? Description { get; set; }
    public int CurrencyCodeNumber { get; set; }
}

public class Loan
{
    public Loan() { }
    public Loan(GivingLoan givLoan, List<ReceivingLoan>? recLoans)
    {
        LoanAmount = givLoan.Amount;
        LoanDescription = givLoan.Description;
        DateOfLoan = givLoan.DateOfLoan;
        IsLoanClosed = givLoan.IsLoanClosed;
        LoanPaymentMethod = givLoan.PaymentMethod;
        LoanProvider = givLoan.Provider;
        LoanGiver = "Me";
        LoanReceiver = LoanProvider.Title;
        var paybacks = new List<LoanPayback>();
        if(recLoans != null)
        {
            recLoans.ForEach(l =>
            {
                paybacks.Add(new LoanPayback(l.DateOfLoan, l.Amount, l.PaymentMethodId, l.Description, l.PaymentMethod.Currency.CodeNumber));
            });
        }
        Paybacks = paybacks;
    }
    public Loan(ReceivingLoan recLoan, List<GivingLoan>? givLoans)
    {
        LoanAmount = recLoan.Amount;
        LoanDescription = recLoan.Description;
        DateOfLoan = recLoan.DateOfLoan;
        IsLoanClosed = recLoan.IsLoanClosed;
        LoanPaymentMethod = recLoan.PaymentMethod;
        LoanProvider = recLoan.Provider;
        LoanGiver = LoanProvider.Title;
        LoanReceiver = "Me";
        var paybacks = new List<LoanPayback>();
        if (givLoans != null)
        {
            givLoans.ForEach(l =>
            {
                paybacks.Add(new LoanPayback(l.DateOfLoan, l.Amount, l.PaymentMethodId, l.Description, l.PaymentMethod.Currency.CodeNumber));
            });
        }
        Paybacks = paybacks;
    }
    [NotMapped]
    public decimal LoanAmount { get; set; }
    [NotMapped]
    public string? LoanDescription { get; set;}
    [NotMapped]
    public DateTime DateOfLoan { get; set; }
    [NotMapped]
    public bool IsLoanClosed { get; set; }
    [NotMapped]
    public PaymentMethod LoanPaymentMethod { get; set; }
    [NotMapped]
    public Provider LoanProvider { get; set; }
    [NotMapped]
    public string LoanGiver { get; set; }
    [NotMapped]
    public string LoanReceiver { get; set; }
    [NotMapped]
    public List<LoanPayback> Paybacks { get; set; }
}
