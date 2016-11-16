# Command Line Arguments

If your test program is constructed properly (See the document “Setting Up the Example Code”) it will process the following arguments before passing any remaining arguments to your own code. These command line arguments are not case-specific, except for the names of files, directories, test suites, and test cases.

Coarse Grind supports configuration files containing the same parameters as would the command line. In a configuration file, arguments should be one-per-line, rather than a single line space-delimited as would be the case for using the command line directly.

### RUN \<test suite name>

After loading and configuring the test program the named test suite will immediately be run. When it finishes, the test program will exit. The web interface will still be activated, to provide an emergency means of killing a test that has stopped responding. This command is necessary if Coarse Grind tests must be run as part of an automated process.

### INCLUDE \<filename>

As described above, load the following file as a one-parameter-per-line configuration file. A “DECLARE” directive will be in effect for the remainder of the file.

### INCLUDE \<directory>

If an INCLUDE directive is pointed at a directory, it will recurse that entire tree and treat every file it encounters as a configuration file described above. This can be useful if there are a large number of test suites you wish to define without recompiling.

### PORT \<port number>

Overrides the default port of 8085 for the web interface. It is not recommended to use ports such as 80 or 8080 which are typically used by other web services (one of which you might be trying to test).
 
### ADDRESS \<address>

Reserved for future use. (This may prevent you from creating your own “address” argument.)

### EXCLUDE \<test case identifier partial>

Any test case with an identifier containing the \<test case identifier partial> parameter will be skipped from any test suites that are run. This is generally only used with the “RUN” argument.

### DECLARE \<test suite name> \<test case identifier> \<test case identifier> \<test case identifier> ...

Construct a new test suite from the command line without recompiling. If this is part of a config file the declare clause ends at the end of the file. If using directly from the command line, DECLARE must be after all other arguments as all remaining arguments are assumed to be test case identifiers.
