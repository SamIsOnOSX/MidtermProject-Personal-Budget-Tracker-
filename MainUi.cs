using Godot;
using System;

public partial class MainUi : Control
{
    private Node transactionHistoryList;
    private OptionButton typeIncomeExpenseButton; // Reference for Type_Income_Expense OptionButton
    private BudgetTracker budgetTracker = new BudgetTracker(); // Create an instance of BudgetTracker

    public override void _Ready()
    {
        // Reference the TransactionHistoryList node
        transactionHistoryList = GetNode<Node>("TransactionHistoryList");

        // Reference the Type_Income_Expense OptionButton node
        typeIncomeExpenseButton = GetNode<OptionButton>("Type_Income_Expense");

        // Add "Income" and "Expense" items to the OptionButton dynamically
        if (typeIncomeExpenseButton.GetItemCount() == 0)
        {
            typeIncomeExpenseButton.AddItem("Income");
            typeIncomeExpenseButton.AddItem("Expense");
        }

        // Connect the AddTransactionButton signal using the Pressed event
        GetNode<Button>("AddTransactionButton").Pressed += OnAddTransactionButtonPressed;

        // **Verify that labels exist before updating them**
        UpdateFinancialSummary();
    }

    private void OnAddTransactionButtonPressed()
    {
        // Retrieve input nodes
        var descriptionInput = GetNode<LineEdit>("DescriptionInput");
        var categoryInput = GetNode<LineEdit>("CategoryInput");
        var amountInput = GetNode<LineEdit>("AmountInput");

        // Validate inputs
        string description = descriptionInput.Text.Trim();
        string category = categoryInput.Text.Trim();
        string dateAdded = DateTime.Now.ToString("MM/dd/yyyy");
        string type = typeIncomeExpenseButton.GetItemText(typeIncomeExpenseButton.Selected); // Get selected type

        if (!decimal.TryParse(amountInput.Text, out decimal amount) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(type))
        {
            GD.PrintErr("Invalid input. Please enter valid values.");
            return;
        }

        // **Create and add a new transaction to BudgetTracker**
        Transaction newTransaction = new Transaction(description, amount, type, category, DateTime.Now);
        budgetTracker.AddTransaction(newTransaction);

        // **Pass the transaction details to TransactionHistoryList**
        var transactionHistoryScript = transactionHistoryList as TransactionHistoryList;
        if (transactionHistoryScript != null)
        {
            transactionHistoryScript.AddTransaction(type, category, description, dateAdded, amount);
        }

        // **Update financial summary labels**
        UpdateFinancialSummary();

        // **Clear input fields**
        descriptionInput.Text = string.Empty;
        categoryInput.Text = string.Empty;
        amountInput.Text = string.Empty;
        typeIncomeExpenseButton.Selected = 0; // Reset to "Income"
    }

    private void UpdateFinancialSummary()
    {
        // **Ensure labels exist before modifying them**
        var incomeLabel = GetNodeOrNull<Label>("TotalIncomeLabel");
        var expensesLabel = GetNodeOrNull<Label>("TotalExpensesLabel");
        var savingsLabel = GetNodeOrNull<Label>("NetSavingsLabel");

        if (incomeLabel != null) incomeLabel.Text = $"Total Income: PHP {budgetTracker.GetTotalIncome():N2}";
        if (expensesLabel != null) expensesLabel.Text = $"Total Expenses: PHP {budgetTracker.GetTotalExpenses():N2}";
        if (savingsLabel != null) savingsLabel.Text = $"Net Savings: PHP {budgetTracker.GetNetSavings():N2}";
    }
}