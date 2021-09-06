<%@ Page Title="關於" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.vb" Inherits="About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>您的應用程式說明頁。</h2>
    </hgroup>

    <article>
        <p>        
            使用此區域提供其他資訊。
        </p>

        <p>        
            使用此區域提供其他資訊。
        </p>

        <p>        
            使用此區域提供其他資訊。
        </p>
    </article>

    <aside>
        <h3>側邊標題</h3>
        <p>        
            使用此區域提供其他資訊。
        </p>
        <ul>
            <li><a runat="server" href="~/">首頁</a></li>
            <li><a runat="server" href="~/About.aspx">關於</a></li>
            <li><a runat="server" href="~/Contact.aspx">連絡人</a></li>
        </ul>
    </aside>
</asp:Content>