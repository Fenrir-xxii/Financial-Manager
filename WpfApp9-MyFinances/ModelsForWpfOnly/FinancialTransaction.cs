﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ModelsForWpfOnly;

public enum TransactionType
{
    EXPENSE=0, INCOME, TRANSFER, EXCHANGE, LOAN
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
        CurrencyCode = expense.PaymentMethod.Currency.CodeNumber;
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
        CurrencyCode = income.PaymentMethod.Currency.CodeNumber;
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
            CurrencyCode = transfer.To.Currency.CodeNumber;
        }
        else
        {
            Amount = -transfer.Amount;
            DateOfTransaction = transfer.DateOfTransfer;
            Title = "Outcoming transfer";
            BalanceBefore = transfer.From.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = transfer.Id;
            TransactionType = TransactionType.TRANSFER;
            CurrencyCode = transfer.From.Currency.CodeNumber;
        }
    }
    public FinancialTransaction(Exchange exchange, bool isIncome)
    {
        if (isIncome)
        {
            Amount = (decimal)((exchange.AmountTo == null) ? 0 : exchange.AmountTo);
            DateOfTransaction = exchange.DateOfExchange;
            Title = "Incoming exchange";
            BalanceBefore = exchange.To.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = exchange.Id;
            TransactionType = TransactionType.EXCHANGE;
            CurrencyCode = exchange.To.Currency.CodeNumber;
        }
        else
        {
            Amount = -exchange.AmountFrom;
            DateOfTransaction = exchange.DateOfExchange;
            Title = "Outcoming exchange";
            BalanceBefore = exchange.From.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = exchange.Id;
            TransactionType = TransactionType.EXCHANGE;
            CurrencyCode = exchange.From.Currency.CodeNumber;
        }
    }
    public FinancialTransaction(GivingLoan loan)
    {
        if(loan.ReceivingLoan == null) // original loan (giving loan to someone)
        {
            Amount = -loan.Amount;
            DateOfTransaction = loan.DateOfLoan;
            Title = "Giving loan";
            BalanceBefore = loan.PaymentMethod.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = loan.Id;
            TransactionType = TransactionType.LOAN;
            CurrencyCode = loan.PaymentMethod.Currency.CodeNumber;
        }
        else // giving back loan that I previously received
        {
            Amount = -loan.Amount;
            DateOfTransaction = loan.DateOfLoan;
            Title = "Paying back loan";
            BalanceBefore = loan.PaymentMethod.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = loan.Id;
            TransactionType = TransactionType.LOAN;
            CurrencyCode = loan.PaymentMethod.Currency.CodeNumber;
        }
    }
    public FinancialTransaction(ReceivingLoan loan)
    {
        if(loan.GivingLoan == null)  // original loan (receiving loan from someone)
        {
            Amount = loan.Amount;
            DateOfTransaction = loan.DateOfLoan;
            Title = "Receiving loan";
            BalanceBefore = loan.PaymentMethod.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = loan.Id;
            TransactionType = TransactionType.LOAN;
            CurrencyCode = loan.PaymentMethod.Currency.CodeNumber;
        }
        else   // receiving back loan that I previously gave
        {
            Amount = loan.Amount;
            DateOfTransaction = loan.DateOfLoan;
            Title = "Receiving loan payback";
            BalanceBefore = loan.PaymentMethod.GetBalanceForDate(DateOfTransaction);
            BalanceAfter = BalanceBefore + Amount;
            TransactionId = loan.Id;
            TransactionType = TransactionType.LOAN;
            CurrencyCode = loan.PaymentMethod.Currency.CodeNumber;
        }
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
    public int CurrencyCode { get; set; }   
    [NotMapped]
    public int TransactionId { get; set; }
    [NotMapped]
    public TransactionType TransactionType { get; set; }
}
