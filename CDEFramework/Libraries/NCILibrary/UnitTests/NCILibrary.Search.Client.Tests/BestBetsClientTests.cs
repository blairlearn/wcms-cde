using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCILibrary.Search.Client;
using System.Net.Http;

using Xunit;
using RichardSzalay.MockHttp;

namespace NCILibrary.Search.Client.Tests
{
    public class BestBetsClientTests
    {
        #region ResultJSON
        private string resultSet = @"
{
'html': '<div>content</div>',
'id': '36398',
'name': 'Childhood Cancers',
'weight': 55
}
";
        #endregion


        [Fact]
        public void ApiReturnsProperResultObject()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When("https://www.cancer.gov/*")
                .Respond("application/json", resultSet);
            var client = new HttpClient(mockHttp);
            BestBetsAPIClient apiClient = new BestBetsAPIClient("http://ncias-s1786-v.nci.nih.gov:5006", new HttpClient());

            var result = apiClient.Search("en", "childhood");

            Assert.True(result.GetType() == typeof(BestBetResult[]));
        }
    }
}
