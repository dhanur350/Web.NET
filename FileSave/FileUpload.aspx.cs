using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;

namespace FileSave
{
    public partial class FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        void loaddata()  //Display data with Link to open it 
        {
            var st = from s in db.saves select s;  // Taking Linq to SQL to add Data in Save table name
            Gridvew.DataSource = st;  //Datasource Variable 
            Gridvew.DataBind();   //It'll permanently bind and write the data in SQL but cannot be inherited 
        }
        DataClasDataContext db = new DataClasDataContext();  //Making an object of DBML Linq to SQL for creating enteries in Database
        protected void btnUpload_Click(object sender, EventArgs e)  //Upload event to upload files to server
        {
            if (Fileuplod.HasFile)    //Condition Check If Fileupload having file selected then
            {
                string fname = Fileuplod.FileName; //Getes the name of the file using user inputted file upload
                var fileloc = "Uploads/";  //giving the program Server location to where those files should go during upload
                string pathstring = System.IO.Path.Combine(fileloc, fname); //Making pathstring to combine 
                var st = new save              //Implicitly typed local variables are strongly typed just as if you had declared the type yourself
                {                               //but the compiler determines the type at run time depending on the value stored in them.
                    FileName = txtbox.Text,    //Fetching data and assigning data to SQL Rows
                    FileLocation = pathstring,
                };
                db.saves.InsertOnSubmit(st);    //This line will insert data on SQL Server through Linq connection via .dbml file
                db.SubmitChanges();
                Fileuplod.SaveAs(Server.MapPath(pathstring));  //Telling code to save file on given server Mappath
                lblMessage.Text = "Your File is uploaded";   //Label Message to code Successful
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.BackColor = System.Drawing.Color.YellowGreen;
            }
            else
            {
                lblMessage.Text = "Your File is not uploaded , please check to select a file";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void Linkbtn_Click(object sender,EventArgs e)
        {
            int rowindex = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;
            string filelocation = Gridvew.Rows[rowindex].Cells[3].Text;
            string filepath = Server.MapPath("~/"+filelocation);
            WebClient web = new WebClient();
            Byte[] FileBuffer = web.DownloadData(filepath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("cotent-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
            try
            {
                File.ReadAllText("non-existing.file");
            }
            catch (FileNotFoundException l)
            {
                Console.WriteLine(l.ToString());
            }
        }
        protected void Gridvew_SelectedIndexChanged(object sender, EventArgs e) 
        {

        }

        protected void Display_Click(object sender, EventArgs e)
        {
            loaddata();
        }

        protected void btnopenfolder_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\FileSave\FileSave\Uploads");
        }
    }
}
//if(Fileuplod.HasFile)
//{ 
//    Fileuplod.SaveAs(Server.MapPath("~/Uploads/" + Fileuplod.FileName));
//    lblMessage.Text = "Your File is uploaded";
//    
//}
//else
//{
//    
//}