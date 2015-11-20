using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCompilerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required arguments. ");
                Console.ReadKey();
                return;
            }

            try
            {
                if (args[0] != "--convert" && args[0] != "--compile")
                {
                }
            }
            catch
            {
                Console.WriteLine("Must specify a mode");
                Console.ReadKey();
                return;
            }

            char[] fileContent;

            try
            {
                string fileContents = File.ReadAllText(args[1]);
                fileContent = fileContents.ToCharArray();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Input file could not be loaded. ");
                Console.ReadKey();
                return;
            }

            StreamWriter theFile;

            try
            {
                 theFile = File.CreateText(args[2] + ".c");
            }
            catch
            {
                Console.WriteLine("Output file could not be opened or written to.");
                Console.ReadKey();
                return;
            }

            theFile.WriteLine("#include <stdio.h>");
            theFile.WriteLine("#include <stdlib.h>");
            theFile.WriteLine("int main(int argc, char* argv[]) {");
            try
            {
                theFile.WriteLine("char array[" + args[3] + "] = {0};");
            }
            catch
            {
                theFile.WriteLine("char array[40000] = {0};");
            }

            theFile.WriteLine("char *ptr=array;");
         
            int programInstructionPointer = 0;

            while (programInstructionPointer < fileContent.Length)
            {
                switch (fileContent[programInstructionPointer])
                {
                    case '>':
                        theFile.WriteLine("++ptr;");
                        break;
                    case '<':
                        theFile.WriteLine("--ptr;");
                        break;
                    case '+':
                        theFile.WriteLine("++*ptr;");
                        break;
                    case '-':
                        theFile.WriteLine("--*ptr;");
                        break;
                    case '.':
                        theFile.WriteLine("putchar(*ptr);");
                        break;
                    case ',':
                        theFile.WriteLine("*ptr = getchar();");
                        break;
                    case '[':
                        theFile.WriteLine("while(*ptr) {");
                        break;
                    case ']':
                        theFile.WriteLine("}");
                        break;

                }
                programInstructionPointer++;
            }
            theFile.WriteLine("*ptr = getchar();");
            theFile.WriteLine(" return 0; }");
            theFile.Flush();
            theFile.Close();
            Console.WriteLine("Program converted to C successfully.");

            if (args[0] == "--compile")
            {
                string strCmdText = "/C gcc -o " + args[2] + " " + args[2] + ".c";

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = strCmdText;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine("Compilation failed");
                }
                else
                {
                    File.Delete(args[2] + ".c");
                    Console.WriteLine("Compilation successful.");

                }
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();


        }
    }
}
