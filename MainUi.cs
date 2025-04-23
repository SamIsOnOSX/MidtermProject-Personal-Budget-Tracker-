using Godot;
using System;

public partial class MainUi : Control
{
    private BudgetTracker tracker = new BudgetTracker();

    public override void _Ready()
    {
        GD.Print("Budget Tracker GUI is ready!");

        // Ensure OptionButton is properly configured with items
        var optionButton = GetNode<OptionButton>("Type_Income_Expense");
        if (optionButton.GetItemCount() == 0) // Avoid duplicating items
        {
            optionButton.AddItem("Income");
            optionButton.AddItem("Expense");
        }
    }

    private void _on_AddTransactionButton_pressed()
    {
        try
        {
            // Retrieve and validate DescriptionInput
            var descriptionInput = GetNode<LineEdit>("DescriptionInput");
            if (descriptionInput == null)
            {
                GD.PrintErr("Node 'DescriptionInput' not found.");
                return;
            }
            var description = descriptionInput.Text;
            if (string.IsNullOrWhiteSpace(description))
            {
                GD.PrintErr("Description is required.");
                return;
            }

            // Retrieve and validate AmountInput
            var amountInput = GetNode<LineEdit>("AmountInput");
            if (amountInput == null)
            {
                GD.PrintErr("Node 'AmountInput' not found.");
                return;
            }
            if (!decimal.TryParse(amountInput.Text, out decimal amount))
            {
                GD.PrintErr("Invalid amount entered.");
                return;
            }

            // Retrieve and validate Type_Income_Expense OptionButton
            var optionButton = GetNode<OptionButton>("Type_Income_Expense");
            if (optionButton == null)
            {
                GD.PrintErr("Node 'Type_Income_Expense' not found.");
                return;
            }
            var type = optionButton.GetItemText(optionButton.Selected);

            // Retrieve and validate CategoryInput
            var categoryInput = GetNode<LineEdit>("CategoryInput");
            if (categoryInput == null)
            {
                GD.PrintErr("Node 'CategoryInput' not found.");
                return;
            }
            var category = categoryInput.Text;
            if (string.IsNullOrWhiteSpace(category))
            {
                GD.PrintErr("Category is required.");
                return;
            }

            // Create transaction and add to tracker
            var transaction = new Transaction(description, amount, type, category, DateTime.Now);
            tracker.AddTransaction(transaction);

            // Update the summary
            UpdateSummary();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error: {ex.Message}");
        }
    }

    private void UpdateSummary()
    {
        try
        {
            // Calculate and display financial summaries
            var totalIncome = tracker.GetTotalIncome();
            var totalExpenses = tracker.GetTotalExpenses();
            var netSavings = tracker.GetNetSavings();

            // Ensure SummaryLabel exists and update its text
            var summaryLabel = GetNode<Label>("SummaryLabel");
            if (summaryLabel == null)
            {
                GD.PrintErr("Node 'SummaryLabel' not found.");
                return;
            }
            summaryLabel.Text =
                $"Income: {totalIncome:C}\nExpenses: {totalExpenses:C}\nNet Savings: {netSavings:C}";
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error in UpdateSummary: {ex.Message}");
        }
    }
}
