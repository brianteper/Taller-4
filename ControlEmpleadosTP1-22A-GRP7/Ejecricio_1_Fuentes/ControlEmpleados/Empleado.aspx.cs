using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Contexts;

public partial class Empleado : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //Context contexto = new Context();
    /*
    protected void btnEnviar_Click(object sender, EventArgs e)
    {
    */
        //Se agrega en el contexto dos items, uno con clave "Valor1" y otro con "Valor2".
        //Estas claves serán utilizadas en la siguiente página para recuperar los valores.
        Context.Items.Add("LegajoNum", TextBox1.Text);
        Context.Items.Add("Apellido", TextBox2.Text);
        Context.Items.Add("Nombre", TextBox3.Text);


       //Se transfiere la ejecución a la siguiente página.
       //Server.Transfer("Ejemplo01ContextFin.aspx");
       //Con esto se redirige a la pagina EmpleadoOk.aspx
       Server.Transfer("EmpleadoOk.aspx");
    }
}