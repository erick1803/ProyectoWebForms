<%@ Page Title="" Language="C#" MasterPageFile="~/PageMaster/PageMaster.Master" AutoEventWireup="true" CodeBehind="WebFormCommonUser.aspx.cs" Inherits="ProyectoWebForms.WebFormCommonUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:Label ID="Label9" runat="server" Text="Vaccine"></asp:Label>
            <asp:DropDownList ID="dlistVaccine" runat="server" Height="20px" Width="144px" DataSourceID="SqlVaccine" DataTextField="name_vaccine" DataValueField="id_vaccine">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlVaccine" runat="server" ConnectionString="Data Source=DESKTOP-SVARKC4\SQLEXPRESS;Initial Catalog = ProyectoWebForms; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" SelectCommand="SELECT id_vaccine, name_vaccine
FROM Vaccine"></asp:SqlDataSource>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Available appointments"></asp:Label>
            <br />
            <asp:Label ID="Label10" runat="server" Text="Hour"></asp:Label>
            <asp:DropDownList ID="cmbHourFree" runat="server" >
            </asp:DropDownList>
            <asp:Button ID="btnApplyGrid" runat="server" Text="Apply" OnClick="btnApplyGrid_Click" />
            <br />
            <asp:GridView ID="GridView1" class="table table-bordered table-condensed table-responsive table-hover " runat="server" AutoGenerateColumns="false" Width="400px">
                <Columns>
            <asp:BoundField DataField="appointment_time" HeaderText="Hora de Cita" />
            <asp:BoundField DataField="full_name" HeaderText="Nombre Completo"/>
            <asp:BoundField DataField="nit" HeaderText="Nit"/>
            </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlGridDates" runat="server" ConnectionString="Data Source=DESKTOP-SVARKC4\SQLEXPRESS;Initial Catalog = ProyectoWebForms; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" SelectCommand="  SELECT A.appointment_time , CONCAT(P.name, ' ', P.last_name, ' ', P.second_last_name), P.nit
  FROM Appointment A 
  INNER JOIN Person P ON P.id_person = A.id_person
  WHERE appointment_time BETWEEN @start AND @end;">
                <SelectParameters>
                    <asp:Parameter Name="start" />
                    <asp:Parameter Name="end" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Appointment hour:"></asp:Label>
            <asp:DropDownList ID="cmbHour" runat="server" Height="16px" Width="151px">
            </asp:DropDownList>
&nbsp;
            <asp:Label ID="Label8" runat="server" Text="Minutes: "></asp:Label>
            <asp:DropDownList ID="cmbMinutes" runat="server" Height="20px" Width="144px">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=DESKTOP-SVARKC4\SQLEXPRESS;Initial Catalog = ProyectoWebForms; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" SelectCommand="  SELECT DATEPART(HOUR, start_time), DATEPART(HOUR, end_time), time_administer
  FROM Vaccine
  WHERE id_vaccine = @id;">
                <SelectParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:Label ID="lblDate" runat="server" Text="Date: "></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="Name:"></asp:Label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" Text="Last name:"></asp:Label>
            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label5" runat="server" Text="Second last name:"></asp:Label>
            <asp:TextBox ID="txtSecondLastName" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Text="Nit:"></asp:Label>
            <asp:TextBox ID="txtNit" runat="server" Width="197px"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label7" runat="server" Text="Mail:"></asp:Label>
            <asp:TextBox ID="txtMail" runat="server" Width="192px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="bt1" runat="server" OnClick="bt1_Click" Text="Register" Width="232px" />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <br />
        </div>
</asp:Content>
