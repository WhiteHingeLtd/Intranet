<%@ Page Title="Network Status" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Status.aspx.vb" Inherits="WHLStatus.Status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Timer ID="UpdateTimer" runat="server" Interval="5000" />
    <asp:UpdatePanel ID="StatusPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="JumbrotronBG" runat="server" CssClass="jumbotron">

                <h1><asp:Label ID="JumboStatus" runat="server" Text="ASP.NET"></asp:Label></h1>
                <p class="lead"><asp:Label ID="JumboSubtext" runat="server" Text="You can see some more text here."></asp:Label></p>
				<h1><asp:Label ID="TimeLabel" runat="server" Text="Label"></asp:Label></h1>

            </asp:Panel>
            <div class="row">
                
                <div class="col-md-4">
                    <h2>External Services</h2>
                    <p><asp:Panel ID="ExternalPanel" runat="server"></asp:Panel></p>
                </div>
                <div class="col-md-4">
                    <h2>Servers</h2>
                    <p><asp:Panel ID="ServersPanel" runat="server"></asp:Panel></p>
                </div>
                <div class="col-md-4">
                    <h2>Internal Services</h2>
                    <p><asp:Panel ID="InternalPanel" runat="server"></asp:Panel></p>
                </div>
            </div>
            
        </ContentTemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
        </triggers>
     </asp:UpdatePanel>
</asp:Content>
