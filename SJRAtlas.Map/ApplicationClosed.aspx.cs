using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ApplicationClosed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void TitleMenu_DataBound(object sender, EventArgs e)
    {
        Menu menu = sender as Menu;
        if (menu != null)
        {
            for (int i = 0; i < menu.Items.Count - 1; i++)
            {
                menu.Items[i].SeparatorImageUrl = "~/images/separator.gif";
            }
        }
    }
}
