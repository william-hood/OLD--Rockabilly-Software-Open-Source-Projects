using System;
using System.Reflection;
using System.IO;
using Rockabilly.CoarseGrind;

namespace CoarseGrindRunner
{
    class MainClass : TestProgram
    {
        public static void Main(string[] args)
        {
            MainClass thisInstance = new MainClass();
            string outputFolder = CoarseGrind.DEFAULT_PARENT_FOLDER + Path.DirectorySeparatorChar + DateTime.Now.ToString(CoarseGrind.DateFormatString);

            foreach (string thisArg in args)
            {
                try
                {
                    Assembly candidate = Assembly.LoadFile(thisArg);
                    thisInstance.Run(candidate, outputFolder);
                } catch
                {
                    //NO-OP for now. If there's a global log then use it.
                }
            }
        }
    }
}
