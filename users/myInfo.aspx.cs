﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DDbook
{
    public partial class myInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                GetData();
                DB db = new DB();
                db.LoadExecuteData("select * from Customer where Id = " + Session["USERID"]);
                TextBox1.Text = db.GetValue("LoginName");
                TextBox2.Text = db.GetValue("Realname");
                TextBox3.Text = db.GetValue("LinkTel");
                TextBox7.Text = db.GetValue("PostCode");
                TextBox4.Text = db.GetValue("Password");
                TextBox5.Text = db.GetValue("Password");
                TextBox1.Enabled = TextBox2.Enabled = TextBox3.Enabled= TextBox7.Enabled= TextBox4.Enabled= TextBox5.Enabled= Button1.Enabled = FileUpload1.Enabled = false;
                db.OffData();
            }
        }


        protected void GetData()
        {
            DB db1 = new DB();
            string sql1 = "select  * from Address where CustomerID=" + Session["USERID"];
            db1.LoadExecuteData(sql1, "Address");
            addressDL.DataSource = db1.MyDataSet.Tables[0].DefaultView;
            addressDL.DataKeyField = "Id";
            addressDL.DataBind();
            db1.OffData();
        }

        // 保存修改
        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = "";
            DB db = new DB();
            string newPwd = db.MD5Hash(TextBox5.Text.Trim());
            if (FileUpload1.HasFile)
            {
                //指定上传文件在服务器上的保存路径
                string savePath = Server.MapPath("~/images/img/");
                if (!System.IO.Directory.Exists(savePath))
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }
                //获取上传名字
                string filename1 = FileUpload1.FileName;
                //返回指定图片的后缀
                string fileext1 = System.IO.Path.GetExtension(filename1);
                //获取当前时间
                string filenamaes1 = DateTime.Now.ToString("yyyyMMddHHffss") + fileext1;
                //上传到此项目根目录的images/img/文件夹
                string savePath1 = savePath + "\\" + filenamaes1;
                FileUpload1.SaveAs(savePath1);//保存展示图
                sql = "update Customer set Realname = '" + TextBox2.Text.Trim() + "',LoginName='" + TextBox1.Text.Trim() + "',LinkTel = '" + TextBox3.Text.Trim() + "', Password='" + newPwd + "',Pic='" + filenamaes1 + "' where Id = " + Session["USERID"];

            }
            else
            {   //没上传头像则不更改
                sql = "update Customer set Realname = '" + TextBox2.Text.Trim() + "',LoginName='" + TextBox1.Text.Trim() + "',LinkTel = '" + TextBox3.Text.Trim() + "', Password='" + newPwd + "' where Id = " + Session["USERID"];
            }
            db.ExecuteNonQuery(sql);
            Response.Write("<script>alert('修改成功！')</script>");
            TextBox1.Enabled = TextBox2.Enabled = TextBox3.Enabled = TextBox7.Enabled = TextBox4.Enabled = TextBox5.Enabled = Button1.Enabled = FileUpload1.Enabled = false;
            db.OffData();
        }

        // 添加地址
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.ExecuteNonQuery("insert into Address values('" + Session["USERID"] + "','" + TextBox6.Text + "','" + TextBox8.Text + "','" + TextBox9.Text + "','" + TextBox10.Text + "')");
            db.OffData();
            Response.Write("<script>alert('添加成功！');</script>");
            GetData();
        }

        protected void addressDL_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            DB db = new DB();
            db.ExecuteNonQuery("delete from Address where Id = " + id);
            db.OffData();
            GetData();
        }

        protected void addressDL_EditCommand(object source, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            addressDL.EditItemIndex = e.Item.ItemIndex;
            GetData();
        }

        protected void addressDL_CancelCommand(object source, DataListCommandEventArgs e)
        {
            addressDL.EditItemIndex = -1;
            GetData();
        }

        protected void addressDL_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            string address = (e.Item.FindControl("two") as TextBox).Text.Trim();
            string username = (e.Item.FindControl("one") as TextBox).Text.Trim();
            string phone = (e.Item.FindControl("three") as TextBox).Text.Trim();
            string postcode = (e.Item.FindControl("four") as TextBox).Text.Trim();
            string sql = "update Address set AddressName = '"+ address+"',CustomerName = '"+ username +"',Phone='"+phone+"',PostCode='"+postcode+"' where Id = " + id;
            DB db = new DB();
            db.ExecuteNonQuery(sql);
            addressDL.EditItemIndex = -1;
            GetData();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TextBox1.Enabled = TextBox2.Enabled = TextBox3.Enabled = TextBox7.Enabled = TextBox4.Enabled = TextBox5.Enabled = Button1.Enabled = FileUpload1.Enabled = true;
        }
    }
}