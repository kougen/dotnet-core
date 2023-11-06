using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Implementation.IO;
using Implementation.Module;
using Infrastructure.IO;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImplementationTest
{
    public class ReaderTests
    {
        private readonly ServiceProvider _provider;

        public ReaderTests()
        {
            var collection = new ServiceCollection();
            new CoreModule(collection, new CancellationTokenSource()).RegisterServices("reader-tests");
            _provider = collection.BuildServiceProvider();
        }

        [Theory]
        [InlineData(@"Resources\RT\0001_0.txt", 0)]
        [InlineData(@"Resources\RT\0001_1.txt", 1)]
        [InlineData(@"Resources\RT\0001_2.txt", 2)]
        [InlineData(@"Resources\RT\0001_3.txt", 3)]
        public void RT_0001_Given_FileWithValidData_When_ReadLineWithSingleValue_Then_ReturnsCorrectValue(string path,
            int expected)
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);

                var result = reader.ReadLine<int>(stream, int.TryParse, out _);

                Assert.Equal(expected, result);
            }
        }

        public static IEnumerable<object[]> RT_0011_MemberData()
        {
            yield return new object[]
            {
                @"Resources\RT\0002_0.txt",
                new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    4
                }
            };
            yield return new object[]
            {
                @"Resources\RT\0002_1.txt",
                new List<int>
                {
                    4,
                    2,
                    5,
                    8
                }
            };
        }
        [Theory]
        [MemberData(nameof(RT_0011_MemberData))]
        public void RT_0011_Given_FileWithValidData_When_ReadLineWithMultipleValues_Then_ReturnsCorrectValue(
            string path, IEnumerable<int> elements)
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);

                var result = reader.ReadLine<int>(stream, int.TryParse, ' ');

                Assert.Equal(elements, result);
            }
        }

        [Fact]
        public void RT_0021_Given_FileWithValidData_When_ReadLine_Then_ReturnsTheString()
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(@"Resources\RT\readlineTest.txt");
                const string contents = "test String";

                var result = Reader.ReadLine(stream, out _);

                Assert.Equal(contents, result);
            }
        }

        public static IEnumerable<object[]> RT_0031_MemberData()
        {
            yield return new object[]
            {
                @"Resources\RT\0031_0.txt",
                new List<int>
                {
                    1,
                    2,
                    3
                }
            };
            yield return new object[]
            {
                @"Resources\RT\0031_1.txt",
                new List<int>
                {
                    4,
                    2,
                    5,
                    8
                }
            };
        }
        [Theory]
        [MemberData(nameof(RT_0031_MemberData))]
        public void
            RT_0031_Given_FileWithValidDataWithDifferentSeparator_When_ReadLineIsCalled_Then_ReturnsCorrectValue(
                string path, IEnumerable<int> elements)
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);
                var result = reader.ReadLine<int>(stream, int.TryParse, ' ', '\t');

                Assert.Equal(elements, result);
            }
        }

        public static IEnumerable<object[]> RT_0041_MemberData()
        {
            yield return new object[]
            {
                @"Resources\RT\0041_0.txt",
                new List<int>
                {
                    4,
                    2,
                    5,
                    8
                }
            };
            yield return new object[]
            {
                @"Resources\RT\0041_1.txt",
                new List<int>
                {
                    1,
                    2,
                    3
                }
            };
        }
        [Theory]
        [MemberData(nameof(RT_0041_MemberData))]
        public void RT_0041_Given_FileWithValidData_When_ReadAllLinesForOneValue_Then_ReturnsCorrectValue(string path,
            IEnumerable<int> elements)
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);
                var result = reader.ReadAllLines<int>(stream, int.TryParse);

                Assert.Equal(elements, result);
            }
        }

        public static IEnumerable<object[]> RT_0051_MemberData()
        {
            yield return new object[]
            {
                @"Resources\RT\0051_0.txt",
                new List<List<int>>
                {
                    new()
                    {
                        4,
                        2,
                        5,
                        8
                    },
                    new()
                    {
                        1,
                        1,
                        1
                    }
                }
            };
            yield return new object[]
            {
                @"Resources\RT\0051_1.txt",
                new List<List<int>>
                {
                    new()
                    {
                        4,
                        2,
                        5,
                        8
                    },
                    new()
                    {
                        1,
                        2,
                        3
                    },
                    new()
                    {
                        7,
                        8,
                        9,
                        4,
                        5
                    }
                }
            };
        }
        [Theory]
        [MemberData(nameof(RT_0051_MemberData))]
        public void RT_0051_Given_FileWithValidData_When_ReadAllLinesForMultipleValues_Then_ReturnsCorrectValue(
            string path, IEnumerable<IEnumerable<int>> elements)
        {
            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);
                var result = reader.ReadAllLines<int>(stream, int.TryParse, ' ', '\t');

                Assert.Equal(elements, result);
            }
        }

        public static IEnumerable<object[]> RT_0061_MemberData()
        {
            yield return new object[]
            {
                @"Resources\RT\0061_0.txt",
                new List<TestClasses.TestStudent>
                {
                    new("YQMHWO")
                    {
                        Subjects = new List<TestClasses.TestSubject>()
                        {
                            new()
                            {
                                Code = "INVMEG-23",
                                Credit = 3,
                                Grade = 5
                            },
                            new()
                            {
                                Code = "IP-asd",
                                Credit = 6,
                                Grade = 5
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                @"Resources\RT\0061_1.txt",
                new List<TestClasses.TestStudent>
                {
                    new("YQMHWO")
                    {
                        Subjects = new List<TestClasses.TestSubject>()
                        {
                            new()
                            {
                                Code = "INVMEG-23",
                                Credit = 3,
                                Grade = 5
                            },
                            new()
                            {
                                Code = "IP-asd",
                                Credit = 6,
                                Grade = 5
                            }
                        }
                    },
                    new("ASD123")
                    {
                        Subjects = new List<TestClasses.TestSubject>()
                        {
                            new()
                            {
                                Code = "IP-asd",
                                Credit = 6,
                                Grade = 5
                            },
                            new()
                            {
                                Code = "INVMEG-23-123",
                                Credit = 5,
                                Grade = 2
                            },
                            new()
                            {
                                Code = "TEST-0",
                                Credit = 1,
                                Grade = 4
                            }
                        }
                    }
                }
            };
        }
        [Theory]
        [MemberData(nameof(RT_0061_MemberData))]
        public void RT_0061_Given_FileWithValidData_When_ReadAllLinesWithCustomConverter_Then_ReturnsCorrectValue(
            string path, IEnumerable<TestClasses.TestStudent> students)
        {


            using (var scope = _provider.CreateScope())
            {
                var reader = scope.ServiceProvider.GetRequiredService<IReader>();
                using var stream = new StreamReader(path);
                var result = reader.ReadAllLines<TestClasses.TestStudent>(
                    stream, TestClasses.TestStudent.TryParse);

                var test = false;
                foreach (var zipElem in students.Zip(result, (e, a) => new
                         {
                             Expected = e,
                             Actual = a
                         }))
                {
                    test = zipElem.Expected.Equals(zipElem.Actual);
                }

                Assert.True(test);
            }

        }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public class TestClasses
        {
            public class TestSubject
            {
                public string Code { get; set; }
                public int Credit { get; set; }
                public double Grade { get; set; }

                public TestSubject()
                { }

                public TestSubject(string code, int credit, double grade)
                {
                    Code = code;
                    Credit = credit;
                    Grade = grade;
                }


                public override bool Equals(object obj)
                {
                    if (obj is TestSubject sub)
                    {
                        return Equals(sub);
                    }

                    return false;
                }

                private bool Equals(TestSubject other)
                {
                    return Code == other.Code && Credit == other.Credit && Grade.Equals(other.Grade);
                }
            }
            public class TestStudent
            {
                private List<TestSubject> _subjects;

                public string NeptunId { get; set; }
                public double Avg { get; private set; }
                public int CreditSum { get; private set; }

                public List<TestSubject> Subjects
                {
                    get => _subjects;
                    set
                    {
                        _subjects = value;
                        Avg = Subjects.Average(s => s.Grade);
                        CreditSum = Subjects.Sum(s => s.Credit);
                    }
                }

                public TestStudent(string neptunId)
                {
                    NeptunId = neptunId;
                }

                public static bool TryParse(string line, out TestStudent student)
                {
                    var data = line.Split(new[]
                    {
                        ' ',
                        '\t'
                    }, StringSplitOptions.RemoveEmptyEntries);
                    var tmpStudent = new TestStudent(data[0]);
                    var dataList = data.ToList();
                    dataList.RemoveAt(0);
                    dataList.RemoveAt(0);
                    var subjectList = new List<TestSubject>();
                    var tempSubject = new TestSubject();
                    for (var i = 0; i < dataList.Count; i++)
                    {
                        switch (i % 3)
                        {
                            case 0:
                                tempSubject = new TestSubject
                                {
                                    Code = dataList[i]
                                };
                                break;
                            case 1:
                                tempSubject.Credit = int.Parse(dataList[i]);
                                break;
                            case 2:
                                tempSubject.Grade = double.Parse(dataList[i]);
                                subjectList.Add(tempSubject);
                                break;
                        }
                    }

                    tmpStudent.Subjects = subjectList;
                    student = tmpStudent;
                    return true;
                }

                public override bool Equals(object obj)
                {
                    if (obj is not TestStudent other) return false;

                    return Equals(other);

                }

                private bool Equals(TestStudent other)
                {
                    return _subjects
                               .Zip(other._subjects, (s, o) => new
                               {
                                   Self = s,
                                   Other = o
                               })
                               .All(z => z.Self.Equals(z.Other))
                           && NeptunId == other.NeptunId
                           && Avg.Equals(other.Avg)
                           && CreditSum == other.CreditSum;
                }
            }
        }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    }
}
