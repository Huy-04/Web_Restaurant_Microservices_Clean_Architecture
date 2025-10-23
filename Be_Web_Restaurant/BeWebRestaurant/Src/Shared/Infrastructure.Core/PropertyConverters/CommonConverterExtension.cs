using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Core.PropertyConverters
{
    public static class CommonConverterExtension
    {
        public static readonly ValueConverter<Description, string>
            DescriptionConverter = new(v => v.Value, v => Description.Create(v));

        public static readonly ValueConverter<Img, string>
            ImgConverter = new(v => v.Value, v => Img.Create(v));

    }
}