using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.ResultRecords;
using SqlBuilder.Querying;
using FluentAssertions;
using Xunit;

namespace Mini_query_builder.Tests.Executors
{
    public class SQLServerExecutorAddParametersTests
    {
        private readonly SQLServerExecutorAddParameters _sut = new();

        [Fact]
        public void AddParameters_ShouldNotAddAnyParameter_WhenBindingsIsEmpty()
        {
            // Arrange
            using var command = new SqlCommand();
            var result = new CompileResult { Bindings = new List<object>() };

            // Act
            _sut.AddParameters(command, result);

            // Assert
            command.Parameters.Cast<SqlParameter>().Should().BeEmpty();
        }

        [Fact]
        public void AddParameters_ShouldNameParametersSequentiallyStartingFromP1_WhenBindingsHasMultipleValues()
        {
            // Arrange
            using var command = new SqlCommand();
            var result = new CompileResult { Bindings = new List<object> { "Ali", 25 } };

            // Act
            _sut.AddParameters(command, result);

            // Assert
            var names = command.Parameters.Cast<SqlParameter>().Select(p => p.ParameterName);
            names.Should().Equal("@p1", "@p2");
        }

        [Fact]
        public void AddParameters_ShouldAssignBindingValuesToParametersInOrder_WhenBindingsHasMultipleValues()
        {
            // Arrange
            using var command = new SqlCommand();
            var result = new CompileResult { Bindings = new List<object> { "Ali", 25 } };

            // Act
            _sut.AddParameters(command, result);

            // Assert
            var values = command.Parameters.Cast<SqlParameter>().Select(p => p.Value);
            values.Should().Equal("Ali", 25);
        }

        [Fact]
        public void AddParameters_ShouldReturnSameCommandInstance_WhenCalled()
        {
            // Arrange
            using var command = new SqlCommand();
            var result = new CompileResult { Bindings = new List<object>() };

            // Act
            var returnedCommand = _sut.AddParameters(command, result);

            // Assert
            returnedCommand.Should().BeSameAs(command);
        }
    }
}