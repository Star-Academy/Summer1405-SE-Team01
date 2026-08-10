using System;
using SqlBuilder;
using SqlBuilder.UsernamePass.Implementations;
using SqlBuilder.UsernamePass.Abstractions;
using Xunit;
using FluentAssertions;

namespace Mini_query_builder.Tests.UsernamePass
{
    public class PostgresUsernamePassTests : IDisposable
    {
        private const string UsernameVar = "PG_USERNAME";
        private const string PasswordVar = "PG_PASSWORD";
        private readonly PostgresUsernamePass _sut;
        public PostgresUsernamePassTests()
        {
            _sut = new PostgresUsernamePass();
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(UsernameVar, null);
            Environment.SetEnvironmentVariable(PasswordVar, null);
        }

        [Fact]
        public void GetUserInfo_Should_ReturnDefaultPostgres_When_EnvironmentVariableIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(UsernameVar, null);

            // Act
            var result = _sut.GetUserInfo();

            // Assert
            result.Should().Be("postgres");
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
        public void GetPassInfo_Should_ReturnDefaultPostgres_When_EnvironmentVariableIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(PasswordVar, null);

            // Act
            var result = _sut.GetPassInfo();

            // Assert
            result.Should().Be("postgres");
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