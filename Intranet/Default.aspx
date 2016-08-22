<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WHLStatus._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Network Status</h2>
            <p>
                View the status of various parts of the White Hinge network, and see any recent known issues. Here you can also see exactly what powers everything and now many components bring it all together.
            </p>
            <p>
                <a class="btn btn-default" href="/Status">View &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Applications</h2>
            <p>
                Download the applications you'll need to do your job here. You can also download a few other useful things which might be helpful.
            </p>
            <p>
                <a class="btn btn-default" href="/Apps">Get Apps &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Links</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="/Links">See Links &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
