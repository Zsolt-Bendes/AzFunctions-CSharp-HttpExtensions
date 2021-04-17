namespace HttpExtensions.Models
{
    public record PaginationData(int Index, int Size)
    {
        public int Offset => (Index - 1) * Size;
    }
}