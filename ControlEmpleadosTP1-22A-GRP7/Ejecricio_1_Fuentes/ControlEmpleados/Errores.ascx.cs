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

public partial class Errores : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Se agrega un error a la tabla de errores.
    /// </summary>
    /// <param name="ex">Objeto exception.</param>
    public void Agregar(Exception ex)
    {
        // Se le agrega una fila a la tabla.
        int tr = tblErrores.Rows.Add(new TableRow());
        // Se le agrega una celda a la fila.
        int cell = tblErrores.Rows[tr].Cells.Add(new TableCell());
        // Se le asgina el texto del mensaje a la celda.
        tblErrores.Rows[tr].Cells[cell].Text = ex.Message;
    }
}
