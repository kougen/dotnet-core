namespace ManualTests
{
    public class TestPersonClass
    {
        public int Age;
        public int Height;

        public TestPersonClass(int age, int height)
        {
            Age = age;
            Height = height;
        }

        public static bool TryParse(string line, out TestPersonClass result)
        {
            var elements = line.Split(';');
            var testPerson = new TestPersonClass(int.Parse(elements[0]), int.Parse(elements[1]));
            result = testPerson;
            return true;
        }
    }
}
