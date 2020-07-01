using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Oracle.DataAccess.Client;

namespace WCFRESTService
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서Service1.svc나 Service1.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class ProductService : IProductService
    {
        //private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private static string sDirPath = @"C:\LOG";
        private static string file = sDirPath + @"\ServerLog_" + DateTime.Now.ToString("yyyyMMdd") + ".TXT";

        private ProductService()
        {
            DirectoryInfo di = new DirectoryInfo(sDirPath);
            if (di.Exists == false)
            {
                di.Create();
            }
        }

        private static string path = AppDomain.CurrentDomain.BaseDirectory;

        public List<Product> GetProductDetails(string productId)
        {
            File.WriteAllText(file, string.Format("[전송파라미터]: {0}\r\n", productId));
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
            File.WriteAllText(file, string.Format("[전송파라미터]: {0}\r\n", product));
            List<Product> pdlist = new List<Product>();

            JObject json = JObject.Parse(product);

            Dictionary<string, string> prod = new Dictionary<string, string>();
            prod = JsonConvert.DeserializeObject<Dictionary<string, string>>(product);
            File.AppendAllText(file, string.Format("[Dictionary Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", prod["ProductId"], prod["ProductName"], prod["ProductCost"]));

            Product objProduct = new Product();
            foreach (JProperty property in json.Properties())
            {
                string Name = property.Name;
                string Value = property.Value.ToString();

                if (Name.Equals("ProductId")) objProduct.ProductId = Value;
                if (Name.Equals("ProductName")) objProduct.ProductName = Value;
                if (Name.Equals("ProductCost")) objProduct.ProductCost = Value;
            }

            File.AppendAllText(file, string.Format("[Product Data Class]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", objProduct.ProductId, objProduct.ProductName, objProduct.ProductCost));

            return objProduct;
        }

        /// <summary>
        /// DataTable Data 처리 테스트
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public MyTableUtilClass GetProductOBJ(string product)
        {
            //DataSet ds = new DataSet();
            DataTable dtOut = new DataTable();

            File.WriteAllText(file, string.Format("[GetProductDT::전송파라미터]: {0}\r\n", product));

            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(product);
            DataTable pdDT = dataSet.Tables["Product"];

            Product objProduct = new Product();
            foreach (DataRow dr in pdDT.Rows)
            {
                objProduct.ProductId = dr["ProductId"].ToString();
                objProduct.ProductName = dr["ProductName"].ToString();
                objProduct.ProductCost = dr["ProductCost"].ToString();

                File.AppendAllText(file, string.Format("[DataTable In Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", dr["ProductId"], dr["ProductName"], dr["ProductCost"]));
            }

            dtOut = pdDT.Clone();
            for (int i = 0; i < 10; i++)
            {
                DataRow newRow = dtOut.NewRow();
                newRow["ProductId"] = i.ToString();
                newRow["ProductName"] = "SAM item" + i;
                newRow["ProductCost"] = (20000 * i).ToString();
                dtOut.Rows.Add(newRow);
            }

            File.AppendAllText(file, string.Format("[Product Out Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", dtOut.Rows[0].ToString(), dtOut.Rows[1].ToString(), dtOut.Rows[2].ToString()));

            //ds.Tables.Add(dtOut);

            MyTableUtilClass pd = new MyTableUtilClass();
            pd.Status = "S";
            pd.Message = "성공";
            pd.Product = dtOut;

            return pd;
        }

        /// <summary>
        /// DataTable Data 처리 테스트
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public DataTable GetProductDT(string product)
        {
            //DataSet ds = new DataSet();
            DataTable dtOut = new DataTable();

            File.WriteAllText(file, string.Format("[GetProductDT::전송파라미터]: {0}\r\n", product));

            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(product);
            DataTable pdDT = dataSet.Tables["Product"];

            Product objProduct = new Product();
            foreach (DataRow dr in pdDT.Rows)
            {
                objProduct.ProductId = dr["ProductId"].ToString();
                objProduct.ProductName = dr["ProductName"].ToString();
                objProduct.ProductCost = dr["ProductCost"].ToString();

                File.AppendAllText(file, string.Format("[DataTable In Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", dr["ProductId"], dr["ProductName"], dr["ProductCost"]));
            }

            dtOut = pdDT.Clone();
            for (int i = 0; i < 5; i++)
            {
                DataRow newRow = dtOut.NewRow();
                newRow["ProductId"] = i.ToString();
                newRow["ProductName"] = "DataTable item" + i;
                newRow["ProductCost"] = (20000 * i).ToString();
                dtOut.Rows.Add(newRow);
            }

            File.AppendAllText(file, string.Format("[Product Out Data]\r\nProduct Id : {0}, ProductName :{1}, ProductCost :{2}\r\n", dtOut.Rows[0].ToString(), dtOut.Rows[1].ToString(), dtOut.Rows[2].ToString()));

            //ds.Tables.Add(dtOut);
            //string jstring = ToJson(dtOut);
            return dtOut;
        }

        public string ToJson(DataTable value)

        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in value.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in value.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return serializer.Serialize(rows);
        }


        public string ConvertDataTabletoString(string employee_id)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string conStr = "Provider = OraOLEDB.Oracle; Data Source = ORCL; User Id = ORAUSER; Password = 1qazxsw2";

            string connectionString =
                "user id=ORAUSER;password=1qazxsw2;" +
                "data source=(DESCRIPTION=(ADDRESS=" +
                "(PROTOCOL=tcp)(HOST=localhost)" +
                "(PORT=1521))(CONNECT_DATA=" +
                "(SERVICE_NAME=ORCL)))";

            File.WriteAllText(file, string.Format("[conStr]\r\n{0}\r\n", conStr));

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                string query = "";
                query = query + "SELECT * FROM ORAUSER.EMPLOYEES ";
                query = query + "WHERE 1=1 ";
                query = query + "AND EMPLOYEE_ID = '" + employee_id + "' ";
                query = query + "";

                File.AppendAllText(file, string.Format("[query]\r\n{0}\r\n", query));

                using (OracleCommand OraCmd = new OracleCommand(query, con))
                {
                    OraCmd.CommandText = query;
                    con.Open();
                    OracleDataAdapter adapter = new OracleDataAdapter(OraCmd);
                    adapter.Fill(ds);
                    dt = ds.Tables[0];
                }
                

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                    File.AppendAllText(file, string.Format("[ROW]\r\n{0}\r\n", row.ToString()));

                }

                return serializer.Serialize(rows);
                
            }
        }

    }
}
