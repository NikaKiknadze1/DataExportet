namespace DataExporter
{
    public interface IProductsFileDataWriter
    {

        void WriteProducts(IEnumerable<Category> categories, string filePath);
    }

    public class ProductsFileDataWriter : IProductsFileDataWriter
    {
        public void WriteProducts(IEnumerable<Category> categories, string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            
            using StreamWriter writer = new(filePath);
            foreach (Category category in categories)
            {
                foreach (Product product in category.Products)
                {
                    writer.WriteLine($"{category.CategoryName}\t{category.CategoryIsDeleted}\t{product.ProductID}\t{product.ProductName}\t{product.UnitPrice}\t{product.ProductIsDeleted}");
                }
            }
        }
        
    }
}
