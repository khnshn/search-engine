using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SearchEngine
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TextBoxQuery.Text))
            {
                return;
            }
            Response.Redirect("/Results.aspx?q=" + TextBoxQuery.Text);
        }
    }
}