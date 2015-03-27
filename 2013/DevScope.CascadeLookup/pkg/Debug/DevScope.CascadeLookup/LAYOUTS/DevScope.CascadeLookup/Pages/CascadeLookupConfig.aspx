<%@ Assembly Name="DevScope.CascadeLookup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=856e47121577668e" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CascadeLookupConfig.aspx.cs" Inherits="DevScope.CascadeLookup.Layouts.Pages.CascadeLookupConfig" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/_layouts/15/DevScope.CascadeLookup/Css/cascadeLookup.min.css" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Sharepoint:ScriptLink ID="ScriptLink1" Name="sp.ui.dialog.js" LoadAfterUI="true" Localizable="false" runat="server"></Sharepoint:ScriptLink>   
    
    <div class="cascadeLookupConfig">
        <h1>
            <asp:Literal ID="ltrTitle" runat="server" /></h1>
        <ul>
            <li>
                <label>
                    <asp:Literal ID="ltrCascadModeLabel" runat="server" /></label>
                <asp:RadioButtonList ID="rbCascadeMode" runat="server" />
            </li>
        </ul>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Cascade Lookup Config
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Cascade Lookup Config
</asp:Content>
