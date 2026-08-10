using System;
using System.IO;
using SqlBuilder;
using SqlBuilder.Exeption.Abstractions;
using SqlBuilder.Exeption.Implementations;
using NSubstitute;
using Xunit;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;

namespace Mini_query_builder.Tests.Exption
{

    public class ErrorLocalizerTests
    {
        private readonly ILanguageFileProvider _fileProvider = Substitute.For<ILanguageFileProvider>();
        private readonly ErrorLocalizer _sut;

        public ErrorLocalizerTests()
        {
            _sut = new ErrorLocalizer(_fileProvider);
        }

        [Fact]
        public void GetMessageValue_Should_ReturnKeyWrappedInBrackets_When_KeyWasNeverLoaded()
        {
            // Act
            var result = _sut.GetMessageValue("SomeUnknownKey");

            // Assert
            result.Should().Be("[SomeUnknownKey]");
        }

        [Fact]
        public void LoadLanguage_Should_LeaveMessagesEmpty_When_FileDoesNotExist()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(false);

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("[Key1]");
        }

        [Fact]
        public void LoadLanguage_Should_PopulateMessages_When_FileContainsValidJson()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(true);
            _fileProvider.ReadAllText(Arg.Any<string>()).Returns("""{"Key1":"مقدار تست"}""");

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("مقدار تست");
        }

        [Fact]
        public void LoadLanguage_Should_ResetMessagesToEmpty_When_FileContainsInvalidJson()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(true);
            _fileProvider.ReadAllText(Arg.Any<string>()).Returns("this is not valid json");

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("[Key1]");
        }

        [Fact]
        public void LoadLanguage_Should_NotThrow_When_ReadAllTextThrowsIOException()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(true);
            _fileProvider.ReadAllText(Arg.Any<string>()).Throws(new IOException("disk error"));

            // Act
            Action act = () => _sut.LoadLanguage("en");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void LoadLanguage_Should_NotThrow_When_ReadAllTextThrowsUnauthorizedAccessException()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(true);
            _fileProvider.ReadAllText(Arg.Any<string>()).Throws(new UnauthorizedAccessException("access denied"));

            // Act
            Action act = () => _sut.LoadLanguage("en");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void LoadLanguage_Should_RequestCorrectFilePath_When_Called()
        {
            // Arrange
            _fileProvider.Exists(Arg.Any<string>()).Returns(false);

            // Act
            _sut.LoadLanguage("fa");

            // Assert
            _fileProvider.Received(1).Exists("Exeption/Json/errors.fa.json");
        }
    }
}