using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WSharp.Extensions;

namespace WSharp.Reflection
{
    /// <inheritdoc/>
    /// <summary>
    ///     Abstraction for the <see cref="IValidatable"/> interface. It validates a classes
    ///     properties with the
    ///     <see cref="ObjectExtensions.Validate(object, out List{ValidationException})"/> method.
    ///     <para>
    ///         Also implements the <see cref="INotifyPropertyChanged"/> by extending the <see cref="BindableBase"/>.
    ///     </para>
    /// </summary>
    public abstract class AValidatable : BindableBase, IValidatable
    {
        /// <summary>
        ///     Message of the <see cref="AggregateException"/> that is thrown when the
        ///     <see cref="Validate"/> method is called an this object is not valid.
        /// </summary>
        protected virtual string ValidateErrorMessage => $"There is something wrong with the {GetType().Name}, check inner exceptions for details.";

        /// <inheritdoc/>
        public virtual bool TryValidate(out AggregateException errors)
        {
            var isValid = ObjectExtensions.Validate(this, out var errorList);
            errors = new AggregateException(ValidateErrorMessage, errorList);
            return isValid;
        }

        /// <inheritdoc/>
        /// <summary>
        ///     This method calles the <see cref="TryValidate(out AggregateException)"/> method to
        ///     validate this object.
        /// </summary>
        public virtual void Validate()
        {
            if (!TryValidate(out var errorList))
                throw new AggregateException(ValidateErrorMessage, errorList);
        }
    }
}