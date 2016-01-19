<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Profile.aspx.vb" Inherits="Profile" title="Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style14
        {
            width: 201px;
        }
        .style15
        {
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align="left" style="margin-left: 20%">
</div>
<br />
<br />
<div style="margin-right: 20%; margin-left: 20%">

<table width="100%">
<tr>
<td>
    &nbsp;</td>
    <td align="left">
    <asp:Label ID="Label8" runat="server" style="font-size: 30px; font-weight: 700" 
        Text="Profile"></asp:Label>
    </td>
</tr>
<tr>
<td>
    &nbsp;</td>
    <td>
    &nbsp;</td>
</tr>
    <tr>
     <td align="right" class="style15">
         <asp:Label ID="Label3" runat="server" Font-Size="Medium" Text="Username:" 
             style="font-size: 12px"></asp:Label>
         </td>
     <td align="left">
         <asp:TextBox ID="txtUname" runat="server" CssClass="txtUppercase" 
             ReadOnly="True" style="font-family: Arial; font-size: 12px"></asp:TextBox>
         </td>
     </tr>
     <tr>
     <td align="right" class="style15">
         <asp:Label ID="Label4" runat="server" Font-Size="Medium" Text="New Password:" 
             style="font-size: 12px"></asp:Label>
         </td>
     <td align="left">
         <asp:TextBox ID="txtPass" runat="server" Enabled="False" TextMode="Password" 
             style="font-family: Arial; font-size: 12px" MaxLength="20"></asp:TextBox>
         &nbsp;</td>
     </tr>
     <tr>
     <td align="right" class="style15">
         <asp:Label ID="Label5" runat="server" Font-Size="Medium" 
             Text="Confirm Password:" style="font-size: 12px"></asp:Label>
         </td>
     <td align="left">
         <asp:TextBox ID="txtConfirmPass" runat="server" Enabled="False" 
             TextMode="Password" style="font-family: Arial; font-size: 12px" 
             MaxLength="64"></asp:TextBox>
         </td>
     </tr>
     <tr>
     <td align="right" class="style15">
         <asp:Label ID="Label6" runat="server" Font-Size="Medium" Text="Fullname:" 
             style="font-size: 12px"></asp:Label>
         </td>
     <td align="left">
         <asp:TextBox ID="txtFname" runat="server" Enabled="False" Width="200px" 
             style="font-family: Arial; font-size: 12px"></asp:TextBox>
         </td>
     </tr>
     <tr>
     <td align="right" class="style15">
         <asp:Label ID="Label7" runat="server" Font-Size="Medium" 
             Text="Job Description:" style="font-size: 12px"></asp:Label>
         </td>
     <td align="left">
         <asp:TextBox ID="txtTask" runat="server" Enabled="False" 
             style="font-family: Arial; font-size: 12px" MaxLength="90"></asp:TextBox>
         </td>
     </tr>
     </table>
     
      <br />
     <div style="margin-right: 5%; margin-left: 5%;" align="left">
         <asp:Label ID="lblmsg" runat="server" style="font-size: 12px" ForeColor="Red"></asp:Label>
     </div>
     <br />
     <div align="left">
     
         <asp:Button ID="Button1" runat="server" Text="Save" Width="70px" 
             Enabled="False" /> &nbsp;
         <asp:Button ID="Button2" runat="server" Text="Edit" Width="70px" /> &nbsp;
         <asp:Button ID="Button3" runat="server" Text="Cancel" Width="70px" />
     
    </div>

</div>
</asp:Content>

