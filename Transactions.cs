using System;

public class Transaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // "Income" or "Expense"
    public string Category { get; set; } // e.g., "Food", "Transportation"
    public DateTime Date { get; set; }

    public Transaction(string description, decimal amount, string type, string category, DateTime date)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
    }
}
