<%@ Page Title="Test Inventory Control Window" Language="vb" AutoEventWireup="false" MasterPageFile="~/FullScreenApp.Master" CodeBehind="InvControl.aspx.vb" Inherits="WHLStatus.InvControl" %>
<%@ Register assembly="WHLStatus" namespace="WHLStatus" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Titlebar" runat="server">
    
    </asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainWindow" runat="server">
    
        
    <!-- BUTTONS -->
    <style type="text/css">
        #SearchButton{background-position:0px -84px;}

    </style>
        <div id="IC_Sidebar" class="RibbonExpanded">
            <asp:TreeView ID="TreeView1" runat="server">
                <Nodes>
                    <asp:TreeNode Text="All Items" Value="All Items">
                        <asp:TreeNode Text="With Children" Value="With Children"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="Status" Value="Status">
                        <asp:TreeNode Text="Active" Value="Active"></asp:TreeNode>
                        <asp:TreeNode Text="Inactive" Value="Inactive"></asp:TreeNode>
                        <asp:TreeNode Text="Entered" Value="Entered"></asp:TreeNode>
                        <asp:TreeNode Text="Priced" Value="Priced"></asp:TreeNode>
                    </asp:TreeNode>
                </Nodes>
            </asp:TreeView>
    </div>
        <div id="IC_Explorer" class="RibbonExpanded">
            <asp:Panel ID="SkuCollViewer" runat="server">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="SkuGrid" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="colSku" HeaderText="Sku"></asp:BoundField>
                        <asp:BoundField DataField="colTitle" FooterText="asd" HeaderText="Title"></asp:BoundField>
                        <asp:BoundField DataField="colRetail" DataFormatString="{0:C2}" HeaderText="Retail"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </asp:Panel>
        </div>
        <div id="IC_Ribbon" class="RibbonExpanded">
            <div id="RibbonToggle" onclick="ToggleRibbon()" class="RibbonExpanded"></div>

            <div id="tabtitles">
                <div id="tab_home" class="tabTitle tab_home" onclick="SetActiveTab('tab_home')">Home</div>
                <div id="tab_two" class="tabTitle tab_two" onclick="SetActiveTab('tab_two')">Tab 2</div>
                <div id="tab_three" class="tabTitle tab_three" onclick="SetActiveTab('tab_three')">Tab 3</div>
            </div>
            <div id="tabs">
                <div id="tab_home" class="tabItem tab_home selected">Home Content<br />
                    <asp:button runat="server" text="Search" CssClass="CoolButton" ID="SearchButton" ClientIDMode="Static" UseSubmitBehavior="False" Width="100px" />
                    <asp:TextBox ID="Searchbox" runat="server"></asp:TextBox>
                    <asp:checkbox runat="server" ID="MatchAllCheck" Checked="True" Text="Match All Terms"></asp:checkbox>
                    <br />
                </div>
                <div id="tab_two" class="tabItem tab_two">Tab 2 content</div>
                <div id="tab_three" class="tabItem tab_three">Tab 3 content</div>
            </div>
    </div>
        
    <script type="text/javascript">
        var ribbon = 1;
        SetActiveTab("tab_home");
        function ToggleRibbon() {
            if (ribbon == 1) {
                ribbon = 0;
                $("#IC_Sidebar").removeClass("RibbonExpanded").addClass("RibbonContracted");
                $("#IC_Explorer").removeClass("RibbonExpanded").addClass("RibbonContracted");
                $("#IC_Ribbon").removeClass("RibbonExpanded").addClass("RibbonContracted");
                $("#RibbonToggle").removeClass("RibbonExpanded").addClass("RibbonContracted");
            } else {
                ribbon = 1;
                $("#IC_Sidebar").removeClass("RibbonContracted").addClass("RibbonExpanded");
                $("#IC_Explorer").removeClass("RibbonContracted").addClass("RibbonExpanded");
                $("#IC_Ribbon").removeClass("RibbonContracted").addClass("RibbonExpanded");
                $("#RibbonToggle").removeClass("RibbonContracted").addClass("RibbonExpanded");
            }
        }

        function SetActiveTab(tabname) {
            //Disable other tabs
            $(".tabTitle").removeClass("selected");
            $(".tabItem").removeClass("selected");
            //Enable new tab
            $("." + tabname).addClass("selected");
            if (ribbon == 0) { ToggleRibbon();}
        }
    </script>
</asp:Content>
