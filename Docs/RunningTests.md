# Running Tests<img align="right" src="Icons/running.png"/>

The recommended way to run tests is to start your test program with appropriate command line arguments and use the web interface. If you know the exact name of the suite you want, add “run \<suite name>” to the command line arguments which will cause the test program to immediately run the suite and exit. If you know the exact test case identifiers, as returned by their getIdentifier() methods, you may run individual tests by creating a custom test suite on the command line: “run CUSTOM declare CUSTOM \<test1> \<test2> \<test3> ... \<testN>. When running from the command line, the web interface will still be available if the suite needs to be terminated prematurely.

- Carry out the instructions in the document “Setting Up the Example Code”. In Eclipse, right-click the project “CoarseGrind Examples” and select Run As → Java Application. In Visual/Xamarin Studio (or Monodevelop) Make sure “CoarseGrind Examples” is the default start project.

- Determine the IP address of the computer you're running the Example Code from.

- Point a web browser to port 8085 of that IP address. If you are using the web browser from the same computer the Example Code is running from, point it at “http://localhost:8085”. You may use either a desktop of mobile browser.

- On the main page of the interface is the selection of tests to run. This varies with the test program. A test suite with less than 25 cases is considered small. 100 or more test cases is considered large.

- When the test is running you have the option of stopping the current test or the entire test suite (this will cause an “Inconclusive” result for the test in-progress). NOTE: Neither Java 8, nor the current .NET framework, like to “really” kill a thread. It only allows “Please, Sir, will you kindly stop running?” In the context of Software Testing, we don't need “please” we need “KILL IT WITH FIRE.” Coarse Grind does the best it can to force the test to exit, but you are advised to code your test cases to eventually time-out and exit on their own whenever possible.

- After the test has run, test case objects should be reset to the same state as when the program was first run. (Coarse Grind will internally destroy and recreate all test cases and suites between test runs.) If there is ever contamination from a previous test run, please report this as a bug [via email](mailto:william.arthur.hood@gmail.com).

- You may terminate the test program from the web interface. It will need to be manually restarted the same way as before.

- Coarse Grind assumes that it is running on a remote server as a “Runnable JAR,” and provides a means to download test data to your desktop computer. Test results will be a series of folders and a summary CSV spreadsheet. The summary spreadsheet should easily import into Microsoft Excel, LibreOffice Calc, and other common spreadsheet programs. In most cases double-clicking the summary file is sufficient to begin the import process (you may need to tell your spreadsheet program to ONLY consider commas as delimiters).

- If you are running the Coarse Grind test program from your desktop machine, the test results are natively stored in a folder named “Test Results” in either the Documents folder of your user account, or directly off your home folder.
 