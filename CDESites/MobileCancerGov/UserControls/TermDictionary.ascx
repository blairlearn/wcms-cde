<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionary.ascx.cs" Inherits="MobileCancerGov.Web.UserControls.TermDictionary" %>

<asp:Panel id="TermSearch" runat="server" ActiveViewIndex="1"> 
<div class="pageTitle">
    <h2>Dictionary of Cancer Terms</h2>
</div> 
<table border="0" cellpadding="5" cellspacing="0" width="100%">
<tbody>
    <tr>
        <td width="85%">
            <input name="SearchStr" type="text" style="width:90%" />
        </td>
    <td>
    <div>
        <div style="width:35px; float:left;">
            <img src="/images/icon-question-mark-red.png" /> 
        </div>
        <div >
            <asp:ImageButton CssClass="btnGo" Name="btnGo" ID="btnGo" runat="server" 
            ImageUrl="/images/go-button.png" AlternateText="Search" ToolTip="Search" />
        </div>
    </div>
    </td>
    </tr>
</tbody>
</table>
<table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tbody>
        <tr>
            <td><strong><a href="#">#</a></strong></td>
            <td><strong><a href="#">A</a></strong></td>
            <td><strong><a href="b.html">B</a></strong></td>
            <td><strong><a href="c.html">C</a></strong></td>

            <td><strong><a href="#">D</a></strong></td>
            <td><strong><a href="#">E</a></strong></td>
            <td><strong>F</strong></td>
            <td><strong><a href="#">G</a></strong></td>
            <td><strong><a href="#">H</a></strong></td></tr><tr>
            <td><strong><a href="#">I</a></strong></td>

            <td><strong>J</strong></td>
            <td><strong><a href="#" data-ajax="false">K</a></strong></td>
            <td><strong><a href="l.html">L</a></strong></td>
            <td><strong><a href="#" data-ajax="false">M</a></strong></td>
            <td><strong><a href="#">N</a></strong></td>
            <td><strong><a href="#" data-ajax="false">O</a></strong></td>

            <td><strong><a href="p.html">P</a></strong></td>
            <td><strong>Q</strong></td></tr><tr>
            <td><strong><a href="#" data-ajax="false">R</a></strong></td>
            <td><strong><a href="s.html">S</a></strong></td>
            <td><strong><a href="#">T</a></strong></td>
            <td><strong><a href="u.html">U</a></strong></td>

            <td><strong><a href="#">V</a></strong></td>
            <td><strong><a href="#">W</a></strong></td>
            <td><strong>X</strong></td>
            <td><strong><a href="#">Y</a></strong></td>
            <td><strong>Z</strong></td>
        </tr>
    </tbody>
</table>  
</asp:Panel>    
<asp:Panel id="ViewResultsList" runat="server" ActiveViewIndex="0"> 
    <asp:ListView ID="resultListView" runat="server">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>
        <ItemTemplate>
            <a name="<%#DataBinder.Eval(Container.DataItem, "TermName")%>"></a><a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%><%=QueryStringLang%>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <%# Eval("TermName")%></a> &nbsp;&nbsp;
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="100%" align="left">
                        <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                    </td>
                </tr>
            </table>
            <br>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Panel ID="pnlNoDataEnglish" runat="server" Visible="false">
                No matches were found for the word or phrase you entered. Please check your spelling,
                and try searching again. You can also type the first few letters of your word or
                phrase, or click a letter in the alphabet and browse through the list of terms that
                begin with that letter.
            </asp:Panel>
            <asp:Panel ID="pnlNoDataSpanish" runat="server" Visible="false">
                No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                lista de términos que empiezan con esa letra.
            </asp:Panel>
        </EmptyDataTemplate>
    </asp:ListView>

</asp:Panel> 
<asp:Panel id="ViewDefinition" runat="server" Visible="false"> 
</asp:Panel> 

</div>
<asp:Panel id="basic" runat="server" Visible="false">
<p>This is Basic</p>
</asp:Panel>
<asp:Panel id="advanced" runat="server" Visible="true">
<p>This is advanced</p>
</asp:Panel>







