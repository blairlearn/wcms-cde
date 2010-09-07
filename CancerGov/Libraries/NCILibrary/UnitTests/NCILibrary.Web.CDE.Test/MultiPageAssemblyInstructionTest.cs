using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web;
using System;
namespace NCI.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for MultiPageAssemblyInstructionTest and is intended
    ///to contain all MultiPageAssemblyInstructionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultiPageAssemblyInstructionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private IPageAssemblyInstruction InitializeTestPageAssemblyInfo()
        {
            string xmlFilePath = TestContext.TestDeploymentDir + "\\PublishedContent\\PageInstructions\\Multicancertopics.xml";

            IPageAssemblyInstruction pageAssemblyInfo = null;
            using (XmlReader xmlReader = XmlReader.Create(xmlFilePath))
            {
                xmlReader.MoveToContent();
                string pageAssemblyInfoTypeName = xmlReader.LocalName;

                //XmlSerializer serializer = _serializers[pageAssemblyInfoTypeName];
                XmlSerializer serializer = new XmlSerializer(typeof(MultiPageAssemblyInstruction));

                // Deserialize the XML into an object.
                pageAssemblyInfo = (IMultiPageAssemblyInstruction)serializer.Deserialize(xmlReader);
                return pageAssemblyInfo;
            }
        }

        ///// <summary>
        /////A test for UrlFilterDelegates
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void UrlFilterDelegatesTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    Dictionary<string, UrlFilterDelegate> actual;
        //    actual = target.UrlFilterDelegates;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Snippets
        /////</summary>
        //[TestMethod()]
        //public void SnippetsTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    IEnumerable<SnippetInfo> actual;
        //    actual = target.Snippets;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for SnippetInfos
        /////</summary>
        //[TestMethod()]
        //public void SnippetInfosTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    SnippetInfoCollection actual;
        //    actual = target.SnippetInfos;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Server
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void ServerTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    HttpServerUtility actual;
        //    actual = target.Server;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for SectionPath
        /////</summary>
        //[TestMethod()]
        //public void SectionPathTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.SectionPath = expected;
        //    actual = target.SectionPath;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for PrettyUrl
        /////</summary>
        //[TestMethod()]
        //public void PrettyUrlTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.PrettyUrl = expected;
        //    actual = target.PrettyUrl;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for PageTemplateName
        /////</summary>
        //[TestMethod()]
        //public void PageTemplateNameTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.PageTemplateName = expected;
        //    actual = target.PageTemplateName;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for PageMetadata
        /////</summary>
        //[TestMethod()]
        //public void PageMetadataTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    PageMetadata expected = null; // TODO: Initialize to an appropriate value
        //    PageMetadata actual;
        //    target.PageMetadata = expected;
        //    actual = target.PageMetadata;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Language
        /////</summary>
        //[TestMethod()]
        //public void LanguageTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Language = expected;
        //    actual = target.Language;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for FieldFilterDelegates
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void FieldFilterDelegatesTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    Dictionary<string, FieldFilterDelegate> actual;
        //    actual = target.FieldFilterDelegates;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for BlockedSlots
        /////</summary>
        //[TestMethod()]
        //public void BlockedSlotsTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    BlockedSlot[] expected = null; // TODO: Initialize to an appropriate value
        //    BlockedSlot[] actual;
        //    target.BlockedSlots = expected;
        //    actual = target.BlockedSlots;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for BlockedSlotNames
        /////</summary>
        //[TestMethod()]
        //public void BlockedSlotNamesTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string[] actual;
        //    actual = target.BlockedSlotNames;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for AlternateContentVersionsKeys
        /////</summary>
        //[TestMethod()]
        //public void AlternateContentVersionsKeysTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string[] actual;
        //    actual = target.AlternateContentVersionsKeys;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for AlternateContentVersions
        /////</summary>
        //[TestMethod()]
        //public void AlternateContentVersionsTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    AlternateContentVersions expected = null; // TODO: Initialize to an appropriate value
        //    AlternateContentVersions actual;
        //    target.AlternateContentVersions = expected;
        //    actual = target.AlternateContentVersions;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for RegisterFieldFilters
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void RegisterFieldFiltersTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    target.RegisterFieldFilters();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for GetUrl
        /////</summary>
        //[TestMethod()]
        //public void GetUrlTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string urlType = string.Empty; // TODO: Initialize to an appropriate value
        //    NciUrl expected = null; // TODO: Initialize to an appropriate value
        //    NciUrl actual;
        //    actual = target.GetUrl(urlType);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMetaDescription
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void GetMetaDescriptionTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.GetMetaDescription();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetField
        /////</summary>
        //[TestMethod()]
        //public void GetFieldTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string fieldName = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.GetField(fieldName);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for FilterCurrentUrl
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void FilterCurrentUrlTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    NciUrl url = null; // TODO: Initialize to an appropriate value
        //    target.FilterCurrentUrl(url);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for CanonicalUrl
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        //public void CanonicalUrlTest()
        //{
        //    MultiPageAssemblyInstruction_Accessor target = new MultiPageAssemblyInstruction_Accessor(); // TODO: Initialize to an appropriate value
        //    NciUrl url = null; // TODO: Initialize to an appropriate value
        //    target.CanonicalUrl(url);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for AddUrlFilter
        /////</summary>
        //[TestMethod()]
        //public void AddUrlFilterTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string urlType = string.Empty; // TODO: Initialize to an appropriate value
        //    UrlFilterDelegate fieldFilter = null; // TODO: Initialize to an appropriate value
        //    target.AddUrlFilter(urlType, fieldFilter);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for AddFieldFilter
        /////</summary>
        //[TestMethod()]
        //public void AddFieldFilterTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction(); // TODO: Initialize to an appropriate value
        //    string fieldName = string.Empty; // TODO: Initialize to an appropriate value
        //    FieldFilterDelegate filter = null; // TODO: Initialize to an appropriate value
        //    target.AddFieldFilter(fieldName, filter);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for MultiPageAssemblyInstruction Constructor
        /////</summary>
        //[TestMethod()]
        //public void MultiPageAssemblyInstructionConstructorTest()
        //{
        //    MultiPageAssemblyInstruction target = new MultiPageAssemblyInstruction();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}


        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void MultiPageAssemblyInstruction_XMLSerializer_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            Assert.IsNotNull(pageAssemblyInfo);
            Assert.IsNotNull(pageAssemblyInfo.PageTemplateName);
            Assert.IsNotNull(pageAssemblyInfo.SectionPath);
        }
    }
}
