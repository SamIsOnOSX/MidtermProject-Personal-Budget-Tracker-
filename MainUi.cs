using Godot;
using System;

public partial class MainUi : Control
{
    private Node transactionHistoryList;
    private OptionButton typeIncomeExpenseButton; 
    private BudgetTracker budgetTracker = new BudgetTracker();

    public override void _Ready()
    {
        transactionHistoryList = GetNode<Node>("TransactionHistoryList");
        typeIncomeExpenseButton = GetNode<OptionButton>("Type_Income_Expense");

        if (typeIncomeExpenseButton.GetItemCount() == 0)
        {
            typeIncomeExpenseButton.AddItem("Income");
            typeIncomeExpenseButton.AddItem("Expense");
        }

        GetNode<Button>("AddTransactionButton").Pressed += OnAddTransactionButtonPressed;

        UpdateFinancialSummary();
    }

    private void OnAddTransactionButtonPressed()
    {
        var descriptionInput = GetNode<LineEdit>("DescriptionInput");
        var categoryInput = GetNode<LineEdit>("CategoryInput");
        var amountInput = GetNode<LineEdit>("AmountInput");

        string description = descriptionInput.Text.Trim();
        string category = categoryInput.Text.Trim();
        string dateAdded = DateTime.Now.ToString("MM/dd/yyyy");
        string type = typeIncomeExpenseButton.GetItemText(typeIncomeExpenseButton.Selected);

        if (!decimal.TryParse(amountInput.Text, out decimal amount) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(type))
        {
            return;
        }

        Transaction newTransaction = new Transaction(description, amount, type, category, DateTime.Now);
        budgetTracker.AddTransaction(newTransaction);

        var transactionHistoryScript = transactionHistoryList as TransactionHistoryList;
        if (transactionHistoryScript != null)
        {
            transactionHistoryScript.AddTransaction(type, category, description, dateAdded, amount);
        }

        UpdateFinancialSummary();

        descriptionInput.Text = string.Empty;
        categoryInput.Text = string.Empty;
        amountInput.Text = string.Empty;
        typeIncomeExpenseButton.Selected = 0;
    }

    private void UpdateFinancialSummary()
    {
        var incomeLabel = GetNodeOrNull<Label>("TotalIncomeLabel");
        var expensesLabel = GetNodeOrNull<Label>("TotalExpensesLabel");
        var savingsLabel = GetNodeOrNull<Label>("NetSavingsLabel");

        if (incomeLabel != null) incomeLabel.Text = $"Total Income: PHP {budgetTracker.GetTotalIncome():N2}";
        if (expensesLabel != null) expensesLabel.Text = $"Total Expenses: PHP {budgetTracker.GetTotalExpenses():N2}";
        if (savingsLabel != null) savingsLabel.Text = $"Net Savings: PHP {budgetTracker.GetNetSavings():N2}";
    }
}
