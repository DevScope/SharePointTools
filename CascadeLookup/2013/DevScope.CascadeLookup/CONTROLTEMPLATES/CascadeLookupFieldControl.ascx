<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" %>
<SharePoint:RenderingTemplate ID="CascadeLookupFieldEditor" runat="server">
    <Template>
        <asp:DropDownList ID="ddlCascade" runat="server" />
        <asp:HiddenField ID="hfCascade" runat="server" />
        <asp:Button ID="btnCascade" runat="server" Style="visibility: hidden;" />
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="CascadeLookupFieldClientEditor" runat="server">
    <Template>
        <asp:Panel ID="pnlContent" runat="server" CssClass="dvclCascadeDropdown">
            <select></select>
            <asp:HiddenField ID="hfCascade" runat="server" />
        </asp:Panel>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="CascadeLookupFieldDisplay" runat="server">
    <Template>
        <asp:HyperLink ID="hypCascade" runat="server" />
    </Template>
</SharePoint:RenderingTemplate>
