// Program.cs
class Program
{
    static void Main()  
    {  
        IURLShortener urlShortener = new URLShortener();  
        IURLStorage urlStorage = new URLStorage();  
        IConsoleApp app = new ConsoleApp(urlShortener, urlStorage);  
        app.Run();  
    }  
}
