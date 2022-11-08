using EmailClientLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Test DLL");
        EmailClient ecTest = new EmailClient();

        Console.WriteLine(ecTest.TestSendEmail());
    }
}