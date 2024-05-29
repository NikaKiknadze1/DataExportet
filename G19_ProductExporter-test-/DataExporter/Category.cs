namespace DataExporter;

public class Category
{
    public string CategoryName { get; set; } = null!;
    public bool CategoryIsDeleted { get; set; }
    public ICollection<Product> Products { get; } = new List<Product>();

    public static Category GetCategory(string[] parts)
    {
        return new Category()
        {
            CategoryName = parts[0],
            CategoryIsDeleted = parts[1] == "1"
        };
    }

    public override string ToString() => $"Name: {CategoryName} IsDeleted {CategoryIsDeleted}";
}
