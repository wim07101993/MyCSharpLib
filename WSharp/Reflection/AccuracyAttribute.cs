using System;

namespace WSharp.Reflection
{
    /// <summary>Attribute that describes how accurate a value should be displayed.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class AccuracyAttribute : Attribute
    {
        /// <summary>Constructs a new instance of the <see cref="AccuracyAttribute"/>.</summary>
        /// <param name="accuracy">The accurracy at which a value should be displayed.</param>
        public AccuracyAttribute(double accuracy)
        {
            Accuracy = accuracy;
        }

        /// <summary>The accurracy at which a value should be displayed.</summary>
        public double Accuracy { get; }
    }
}
