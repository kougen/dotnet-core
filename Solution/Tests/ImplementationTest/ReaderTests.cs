using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Implementation.IO;
using Implementation.IO.Factories;
using Implementation.Logger.Factories;
using Xunit;

namespace ImplementationTest
{
    public class ReaderTests
    {
        [Theory]
        [InlineData(@"Resources\RT\0001_0.txt", 0)]
        [InlineData(@"Resources\RT\0001_1.txt", 1)]
        [InlineData(@"Resources\RT\0001_2.txt", 2)]
        [InlineData(@"Resources\RT\0001_3.txt", 3)]
        public void RT_0001_Given_FileWithValidData_When_ReadLineWithSingleValue_Then_ReturnsCorrectValue(string path, int expected)
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(path);

            var result = reader.ReadLine<int>(stream, int.TryParse);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(RT_0011_MemberData))]
        public void RT_0011_Given_FileWithValidData_When_ReadLineWithMultipleValues_Then_ReturnsCorrectValue(string path, IEnumerable<int> elements)
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(path);

            var result = reader.ReadLine<int>(stream, int.TryParse, ' ');

            Assert.Equal(elements, result);
        }

        [Fact]
        public void RT_0021_Given_FileWithValidData_When_ReadLine_Then_ReturnsTheString()
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(@"Resources\RT\readlineTest.txt");
            const string contents = "test String";
            
            var result = reader.ReadLine(stream);

            Assert.Equal(contents, result);

        }
        
        [Theory]
        [MemberData(nameof(RT_0031_MemberData))]
        public void RT_0031_Given_FileWithValidDataWithDifferentSeparator_When_ReadLineIsCalled_Then_ReturnsCorrectValue(string path, IEnumerable<int> elements)
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(path);
            var result = reader.ReadLine<int>(stream, int.TryParse, ' ', '\t');

            Assert.Equal(elements, result);

        }
        
        [Theory]
        [MemberData(nameof(RT_0041_MemberData))]
        public void RT_0041_Given_FileWithValidData_When_ReadAllLinesForOneValue_Then_ReturnsCorrectValue(string path, IEnumerable<int> elements)
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(path);
            var result = reader.ReadAllLines<int>(stream, int.TryParse);

            Assert.Equal(elements, result);

        }
        
        [Theory]
        [MemberData(nameof(RT_0051_MemberData))]
        public void RT_0051_Given_FileWithValidData_When_ReadAllLinesForMultipleValues_Then_ReturnsCorrectValue(string path, IEnumerable<IEnumerable<int>> elements)
        {
            var reader = InitializeReader();
            using var stream = new StreamReader(path);
            var result = reader.ReadAllLines<int>(stream, int.TryParse, ' ', '\t');

            Assert.Equal(elements, result);

        }

        public static IEnumerable<object[]> RT_0011_MemberData()
        {
            yield return new object[] { @"Resources\RT\0002_0.txt", new List<int> { 0, 1, 2, 3, 4 } };
            yield return new object[] { @"Resources\RT\0002_1.txt", new List<int> { 4, 2, 5, 8 } };
        }

        public static IEnumerable<object[]> RT_0031_MemberData()
        {
            yield return new object[] { @"Resources\RT\0031_0.txt", new List<int> {  1, 2, 3  } };
            yield return new object[] { @"Resources\RT\0031_1.txt", new List<int> {  4, 2, 5, 8 } };
        }
        
        public static IEnumerable<object[]> RT_0041_MemberData()
        {
            yield return new object[] { @"Resources\RT\0041_0.txt", new List<int> {  4, 2, 5, 8  } };
            yield return new object[] { @"Resources\RT\0041_1.txt", new List<int> {  1, 2, 3 } };
        }
        
        public static IEnumerable<object[]> RT_0051_MemberData()
        {
            yield return new object[] { @"Resources\RT\0051_0.txt", new List<List<int>> { new() { 4, 2, 5, 8 }, new() { 1, 1, 1 } } };
            yield return new object[] { @"Resources\RT\0051_1.txt", new List<List<int>> { new() { 4, 2, 5, 8 }, new() { 1, 2, 3 }, new() { 7, 8, 9, 4, 5 } } };
        }
        
        private static Reader InitializeReader()
        {
            var id = Guid.NewGuid();
            using var logger = new LoggerFactory().CreateLogger(id);
            var ioFactory = new IOFactory();
            return new Reader(logger, ioFactory.CreateWriter(logger));
        }
    }
}
