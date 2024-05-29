using DataExporter.G19_ProductImport;
using Microsoft.Data.SqlClient;

namespace DataExporter
{
    
    public interface IProductsDatabaseReader
    {
        IEnumerable<Category> GetProducts();
    }
    public class ProductsDatabaseReader : IProductsDatabaseReader
    {
        private string _connectionString { get; set; }
        public ProductsDatabaseReader(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<Category> GetProducts()
        {
            List<Category> products = new();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "select c.CategoryName, 1 as CategoryIsDeleted,\r\n\t\t   p.ProductID, p.ProductName, p.UnitPrice, 1 as ProductIsDeleted\r\n\tfrom Categories as c\r\n\t\tinner join Products as p on c.CategoryID = p.CategoryID\r\n\t\torder by c.CategoryID";
                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Category? category = null;
                        while (reader.Read())
                        {
                            string categoryName = reader.GetString(0);
                            if (category == null || category.CategoryName != categoryName)
                            {
                                category = new Category()
                                {
                                    CategoryName = categoryName,
                                };
                                products.Add(category);
                            }

                            Product product = new Product()
                            {
                                ProductName = reader.GetString(3),
                                UnitPrice = reader.GetDecimal(4),
                            };

                            category.Products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        public void Dispose()
        {
            _connectionString = null;
        }
    }
}
