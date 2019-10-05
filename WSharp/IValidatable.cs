using System;

namespace WSharp
{
    /// <summary>
    /// An <see cref="IValidatable"/> can validate itself.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validates this object and puts all of the errors it comes across in a list.
        /// (Checks properties recursivly).
        /// </summary>
        /// <param name="errors">List of all the errors this object has.</param>
        /// <returns><see langword="true"/>: object is valid, <see langword="false"/>: objet is invalid.</returns>
        bool TryValidate(out AggregateException errors);

        /// <summary>
        /// Validates this object.
        /// An <see cref="AggregateException"/> is thrown when this object is not valid.
        /// (Checks properties recursivly).
        /// </summary>
        /// <exception cref="AggregateException">
        /// An <see cref="AggregateException"/> is thrown when this object is not valid.
        /// </exception>
        void Validate();
    }
}
