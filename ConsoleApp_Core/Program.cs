using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace ConsoleApp_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            List<DirectoryInfo> dirInfo = new List<DirectoryInfo>();

            while (flag)
            {
                ShowMenu();

                char key = Console.ReadKey().KeyChar;
                switch(key)
                {
                    case '1':
                        FizzBuzz();
                        break;
                    case '2':
                        dirInfo = DeepDive();
                        break;
                    case '3':
                        DrownItDown(ref dirInfo);
                        break;
                    case '4':
                        flag = false;
                        break;
                    default:
                        ShowMenu();
                            break;
                }

            }
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Which method would you like to use?\n\n");
            Console.WriteLine("1. FizzBuzz");
            Console.WriteLine("2. DeepDive");
            Console.WriteLine("3. DrownItDown");
            Console.WriteLine("4. Exit");
        }
        static void FizzBuzz()
        {
            Console.Clear();
            Console.WriteLine("Enter number from 0 to 1000");

            string input = Console.ReadLine();
            int number;
            try
            {
                number = Int32.Parse(input);

                if (number > 1000 || number < 0)
                {
                    Console.WriteLine("ENTER NUMBER 0-1000");
                    Console.ReadKey();
                    return;
                }
                if (number % 6 == 0)
                    Console.WriteLine("FizzBuzz");
                else if (number % 3 == 0)
                    Console.WriteLine("Buzz");
                else if (number % 2 == 0)
                    Console.WriteLine("Fizz");
                else Console.WriteLine("INDIVISIBLE BY 2 OR 3");
                Console.ReadKey();
            }
            catch(FormatException)
            {
                Console.WriteLine("{0}:Bad format", input);
                Console.WriteLine("Press Enter to Continue");
                Console.ReadKey();
                return;
            }
        }

        static private List<DirectoryInfo> DeepDive()
        {
            Console.Clear();
            Console.WriteLine("How many folders should this method create?");
            List<DirectoryInfo> dirInfo = new List<DirectoryInfo>();

            string input = Console.ReadLine();
            int number;

            try
            {
                number = Int32.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0}:Bad format", input);
                Console.WriteLine("Press Enter to Continue");
                Console.ReadKey();
                return dirInfo;
            }

            if (number <= 0 || number > 5)
            {
                Console.WriteLine("Number of directiories must be 1-5 \n\nPress Enter To Continue");
                Console.ReadKey();
                return dirInfo;
            }

            string path = System.IO.Directory.GetCurrentDirectory();

            dirInfo = CreateFolders_r(number, path, ref dirInfo);

            Console.WriteLine("Directories have been created in: {0}", path);

            //Console.WriteLine(dirInfo[0].FullName);
            Console.WriteLine("Press Enter to Continue");
            Console.ReadKey();
            return dirInfo;
        }

        private static ref List<DirectoryInfo> CreateFolders_r(int n, string path, ref List<DirectoryInfo> dirInfo)
        {
            if (n == 0)
                return ref dirInfo;
            
            string name = Guid.NewGuid().ToString();
            try
            {
                path = Path.Combine(path, name);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return ref dirInfo;
            }
            
            if (System.IO.Directory.Exists(path)) { Console.WriteLine("Directory already exists"); return ref dirInfo; }

            System.IO.DirectoryInfo dir = System.IO.Directory.CreateDirectory(path);
        
            dirInfo.Add(dir);

            return ref CreateFolders_r(n - 1, path, ref dirInfo);
        }

        private static void DrownItDown(ref List<DirectoryInfo> dirInfo)
        {
            Console.Clear();
            Console.WriteLine("On which level should file be created? (1-5)");
            string input = Console.ReadLine();
            int level;

            try
            {
                level = Int32.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0}:Bad format", input);
                Console.WriteLine("Press Enter to Continue");
                Console.ReadKey();
                return;
            }
            if (dirInfo.Count == 0 || dirInfo.Count<level || level < 0)
            {
                Console.WriteLine("This directory does not exist. You can create one with DeepDive method \n\nPress Enter To Continue");
                Console.ReadKey();
                return;
            }

            string path = dirInfo[level - 1].FullName + Path.DirectorySeparatorChar + "file";
            if(File.Exists(path))
            {
                Console.WriteLine("File already exists, overwrite file? (Y/N)");
                bool flag = true;
                while (flag)
                {
                    char answer = Console.ReadKey().KeyChar;
                    switch (char.ToLower(answer))
                    {
                        case 'y':
                            File.Delete(path);
                            flag = false;
                            break;
                        case 'n':
                            return;
                        default:
                            Console.WriteLine("\nAnswer Y (for yes) or N (for no)");
                            break;
                    }
                }
            }
            File.Create(path).Dispose();
            Console.Write("\nFile has been created in : {0}\n\nPress Enter To Continue", path);
            Console.ReadKey();
            return;
        }
       
    }
}
