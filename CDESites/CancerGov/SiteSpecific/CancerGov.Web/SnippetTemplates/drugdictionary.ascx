<%@ Control Language="c#" CodeBehind="drugdictionary.ascx.cs"   AutoEventWireup="True" Inherits="Www.Templates.drugdictionary" %>

<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="/Common/UserControls/AlphaListBox.ascx"%>
<%@ Register TagPrefix="CancerGovWww" TagName="LangSwitch" Src="/Common/UserControls/LangSwitch.ascx"%>
<%@ Register TagPrefix="CGov"  Assembly="NCILibrary.Web.UI.WebControls"   Namespace="NCI.Web.UI.WebControls.FormControls" %>


<script type="text/javascript">
    // function used by AutoComplete to submit to server when user
    // selects an item
    function ACOnSubmit() {
        document.getElementById('<%=btnGo.ClientID%>').click();
    }
</script>
<script type="text/javascript">
    var ids = {
    radioStarts: "<%=radioStarts.ClientID %>"
    ,radioContains: "<%=radioContains.ClientID %>"
    ,AutoComplete1:"<%=AutoComplete1.ClientID %>"
    }
</script>    

<script src="/JS/popEvents.js" type="text/javascript"></script>
<script src="/JS/drugDictionary.js" type="text/javascript"></script>


 <div>
        <p>The NCI Drug Dictionary contains technical definitions and synonyms for drugs/agents
                used to treat patients with cancer or conditions related to cancer. Each drug entry
                includes links to check for clinical trials listed in NCI's List of Cancer Clinical
                Trials.</p>
    </div>
    <div class="hidden">
         The search textbox has an autosuggest feature. When you enter three or more characters,
         a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
         to move through the suggestions. To select a suggestion, hit the enter key. Using
         the escape key closes the listbox and puts you back at the textbox. The radio buttons
         allow you to toggle between having all search items start with or contain the text
         you entered in the search box.
     </div>

<asp:Panel ID="pnlDrugSearch2" runat="server" CssClass="dictionary-search">
   
            <form id="drugForm" runat="server">
                <div class="row">
                    
                     <div class="medium-2 columns">
                        <span class="radio">
                        <asp:RadioButton ID="radioStarts" runat="server" GroupName="sgroup" checked="true"   />
                        <asp:Label ID="lblStartsWith" runat="server" Text="Starts with" class="inline"
                            AssociatedControlID="radioStarts"></asp:Label>
                        </span>
                    </div>
                    
                    
                    <div class="medium-2 columns left">
                        <span class="radio">
                        <asp:RadioButton  ID="radioContains" runat="server" GroupName="sgroup" />
                        <asp:Label ID="lblContains" runat="server" Text="Contains" class="inline"
                            AssociatedControlID="radioContains"></asp:Label> 
                        </span>
                    </div>
                    </div>
                    <div class="row">
                            
                    <div class="medium-6 columns">
                        <asp:TextBox CssClass="drug-dictionary" ID="AutoComplete1" Name="AutoComplete1" 
                           runat="server" />
                        <!-- <asp:Label ID="lblAccessSearch" CssClass="hidden" runat="server" Text="Search text box"
                           AssociatedControlID="AutoComplete1"></asp:Label> -->
                    </div>
                    
                    <div class="medium-2 columns">
                        <asp:Button ID="btnGo" CssClass="submit button postfix" Name="btnGo" runat="server" ToolTip="Search"
                            Text="Search" />
                    </div>
                    
                    <div class="medium-1 columns left">
                        <a  class="text-icon-help" aria-label="Help"  href="javascript:dynPopWindow('/Common/PopUps/popHelp.aspx','popup','width=500,height=700,scrollbars=1,resizable=1,menubar=0,location=0,status=0,toolbar=0')">
                            ?
                        </a> 
                    </div>
                </div>
            </form>
       
</asp:Panel>


<div class="az-list">
            <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
             NumericItems="true" ShowAll="true" />
