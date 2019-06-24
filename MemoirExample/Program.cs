using System;
using System.IO;
using Rockabilly.Common;
using MemoirV1;
namespace MemoirExample
{
    class MainClass
    {
        public static readonly Memoir Log = new Memoir();
        public static void Main(string[] args)
        {
            Log.StartFile("its_in_the_working_dir.html");

            Log.Header("Memoir Demonstration");

            Log.Message("This is just a normal message");

            Log.Debug("Debug messages look like this");
            Log.Warn("This message has a warning icon");



            try
            {
                Log.Debug("Gonna throw a nasty ol' exception...");
                //int return2 = doSomethingTwo();

                throw new NullReferenceException("This is a FAKE null reference exception. Nothing's actually wrong.", new ArgumentException("Also a FAKE exception. Carry on."));
            }
            catch (Exception loggedException)
            {
                Log.ShowException(loggedException);
            }


            Log.Message("Just a test message again.");
            Log.Message("Woooooohoooooooooooo!!!");
            Log.Warn("Yikes");
            Log.Message("Just a test message yet again.");
            Log.SectionHeader("New Section");
            Log.Message("Just a test message again.");
            Log.Message("Woooooohoooooooooooo!!!");
            Log.Message("Just a test message again.");
            Log.Message("Woooooohoooooooooooo!!!");
            Log.Message("Just a test message again.");
            Log.Message("Woooooohoooooooooooo!!!");
            Log.Debug("OK, I'm done.");
        }
    }
}
