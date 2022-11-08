using EmailClientLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Test DLL");
        EmailClient ecTest = new EmailClient();

        String bodyBlock = "Drinking water is really important.\n" +
            "The average person can only go 72 hours at most without water.";

        ecTest.CreateEmail("Remember to drink water", bodyBlock, "abraham96@ethereal.email");
        ecTest.SendEmail();

        ecTest.UpdateRecipient("zpage3@hotmail.com");
        ecTest.SendEmail();
    }
}