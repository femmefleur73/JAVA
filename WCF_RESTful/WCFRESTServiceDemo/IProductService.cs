using System;
using System.Collections.Generic;
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
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCost { get; set; }
    }

}
