using System.Linq;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using Xunit;

namespace Mini_query_builder.Tests.Executors
{

    public class NpgSqlExecutorAddParameterTests
    {
        private readonly NpgSqlExecutorAddParameter _sut = new();

        [Fact]
        public void AddParameters_Should_NotAddAnyParameter_When_BindingsIsEmpty()
        {
            // Arrange
            using var command = new NpgsqlCommand();

            var compileResult = new CompileResult { Bindings = new List<object>() };

            // Act
            var commandResult = _sut.AddParameters(command, compileResult);

            // Assert
            commandResult.Parameters.Should().BeEmpty();
        }

        [Fact]
        public void AddParameters_Should_AddOneParameter_When_BindingsHasOneValue()
        {
            // Arrange
            using var command = new NpgsqlCommand();
            var compileResult = new CompileResult { Bindings = new List<object> { "Ali" } };

            // Act
            _sut.AddParameters(command, compileResult);

            // Assert
            var parameters = command.Parameters.Cast<NpgsqlParameter>().ToList();
            parameters.Should().ContainSingle()
                .Which.Value.Should().Be("Ali");
        }

        [Fact]
        public void AddParameters_Should_AddParametersInBindingsOrder_When_BindingsHasMultipleValues()
        {
            // Arrange
            using var command = new NpgsqlCommand();
            var compileResult = new CompileResult { Bindings = new List<object> { "Ali", 25, true } };

            // Act
            _sut.AddParameters(command, compileResult);

            // Assert
            var values = command.Parameters.Cast<NpgsqlParameter>().Select(p => p.Value);
            values.Should().Equal("Ali", 25, true);
        }

        [Fact]
        public void AddParameters_Should_ReturnSameCommandInstance_When_Called()
        {
            // Arrange
            using var command = new NpgsqlCommand();
            var compileResult = new CompileResult { Bindings = new List<object>() };

            // Act
            var returnedCommand = _sut.AddParameters(command, compileResult);

            // Assert
            returnedCommand.Should().BeSameAs(command);
        }
    }
}