using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WCFRESTServiceDemo
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서Service1.svc나 Service1.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class ProductService : IProductService
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        public List<Product> GetProductDetails(string productId)
        {
            File.WriteAllText(path + @"\Product.TXT", string.Format("[전송파라미터]: {0}\r\n", productId));
            Product objProduct = new Product();
            List<Product> objProductData = new List<Product>();
            objProduct.ProductId = productId;
            objProduct.ProductName = "Laptop";
            objProduct.ProductCost = "1000 $";
            objProductData.Add(objProduct);
            return objProductData;
        }

        public Product GetProduct(string product)
        {
            File.WriteAllText(path + @"\Product.TXT", string.Format("[전송파라미터]: {0}\r\n", product));
            List<Product> pdlist = new List<Product>();

            JObject json = JObject.Parse(product);

            Dictionary<string, string> prod = new Dictionary<string, string>();
            prod = JsonConvert.DeserializeObject<Dictionary<string, string>>(product);
            File.AppendAllText(path + @"\Product.TXT", string.Format("[Dictionary Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", prod["ProductId"], prod["ProductName"], prod["ProductCost"]));

            Product objProduct = new Product();
            foreach (JProperty property in json.Properties())
            {
                string Name = property.Name;
                string Value = property.Value.ToString();

                if (Name.Equals("ProductId")) objProduct.ProductId = Value;
                if (Name.Equals("ProductName")) objProduct.ProductName = Value;
                if (Name.Equals("ProductCost")) objProduct.ProductCost = Value;
            }

            File.AppendAllText(path + @"\Product.TXT", string.Format("[Product Data Class]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", objProduct.ProductId, objProduct.ProductName, objProduct.ProductCost));

            return objProduct;
        }
    }
}
