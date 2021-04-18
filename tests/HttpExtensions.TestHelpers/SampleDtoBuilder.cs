using System;
using FluentValidation;

namespace HttpExtensions.TestHelpers
{
    public class SampleDtoBuilder
    {
        private Guid _id;
        private int _integerSample;
        private double _doubleSample;
        private string _stringSample;

        public SampleDto Build() => new SampleDto(_id, _integerSample, _doubleSample, _stringSample);

        public SampleDtoBuilder WithSampleData()
        {
            var rnd = new Random();
            _id = Guid.NewGuid();
            _integerSample = rnd.Next(-100, 100);
            _doubleSample = rnd.NextDouble();
            _stringSample = "sdf";

            return this;
        }

        public SampleDtoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SampleDtoBuilder WithString(string param)
        {
            _stringSample = param;
            return this;
        }
    }

    public record SampleDto(Guid Id, int IntegerSample, double DoubleSample, string StringSample);

    public class SampleDtoValidator : AbstractValidator<SampleDto>
    {
        public SampleDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.StringSample).NotEmpty();
        }
    }
}