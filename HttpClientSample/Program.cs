using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace HttpClientSample
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            //RunAsync().GetAwaiter().GetResult();

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("GET PRODUCT BY ID");
            Console.WriteLine("-----------------------------------------------");
            Console.Write("Input ID = ");
            var id = Console.ReadLine();

            var dataProduct = GetProductByIdAsync("http://localhost:59575/api/product/" + int.Parse(id))
                .GetAwaiter().GetResult();

            Console.WriteLine("Result = ");
            Console.WriteLine(System.Environment.NewLine);
            ShowProduct(dataProduct);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("INSERT PRODUCT");
            Console.WriteLine("-----------------------------------------------");
            Product prd = new Product();
            Console.Write("ID = ");
            prd.Id = int.Parse(Console.ReadLine());
            Console.Write("Name = ");
            prd.Name = Console.ReadLine();
            Console.Write("Category = ");
            prd.Category = Console.ReadLine();
            Console.Write("Price = ");
            prd.Price = Convert.ToDecimal(Console.ReadLine());

            string strResult = CreateProductAsync(prd).GetAwaiter().GetResult();
            Console.WriteLine(strResult);

            ShowListProduct("http://localhost:59575/api/product/");

            Console.ReadLine();
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine("Name     : " + product.Name);
            Console.WriteLine("Price    : " + product.Price);
            Console.WriteLine("Category : " + product.Category);
            Console.WriteLine(System.Environment.NewLine);
        }

        static void ShowListProduct(string path)
        {
            var listPrd = GetAllProduct(path).GetAwaiter().GetResult();

            Console.WriteLine("============LIST PRODUCT============");
            foreach (var detail in listPrd)
            {
                Console.WriteLine("Name     : " + detail.Name);
                Console.WriteLine("Price    : " + detail.Price);
                Console.WriteLine("Category : " + detail.Category);
            }
            Console.WriteLine("============LIST PRODUCT============");
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        static async Task<String> CreateProductAsync(Product product)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync
                    ("http://localhost:59575/api/product/", product);
                response.EnsureSuccessStatusCode();

                return response.StatusCode.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static async Task<List<Product>> GetAllProduct(string path)
        {
            try
            {
                List<Product> listPrd = new List<Product>();
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    listPrd = JsonConvert.DeserializeObject<List<Product>>(strJson);
                }
                return listPrd;
            }
            catch { throw; }
        }

        /// <summary>
        /// Get product by ID 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static async Task<Product> GetProductByIdAsync(string path)
        {
            try
            {
                Product product = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(strJson);
                }
                return product;
            }
            catch
            {
                throw;
            }
        }

        static async Task<String> UpdateProductAsync(Product product)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync
                    ("http://localhost:59575/api/product/" + product.Id, product);
                response.EnsureSuccessStatusCode();

                return response.StatusCode.ToString();
            }
            catch { throw; }
           
        }

        static async Task<HttpStatusCode> DeleteProductAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("http://localhost:59575/api/product/" + id);
            return response.StatusCode;
        }
    }
}
