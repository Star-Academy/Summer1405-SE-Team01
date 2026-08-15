using System;
using SqlBuilder;
using Xunit;
using FluentAssertions;
using System.Data;
using System.Data.SqlClient;
using SqlBuilder.UsernamePass.Abstractions;
using SqlBuilder.UsernamePass.Implementations;

namespace Mini_query_builder.Tests.UsernamePass
{
    public class SqlServerUsernamePassTests : IDisposable
    {
        private const string UsernameVar = "SQL_USERNAME";
        private const string PasswordVar = "SQL_PASSWORD";
        private readonly SqlServerUsernamePass _sut = new();

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(UsernameVar, null);
            Environment.SetEnvironmentVariable(PasswordVar, null);
        }

        [Fact]
        public void GetUserInfo_Should_ReturnDefaultSa_When_EnvironmentVariableIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(UsernameVar, null);

            // Act
            var result = _sut.GetUserInfo();

            // Assert
            result.Should().Be("sa");
        }

        [Fact]
        public void GetUserInfo_Should_ReturnEnvironmentVariableValue_When_EnvironmentVariableIsSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(UsernameVar, "custom_user");

            // Act
            var result = _sut.GetUserInfo();

            // Assert
            result.Should().Be("custom_user");
        }

        [Fact]
        public void GetPassInfo_Should_ReturnDefaultPassword_When_EnvironmentVariableIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(PasswordVar, null);

            // Act
            var result = _sut.GetPassInfo();

            // Assert
            result.Should().Be("Your_strong_Password123");
        }

        [Fact]
        public void GetPassInfo_Should_ReturnEnvironmentVariableValue_When_EnvironmentVariableIsSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(PasswordVar, "custom_pass");

            // Act
            var result = _sut.GetPassInfo();

            // Assert
            result.Should().Be("custom_pass");
        }
    }
}