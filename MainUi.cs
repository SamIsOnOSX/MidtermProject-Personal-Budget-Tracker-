using Godot;
using System;

public partial class MainUi : Control
{
    private Node transactionHistoryList;
    private OptionButton typeIncomeExpenseButton; // Reference for Type_Income_Expense OptionButton

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
            return;
        }

        // Pass the transaction details to the TransactionHistoryList script
        var transactionHistoryScript = transactionHistoryList as TransactionHistoryList;
        if (transactionHistoryScript != null)
        {
            transactionHistoryScript.AddTransaction(type, category, description, dateAdded, amount);
        }

        // Clear input fields
        descriptionInput.Text = string.Empty;
        categoryInput.Text = string.Empty;
        amountInput.Text = string.Empty;

        // Reset Type_Income_Expense OptionButton to default
        typeIncomeExpenseButton.Selected = 0; // Default to "Income"
    }
}
