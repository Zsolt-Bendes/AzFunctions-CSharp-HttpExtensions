namespace HttpExtensions.TestHelpers
{
    public static partial class Mother
    {
        public static class SampleDto
        {
            private static SampleDtoBuilder _builder => new ();

            public static SampleDtoBuilder Random => _builder.WithSampleData();
        }
    }
}