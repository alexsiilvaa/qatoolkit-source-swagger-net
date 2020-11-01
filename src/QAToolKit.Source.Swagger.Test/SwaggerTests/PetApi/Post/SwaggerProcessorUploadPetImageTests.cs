﻿using ExpectedObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Core.Models;
using QAToolKit.Source.Swagger.Test.Fixtures.PetApi.Post;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Source.Swagger.Test.SwaggerTests.PetApi.Post
{
    public class SwaggerProcessorUploadPetImageTests
    {
        private readonly ILogger<SwaggerProcessorUploadPetImageTests> _logger;

        public SwaggerProcessorUploadPetImageTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<SwaggerProcessorUploadPetImageTests>();
        }

        [Fact]
        public async Task UploadPetImageWithoutExampleValuesTest_Successfull()
        {
            var fileSource = new SwaggerFileSource(options =>
            {
                options.AddBaseUrl(new Uri("https://petstore3.swagger.io/"));
                options.AddRequestFilters(new RequestFilter()
                {
                    EndpointNameWhitelist = new string[] { "uploadFile" }
                });
            });

            var requests = await fileSource.Load(new List<FileInfo>() {
                new FileInfo("Assets/swagger-pets-test.json")
            });

            _logger.LogInformation(JsonConvert.SerializeObject(requests, Formatting.Indented));

            Assert.NotNull(requests);
            Assert.Equal(1, requests.Count);

            Assert.Empty(requests.FirstOrDefault().AuthenticationTypes);
            Assert.Equal("https://petstore3.swagger.io/api/v3", requests.FirstOrDefault().BasePath);
            Assert.Equal("", requests.FirstOrDefault().Description);
            Assert.Equal(HttpMethod.Post, requests.FirstOrDefault().Method);
            Assert.Equal("uploadFile", requests.FirstOrDefault().OperationId);
            Assert.Equal("/pet/{petId}/uploadImage", requests.FirstOrDefault().Path);
            Assert.Single(requests.FirstOrDefault().Responses);
            Assert.Equal(2, requests.FirstOrDefault().Parameters.Count);

            var expectedUploadPetImageParameters = UploadPetImageParameters.Get(true).ToExpectedObject();
            expectedUploadPetImageParameters.ShouldEqual(requests.FirstOrDefault().Parameters);

            var expectedUploadPetImageBody = UploadPetImageBody.Get().ToExpectedObject();
            expectedUploadPetImageBody.ShouldEqual(requests.FirstOrDefault().RequestBodies
                .Where(c => c.ContentType == ContentType.Enumeration.OctetStream).ToList());

            var expectedUploadPetImageResponse = UploadPetImageResponse.Get().ToExpectedObject();
            expectedUploadPetImageResponse.ShouldEqual(requests.FirstOrDefault().Responses);

            Assert.Equal("uploads an image", requests.FirstOrDefault().Summary);
            Assert.Collection(requests.FirstOrDefault().Tags, item =>
            {
                item = "pet";
            });
            Assert.Empty(requests.FirstOrDefault().TestTypes);
        }

        [Fact]
        public async Task UploadPetImageWithExampleValuesTest_Successfull()
        {
            var fileSource = new SwaggerFileSource(options =>
            {
                options.AddBaseUrl(new Uri("https://petstore3.swagger.io/"));
                options.AddRequestFilters(new RequestFilter()
                {
                    EndpointNameWhitelist = new string[] { "uploadFile" }
                });
                options.UseSwaggerExampleValues = true;
            });

            var requests = await fileSource.Load(new List<FileInfo>() {
                new FileInfo("Assets/swagger-pets-test.json")
            });

            _logger.LogInformation(JsonConvert.SerializeObject(requests, Formatting.Indented));

            Assert.NotNull(requests);
            Assert.Equal(1, requests.Count);

            Assert.Empty(requests.FirstOrDefault().AuthenticationTypes);
            Assert.Equal("https://petstore3.swagger.io/api/v3", requests.FirstOrDefault().BasePath);
            Assert.Equal("", requests.FirstOrDefault().Description);
            Assert.Equal(HttpMethod.Post, requests.FirstOrDefault().Method);
            Assert.Equal("uploadFile", requests.FirstOrDefault().OperationId);
            Assert.Equal("/pet/{petId}/uploadImage", requests.FirstOrDefault().Path);
            Assert.Single(requests.FirstOrDefault().Responses);
            Assert.Equal(2, requests.FirstOrDefault().Parameters.Count);

            var expectedUploadPetImageParameters = UploadPetImageParameters.Get(true).ToExpectedObject();
            expectedUploadPetImageParameters.ShouldEqual(requests.FirstOrDefault().Parameters);

            var expectedUploadPetImageBody = UploadPetImageBody.Get().ToExpectedObject();
            expectedUploadPetImageBody.ShouldEqual(requests.FirstOrDefault().RequestBodies
                .Where(c => c.ContentType == ContentType.Enumeration.OctetStream).ToList());

            var expectedUploadPetImageResponse = UploadPetImageResponse.Get().ToExpectedObject();
            expectedUploadPetImageResponse.ShouldEqual(requests.FirstOrDefault().Responses);

            Assert.Equal("uploads an image", requests.FirstOrDefault().Summary);
            Assert.Collection(requests.FirstOrDefault().Tags, item =>
            {
                item = "pet";
            });
            Assert.Empty(requests.FirstOrDefault().TestTypes);
        }
    }
}
