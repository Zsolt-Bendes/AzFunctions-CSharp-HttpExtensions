using System;

namespace HttpExtensions.Models
{
    public record PaginationData
    {
        public PaginationData(int index, int size)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));

            Index = index;
            Size = size;
        }

        public int Index { get; }
        public int Size { get; }

        public int Offset => (Index - 1) * Size;
    }
}