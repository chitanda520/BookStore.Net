﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DDbook.admin
{
    public partial class orderManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Adminame"] == null)
            {
                Response.Write("<script>alert('您还没登录，请先登录');window.location='/users/login.aspx';</script>");
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "Btn_Operation")
            {
                string id = e.CommandArgument.ToString();
                string sql = "update Torder set IsSend = 1 where Id=" + id;
                DB db = new DB();
                db.ExecuteNonQuery(sql);
                GridView2.DataBind();
                db.OffData();
            }
        }
    }
}