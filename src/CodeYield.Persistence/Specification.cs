using System.Linq.Expressions;

namespace CodeYield.Persistence
{
    /// <summary>
    /// Base class for specifications that can be composed with <see cref="And"/>,
    /// <see cref="Or"/>, and <see cref="Not"/> operators.
    /// Subclasses override <see cref="ToExpression"/> to define the filtering logic.
    /// </summary>
    /// <typeparam name="T">The entity type this specification applies to.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <inheritdoc />
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <inheritdoc />
        public bool IsSatisfiedBy(T entity) => ToExpression().Compile()(entity);

        /// <summary>Combines two specifications with a logical AND.</summary>
        public Specification<T> And(Specification<T> other)
        {
            var left = ToExpression();
            var right = other.ToExpression();
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left, parameter),
                Expression.Invoke(right, parameter));
            return new CompositeSpecification<T>(
                Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        /// <summary>Combines two specifications with a logical OR.</summary>
        public Specification<T> Or(Specification<T> other)
        {
            var left = ToExpression();
            var right = other.ToExpression();
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(left, parameter),
                Expression.Invoke(right, parameter));
            return new CompositeSpecification<T>(
                Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        /// <summary>Negates this specification.</summary>
        public Specification<T> Not()
        {
            var expression = ToExpression();
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.Not(Expression.Invoke(expression, parameter));
            return new CompositeSpecification<T>(
                Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        /// <summary>Implicitly converts a specification to its expression form for LINQ providers.</summary>
        public static implicit operator Expression<Func<T, bool>>(Specification<T> spec) =>
            spec.ToExpression();
    }

    /// <summary>
    /// A specification created by composing other specifications via And/Or/Not.
    /// </summary>
    internal sealed class CompositeSpecification<T>(Expression<Func<T, bool>> expression) : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression = expression;

        /// <inheritdoc />
        public override Expression<Func<T, bool>> ToExpression() => _expression;
    }
}
