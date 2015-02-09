<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CTCustomSelection.ascx.cs" Inherits="www.Search.UserControls.CTCustomSelection" %>
<div>
    <span>Check sections to include in the display of your clinical trials
        search results</span><br/>
    
    <table class="table-default">
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections1" Text="Objectives" /></div></td>
            <td>Goals of the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections2" Text="Entry Criteria" /></div></td>
            <td>
                Eligibility criteria for a patient to participate in the trial, including patient
                characteristics, disease characteristics, and treatment history.
            </td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections3" Text="Expected Enrollment" /></div></td>
            <td>Total number of patients expected to participate in the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections4" Text="Outcomes" /></div></td>
            <td>
                Specific measures that can be used to evaluate an intervention or determine the
                effect of the intervention.
            </td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" id="customSections5" Text="Trial Outline" /></div></td>
            <td>Detailed summary of the plan of the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections6" Text="Published Results of Trial" /></div></td>
            <td>Published scientific papers reporting results of the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections7" Text="Related Publications" /></div></td>
            <td>
                Citations for published scientific reports and papers that contain information related
                to the trial.
            </td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections8" Text="Trial Lead Organizations" /></div></td>
            <td>Principal organizations and health professionals conducting the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections9" Text="Trial Sites/Contacts" /></div></td>
            <td>
                Contact information for hospitals/institutions participating in the trial, categorized
                by country and state.
            </td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections10" Text="Terms" /></div></td>
            <td>Key terms associated with the trial.</td>
        </tr>
        <tr>
            <td> <div class="checkbox"><asp:CheckBox runat="server" ID="customSections11" Text="Registry Information" /></div></td>
            <td>
                Key date and identification information about the trial required for clinical trial
                registration.
            </td>
        </tr>
    </table>
            
    <p>
        <asp:Button ID="imgBtnSelect" runat="server" CssClass="action button" Text="Add Selected" AlternateText="Select Checked" BorderWidth="0" OnClick="imgBtnSelect_Click" />
        <asp:Button ID="btnSelect" runat="server" Text="Select Checked" OnClick="btnSelect_Click" />
    </p>
</div>
