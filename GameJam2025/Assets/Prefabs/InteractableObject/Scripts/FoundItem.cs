public class FoundItem
{
    public string itemName;
    public int amount;

    public FoundItem(string itemName)
    {
        this.itemName = itemName;
        amount = 1;
    }

    public FoundItem(string itemName, int amount)
    {
        this.itemName = itemName;
        this.amount = amount;
    }
}