</div>

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="ViewDefault" runat="server" EnableViewState="false">
        <%-- No Longer Exists but view needed --%>
    </asp:View>
    <asp:View ID="ViewResultList" runat="server" EnableViewState="false">
        <!-- Number of results -->
        
        <asp:Panel ID="numResDiv" runat="server">
            <span class="results-count">
                <asp:Label ID="lblNumResults" CssClass="results-num"  runat="server"></asp:Label>
                <asp:Label ID="lblResultsFor"  Text="result found for: " runat="server"></asp:Label>
                <asp:Label ID="lblWord" CssClass="term" runat="server"></asp:Label>
            </span>
            
           
            
        </asp:Panel>
        <asp:ListView ID="resultListView" runat="server">
            <LayoutTemplate>
            <div class="results">
                <dl class="dictionary-list">
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </dl>
            </div>
            </LayoutTemplate>
            <ItemTemplate>
                
                        <dt>
                                <dfn>
                                    <a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "TermID")%>" <%# ResultListViewHrefOnclick(Container)%>>
                                        <%#HiLite(DataBinder.Eval(Container.DataItem, "PreferredName"))%></a> 
                                    <span class="dictionary-partial-match-n">
                                        <%#AddBrackets(DataBinder.Eval(Container.DataItem, "OtherName"))%>
                                    </span>
                                </dfn>
                        </dt>
                        <dd class="definition">
                                    <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                        </dd>
                    
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="pnlNoData" runat="server">
                    <div id="noMatchDiv" runat="server">
                        No matches were found for the word or phrase you entered. Please check your spelling,
                        and try searching again. You can also type the first few letters of your word or
                        phrase, or click a letter in the alphabet and browse through the list of terms that
                        begin with that letter. You can also search the <a href="http://nciterms.nci.nih.gov/NCIBrowser/">
                            NCI Thesaurus</a>, which is the source of the information in the NCI Drug Dictionary.
                        NCI Thesaurus is produced by NCI’s Enterprise Vocabulary Services, a collaboration
                        involving NCI’s Office of Communications and NCI’s Center for Bioinformatics. NCI
                        Thesaurus includes information about several thousand additional drugs.
                    </div>
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
    </asp:View>
    <asp:View ID="ViewDefinition" runat="server" EnableViewState="false">
        
        <!-- Term and def -->
        <dl>
            <dt>
           <dfn><asp:Label ID="lblTermName" runat="server"></asp:Label></dfn></dt>
        <dd class="definition"><asp:ImageButton ID="ibtnPatientInfo" CssClass="btn-patient-info" ImageUrl="/images/btn-patient-info.gif"
            runat="server" Visible="false" AlternateText="Patient Information" ToolTip="Patient Information"
            ImageAlign="AbsMiddle" Height="20" Width="139" />
        <asp:HyperLink ID="hlPatientInfo" runat="server" Visible="false">
                <img src="/images/btn-patient-info.gif" alt="Patient Information" title="Patient Information" width="139" height="20" hspace="12" border="0"  align="absmiddle" />
        </asp:HyperLink>

        <asp:Literal ID="litDefHtml" runat="server"></asp:Literal>
        
        <asp:Literal ID="litOtherNames" runat="server"></asp:Literal>
        <!-- <asp:Panel ID="pnlDefImages" runat="server">
                <img src="/images/spacer.gif" width="10" height="25" alt="" border="0"/><br/>
                <img src="/images/gray_spacer.gif" width="571" height="1" alt="" border="0"/><br/>
                <img src="/images/spacer.gif" width="1" height="25" alt="" border="0"/><br/>
            </asp:Panel> -->
        <asp:Panel ID="pnlDefPrint" runat="server" Visible="false">
            
        </asp:Panel></dd></dl>
    </asp:View>
</asp:MultiView>
<asp:Literal ID="litBackToTop" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="litPager" runat="server"></asp:Literal>

<!-- Footer -->
<div id="footerzone" align="center">
    <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litOmniturePageLoad" mode="PassThrough" runat="server" />
