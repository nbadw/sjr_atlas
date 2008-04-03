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

public partial class ErrorPage : System.Web.UI.Page
{
    // Before deploying application, set showTrace to false
    // to prevent web application users from seeing error details
    private bool showTrace = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        //get error message stored in session
        string message = (string)Session["ErrorMessage"];

        //get details of error from exception stored in session
        string errorDetail = String.Empty;
        Exception exception = Session["Error"] as Exception;
        if (exception != null)
        {
            switch (exception.GetType().ToString())
            {
                case "System.UnauthorizedAccessException":
                    UnauthorizedAccessException errorAccess = exception as UnauthorizedAccessException;
                    if (errorAccess.StackTrace.ToUpper().IndexOf("SERVERCONNECTION.CONNECT") > 0)
                        errorDetail = "Unable to connect to server. <br>";
                    break;
            }
            errorDetail += exception.Message;
        }

        //create response and display it
        string response;
        if (message != null && message != String.Empty)
            response = String.Format("{0}<br>{1}", message, errorDetail);
        else
            response = errorDetail;
        lblError.Text = response;
        if ((showTrace) && (exception != null))
            lblExtendedMessage.Text = exception.StackTrace;

    }
}
