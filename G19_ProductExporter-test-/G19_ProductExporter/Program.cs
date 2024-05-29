using DataExporter;
using Microsoft.Data.SqlClient;


namespace G19_ProductExporter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filePath = @"D:\products.txt";
            const string connectionString = "Server=.;Database=Northwind;Integrated Security=true;TrustServerCertificate=true";
            ProductsDatabaseReader reader = new(connectionString);
            ProductsFileDataWriter writer = new();
            writer.WriteProducts(reader.GetProducts(), filePath);
            try
            {
                foreach (Category category in reader.GetProducts())
                {
                    foreach (Product product in category.Products)
                    {

                        Console.WriteLine($"{category}\t{product}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Dispose();
            }
        }
    }
}
