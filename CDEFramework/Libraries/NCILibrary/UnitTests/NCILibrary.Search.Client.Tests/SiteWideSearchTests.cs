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
    public class SiteWideSearchTests
    {
        #region ResultJSON
        private string resultSet = @"
                    {
                    'results': [
                    {
                    'title': 'Cancer Screening (or Screening for Cancer) - National Cancer Institute',
                    'url': 'https://www.cancer.gov/about-cancer/screening',
                    'contentType': 'cgvTopicPage',
                    'description': 'A summary of the benefits and possible harms of cancer screening tests.'
                    },
                    {
                    'title': 'Recurrent Cancer - National Cancer Institute',
                    'url': 'https://www.cancer.gov/types/recurrent-cancer',
                    'contentType': 'cgvArticle',
                    'description': 'Cancer can recur when treatment doesn’t fully remove or destroy all the cancer cells. Learn about the different types of recurrence and how recurrent cancer is restaged and treated.'
                    },
                    {
                    'title': 'Gallbladder Cancer—Patient Version - National Cancer Institute',
                    'url': 'https://www.cancer.gov/types/gallbladder',
                    'contentType': 'cgvCancerTypeHome',
                    'description': 'Information about gallbladder cancer treatment, clinical trials, research, and other topics from the National Cancer Institute.'
                    },
                    {
                    'title': 'Cancer Statistics - National Cancer Institute',
                    'url': 'https://www.cancer.gov/about-cancer/understanding/statistics',
                    'contentType': 'cgvArticle',
                    'description': 'Basic information about cancer statistics in the U.S. and how they are used to understand the impact of cancer on society and to develop strategies that address the challenges that cancer poses.'
                    },
                    {
                    'title': 'Understanding Cancer - National Cancer Institute',
                    'url': 'https://www.cancer.gov/about-cancer/understanding',
                    'contentType': 'cgvTopicPage',
                    'description': 'This page is a gateway to basic information about how cancer develops, trends in cancer cases and deaths, and how cancer affects different populations.'
                    },
                    {
                    'title': 'About Cancer - National Cancer Institute',
                    'url': 'https://www.cancer.gov/about-cancer',
                    'contentType': 'nciLandingPage',
                    'description': 'Information from the National Cancer Institute about cancer treatment, prevention, screening, genetics, causes, and how to cope with cancer.'
                    },
                    {
                    'title': 'Breast Cancer—Patient Version - National Cancer Institute',
                    'url': 'https://www.cancer.gov/types/breast',
                    'contentType': 'cgvCancerTypeHome',
                    'description': 'Information about breast cancer treatment, prevention, genetics, causes, screening, clinical trials, research and statistics from the National Cancer Institute.'
                    },
                    {
                    'title': 'Prostate Cancer—Patient Version - National Cancer Institute',
                    'url': 'https://www.cancer.gov/types/prostate',
                    'contentType': 'cgvCancerTypeHome',
                    'description': 'Information about prostate cancer treatment, prevention, genetics, causes, screening, clinical trials, research and statistics from the National Cancer Institute.'
                    },
                    {
                    'title': 'Cancer Moonshot℠ - National Cancer Institute',
                    'url': 'https://www.cancer.gov/research/key-initiatives/moonshot-cancer-initiative',
                    'contentType': 'cgvTopicPage',
                    'description': 'The Cancer Moonshot will marshal resources across the federal government to speed progress in cancer research and lead to improved cancer prevention, detection, and treatment.'
                    },
                    {
                    'title': 'Lung Cancer—Patient Version - National Cancer Institute',
                    'url': 'https://www.cancer.gov/types/lung',
                    'contentType': 'cgvCancerTypeHome',
                    'description': 'Information about lung cancer treatment, prevention, causes, screening, clinical trials, research, statistics and other topics from the National Cancer Institute.'
                    }
                    ],
                    'totalResults': 37040
                    }";
        #endregion


        [Fact]
        public void ApiReturnsProperResultObject()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When("https://www.cancer.gov/*")
                .Respond("application/json", resultSet);
            var client = new HttpClient(mockHttp);
            SiteWideSearchAPIClient apiClient = new SiteWideSearchAPIClient("https://www.cancer.gov", client);
            var results = apiClient.Search("cgov", "en", "cancer");
            Assert.Equal(results.GetType(), typeof(SiteWideSearchResults));
            Assert.Equal(results.TotalResults, 37040);
        }

        [Fact]
        public void ServerErrorHandledProperly()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When("https://www.cancer.gov/*")
                .Respond(HttpStatusCode.InternalServerError);
            var client = new HttpClient(mockHttp);
            SiteWideSearchAPIClient apiClient = new SiteWideSearchAPIClient("https://www.cancer.gov", client);

            Assert.Throws<Exception>(() => apiClient.Search("cgov", "en", "cancer"));


        }

        [Fact]
        public void MalformedDataFromApiThrowsException()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When("https://www.cancer.gov/*")
                .Respond("application/json", "{malformed/data)}{");
            var client = new HttpClient(mockHttp);
            SiteWideSearchAPIClient apiClient = new SiteWideSearchAPIClient("https://www.cancer.gov", client);

            try
            {
                apiClient.Search("cgov", "en", "cancer");
            }
            catch (Exception ex)
            {
                Assert.True((ex.GetType() == typeof(AggregateException)) && ex.InnerException != null);
            }

        }       
    }
}
