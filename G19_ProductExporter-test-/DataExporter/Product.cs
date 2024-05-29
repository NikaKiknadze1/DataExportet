namespace DataExporter;

public class Product
{
    public string ProductID { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public bool ProductIsDeleted { get; set; }

    public static Product GetProduct(string[] parts)
    {
        return new Product()
        {
            ProductID = parts[2],
            ProductName = parts[3],
            UnitPrice = decimal.Parse(parts[4]),
            ProductIsDeleted = parts[5] == "1"
        };
    }

    public override string ToString() => $"Code: {ProductID} Name: {ProductName} Price: {UnitPrice:0.00} IsDeleted: {ProductIsDeleted}";
}