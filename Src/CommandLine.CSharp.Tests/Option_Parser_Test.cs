using NUnit.Framework;
using System;

/// <summary>
/// 
/// name:Option_Parser_Test
/// author:Йозг
/// vindicator:
/// versions:
/// introduce:
/// note:
/// 
/// 
/// list:
/// 
/// 
/// 
/// </summary>
namespace CommandLine.CSharp.Tests
{
    [TestFixture]
    public class Option_Parser_Test
    {
        [Flags]
        public enum TestOptionEnum
        {
            enum1 = 0,
            enum2 = 1,
            enum3 = 2,
            enum4 = 4,
        }

        private class TestOptionObject
        {
            [Option('s', "short")]
            private short m_ShortValue;

            public short shortValue
            {
                get
                {
                    return m_ShortValue;
                }
            }

            [Option]
            public int intValue { get; set; }

            [Option]
            public static string staticStringValue;

            [Option(ignoreEmpty = true)]
            public string stringValue;

            [Option("enum")]
            public TestOptionEnum enumValue;
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(100)]
        [TestCase(10)]
        [TestCase(-123)]
        [TestCase(0)]
        public void Parser_Private_Field_Success(short value)
        {
            string[] args = new string[] { "-s", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.shortValue);
        }

        [TestCase(100)]
        [TestCase(10)]
        [TestCase(-123)]
        [TestCase(0)]
        public void Parser_Public_Field_Success(int value)
        {
            string[] args = new string[] { "-intValue", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.intValue);
        }

        [TestCase(100)]
        [TestCase(10)]
        [TestCase(-123)]
        [TestCase(0)]
        public void Parser_Public_Property_Success(int value)
        {
            string[] args = new string[] { "-intValue", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.intValue);
        }

        [TestCase("asdh")]
        [TestCase("123a123")]
        [TestCase("")]
        public void Parser_Public_Static_Property_Success(string value)
        {
            string[] args = new string[] { "-staticStringValue", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, TestOptionObject.staticStringValue);
        }

        [TestCase(TestOptionEnum.enum1)]
        [TestCase(TestOptionEnum.enum2)]
        [TestCase(TestOptionEnum.enum3)]
        public void Parser_Enum_Success(TestOptionEnum value)
        {
            string[] args = new string[] { "-enum", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);

            args[1] = ((int)value).ToString();

            obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);
        }

        [TestCase(TestOptionEnum.enum1)]
        [TestCase(TestOptionEnum.enum1 | TestOptionEnum.enum2 | TestOptionEnum.enum3)]
        [TestCase(TestOptionEnum.enum2 | TestOptionEnum.enum4)]
        [TestCase(TestOptionEnum.enum3 | TestOptionEnum.enum1)]
        public void Parser_Flag_Enum_Success(TestOptionEnum value)
        {
            string[] args = new string[] { "-enum", value.ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);

            args[1] = ((int)value).ToString();

            obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);
        }

        [TestCase(TestOptionEnum.enum1)]
        [TestCase(TestOptionEnum.enum1 | TestOptionEnum.enum2 | TestOptionEnum.enum3)]
        [TestCase(TestOptionEnum.enum2 | TestOptionEnum.enum4)]
        [TestCase(TestOptionEnum.enum3 | TestOptionEnum.enum1)]
        public void Parser_Flag_Int_Enum_Success(TestOptionEnum value)
        {
            string[] args = new string[] { "-enum", ((int)value).ToString() };

            OptionParser parser = new OptionParser();

            var obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);

            args[1] = ((int)value).ToString();

            obj = parser.Parse<TestOptionObject>(args);

            Assert.AreEqual(value, obj.enumValue);
        }

        [TestCase(100, new string[] { "-intValue", "30" }, 30)]
        [TestCase(10, new string[] { "-s", "100" }, 10)]
        [TestCase(-123, new string[] { "-enum", "30" }, -123)]
        [TestCase(0, new string[] { "-stringValue", "asd12" }, 0)]
        public void Parser_Default_Value_Success(int value, string[] args, short result)
        {
            OptionParser parser = new OptionParser();

            TestOptionObject obj = new TestOptionObject();

            obj.intValue = value;

            parser.Parse<TestOptionObject>(args, ref obj);

            Assert.AreEqual(obj.intValue, result);
        }

        [TestCase("123")]
        [TestCase("213")]
        [TestCase("asv")]
        [TestCase("")]
        public void Parser_Empty_Success(string value)
        {
            string[] args = new string[] { "-stringValue", "" };

            OptionParser parser = new OptionParser();

            var obj = new TestOptionObject();
            obj.stringValue = value;

            parser.Parse<TestOptionObject>(args, ref obj);

            Assert.AreEqual(obj.stringValue, value);
        }
    }
}