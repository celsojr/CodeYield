using CodeYield.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeYield.Tests
{
    public class MediatorTests
    {
        public record SimpleCommand(string Value) : ICommand<string>;
        public record SimpleQuery(string Value) : IQuery<string>;

        public record ValidatedCommand(string Value) : ICommand<Result<string>>;

        public class SimpleCommandHandler : ICommandHandler<SimpleCommand, string>
        {
            public Task<string> HandleAsync(SimpleCommand command, CancellationToken ct = default) =>
                Task.FromResult($"handled:{command.Value}");
        }

        public class SimpleQueryHandler : IQueryHandler<SimpleQuery, string>
        {
            public Task<string> HandleAsync(SimpleQuery query, CancellationToken ct = default) =>
                Task.FromResult($"queried:{query.Value}");
        }

        public class ValidatedCommandHandler : ICommandHandler<ValidatedCommand, Result<string>>
        {
            public Task<Result<string>> HandleAsync(ValidatedCommand command, CancellationToken ct = default) =>
                Task.FromResult(Result<string>.Success($"handled:{command.Value}"));
        }

        public class TestValidator : IValidator<ValidatedCommand>
        {
            public ValueTask<ValidationResult> ValidateAsync(ValidatedCommand instance, CancellationToken ct = default)
            {
                if (string.IsNullOrEmpty(instance.Value))
                    return ValueTask.FromResult(ValidationResult.Failure("Value is required"));

                return ValueTask.FromResult(ValidationResult.Success());
            }
        }

        [Fact]
        public async Task SendAsync_Command_ReturnsHandlerResult()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));
            services.AddMediator(typeof(SimpleCommandHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            var result = await mediator.SendAsync(new SimpleCommand("hello"));

            Assert.Equal("handled:hello", result);
        }

        [Fact]
        public async Task SendAsync_Query_ReturnsHandlerResult()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));
            services.AddMediator(typeof(SimpleQueryHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            var result = await mediator.SendAsync(new SimpleQuery("world"));

            Assert.Equal("queried:world", result);
        }

        [Fact]
        public async Task SendAsync_WithValidationBehavior_FailsValidation()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));
            services.AddMediator(false, typeof(ValidatedCommandHandler).Assembly);
            services.AddValidationBehavior();
            services.AddTransient<IValidator<ValidatedCommand>, TestValidator>();
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            var result = await mediator.SendAsync(new ValidatedCommand(""));

            Assert.False(result.IsSuccess);
            Assert.Contains("Value is required", result.Error);
        }

        [Fact]
        public async Task SendAsync_WithValidationBehavior_PassesValidation()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));
            services.AddMediator(false, typeof(ValidatedCommandHandler).Assembly);
            services.AddValidationBehavior();
            services.AddTransient<IValidator<ValidatedCommand>, TestValidator>();
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            var result = await mediator.SendAsync(new ValidatedCommand("valid"));

            Assert.True(result.IsSuccess);
            Assert.Equal("handled:valid", result.Data);
        }

        [Fact]
        public async Task AddMediator_WithoutBehaviors_NoPipeline()
        {
            var services = new ServiceCollection();
            services.AddMediator(registerBehaviors: false, typeof(SimpleCommandHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            var result = await mediator.SendAsync(new SimpleCommand("test"));

            Assert.Equal("handled:test", result);
        }

        [Fact]
        public async Task AddMediator_WithBehaviors_RegistersPipeline()
        {
            var services = new ServiceCollection();
            services.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));
            services.AddMediator(typeof(SimpleCommandHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var behaviors = provider.GetServices<IPipelineBehavior<SimpleCommand, string>>().ToList();
            Assert.Equal(3, behaviors.Count);
        }
    }
}
