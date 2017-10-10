using System;
using Complex = System.Numerics.Complex;

namespace simTest
{
    public class Something : IEquatable<Something>
    {
        public enum SomeColor
        {
            None,
            Black,
            Red,
            Green,
            Yellow,
            Blue,
            White,
            Octarine
        }

        public string Name;
        public Guid Uid;
        public int Number { get; set; }
        public double Value { get; set; }
        public SomeColor Color { get; set; }
        public Complex Coordinates;

        public bool Equals(Something other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return (Name == other.Name && Uid == other.Uid && Number == other.Number && Value == other.Value && Color == other.Color && Coordinates == other.Coordinates);
        }
    }
}
