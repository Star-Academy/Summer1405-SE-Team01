using NSubstitute;
using NSubstitute.ExceptionExtensions;
using FluentAssertions;
using System;
using System.IO;
using System.Net.Security;
using SqlBuilder.Exceptions.Abstractions;
using SqlBuilder.Exceptions.Implementations;
using Xunit;

namespace Mini_query_builder.Tests.Exceptions
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
        public void GetMessageValue_ShouldReturnKeyWrappedInBrackets_WhenKeyWasNeverLoaded()
        {
            // Arrange
            // Act
            var result = _sut.GetMessageValue("SomeUnknownKey");

            // Assert
            result.Should().Be("[SomeUnknownKey]");
        }

        [Fact]
        public void LoadLanguage_ShouldLeaveMessagesEmpty_WhenFileDoesNotExist()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(false);

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("[Key1]");
            _fileProvider.DidNotReceive().ReadAllText(Arg.Any<string>());
        }
        
        [Theory]
        [InlineData("DatabaseTimeout", "The database operation took too long.")]
        [InlineData("ConnectionFailed", "Failed to connect to the database.")]
        [InlineData("EmptyTableName", "Table name cannot be null or empty.")]
        public void LoadLanguage_ShouldExposeEachEnglishMessage_WhenFileContainsRealEnglishJson(
            string key, string expectedMessage)
        {
            // Arrange
            const string realEnglishJson = """
            {
              "DatabaseTimeout": "The database operation took too long.",
              "ConnectionFailed": "Failed to connect to the database.",
              "InvalidQuery": "The provided SQL query is invalid.",
              "EmptyTableName": "Table name cannot be null or empty.",
              "EmptyColumnName": "Column names cannot be null or whitespace."
            }
            """;
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns(realEnglishJson);

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue(key).Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("DatabaseTimeout", "عملیات دیتابیس بیش از حد طول کشید.")]
        [InlineData("ConnectionFailed", "اتصال به دیتابیس برقرار نشد.")]
        [InlineData("EmptyColumnName", "نام ستون‌ها نمی‌تواند خالی باشد.")]
        public void LoadLanguage_ShouldExposeEachPersianMessage_WhenFileContainsRealPersianJson(
            string key, string expectedMessage)
        {
            // Arrange
            const string realPersianJson = """
            {
              "DatabaseTimeout": "عملیات دیتابیس بیش از حد طول کشید.",
              "ConnectionFailed": "اتصال به دیتابیس برقرار نشد.",
              "InvalidQuery": "کوئری SQL وارد شده نامعتبر است.",
              "EmptyTableName": "نام جدول نمی‌تواند خالی باشد.",
              "EmptyColumnName": "نام ستون‌ها نمی‌تواند خالی باشد."
            }
            """;
            _fileProvider.Exists("Exceptions/Json/errors.fa.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.fa.json").Returns(realPersianJson);

            // Act
            _sut.LoadLanguage("fa");

            // Assert
            _sut.GetMessageValue(key).Should().Be(expectedMessage);
        }

        [Fact]
        public void LoadLanguage_ShouldClearPreviousMessagesFile_WhenNewFileLoads()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns("""{"Key1":"Value1"}""");
            _sut.LoadLanguage("en");
            
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns("this is not valid json");
            
            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("[Key1]");
        }

        [Fact]
        public void LoadLanguage_ShouldLeaveMessagesEmpty_WhenJsonLiteralIsNull()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns("null");

            // Act
            _sut.LoadLanguage("en");

            // Assert
            _sut.GetMessageValue("Key1").Should().Be("[Key1]");
        }

        [Fact]
        public void LoadLanguage_ShouldNotThrow_WhenReadAllTextThrowsIOException()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Throws(new IOException("disk error"));

            // Act
            Action act = () => _sut.LoadLanguage("en");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void LoadLanguage_ShouldNotThrow_WhenReadAllTextThrowsUnauthorizedAccessException()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Throws(new UnauthorizedAccessException("access denied"));

            // Act
            Action act = () => _sut.LoadLanguage("en");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("en")]
        [InlineData("fa")]
        public void LoadLanguage_ShouldRequestPathContainingLanguageCode_Whenever(string languageCode)
        {
            // Arrange
            var expectedPath = $"Exceptions/Json/errors.{languageCode}.json";
            _fileProvider.Exists(expectedPath).Returns(false);

            // Act
            _sut.LoadLanguage(languageCode);

            // Assert
            _fileProvider.Received(1).Exists(Arg.Is<string>(path =>
                path.Contains($".{languageCode}.") && 
                path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)));
        }
        
        [Fact]
        public void LoadLanguage_ShouldReplacePreviousMessages_WhenCalledTwiceWithDifferentLanguages()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json")
                .Returns("""{"Greeting":"Hello"}""");

            _fileProvider.Exists("Exceptions/Json/errors.fa.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.fa.json")
                .Returns("""{"Greeting":"سلام"}""");

            // Act
            _sut.LoadLanguage("en");
            _sut.LoadLanguage("fa");

            // Assert
            _sut.GetMessageValue("Greeting").Should().Be("سلام");
        }

        [Fact]
        public void GetMessageValue_ShouldReturnKeyWrappedInBrackets_WhenKeyIsNotInLoadedFile()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns("""{"Key1":"Value1"}""");
            _sut.LoadLanguage("en");

            // Act
            var result = _sut.GetMessageValue("KeyThatDoesNotExist");

            // Assert
            result.Should().Be("[KeyThatDoesNotExist]");
        }

        [Fact]
        public void LoadLanguage_ShouldNotCrashAndKeepData_OnSystemError()
        {
            // Arrange
            _fileProvider.Exists("Exceptions/Json/errors.en.json").Returns(true);
            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Returns("""{"Key1":"Value1"}""");
            _sut.LoadLanguage("en");

            _fileProvider.ReadAllText("Exceptions/Json/errors.en.json").Throws(new IOException("disk error"));

            // Act
            Action act = () => _sut.LoadLanguage("en");

            // Assert
            act.Should().NotThrow();
            _sut.GetMessageValue("Key1").Should().Be("Value1");
        }
    }
}