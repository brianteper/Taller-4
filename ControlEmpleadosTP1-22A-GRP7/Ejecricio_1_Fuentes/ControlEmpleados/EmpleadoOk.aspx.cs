using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmpleadoOk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(Context.Items["LegajoNum"]);
        Response.Write(Context.Items["Apellido"]);
        Response.Write(Context.Items["Nombre"]);
    }
}