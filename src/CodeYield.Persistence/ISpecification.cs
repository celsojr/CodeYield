using System.Linq.Expressions;

namespace CodeYield.Persistence
{
    /// <summary>
    /// Defines a reusable, composable query predicate for filtering entities.
    /// Specifications encapsulate business rules as objects, enabling testable and
    /// reusable query logic across repository implementations.
    /// </summary>
    /// <typeparam name="T">The entity type this specification applies to.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>Evaluates whether the specified entity satisfies this specification.</summary>
        bool IsSatisfiedBy(T entity);

        /// <summary>Returns an expression tree representation of this specification for
        /// translation to SQL or other query providers.</summary>
        Expression<Func<T, bool>> ToExpression();
    }
}
