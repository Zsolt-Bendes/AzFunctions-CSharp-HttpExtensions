using System;
using FluentAssertions;
using HttpExtensions.Models;
using Xunit;

namespace HttpExtensions.Domain.UnitTests
{
    public class PaginationDataTests
    {
        [Theory]
        [InlineData(-1, 1)]
        [InlineData(-1, -1)]
        [InlineData(1, -1)]
        public void Ctor_InvalidParameters_ShouldThrowValidationException(int index, int size)
        {
            // arrange
            Action action = () => new PaginationData(index, size);
            
            // act & assert
            FluentActions.Invoking(action).Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(10, 25)]
        [InlineData(1, 1)]
        public void Ctor_ValidParameters_ShouldNotThrow(int index, int size)
        {
            // arrange
            Action action = () => new PaginationData(index, size);
            
            // act & assert
            FluentActions.Invoking(action).Should().NotThrow();
        }

        [Theory]
        [InlineData(1, 20, 0)]
        [InlineData(2, 20, 20)]
        [InlineData(4, 20, 60)]
        public void Offset_ValidParameters_ShouldHaveExpectedValue(int index, int size, int expected)
        {
            // arrange
            var paginationModel = new PaginationData(index, size);
            
            // act
            var offset = paginationModel.Offset;

            offset.Should().Be(expected);
        }
    }
}