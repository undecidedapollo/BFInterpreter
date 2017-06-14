using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFInterpeterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] fileContent;
            var validChars = "><+-.,[]".ToCharArray().ToLookup(x => x);

            try
            {
                string fileContents = File.ReadAllText(args[0]);
                fileContent = fileContents.ToCharArray();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Illegal File Name or Invalid File");
                Console.ReadKey();
                return;
            }

            Console.SetWindowSize(90, 24);

            fileContent = fileContent.Where(x => validChars.Contains(x)).ToArray();

            char[] dataList;
            try
            {
                dataList = new char[int.Parse(args[1])];
            }
            catch
            {
                dataList = new char[40000];
            }

            //List<char> dataList = new List<char>(40000);
            int dataPointer = 0;
            int programInstructionPointer = 0;

            var bracketStack = new Stack<int>();
            var sbuilder = new StringBuilder();

            while (programInstructionPointer < fileContent.Length)
            {
                switch (fileContent[programInstructionPointer])
                {
                    case '>':
                        dataPointer++;
                        break;
                    case '<':
                        dataPointer--;
                        break;
                    case '+':
                        dataList[dataPointer]++;
                        break;
                    case '-':
                        dataList[dataPointer]--;
                        break;
                    case '.':
                        var theChar = dataList[dataPointer];
                        if(theChar == 10)
                        {
                            Console.WriteLine(sbuilder.ToString());
                            sbuilder.Clear();
                        }
                        else
                        {
                            sbuilder.Append(theChar);
                        }
                       
                        break;
                    case ',':
                        dataList[dataPointer] = Console.ReadKey().KeyChar;
                        break;
                    case '[':
                        if (dataList[dataPointer] != 0)
                        {
                            bracketStack.Push(programInstructionPointer);
                        }
                        else
                        {
                            int tempBracketLevel = bracketStack.Count;
                            int intendedBracketLevel = tempBracketLevel - 1;
                            programInstructionPointer++;
                            for (int i = programInstructionPointer; i < fileContent.Length; i++)
                            {
                                if (fileContent[i] == '[')
                                {
                                    tempBracketLevel++;
                                }
                                else if (fileContent[i] == ']')
                                {
                                    tempBracketLevel--;
                                }

                                if (intendedBracketLevel == tempBracketLevel)
                                {
                                    programInstructionPointer = i;
                                    break;
                                }
                            }
                        }
                        break;
                    case ']':
                        if (dataList[dataPointer] == 0)
                        {
                            bracketStack.Pop();
                        }
                        else
                        {
                            programInstructionPointer = bracketStack.Peek();
                        }
                        break;

                }
                programInstructionPointer++;
            }

            Console.ReadKey();
        }
    }
}
