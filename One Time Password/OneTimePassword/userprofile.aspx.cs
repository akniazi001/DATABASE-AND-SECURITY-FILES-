using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OneTimePassword
{
    public partial class userprofile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] ==null)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
        }
    }

}