using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        static readonly IProductRepository productRepository = new ProductRepository();

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAll();
        }

        // example uri = http://localhost:59575/api/product/
        public Product GetProduct(int id)
        {
            Product item = productRepository.Get(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        // example uri = http://localhost:59575/api/product/getproductcategory/?category=groceries
        [Route("GetProductCategory")]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            var data = productRepository.GetAll()
                .Where(o => string.Equals(o.Category, category, StringComparison.OrdinalIgnoreCase));

            return data;
        }

        public HttpResponseMessage PostProduct(Product item)
        {
            item = productRepository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!productRepository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteProduct(int id)
        {
            productRepository.Remove(id);
        }
    }
}
