using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ModelsForWpfOnly;

public enum TransactionType
{
    EXPENSE=0, INCOME, TRANSFER
}
    
public class FinancialTransaction
{
    public FinancialTransaction() { }
    public FinancialTransaction(Expense expense)
    {
        Amount = -expense.Amount;
        DateOfTransaction = expense.DateOfExpense;
        Title= expense.Title;
        BalanceBefore = expense.PaymentMethod.GetBalanceForDate(DateOfTransaction);
        BalanceAfter = BalanceBefore + Amount;
        TransactionId = expense.Id;
        TransactionType = TransactionType.EXPENSE;
    }
    public FinancialTransaction(Income income)
    {
        Amount = income.Amount;
        DateOfTransaction = income.DateOfIncome;
        Title = income.Title;
        BalanceBefore = income.PaymentMethod.GetBalanceForDate(DateOfTransaction);
        BalanceAfter = BalanceBefore + Amount;
        TransactionId = income.Id;
        TransactionType = TransactionType.INCOME;
    }
    public FinancialTransaction(Transfer transfer, bool isIncome)
    {
        if(isIncome)
        {
            Amount = transfer.Amount;
            DateOfTransaction = transfer.DateOfTransfer;
            Title = "Incoming transfer";
            BalanceBefore = transfer.To.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = transfer.Id;
            TransactionType = TransactionType.TRANSFER;
        }
        else
        {
            Amount = -transfer.Amount;
            DateOfTransaction = transfer.DateOfTransfer;
            Title = "Outcoming Transfer";
            BalanceBefore = transfer.From.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = transfer.Id;
            TransactionType = TransactionType.TRANSFER;
        }
        //Amount = -transferFrom.Amount;
        //DateOfTransaction = transferFrom.DateOfTransfer;
        //Title = "Transfer";
        //BalanceBefore = transferFrom.From.GetBalanceForDate(DateOfTransaction);
        //BalanceAfter = BalanceBefore + Amount;
    }
    [NotMapped]
    public decimal BalanceBefore { get; set; }
    [NotMapped]
    public decimal Amount { get; set; }
    [NotMapped]
    public decimal BalanceAfter { get; set; }
    [NotMapped]
    public DateTime DateOfTransaction { get; set; }
    [NotMapped]
    public string Title { get; set; }
    [NotMapped]
    public int TransactionId { get; set; }
    [NotMapped]
    public TransactionType TransactionType { get; set; }
}
