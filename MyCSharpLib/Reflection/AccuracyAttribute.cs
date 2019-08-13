using System;

namespace MyCSharpLib.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class AccuracyAttribute : Attribute
    {
        public AccuracyAttribute(double accuracy)
        {
            Accuracy = accuracy;
        }

        public double Accuracy { get; }
    }
}
