<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Empleado.aspx.cs" Inherits="Empleado" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label1" runat="server" Text="Número de Legajo:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBox1" 
                    ErrorMessage="El campo Número de Legajo es OBLIGATORIO"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label2" runat="server" Text="Apellido:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="TextBox2" 
                    ErrorMessage="El campo Apellido es OBLIGATORIO"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label3" runat="server" Text="Nombre:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="TextBox3" ErrorMessage="El campo Nombre es OBLIGATORIO"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label4" runat="server" Text="Teléfono:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="TextBox4" 
                    ErrorMessage="El campo Teléfono es OBLIGATORIO"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px; height: 26px;">
                <asp:Label ID="Label5" runat="server" Text="E-Mail:"></asp:Label>
            </td>
            <td style="height: 26px">
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="TextBox5" ErrorMessage="E-Mail Invalido" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label6" runat="server" Text="Fecha de Nacimiento:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="TextBox6" ErrorMessage="Fecha Invalida" 
                    Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                <asp:Label ID="Label7" runat="server" Text="Sueldo por Hora:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="TextBox7" ErrorMessage="El campo Sueldo es OBLIGATORIO"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                    ControlToValidate="TextBox7" 
                    ErrorMessage="El Sueldo debe ser Mayor o Igual a 0" MaximumValue="99999999999" 
                    MinimumValue="0" Type="Double"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;</td>
            <td>
    <asp:Button ID="Button3" runat="server" style="margin-left: 184px" Text="Aceptar" 
                    oWidth="86px" onclick="Button3_Click" />
    <asp:Button ID="Button4" runat="server" style="margin-left: 0px" Text="Cancelar" 
                    Width="86px" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

