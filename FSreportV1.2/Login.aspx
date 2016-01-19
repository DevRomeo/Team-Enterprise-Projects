<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="main.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="Image/application.ico"/>
    <title>Login Page</title>
    <style type="text/css">
        .style3
        {
            width: 50px;
        }
         .style1
        {
            width: 108px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="IR_Header_Main_Menu" class ="header">
    <div class="head_interior-left_column">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/image/ml.jpg" />
    </div>
    <br />
    <br />
    <br />
    <div style="text-align: right; font-family: Arial; font-size: small;">
        Executive Summary Report Web v1.2&nbsp;&nbsp;&nbsp;&nbsp;
    </div>
    <br />
    </div>
   <div>
       <br style="clear: both" />
   </div>
    <div align="center" style="height:317px; clear: both; margin-top: 60px;">
        <table style="border: 1px solid #268CCD; padding: 1px 4px; width: 30%;" 
            bgcolor="#E9E9E9">
            <tr>
                <td style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right" colspan="2">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: right" class="style3">
                    &nbsp;</td>
                <td style="text-align: right" class="style1">
                    User Name :</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtUserName" runat="server" 
                        style="font-family: Arial; font-size: 12px" CssClass="txtUppercase"></asp:TextBox>
                </td>
                <td style="text-align: left" class="style3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    Password :</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtPassword" runat="server" 
                        style="font-family: Arial; font-size: 12px" TextMode="Password"></asp:TextBox>
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" />
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3" style="text-align: right">
                    &nbsp;</td>
                <td style="text-align: center" colspan="2" rowspan="2">
                    <asp:Label ID="txtInfo" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: center">
                    &nbsp;</td>
                <td style="text-align: center">
                    &nbsp;</td>
            </tr>
        </table>
        
    </div>
    
   <br style="clear: both" />
     <div id="footer"><div class="footer-inner">
    <br clear="all" />
    <hr width="100%"  noshade="noshade" style="border-top:1px solid #FFFFFF; border-bottom: none; margin-bottom:5px;" />
    <div style="text-align:center; font-size: x-small;">M. Lhuillier Financial Services Inc. 2010 </div>
    </div><br clear="all" /></div>
    </form>
</body>
</html>
