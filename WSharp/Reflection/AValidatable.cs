using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using WSharp.Extensions;
using WSharp.Observables;

namespace WSharp.Reflection
{
    /// <inheritdoc />
    /// <summary>
    ///     Abstraction for the <see cref="IValidatable" /> interface. It validates a classes
    ///     properties with the <see cref="ObjectExtensions.Validate(object, out
    ///     List{ValidationException})" /> method.
    ///     <para>
    ///         Also implements the <see cref="INotifyPropertyChanged" /> by extending the <see
    ///         cref="BindableBase" />.
    ///     </para>
    /// </summary>
    public abstract class AValidatable : ObservableObject, IValidatable
    {
        /// <summary>
        ///     Message of the <see cref="AggregateException" /> that is thrown when the <see
        ///     cref="Validate" /> method is called an this object is not valid.
        /// </summary>
        protected virtual string ValidateErrorMessage => $"There is something wrong with the {GetType().Name}, check inner exceptions for details.";

        /// <summary>
        ///     Validates this object and puts all of the errors it comes across in a list. (Checks
        ///     properties recursivly).
        /// </summary>
        /// <param name="errors">List of all the errors this object has.</param>
        /// <returns>
        ///     <see langword="true" />: object is valid, <see langword="false" />: objet is invalid.
        /// </returns>
        public virtual bool TryValidate(out AggregateException errors)
        {
            var isValid = this.Validate(true, out var errorList);
            errors = new AggregateException(ValidateErrorMessage, errorList);
            return isValid;
        }

        /// <summary>
        ///     This method calles the <see cref="TryValidate(out AggregateException)" /> method to
        ///     validate this object.
        /// </summary>
        /// <exception cref="AggregateException">
        ///     An <see cref="AggregateException" /> is thrown when this object is not valid.
        /// </exception>
        public virtual void Validate()
        {
            if (!TryValidate(out var errorList))
                throw new AggregateException(ValidateErrorMessage, errorList);
        }
    }
}
