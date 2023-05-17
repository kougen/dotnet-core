using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Implementation.Logger;
using Infrastructure.Logger;
using Xunit;

namespace ImplementationTest
{
    public class LoggerTests
    {
        [Theory]
        [MemberData(nameof(GetMemberData_0001))]
        async Task LT_0001_Given_IdOnly_When_WriteAsyncCalled_Then_DeletesTheFile(Guid id)
        {
            var logger = new Logger(id);
            await logger.LogWriteAsync("test1234");
            
            logger.Dispose();
            var exists = File.Exists($"{id}.txt");
            
            Assert.True(!exists);
        }

        [Theory]
        [MemberData(nameof(GetMemberData_0011))]
        async Task LT_0011_Given_IdAndFilepath_When_WriteAsyncCalled_Then_DeletesTheFile(Guid id, string name)
        {
            var logger = new Logger(id, name);
            await logger.LogWriteAsync("test1234");
            
            logger.Dispose();
            var exists = File.Exists($"{name}.txt");
            
            Assert.True(!exists);
        }

        [Fact] 
        void LT_0015_Given_NullFilePath_When_ConstructorCalled_Then_ThrowsArgumentNullException()
        {
            var ex = Record.Exception(() => { _ = new Logger(Guid.NewGuid(), null); });

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [InlineData("sometext")]
        [InlineData("")]
        [InlineData(@"dksfjds opsdjopfj \pfsdpo kopsdfkop")]
        [InlineData(@"日本語のテクスト")]
        async Task LT_0021_Given_ValidString_When_WriteAsync_Then_ReturnsTheString(string message)
        {
            var logger = CreateLogger(nameof(LT_0021_Given_ValidString_When_WriteAsync_Then_ReturnsTheString));
            
            var result = await logger.LogWriteAsync(message);
            
            Assert.Equal(message, result);
        }
        
        [Theory]
        [InlineData("sometext")]
        [InlineData("")]
        [InlineData(@"dksfjds opsdjopfj \pfsdpo kopsdfkop")]
        [InlineData(@"日本語のテクスト")]
        async Task LT_0031_Given_ValidString_When_WriteLineAsync_Then_ReturnsTheStringWithNewLine(string message)
        {
            var logger = CreateLogger(nameof(LT_0031_Given_ValidString_When_WriteLineAsync_Then_ReturnsTheStringWithNewLine));
            var expected = $"{message}\n";
            
            var result = await logger.LogWriteLineAsync(message);
            
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetMemberData_0001()
        {
            yield return new object[] { Guid.NewGuid() };
            yield return new object[] { Guid.NewGuid() };
            yield return new object[] { Guid.NewGuid() };
        }
        
        public static IEnumerable<object[]> GetMemberData_0011()
        {
            yield return new object[] { Guid.NewGuid(), "test1.txt" };
            yield return new object[] { Guid.NewGuid(), "test2.txt" };
            yield return new object[] { Guid.NewGuid(), "test3.txt" };
        }

        private Logger CreateLogger(string testName)
        {
            return new Logger(Guid.NewGuid(), testName);
        }
    }
}