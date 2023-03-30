using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Implementation.Logger;
using Implementation.StandardIOManager;
using Xunit;

namespace ImplementationTest
{
    public class ReaderTests
    {
        
        private readonly Reader _reader;
        public ReaderTests()
        {
            var id = Guid.NewGuid();
            var logger = new Logger(id, "testLog.txt");
            var writer = new Writer(logger);
            _reader = new Reader(logger, writer);
        }

        [Theory]
        [InlineData(@"Resources\RT\0001_0.txt", 0)]
        [InlineData(@"Resources\RT\0001_1.txt", 1)]
        [InlineData(@"Resources\RT\0001_2.txt", 2)]
        [InlineData(@"Resources\RT\0001_3.txt", 3)]
        public void RT_0001_Given_FileWithValidData_When_ReadLineWithSingleValue_Then_ReturnsCorrectValue(string path, int expected)
        {
            using (var reader = new StreamReader(path))
            {
                var result = _reader.ReadLine<int>(reader, int.TryParse);
                reader.Close();
            
                Assert.Equal(expected, result.FirstOrDefault());    
            }
        }
        
        [Theory]
        [MemberData(nameof(RT_0011_MemberData))]
        public void RT_0011_Given_FileWithValidData_When_ReadLineWithMultipleValues_Then_ReturnsCorrectValue(string path, IEnumerable<int> elements)
        {
            using (var reader = new StreamReader(path))
            {
                var result = _reader.ReadLine<int>(reader, int.TryParse, ' ');
                reader.Close();
            
                Assert.Equal(elements, result.FirstOrDefault());    
            }
        }
        
        
        
        public static IEnumerable<object[]> RT_0011_MemberData(){
            yield return new object[] { @"Resources\RT\0002_0.txt", new List<int>{ 0,1,2,3,4 } };
            yield return new object[] { @"Resources\RT\0002_1.txt", new List<int>{ 4,2,5,8 } };
        }
    }
}
