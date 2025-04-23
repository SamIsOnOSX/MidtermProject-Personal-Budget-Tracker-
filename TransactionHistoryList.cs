using Godot;

public partial class TransactionHistoryList : Node
{
    // Method to add a transaction with type, category, description, date, and amount
    public void AddTransaction(string type, string category, string description, string dateAdded, decimal amount)
    {
        // Create a new Label for the transaction entry
        Label transactionLabel = new Label
        {
            Text = $"Type: {type} | Date: {dateAdded} | Category: {category} | Description: {description} | Amount: PHP {amount:N2}"
        };

        // Add the Label to this node
        AddChild(transactionLabel);
        
    }
}
