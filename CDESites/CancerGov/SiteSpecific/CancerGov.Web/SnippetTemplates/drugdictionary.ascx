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
    ,AutoComplete1:"<%=AutoComplete1.ClientID %>"
    }
</script>    

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
                        <asp:RadioButton ID="radioStarts" runat="server" GroupName="sgroup"   />
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
                        <CGov:AutoComplete CssClass="drug-dictionary" ID="AutoComplete1" Name="AutoComplete1" 
                           runat="server" CallbackFunc="ACOnSubmit" autocomplete="off" aria-label="Enter keywords or phrases" aria-autocomplete="list" MinWidth="384" placeholder="Enter keywords or phrases" />
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
        <div id="welcomeDiv">
            <p><b>Tips on Looking Up a Drug</b></p>
            <ul>
                <li>In the search box, type the name or part of the name of the drug/agent you are looking
                    for and click the “Go” button.</li>
                <li>You can use the generic name (e.g., doxorubicin), U.S. brand names (e.g., Rubex),
                    NSC number, chemical structure names, or other names to find the drug.</li>
                <li>Click on a letter of the alphabet to browse through the dictionary or click on "All"
                    to see a listing of all drugs in the dictionary.</li>
                <li>Change the search from "Starts with" to "Contains" to find all drugs in the dictionary
                    that include a word or set of letters or numbers (e.g., "rubicin" to find daunorubicin,
                    doxorubicin, and epirubicin).</li>
                <li>Use the
                    <img src="/images/drug-dictionary-help.gif" width="13" height="13" alt="Help icon"
                        border="0" align="absmiddle"/>
                    icon to get more help.</li>
                <li>The search box has an <strong>autosuggest</strong> feature. When you type three
                    or more letters, a list of up to 10 suggestions will pop up below the box. Click
                    on a suggestion with your mouse or use the arrow keys on your keyboard to move through
                    the suggestions and then hit the Enter key to choose one.</li>
                <li>Using the Escape key or clicking "close" within the autosuggest box closes the box
                    and turns off the feature until you start a new search.</li>
                <li>Some drug entries include a
                    <img src="http://www.cancer.gov/images/btn-patient-info.gif" alt="Patient Information"
                        title="Patient Information" width="139" height="20" border="0" align="absmiddle" />
                    button that links to a drug information summary page.</li>
            </ul>
            <!-- shaded box-->
            <div style="background: #EFEFEF; padding: 17px;">
                Information in the NCI Drug Dictionary is from the <a href="http://ncit.nci.nih.gov/">
                    NCI Thesaurus</a>, which is produced by NCI's Enterprise Vocabulary Services. Each drug 
                    entry includes a link to additional information available from the full NCI Thesaurus 
                    database, which contains many drugs and other terms not included here.
            </div>
            <!-- end shaded box-->
        </div>
    </asp:View>
    <asp:View ID="ViewResultList" runat="server" EnableViewState="false">
        <!-- Number of results -->
        
        <asp:Panel ID="numResDiv" runat="server">
            <span class="page-title">
                <asp:Label ID="lblNumResults" CssClass="page-title" runat="server"></asp:Label>
                <asp:Label ID="lblResultsFor" CssClass="page-title" Text="result found for:" runat="server"></asp:Label>
            </span>
            <asp:Label ID="lblWord" CssClass="search-result" runat="server"></asp:Label>
            
        </asp:Panel>
        <asp:ListView ID="resultListView" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="result">
                    <dl class="dictionary-list">
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
                    </dl>
                </div>
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
        <b>
            <asp:Label ID="lblTermName" runat="server"></asp:Label></b>
        <asp:ImageButton ID="ibtnPatientInfo" CssClass="btn-patient-info" ImageUrl="/images/btn-patient-info.gif"
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
            <p>
            </p>
        </asp:Panel>
    </asp:View>
</asp:MultiView>
<asp:Literal ID="litBackToTop" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="litPager" runat="server"></asp:Literal>

<!-- Footer -->
<div id="footerzone" align="center">
    <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litOmniturePageLoad" mode="PassThrough" runat="server" />
