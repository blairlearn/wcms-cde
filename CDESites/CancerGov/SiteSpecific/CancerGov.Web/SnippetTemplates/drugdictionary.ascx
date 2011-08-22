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
    <!-- Main Area -->
    <form id="drugForm" runat="server">
    <a name="top"></a>
    <!--<div style="margin-right: auto; margin-left: auto; ">-->
        <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <!-- Main content of page -->
                <td id="contentzone" valign="top" width="100%">
                    <a name="skiptocontent"></a>
                    <!-- ADMIN TOOL CONTENT GOES HERE -->
                    <!-- Search area -->
                    <asp:Panel ID="pnlDrugSearch" runat="server" CssClass="pnlDrugSearch">
                        <div class="searchboxTopRow" class="searchboxTopRow">
                            <div id="searchboxContainer" runat="server" class="searchboxContainer">
                                <div class="hidden">
                                    The search textbox has an autosuggest feature. When you enter three or more characters,
                                    a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
                                    to move through the suggestions. To select a suggestion, hit the enter key. Using
                                    the escape key closes the listbox and puts you back at the textbox. The radio buttons
                                    allow you to toggle between having all search items start with or contain the text
                                    you entered in the search box.</div>
                                <asp:Label CssClass="lblStrSearch" ID="lblStrSearch" runat="server" Text="Search for"></asp:Label>
                                <CGov:AutoComplete CssClass="AutoComplete1" ID="AutoComplete1" Name="AutoComplete1" runat="server" CallbackFunc="ACOnSubmit" autocomplete="off"
                                    MinWidth="384" />
                                <asp:Label ID="lblAccessSearch" CssClass="hidden" runat="server" Text="Search text box"
                                    AssociatedControlID="AutoComplete1"></asp:Label>
                            </div>
                            <div id="drugSearchboxBtn" class="searchboxBtn">
                                <asp:ImageButton ID="btnGo"  Name="btnGo" runat="server" ImageUrl="/images/red_go_button.gif" 
                                    AlternateText="Search" ToolTip="Search" />
                            </div>
                            <div id="drugSearchboxStarts" class="searchboxStarts">
                                <asp:RadioButton ID="radioStarts" runat="server" Checked="True" GroupName="sgroup"
                                    Text="Starts with" ToolTip="Search item starts with this" />
                            </div>
                            <div id="searchboxSeparator" class="searchboxSeparator" class="searchboxSeparator">
                                <img alt="" src="/images/dictionary-search-radio.gif" />
                            </div>
                            <div id="drugSearchboxContains" class="searchboxContains">
                                <asp:RadioButton  Name="radioContains" ID="radioContains" runat="server" GroupName="sgroup" />
                                <asp:Label ID="lblAccessRadioContains" CssClass="hidden" runat="server" Text="Search item contains this"
                                    AssociatedControlID="radioContains"></asp:Label>
                                <asp:Label ID="lblContains" runat="server" Text="Contains"></asp:Label>
                            </div>
                             <div id="searchboxSeparator2" class="searchboxContains">
                                <img alt="" src="/images/dictionary-search-radio.gif" />
                             </div>
                             <div id="drugSearchboxHelp" class="drugSearchboxHelp">
                                <a href="javascript:dynPopWindow('/Common/PopUps/popHelp.aspx','popup','width=500,height=700,scrollbars=1,resizable=1,menubar=0,location=0,status=0,toolbar=0')">
                                    <img src="/images/drug-dictionary-help.gif" width="13" height="13" alt="" border="0">
                                </a>                            
                             </div>
                        </div>
                        <div class="search-control-div">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
                                    NumericItems="true" ShowAll="true" />
                            </table>
                        </div>
                    </asp:Panel>
                    <!-- end Search area -->
                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="ViewDefault" runat="server" EnableViewState="false">
                            <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
                            <div id="welcomeDiv">
                                    <p>The NCI Drug Dictionary contains technical definitions and synonyms for drugs/agents
                                    used to treat patients with cancer or conditions related to cancer. Each drug entry
                                    includes links to check for clinical trials listed in NCI's PDQ® Cancer Clinical
                                    Trials Registry.</p>
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
                                            border="0" align="absmiddle">
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
                                        NCI Thesaurus</a>, which is produced by NCI's Enterprise Vocabulary Services,
                                    a collaboration involving NCI's Office of Communications and Education and NCI's Center for Bioinformatics.
                                    Each drug entry includes a link to additional information available from the full
                                    NCI Thesaurus database, which contains many drugs and other terms not included here.
                                </div>
                                <!-- end shaded box-->
                            </div>
                        </asp:View>
                        <asp:View ID="ViewResultList" runat="server" EnableViewState="false">
                            <!-- Number of results -->
                            <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
                            <asp:Panel ID="numResDiv" runat="server">
                                <span class="page-title">
                                    <asp:Label ID="lblNumResults" CssClass="page-title" runat="server"></asp:Label>
                                    <asp:Label ID="lblResultsFor" CssClass="page-title" Text="result found for:" runat="server"></asp:Label>
                                </span>&nbsp;&nbsp; &nbsp;&nbsp;
                                <asp:Label ID="lblWord" CssClass="search-result" runat="server"></asp:Label><br />
                                <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
                                <img src="/images/gray_spacer.gif" width="571" height="1" alt="" border="0" /><br />
                                <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
                            </asp:Panel>
                            <asp:ListView ID="resultListView" runat="server">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <a name="<%#DataBinder.Eval(Container.DataItem, "PreferredName")%>"></a><a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "TermID")%>" <%# ResultListViewHrefOnclick(Container)%>>
                                        <%#HiLite(DataBinder.Eval(Container.DataItem, "PreferredName"))%></a> &nbsp;
                                    <span class="dictionary-partial-match-n">
                                        <%#AddBrackets(DataBinder.Eval(Container.DataItem, "OtherName"))%>
                                    </span>
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
                            <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
                            <!-- Term and def -->
                            <b>
                                <asp:Label ID="lblTermName" runat="server"></asp:Label></b>&nbsp;
                            <asp:ImageButton ID="ibtnPatientInfo" CssClass="btn-patient-info" ImageUrl="/images/btn-patient-info.gif"
                                runat="server" Visible="false" AlternateText="Patient Information" ToolTip="Patient Information"
                                ImageAlign="AbsMiddle" Height="20" Width="139" />
                            <asp:HyperLink ID="hlPatientInfo" runat="server" Visible="false">
                                    <img src="/images/btn-patient-info.gif" alt="Patient Information" title="Patient Information" width="139" height="20" hspace="12" border="0"  align="absmiddle" />
                            </asp:HyperLink>
                            <br />
                            <asp:Literal ID="litDefHtml" runat="server"></asp:Literal>
                            <br />
                            <img src="/images/spacer.gif" width="10" height="22" alt="" border="0" />
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
                </td>
                <td valign="top">
                    <img src="/images/spacer.gif" width="10" height="1" alt="" border="0">
                </td>
            </tr>
        </table>
    <!--</div>-->
    </form>
    <!-- Footer -->
    <div id="footerzone" align="center">
        <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
    </div>
<asp:Literal ID="litOmniturePageLoad" mode="PassThrough" runat="server" />
