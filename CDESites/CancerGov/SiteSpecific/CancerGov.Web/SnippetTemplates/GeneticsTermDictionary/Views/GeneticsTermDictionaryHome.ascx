<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryHome" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>
  
<div id="welcomeDiv">
    <p>Welcome to the NCI Dictionary of Genetics Terms, which contains technical definitions for more than 150 terms related to genetics. These definitions were developed by the <a href="/cancertopics/pdq/cancer-genetics-board">PDQ® Cancer Genetics Editorial Board</a> to support the evidence-based, peer-reviewed <a href="/cancertopics/pdq/genetics">PDQ cancer genetics information summaries</a>.</p>
</div>

<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />



