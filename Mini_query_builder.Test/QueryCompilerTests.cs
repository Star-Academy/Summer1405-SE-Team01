using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace Mini_query_builder.Tests.QueryBuilders
{

    public class QueryCompilerTests
    {
        private readonly IQueryDecomposer _decomposer = Substitute.For<IQueryDecomposer>();
        private readonly QueryCompiler _sut;

        public QueryCompilerTests()
        {
            _sut = new QueryCompiler(_decomposer);
        }

        [Fact]
        public void Compile_ShouldReturnRawQueryComposedFromSelectFromAndWhereClauses_WhenDecomposerReturnsAllParts()
        {
            // Arrange
            var query = new Query().From("Student");
            _decomposer.selectClause(query).Returns("SELECT [FirstName]");
            _decomposer.fromClause(query).Returns("FROM [Student]");
            _decomposer.whereClause(query).Returns(new CompileResult
            {
                RawQuery = " WHERE [Id] = @p1",
                Bindings = [1]
            });

            // Act
            var result = _sut.Compile(query);

            // Assert
            result.RawQuery.Should().Be("SELECT [FirstName] FROM [Student]  WHERE [Id] = @p1");
        }

        [Fact]
        public void Compile_ShouldReturnBindingsFromWhereClause_WhenDecomposerReturnsBindings()
        {
            // Arrange
            var query = new Query().From("Student");
            _decomposer.selectClause(query).Returns("SELECT *");
            _decomposer.fromClause(query).Returns("FROM [Student]");
            _decomposer.whereClause(query).Returns(new CompileResult
            {
                RawQuery = " WHERE [Grade] = @p1 AND [IsMale] = @p2",
                Bindings = [12, 1]
            });

            // Act
            var result = _sut.Compile(query);

            // Assert
            result.Bindings.Should().BeEquivalentTo(new List<object> { 12, 1 });
        }

        [Fact]
        public void Compile_ShouldCallSelectClauseAndFromClauseExactlyOnce_WhenInvoked()
        {
            // Arrange
            var query = new Query().From("Student");
            _decomposer.whereClause(query).Returns(new CompileResult());

            // Act
            _sut.Compile(query);

            // Assert
            _decomposer.Received(1).selectClause(query);
            _decomposer.Received(1).fromClause(query);
        }

        [Fact]
        public void Compile_Should_CallWhereClauseTwice_WhenInvoked()
        {
            // Arrange
            var query = new Query().From("Student");
            _decomposer.whereClause(query).Returns(new CompileResult());

            // Act
            _sut.Compile(query);

            // Assert
            _decomposer.Received(1).whereClause(query);
        }

        [Fact]
        public void Compile_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            // Arrange
            Action act = () => _sut.Compile(null!);

            // Act & Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenDecomposerIsNull()
        {
            // Arrange
            Action act = () => new QueryCompiler(null!);

            // Act & Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
