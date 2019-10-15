namespace WSharp.Extensions
{
    /// <summary>Extension methods for <see cref="double"/></summary>
    public static class DoubleExtensions
    {
        /// <summary>Converts radians to degrees.</summary>
        /// <param name="radians">The radians.</param>
        /// <returns>The degrees.</returns>
        public static double ToDegrees(this double radians) => radians * 180 / System.Math.PI;

        /// <summary>Converts degrees to radians.</summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns>The radians.</returns>
        public static double ToRadians(this double degrees) => degrees * System.Math.PI / 180;
    }
}