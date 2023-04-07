namespace ManualTests
{
    public class TestPersonClass
    {
        public string Name;
        public int Age;
        public int Height;

        public TestPersonClass(string name, int age, int height)
        {
            Age = age;
            Height = height;
            Name = name;
        }

        public static bool TryParse(string line, out TestPersonClass result)
        {
            var elements = line.Split('\t');
            var testPerson = new TestPersonClass(elements[0], int.Parse(elements[1]), int.Parse(elements[2]));
            result = testPerson;
            return true;
        }
    }
}
