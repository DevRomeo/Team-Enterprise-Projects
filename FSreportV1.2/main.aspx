<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="main.aspx.vb" Inherits="main" Title="Main Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row" style="margin-top: 20px;">
            <div class="col-sm-4 col-md-4 col-md-offset-3">
                <div class="input-group">
                    <span class="input-group-addon">ReportType</span>
                    <asp:DropDownList ID="DrpReportType" runat="server" class="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-sm-1 col-md-1">
                <div class="input-group">
                    <asp:CheckBox ID="CheckBox1" runat="server" CssClass="style14" 
                        AutoPostBack="True" />
                    <span class="input-group-addon">All</span>
                </div>
            </div>
        </div>
        <div class="row" style="margin-top: 15px;">
            <div class="col-sm-2 col-md-2 col-md-offset-3">
                <div class="input-group">
                    <span class="input-group-addon">Date</span>
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="10" BorderWidth="1px" AutoPostBack="True"
                        CssClass="style13">
                    </asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="TextBox1_TextBoxWatermarkExtender" runat="server"
                        Enabled="True" TargetControlID="TextBox1" WatermarkCssClass="watermarked" WatermarkText="mm/dd/yyyy">
                    </asp:TextBoxWatermarkExtender>
                    <asp:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox1" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" DaysModeTitleFormat="MMMM,yyyy"
                        Enabled="True" Format="MM/dd/yyyy" TargetControlID="TextBox1">
                    </asp:CalendarExtender>
                </div>
            </div>
            <div class="col-sm-2 col-md-2">
                <div class="input-group">
                    <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-default form-control" />
                </div>
            </div>
        </div>
        <div class="row" style="margin-top: 15px;">
            <div class="col-sm-6 col-md-6 col-md-offset-2">
                <div style="background-color: #E4E6E7; height: 260px; width: 650px; margin-top: 20px;
                    overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" EmptyDataText="NO DATA FOUND" BackColor="White" 
                        BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px">
                        <RowStyle BackColor="White" ForeColor="#003399" />
                        <Columns>
                            <asp:BoundField DataField="pdfFileName" HeaderText="FILENAME">
                                <HeaderStyle Font-Bold="True" Font-Size="16pt" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="600px" />
                            </asp:BoundField>
                            <asp:CommandField ButtonType="Button" SelectText="download" ShowSelectButton="True" />
                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="row" style="margin-top: 5px;">
            <div class="col-sm-6 col-md-6 col-md-offset-2" align="center">
                <asp:Label ID="lblNodata" runat="server" Text="" Font-Bold="true" Font-Size="Medium">
                </asp:Label>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style13
        {
            display: block;
            width: 100%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            left: -14px;
            top: -31px;
        }
        .style14
        {
            display: block;
            width: 100%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            left: -299px;
            top: 233px;
        }
    </style>
</asp:Content>
