<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CascadeLookupFieldEditor.ascx.cs" Inherits="DevScope.CascadeLookup.CONTROLTEMPLATES.CascadeLookupFieldEditor" %>
<table class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">
    <tr>
        <td class="ms-authoringcontrols" colspan="2">
            <asp:Literal ID="ltrListLabel" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <img src="/_layouts/15/images/blank.gif" width="1" height="3" style="display: block" alt=""></td>
    </tr>
    <tr>
        <td width="11px">
            <img src="/_layouts/15/images/blank.gif" width="11" height="1" style="display: block" alt="" /></td>
        <td class="ms-authoringcontrols" width="99%">
            <asp:DropDownList ID="ddlCascadeList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCascadeList_SelectedChanged" /></td>
    </tr>
    <tr>
        <td class="ms-authoringcontrols" colspan="2">
            <asp:Literal ID="ltrColumnLabel" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <img src="/_layouts/15/images/blank.gif" width="1" height="3" style="display: block" alt=""></td>
    </tr>
    <tr>
        <td width="11px">
            <img src="/_layouts/15/images/blank.gif" width="11" height="1" style="display: block" alt="" /></td>
        <td class="ms-authoringcontrols" width="99%">
            <asp:DropDownList ID="ddlCascadeListColumn" runat="server" /></td>
    </tr>
    <tr>
        <td class="ms-authoringcontrols" colspan="2">
            <asp:Literal ID="ltrDependenciesLabel" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <img src="/_layouts/15/images/blank.gif" width="1" height="3" style="display: block" alt=""></td>
    </tr>
    <tr>
        <td width="11px">
            <img src="/_layouts/15/images/blank.gif" width="11" height="1" style="display: block" alt="" /></td>
        <td class="ms-authoringcontrols" width="99%">
            <asp:CheckBox ID="chkCascadeDependencies" runat="server" AutoPostBack="true" OnCheckedChanged="chkCascadeDependencies_CheckedChange" /></td>
    </tr>
</table>
<table id="tblDependencies" runat="server" visible="false" class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">
    <tr>
        <td class="ms-authoringcontrols" colspan="2">
            <asp:Literal ID="ltrDependencyColumnLabel" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <img src="/_layouts/15/images/blank.gif" width="1" height="3" style="display: block" alt=""></td>
    </tr>
    <tr>
        <td width="11px">
            <img src="/_layouts/15/images/blank.gif" width="11" height="1" style="display: block" alt="" /></td>
        <td class="ms-authoringcontrols" width="99%">
            <asp:DropDownList ID="ddlCascadeDependencyColumn" runat="server" /></td>
    </tr>
    <tr>
        <td class="ms-authoringcontrols" colspan="2">
            <asp:Literal ID="ltrDependencyListColumnLabel" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <img src="/_layouts/15/images/blank.gif" width="1" height="3" style="display: block" alt=""></td>
    </tr>
    <tr>
        <td width="11px">
            <img src="/_layouts/15/images/blank.gif" width="11" height="1" style="display: block" alt="" /></td>
        <td class="ms-authoringcontrols" width="99%">
            <asp:DropDownList ID="ddlCascadeDependencyListColumn" runat="server" /></td>
    </tr>
</table>
<asp:Label ID="lblError" runat="server" Visible="false" />

