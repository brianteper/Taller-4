<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmpleadoBusqueda.aspx.cs" Inherits="EmpleadoBusqueda" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td style="width: 104px">
                <asp:Label ID="Label1" runat="server" Text="Legajo"></asp:Label>
            </td>
            <td style="width: 351px">
                <asp:TextBox ID="TextBox1" runat="server" Width="338px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 104px; height: 26px">
                <asp:Label ID="Label2" runat="server" Text="Apellido"></asp:Label>
            </td>
            <td style="width: 351px; height: 26px">
                <asp:TextBox ID="TextBox2" runat="server" Width="338px"></asp:TextBox>
            </td>
            <td style="height: 26px">
            </td>
        </tr>
        <tr>
            <td style="width: 104px; height: 26px">
                <asp:Label ID="Label3" runat="server" Text="Nombre"></asp:Label>
            </td>
            <td style="height: 26px; width: 351px">
                <asp:TextBox ID="TextBox3" runat="server" Width="338px"></asp:TextBox>
            </td>
            <td style="height: 26px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 104px; height: 26px">
                &nbsp;</td>
            <td style="height: 26px; width: 351px">
                <asp:Button ID="Button1" runat="server" Text="Buscar" />
            </td>
            <td style="height: 26px">
                <asp:Button ID="Button2" runat="server" Text="Nuevo Empleado" />
            </td>
        </tr>
        <tr>
            <td style="width: 104px; height: 26px">
                &nbsp;</td>
            <td style="height: 26px; width: 351px">
&nbsp;<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="294px">
                    <Columns>
                        <asp:BoundField HeaderText="Legajo" />
                        <asp:BoundField HeaderText="Apellido" />
                        <asp:BoundField HeaderText="Nombre" />
                        <asp:BoundField HeaderText="Teléfono" />
                        <asp:CommandField ShowEditButton="True" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="height: 26px">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

