using System;
using System.Drawing;
using System.Globalization;

namespace MyCSharpLib.Math
{
    public struct XyzwPoint : IComparable, IComparable<XyzwPoint>, IEquatable<XyzwPoint>
    {
        #region CONSTRUCTOR

        public XyzwPoint(double x = default, double y = default, double z = default, double w = default, double comparisonTolerance = 0.0001)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            ComparisonTolerance = comparisonTolerance;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public double ComparisonTolerance { get; set; }

        public double Magnitude
        {
            get
            {
                var sqVal = System.Math.Pow(X, 2) + System.Math.Pow(Y, 2) + System.Math.Pow(Z, 2) + System.Math.Pow(W, 2);
                return System.Math.Sqrt(sqVal);
            }
        }

        #endregion PROPERTIES


        #region METHODS

        public bool Equals(XyzwPoint other)
            => System.Math.Abs(other.X - X) < ComparisonTolerance &&
            System.Math.Abs(other.Y - Y) < ComparisonTolerance &&
            System.Math.Abs(other.Z - Z) < ComparisonTolerance &&
            System.Math.Abs(other.W - W) < ComparisonTolerance;

        public override bool Equals(object obj) => obj != null && (obj is XyzwPoint xyzwPoint) && Equals(xyzwPoint);

        public int CompareTo(XyzwPoint other) => Magnitude.CompareTo(other.Magnitude);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is XyzwPoint xyzwPoint))
                throw new ArgumentException($"Argument must be of type {nameof(XyzwPoint)}", nameof(obj));

            return CompareTo(xyzwPoint);
        }

        public override int GetHashCode()
        {
            var hashCode = 1464954422;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + ComparisonTolerance.GetHashCode();
            hashCode = hashCode * -1521134295 + Magnitude.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{X};{Y};{Z};{W}";

        public static XyzwPoint Parse(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (str == string.Empty)
                throw new ArgumentException("Cannot parse empty string", nameof(str));

            var split = str.Split(';');
            if (split.Length < 2)
                throw new FormatException($"Cannot parse a string that contains no ';' ({str})");

            var point = new XyzwPoint
            {
                X = double.Parse(split[0]),
                Y = double.Parse(split[1])
            };

            if (split.Length > 2)
                point.Z = double.Parse(split[2]);

            if (split.Length > 3)
                point.W = double.Parse(split[3]);

            return point;
        }

        public static bool TryParse(string str, out XyzwPoint point)
        {
            try
            {
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                if (str == string.Empty)
                    throw new ArgumentException("Cannot parse empty string", nameof(str));

                var split = str.Split(';');
                if (split.Length < 2)
                    throw new FormatException($"Cannot parse a string that contains no ';' ({str})");

                point = new XyzwPoint
                {
                    X = double.Parse(split[0]),
                    Y = double.Parse(split[1])
                };

                if (split.Length > 2)
                    point.Z = double.Parse(split[2]);

                if (split.Length > 3)
                    point.W = double.Parse(split[3]);

                return true;
            }
            catch
            {
                point = default;
                return false;
            }
        }

        #region operators

        public static XyzwPoint operator +(XyzwPoint point) => new XyzwPoint(point.X, point.Y, point.Z, point.W);
        public static XyzwPoint operator +(XyzwPoint first, XyzwPoint second) => new XyzwPoint(first.X + second.X, first.Y + second.Y, first.Z + second.Z, first.W + second.W);

        public static XyzwPoint operator -(XyzwPoint point) => new XyzwPoint(-point.X, -point.Y, -point.Z, -point.W);
        public static XyzwPoint operator -(XyzwPoint first, XyzwPoint second) => new XyzwPoint(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.W - second.W);

        public static XyzwPoint operator *(XyzwPoint first, double value) => new XyzwPoint(first.X * value, first.Y * value, first.Z * value, first.W * value);
        public static XyzwPoint operator /(XyzwPoint first, double value) => new XyzwPoint(first.X / value, first.Y / value, first.Z / value, first.W / value);

        public static bool operator ==(XyzwPoint first, XyzwPoint second) => first.Equals(second);
        public static bool operator !=(XyzwPoint first, XyzwPoint second) => !first.Equals(second);

        public static explicit operator Point(XyzwPoint xyzw) => new Point((int)xyzw.X, (int)xyzw.Y);
        public static explicit operator XyzwPoint(Point point) => new XyzwPoint(x: point.X, y: point.Y);

        #endregion operators

        #endregion METHODS
    }
}
