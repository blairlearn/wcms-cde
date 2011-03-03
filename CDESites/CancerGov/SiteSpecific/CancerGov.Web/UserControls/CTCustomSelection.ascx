<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CTCustomSelection.ascx.cs" Inherits="www.Search.UserControls.CTCustomSelection" %>
<div>
    <span class="header-A">Check sections to include in the display of your clinical trials
        search results</span><br>
    
    <table class="clinicaltrials-customize-results-table">
        <tr class="odd">
            <td width="40%"><asp:CheckBox runat="server" ID="customSections1" Text="Objectives" /></td>
            <td width="60%">Goals of the trial.</td>
        </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="customSections2" Text="Entry Criteria" /></td>
            <td>
                Eligibility criteria for a patient to participate in the trial, including patient
                characteristics, disease characteristics, and treatment history.
            </td>
        </tr>
        <tr class="odd">
            <td><asp:CheckBox runat="server" ID="customSections3" Text="Expected Enrollment" /></td>
            <td>Total number of patients expected to participate in the trial.</td>
        </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="customSections4" Text="Outcomes" /></td>
            <td>
                Specific measures that can be used to evaluate an intervention or determine the
                effect of the intervention.
            </td>
        </tr>
        <tr class="odd">
            <td><asp:CheckBox runat="server" id="customSections5" Text="Trial Outline" /></td>
            <td>Detailed summary of the plan of the trial.</td>
        </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="customSections6" Text="Published Results of Trial" /></td>
            <td>Published scientific papers reporting results of the trial.</td>
        </tr>
        <tr class="odd">
            <td><asp:CheckBox runat="server" ID="customSections7" Text="Related Publications" /></td>
            <td>
                Citations for published scientific reports and papers that contain information related
                to the trial.
            </td>
        </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="customSections8" Text="Trial Lead Organizations" /></td>
            <td>Principal organizations and health professionals conducting the trial.</td>
        </tr>
        <tr class="odd">
            <td><asp:CheckBox runat="server" ID="customSections9" Text="Trial Sites/Contacts" /></td>
            <td>
                Contact information for hospitals/institutions participating in the trial, categorized
                by country and state.
            </td>
        </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="customSections10" Text="Terms" /></td>
            <td>Key terms associated with the trial.</td>
        </tr>
        <tr class="odd">
            <td><asp:CheckBox runat="server" ID="customSections11" Text="Registry Information" /></td>
            <td>
                Key date and identification information about the trial required for clinical trial
                registration.
            </td>
        </tr>
    </table>
            
    <p>
        <asp:ImageButton ID="imgBtnSelect" runat="server" ImageUrl="/images/ctsearch/add-selected-btn.gif"
            Width="82" Height="15" AlternateText="Select Checked" BorderWidth="0" OnClick="imgBtnSelect_Click" />
        <asp:Button ID="btnSelect" runat="server" Text="Select Checked" OnClick="btnSelect_Click" />
    </p>
</div>
