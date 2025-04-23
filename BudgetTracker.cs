using System;
using System.Collections.Generic;
using System.Linq;

public class BudgetTracker
{
    private List<Transaction> transactions = new List<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public decimal GetTotalIncome()
    {
        return transactions
            .Where(t => t.Type == "Income")
            .Sum(t => t.Amount);
    }

    public decimal GetTotalExpenses()
    {
        return transactions
            .Where(t => t.Type == "Expense")
            .Sum(t => t.Amount);
    }

    public decimal GetNetSavings()
    {
        return GetTotalIncome() - GetTotalExpenses();
    }

    public Dictionary<string, decimal> GetCategoryAnalytics()
    {
        return transactions
            .Where(t => t.Type == "Expense")
            .GroupBy(t => t.Category)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(t => t.Amount)
            );
    }

    public List<Transaction> GetSortedTransactions(string sortBy)
    {
        return sortBy switch
        {
            "Date" => transactions.OrderBy(t => t.Date).ToList(),
            "Category" => transactions.OrderBy(t => t.Category).ToList(),
            "Amount" => transactions.OrderBy(t => t.Amount).ToList(),
            _ => transactions
        };
    }
}
