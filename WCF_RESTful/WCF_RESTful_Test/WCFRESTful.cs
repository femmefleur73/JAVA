using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WCF_RESTful_Test
{
    public partial class frmWCFRESTful : Form
    {
        private static string sUrl = @"http://localhost:12419/ProductService.svc/GetProduct";
        //private static string sInput = "{\"productId\" : \"1\"}";
        private static string sInput = @"{""product"" : ""{\""ProductCost\"":\""1000 $\"",\""ProductId\"":\""1\"",\""ProductName\"":\""Laptop\""}""}";
        private static string path = AppDomain.CurrentDomain.BaseDirectory;

        public frmWCFRESTful()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //SendString();
            SendObject();
            //SendDataTable();
        }


        private void SendString()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCost");

            HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(txtURL.Text)) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(txtPostData.Text);
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            try
            {


                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    string sResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    //JObject json = new JObject();
                    txtResult.Text = sResult;

                    JObject json = JObject.Parse(sResult);

                    DataRow dr = dt.NewRow();
                    foreach (JProperty property in json.Properties())
                    {

                        string Name = property.Name;
                        string Value = property.Value.ToString();

                        dr[Name] = Value;

                    }
                    dt.Rows.Add(dr);
                }

                dtTolistView(dt, listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCost");

            HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(txtURL.Text)) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(txtPostData.Text);
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            try
            {
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    string sResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var jsonLinq = JObject.Parse(sResult);
                    string jLinq = jsonLinq["Product"].ToString();
                    DataTable myTable = null;
                    myTable = XMLtoDT(jLinq);
                    dtTolistView(myTable, listView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendObject()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCost");

            HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(txtURL.Text)) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(txtPostData.Text);
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            try
            {
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    string sResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var jsonLinq = JObject.Parse(sResult);

                    string status = jsonLinq["Status"].ToString();
                    string message = jsonLinq["Message"].ToString();
                    string jLinq = jsonLinq["Product"].ToString();
                    txtResult.Text = string.Format("[응답결과] Status :{0}, Message :{1}, Product :{2}\r\n", status, message, jLinq.ToString());

                    DataTable myTable = null;
                    myTable = XMLtoDT(jLinq);
                    dtTolistView(myTable, listView1);

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmWCFRESTful_Load(object sender, EventArgs e)
        {
            txtURL.Text = sUrl;
            txtPostData.Text = sInput;
            SetListView();
        }

        private void SetListView()
        {
            listView1.View = View.Details; 
            listView1.GridLines = true; 
            listView1.FullRowSelect = true; 
            listView1.CheckBoxes = true;
            // listView1.LabelEdit = true; 

            ColumnHeader header1, header2;
            header1 = new ColumnHeader();
            header2 = new ColumnHeader();

            header1.Text = "V";
            header1.TextAlign = HorizontalAlignment.Center;
            header1.Width = 30;
            listView1.Columns.Add(header1);

            listView1.Columns.Add("ProductId", 200, HorizontalAlignment.Center);

            header2.Text = "ProductName";
            header2.TextAlign = HorizontalAlignment.Center;
            header2.Width = 200;
            listView1.Columns.Add(header2);

            //listView1.Columns.Add("ProductName", 200, HorizontalAlignment.Left); 
            listView1.Columns.Add("ProductCost", -2, HorizontalAlignment.Right);

            
            

            /* 
            // String[] aa = {"aa1", "aa2"}; 와 같이 일부 항목만 입력해도 ListView 에 표시되지만, 
            // 나중에 item.SubItems[index].Text 로 나중에 접근시 ArgumentOutOfRangeException 발생한다. 
            // 모두 입력하는게 좋다. 
            */
            //String[] aa = {"aa1", "aa2", "", "" }; 
            //ListViewItem newitem = new ListViewItem(aa); 
            //listView1.Items.Add(newitem); 
            //newitem = new ListViewItem(new String[] { "bb1", "bb2", "bb3", ""}); 
            //listView1.Items.Add(newitem);


        }

        /// <summary>
        /// DataTable to ListView
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lvw"></param>
        private void dtTolistView(DataTable dt, ListView lvw)
        { // 테이블 Row가 없을때 
            if (dt.Rows.Count <= 0)
            {
                lvw.Clear();
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ListViewItem lvwi = new ListViewItem();

                    //lvwi.Text = dr[0].ToString();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        lvwi.SubItems.Add(dr[i].ToString());
                    }

                    lvw.Items.Add(lvwi);
                }
            }
        }

        private void btnJsonInput_Click(object sender, EventArgs e)
        {


            string sResult = txtPostData.Text;

            JObject json = JObject.Parse(sResult);

            foreach (JProperty property in json.Properties())
            {
                if (property.Value.Type == JTokenType.Array) // Array일 경우
                {
                    List<string> listInProperty = JsonConvert.DeserializeObject<List<string>>(property.Value.ToString());
                    property.Value = string.Join(",", listInProperty.ToArray());

                    File.WriteAllText(path + @"\ProductClient.TXT", string.Format("Value : {0}\r\n", property.Value));
                }

            }

            Dictionary<string, string> location = new Dictionary<string, string>
            {
                {"ProductId", "2"},
                {"ProductName", "SAMSUNG NOTE"},
                {"ProductCost", "1000 $"}
            };
            string jcon = JsonConvert.SerializeObject(location, Formatting.Indented);
            JObject job = new JObject();
            job.Add("product", jcon);
            //ArrayList converted = new ArrayList() { @"""ProductCost"":""1000 $""",@"""ProductId"":""1""",@"""ProductName"":""Laptop""" };
            //string jcon = JsonConvert.SerializeObject(converted, Formatting.Indented);

            File.AppendAllText(path + @"\ProductClient.TXT", string.Format("jcon : {0}\r\n", job.ToString()));
            txtPostData.Text = job.ToString();
        }


        private string SetDataTable()
        {
            DataSet dataSet = new DataSet("dataSet");
            dataSet.Namespace = "NetFrameWork";
            DataTable table = new DataTable("Product");
            DataColumn IdColumn = new DataColumn("ProductId");
            //IdColumn.AutoIncrement = true;

            DataColumn ProductNameCol = new DataColumn("ProductName");
            DataColumn ProductCostCol = new DataColumn("ProductCost");
            table.Columns.Add(IdColumn);
            table.Columns.Add(ProductNameCol);
            table.Columns.Add(ProductCostCol);

            dataSet.Tables.Add(table);

            for (int i = 0; i < 5; i++)
            {
                DataRow newRow = table.NewRow();
                newRow["ProductId"] = i.ToString();
                newRow["ProductName"] = "Kor item" + i;
                newRow["ProductCost"] = (10000 * i).ToString();
                table.Rows.Add(newRow);
            }

            dataSet.AcceptChanges();

            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            File.AppendAllText(path + @"\ProductClient.TXT", string.Format("[DataTable json DataTable]\r\n {0}\r\n", json.ToString()));
            JObject job = new JObject();
            job.Add("product", json);

            return job.ToString();
        }

        /// <summary>
        /// DataTable Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataSet_Click(object sender, EventArgs e)
        {

            txtURL.Text = @"http://localhost:12419/ProductService.svc/GetProductDT";
            txtPostData.Text = SetDataTable();
        }

        private void btnDataTableLoad_Click(object sender, EventArgs e)
        {
            string jsonText = txtResult.Text;
            var dataSet = JsonConvert.DeserializeObject<DataSet>(jsonText);
            var table = dataSet.Tables[0];
            listView1.Clear();
            SetListView();
            dtTolistView(table, listView1);
        }

        public static DataTable Tabulate(string jsonContent)
        {
            var jsonLinq = JObject.Parse(jsonContent);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        public static DataTable XMLtoDT(string xmlData)
        {
            StringReader theReader = new StringReader(xmlData);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);

            return theDataSet.Tables[0];
        }

    }

    public class MyTableUtilClass
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public DataTable Product { get; set; }
    }


    
}
