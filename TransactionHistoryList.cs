using Godot;
using System;
using System.IO;

public partial class TransactionHistoryList : Node
{
    private VBoxContainer transactionContainer;
    private string filePath; 

    public override void _Ready()
    {
        // Get the application's directory and construct the file path
        string appDirectory = System.IO.Path.GetDirectoryName(OS.GetExecutablePath());
        filePath = System.IO.Path.Combine(appDirectory, "transactions.txt");

        // Reference the VBoxContainer inside ScrollContainer
        transactionContainer = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");

        // Load existing transactions from file when the game starts
        LoadTransactionsFromFile();
    }

    public void AddTransaction(string type, string category, string description, string dateAdded, decimal amount)
    {
        Label transactionLabel = new Label
        {
            Text = $"Type: {type} \nDate: {dateAdded} \nCategory: {category} \nDescription: {description} \nAmount: PHP {amount:N2}",
            AutowrapMode = TextServer.AutowrapMode.Word,
            CustomMinimumSize = new Vector2(400, 50),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };

        transactionLabel.AddThemeConstantOverride("margin_bottom", 10);
        transactionContainer.AddChild(transactionLabel);

        // Save transaction to file
        SaveTransactionToFile(type, category, description, dateAdded, amount);
    }

    private void SaveTransactionToFile(string type, string category, string description, string dateAdded, decimal amount)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.UTF8)) // Append mode + UTF-8 encoding
        {
            writer.WriteLine($"{dateAdded}, {type}, {category}, {description}, PHP {amount:N2}");
        }
    }

    private void LoadTransactionsFromFile()
    {
        if (!File.Exists(filePath)) return; // No file yet, skip loading

        using (StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split(',');

                if (parts.Length == 5)
                {
                    AddTransaction(parts[1].Trim(), parts[2].Trim(), parts[3].Trim(), parts[0].Trim(), decimal.Parse(parts[4].Replace("PHP", "").Trim()));
                }
            }
        }
    }
}
