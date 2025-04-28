using System;
using System.Collections.Generic;
using System.Linq;

public class BudgetTracker
{
    private List<Transaction> transactions = new List<Transaction>();

    public void AddTransaction(Transaction transaction) => transactions.Add(transaction);

    public decimal GetTotalIncome() =>
        transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);

    public decimal GetTotalExpenses() =>
        transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);

    public decimal GetNetSavings() =>
        GetTotalIncome() - GetTotalExpenses();

    public Dictionary<string, decimal> GetCategoryAnalytics() =>
        transactions
            .Where(t => t.Type == "Expense")
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

    public List<Transaction> GetSortedTransactions(string sortBy) =>
        sortBy switch
        {
            "Date" => transactions.OrderBy(t => t.Date).ToList(),
            "Category" => transactions.OrderBy(t => t.Category).ToList(),
            "Amount" => transactions.OrderBy(t => t.Amount).ToList(),
            _ => transactions
        };
}
