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
            char[] dataList = new char[40000];
            //List<char> dataList = new List<char>(40000);
            int dataPointer = 0;
            int programInstructionPointer = 0;
            int bracketLevel = 0;

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
                        Console.Write(dataList[dataPointer]);
                        break;
                    case ',':
                        dataList[dataPointer] = Console.ReadKey().KeyChar;
                        break;
                    case '[':
                        if (dataList[dataPointer] != 0)
                        {
                            bracketLevel++;
                        }
                        else
                        {
                            int tempBracketLevel = bracketLevel;
                            int intendedBracketLevel = bracketLevel - 1;
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
                            bracketLevel--;
                        }
                        else
                        {
                            int tempBracketLevel = bracketLevel;
                            int intendedBracketLevel = bracketLevel - 1;
                            programInstructionPointer--;
                            for (int i = programInstructionPointer; i >= 0; i--)
                            {
                                if (fileContent[i] == '[')
                                {
                                    tempBracketLevel--;
                                }else if (fileContent[i] == ']')
                                {
                                    tempBracketLevel++;
                                }

                                if (intendedBracketLevel == tempBracketLevel)
                                {
                                    programInstructionPointer = i;
                                    break;
                                }
                            }
                        }
                        break;

                }
                programInstructionPointer++;
            }

            Console.ReadKey();
        }
    }
}
