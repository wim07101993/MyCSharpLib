namespace MyCSharpLib.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToDegrees(this double radians) => radians * 180 / System.Math.PI;

        public static double ToRadians(this double degrees) => degrees * System.Math.PI / 180;
    }
}
