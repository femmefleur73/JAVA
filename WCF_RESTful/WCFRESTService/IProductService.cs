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
        [WebInvoke(UriTemplate = "GetProductObj",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        MyTableUtilClass GetProductObj(string product);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetProductDT",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        DataTable GetProductDT(string product);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetProductORA",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        string ConvertDataTabletoString(string EMPLOYEE_ID);

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
