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
    }
}
