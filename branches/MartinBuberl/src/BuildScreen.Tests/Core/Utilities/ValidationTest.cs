using BuildScreen.Core.Utilities;
using BuildScreen.Tests.Helpers.Extensions;
using NUnit.Framework;

namespace BuildScreen.Tests.Core.Utilities
{
    [TestFixture]
    public class ValidationTest
    {
        [Test]
        public void IsDomain_Null_ShouldBeFalse()
        {
            Validation.IsDomain(null).ShouldBeFalse();
        }

        [Test]
        public void IsDomain_Empty_ShouldBeFalse()
        {
            Validation.IsDomain(string.Empty).ShouldBeFalse();
        }

        [Test]
        public void IsDomain_InvalidDomain_ShouldBeFalse()
        {
            Validation.IsDomain(" ").ShouldBeFalse();
            Validation.IsDomain("http://").ShouldBeFalse();
            Validation.IsDomain(".com").ShouldBeFalse();
            Validation.IsDomain("foo.1").ShouldBeFalse();
            Validation.IsDomain("www.foo.com/bar").ShouldBeFalse();
        }

        [Test]
        public void IsDomain_ValidDomain_ShouldBeTrue()
        {
            Validation.IsDomain("localhost").ShouldBeTrue();
            Validation.IsDomain("www.foo.com").ShouldBeTrue();
            Validation.IsDomain("bar.foo.com").ShouldBeTrue();
            Validation.IsDomain("foo.com").ShouldBeTrue();
            Validation.IsDomain("foo1.com").ShouldBeTrue();
            Validation.IsDomain("äüö.com").ShouldBeTrue();
        }

        [Test]
        public void IsIPv4_Null_ShouldBeFalse()
        {
            Validation.IsIPv4(null).ShouldBeFalse();
        }

        [Test]
        public void IsIPv4_Empty_ShouldBeFalse()
        {
            Validation.IsIPv4(string.Empty).ShouldBeFalse();
        }

        [Test]
        public void IsIPv4_InvalidIPv4_ShouldBeFalse()
        {
            Validation.IsIPv4("65536").ShouldBeFalse();
            Validation.IsIPv4("aaa.aaa.aaa.aaa").ShouldBeFalse();
            Validation.IsIPv4("255.255.255.256").ShouldBeFalse();
            Validation.IsIPv4("255.255.255").ShouldBeFalse();
            Validation.IsIPv4("0.0.0.0").ShouldBeFalse();
        }

        [Test]
        public void IsIPv4_ValidIPv4_ShouldBeTrue()
        {
            Validation.IsIPv4("1.0.0.0").ShouldBeTrue();
            Validation.IsIPv4("255.255.255.255").ShouldBeTrue();
        }

        [Test]
        public void IsPort_Null_ShouldBeFalse()
        {
            Validation.IsPort(null).ShouldBeFalse();
        }

        [Test]
        public void IsPort_Empty_ShouldBeFalse()
        {
            Validation.IsPort(string.Empty).ShouldBeFalse();
        }

        [Test]
        public void IsPort_InvalidPort_ShouldBeFalse()
        {
            Validation.IsPort("65536").ShouldBeFalse();
            Validation.IsPort("2147483648").ShouldBeFalse(); // OverflowException
            Validation.IsPort("a").ShouldBeFalse();
        }

        [Test]
        public void IsPort_ValidPort_ShouldBeTrue()
        {
            Validation.IsPort("0").ShouldBeTrue();
            Validation.IsPort("65535").ShouldBeTrue();
        }
    }
}
