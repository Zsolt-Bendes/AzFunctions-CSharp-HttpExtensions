using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Xunit;
using HttpExtensions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace HttpExtensions.IntegrationTests
{
    public class HttpRequestExtensionsTests
    {
        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(10, "dfs")]
        [InlineData(200, "dfg")]
        public void GetPaginationData_WithValidPageSizeAndCustomPageSizeName_ShouldReturnPaginationModel(
            int maxPageSize,
            string sizeParamName)
        {
            // arrange
            var paramsDictionary = new Dictionary<string, StringValues> {{sizeParamName, maxPageSize.ToString()}};

            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query).Returns(new QueryCollection(paramsDictionary));

            // act
            var paginationModel = request.GetPaginationData(
                sizeParameterName: sizeParamName,
                maxPageSize: maxPageSize);

            // assert
            paginationModel.Should().NotBeNull();
            paginationModel.Size.Should().Be(maxPageSize);
        }

        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(10, "dfs")]
        [InlineData(200, "dfg")]
        public void GetPaginationData_WithValidPageSizeAndCustomPageIndexName_ShouldReturnPaginationModel(
            int pageIndex,
            string pageIndexParamName)
        {
            // arrange
            var paramsDictionary = new Dictionary<string, StringValues> {{pageIndexParamName, pageIndex.ToString()}};

            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query).Returns(new QueryCollection(paramsDictionary));

            // act
            var paginationModel = request.GetPaginationData(
                indexParameterName: pageIndexParamName);

            // assert
            paginationModel.Should().NotBeNull();
            paginationModel.Index.Should().Be(pageIndex);
        }

        [Fact]
        public void ParseStringList_WithExistingKeys_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("value1,value2");

            // act
            var result = request.ParseStringList("key");

            // assert         
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.First().Should().Be("value1");
        }
        
        [Fact]
        public void ParseStringList_WithNonExistingKey_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("value1,value2");

            // act
            var result = request.ParseStringList("asd");

            // assert         
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void ParseIntegerList_WithExistingKeys_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("1,2");

            // act
            var result = request.ParseIntegerList("key");

            // assert         
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.First().Should().Be(1);
        }
        
        [Fact]
        public void ParseIntegerList_WithNonExistingKey_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("1,2");

            // act
            var result = request.ParseIntegerList("asd");

            // assert         
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void ParseIntegerList_WithNonIntegerQuery_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("sdf,2");

            // act
            var result = request.ParseIntegerList("key");

            // assert         
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(2);
        }
        
        [Fact]
        public void ParseDoubleList_WithExistingKeys_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("1.1,2.3");

            // act
            var result = request.ParseDoubleList("key");

            // assert         
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.First().Should().Be(1.1d);
        }
        
        [Fact]
        public void ParseDoubleList_WithNonExistingKey_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("1.2,2.2");

            // act
            var result = request.ParseDoubleList("asd");

            // assert         
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void ParseDoubleList_WithNonIntegerQuery_ShouldReturnParsedValues()
        {
            // arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Query["key"]).Returns("sdf,2.3");

            // act
            var result = request.ParseDoubleList("key");

            // assert         
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(2.3d);
        }
    }
}