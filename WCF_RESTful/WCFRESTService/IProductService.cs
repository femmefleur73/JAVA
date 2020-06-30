using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFRESTServiceDemo
{
    [ServiceContract]
    public interface IProductService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/GetProduct/{productId}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Product> GetProductDetails(string productId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetProduct",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        Product GetProduct(string product);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetProductDT",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
<<<<<<< HEAD:WCF_RESTful/WCFRESTService/IProductService.cs
        DataTable GetProductDT(string product);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetProductORA",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        string ConvertDataTabletoString(string EMPLOYEE_ID);

=======
        MyTableUtilClass GetProductDT(string product);
>>>>>>> parent of d515a4e... oracle:WCF_RESTful/WCFRESTServiceDemo/IProductService.cs
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCost { get; set; }
    }

    public class MyTableUtilClass
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public DataTable Product { get; set; }
    }
}
