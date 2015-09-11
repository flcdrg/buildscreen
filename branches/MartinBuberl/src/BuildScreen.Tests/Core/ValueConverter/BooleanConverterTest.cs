using System.Globalization;
using BuildScreen.Core.ValueConverter;
using BuildScreen.Tests.Helpers.Extensions;
using NUnit.Framework;

namespace BuildScreen.Tests.Core.ValueConverter
{
    [TestFixture]
    public class BooleanConverterTest
    {
        [Test]
        public void Convert_ValueIsNull_ShouldBeFalse()
        {
            BooleanConverter booleanConverter = new BooleanConverter();

            ((bool)booleanConverter.Convert(null, typeof(bool?), false, CultureInfo.InvariantCulture)).ShouldBeTrue();
        }

        [Test]
        public void Convert_ValueAndParamAreEqual_ShouldBeTrue()
        {
            BooleanConverter booleanConverter = new BooleanConverter();

            ((bool)booleanConverter.Convert(false, typeof (bool?), false, CultureInfo.InvariantCulture)).ShouldBeTrue();
            ((bool)booleanConverter.Convert(true, typeof (bool?), true, CultureInfo.InvariantCulture)).ShouldBeTrue();
        }

        [Test]
        public void Convert_ValueAndParamAreNotEqual_ShouldBeFalse()
        {
            BooleanConverter booleanConverter = new BooleanConverter();

            ((bool)booleanConverter.Convert(false, typeof(bool?), true, CultureInfo.InvariantCulture)).ShouldBeFalse();
            ((bool)booleanConverter.Convert(true, typeof(bool?), false, CultureInfo.InvariantCulture)).ShouldBeFalse();
        }

        [Test]
        public void ConvertBack_ValueAndParamAreEqual_ShouldBeTrue()
        {
            BooleanConverter booleanConverter = new BooleanConverter();

            ((bool)booleanConverter.ConvertBack(false, typeof(bool?), false, CultureInfo.InvariantCulture)).ShouldBeTrue();
            ((bool)booleanConverter.ConvertBack(true, typeof(bool?), true, CultureInfo.InvariantCulture)).ShouldBeTrue();
        }

        [Test]
        public void ConvertBack_ValueAndParamAreNotEqual_ShouldBeFalse()
        {
            BooleanConverter booleanConverter = new BooleanConverter();

            ((bool)booleanConverter.ConvertBack(false, typeof(bool?), true, CultureInfo.InvariantCulture)).ShouldBeFalse();
            ((bool)booleanConverter.ConvertBack(true, typeof(bool?), false, CultureInfo.InvariantCulture)).ShouldBeFalse();
        }
    }
}
