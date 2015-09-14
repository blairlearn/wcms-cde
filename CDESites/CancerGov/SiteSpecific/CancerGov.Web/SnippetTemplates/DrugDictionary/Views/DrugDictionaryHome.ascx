<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.DrugDictionaryHome" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>


 <div>
        <p>The NCI Drug Dictionary contains technical definitions and synonyms for drugs/agents
                used to treat patients with cancer or conditions related to cancer. Each drug entry
                includes links to check for clinical trials listed in NCI's List of Cancer Clinical
                Trials.</p>
    </div>

 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />
