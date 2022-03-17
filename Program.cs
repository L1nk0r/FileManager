using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static System.Console;
using static System.IO.DriveInfo;

namespace FileManagerConsole
{
    class Program
    {

        /// <summary>
        /// This method firstly output list of disks, than
        /// cycle work with command, until a special word is entered.
        /// </summary>
        public static void FileManagerConsole()
        {
            // Вызов метода, который отображает диски.
            DrivesList();

            Write(Directory.GetCurrentDirectory() + '>');
            // Считываем строку с выполняемой в будующем командой.
            string command = ReadLine();

            // Цикл, работающий, пока не будет введена ключевая  ккоманда выхода.
            while (GetCommand(command) != "exit")
            {
                // Контроллер просто служит для того, чтобы удобнее было обрабатывать команды.
                Controller(command);
                Write(Directory.GetCurrentDirectory() + ">");
                command = ReadLine();
            }
        }

        /// <summary>
        /// This method checks the correctness of the entered command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns> Returns true if the command is correct, and false if the command is incorrect. </returns>
        public static bool IsCorrectCommand(string command)
        {
            // Список со всеми командами.
            string[] commands = { "help", "ls", "cd", "read", "copy", "move", "delete", "create", "con", "exit", "gd", "clear"};
            // В stringA я помещаю рассплитенную строку, для удобства.
            string[] stringA = command.Split(' ');

            // Проверка на то, что введённая команда кореректна.
            if (commands.Contains(stringA[0].ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method that works until the user enters the correct program.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string GetCommand(string command)
        {
            // Цикл, работающий пока не будет введена корректная команда.
            while (!IsCorrectCommand(command))
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands");
                command = ReadLine();
            }

            return command;
        }

        /// <summary>
        /// The method of disk selection.
        /// </summary>
        public static void DrivesList()
        {
            // В массив строк помещаю названия дисков.
            string[] drivesNames = new string[GetDrives().Length];
            // Просто итератор i.
            int i = 0;
            // Цикл, выводящий названия дисков.
            foreach (var drive in GetDrives())
            {
                WriteLine($"{i + 1}. {drive.Name}");
                drivesNames[i] = drive.Name;
                i += 1;
            }
            // Вторая часть метода, где пользователю даётся выбрать диск.
            WriteLine("Choose disk");
            // Считываем путь к диску.
            string drivePath = ReadLine();
            // Пока не будет введён существующий путь, делаем цикл.
            while (!drivesNames.Contains(drivePath))
            {
                WriteLine("This disk didn't exist");
                drivePath = ReadLine();
            }
            // Также записываем в переменную наш выбор.
            Directory.SetCurrentDirectory(drivePath);
        }

        /// <summary>
        /// It's basically the same method, just overloaded.
        /// You will ask why, but believe me, it's just necessary.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void DrivesList(string[] commandsParts)
        {
            if (commandsParts.Length == 1)
            {
                // В массив строк помещаю названия дисков.
                string[] drivesNames = new string[GetDrives().Length];
                // Просто итератор i.
                int i = 0;
                // Цикл, выводящий названия дисков.
                foreach (var drive in GetDrives())
                {
                    WriteLine($"{i + 1}. {drive.Name}");
                    drivesNames[i] = drive.Name;
                    i += 1;
                }
                // Вторая часть метода, где пользователю даётся выбрать диск.
                WriteLine("Choose disk");
                // Считываем путь к диску.
                string drivePath = ReadLine();
                // Пока не будет введён существующий путь, делаем цикл.
                while (!drivesNames.Contains(drivePath))
                {
                    WriteLine("This disk didn't exist");
                    drivePath = ReadLine();
                }
                // Также записываем в переменную наш выбор.
                try
                {
                    Directory.SetCurrentDirectory(drivePath);
                } catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. gd");
            }
        }

        /// <summary>
        /// The output of the user commands.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void Instruction(string[] commandsParts)
        {
            if (commandsParts.Length == 1)
            {
                // Вывод инструкции с подсказками.
                WriteLine("gd - просмотр списка дисков компьютера и выбор диска");
                WriteLine("cd <path> - переход в другую директорию(выбор папки)");
                WriteLine("ls <>/<(F)>/<(D)> - просмотр списка файлов в директории");
                WriteLine("read <path> <coding> - вывод содержимого текстового файла в консоль. кодировка может быть askii, unicode и utf8");
                WriteLine("copy <path1> <path2> - копирование файла");
                WriteLine("move <path1> <path2>/<filename> - перемещение файла в выбранную пользователем директорию. кодировка может быть askii, unicode и utf8");
                WriteLine("delete <path> - удаление файла");
                WriteLine("create <path> <coding>  - создание простого текстового файла");
                WriteLine("con <path1> ... <pathn> <coding> - конкатенация содержимого двух или более текстовых файлов и вывод результата в консоль");
                WriteLine("clear  - чистка консоли");
                WriteLine("exit  - прекращение работы файлового менеджера");
            }
            else
            {
                // Если команда не корректна, сигнализируем об этом.
                WriteLine("Incorrect command. Input 'help' to see the list of commands");
            }
        }

        /// <summary>
        /// Method for changing a folder.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void ChangeDirectory(string[] commandsParts)
        {
            // Проверяем, что количество аргументов команды корректно.
            if (commandsParts.Length == 2)
            {
                // Если пользователь ничего не вводит вторым аргументом, ничего не меняем.
                if (commandsParts[1] == "")
                {
                    WriteLine(Directory.GetCurrentDirectory());
                }
                else
                {
                    // Пытаемся выполнить переход.
                    try
                    {
                        // Проверяем переход к предыдущей папке(нужно, чтоб пользователь не вводил полный путь).
                        if (commandsParts[1].Contains(".."))
                        {
                            Directory.SetCurrentDirectory(commandsParts[1]);
                        } else
                        {
                            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + commandsParts[1]);
                        }
                    }
                    // Если перейти не удаётся, выдаём ошибку.
                    catch (Exception e)
                    {
                        WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Method for displaying the contents of the current directory.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void List(string[] commandsParts)
        {
            // Проверка на то, что команда корректна.
            if (commandsParts.Length == 1)
            {
                // Записываем в один массив строк файлы текущей директории, в другой папки.
                string[] filesInLocalDirectory = Directory.GetFiles(Directory.GetCurrentDirectory());
                string[] directoriesInLocalDirectory = Directory.GetDirectories(Directory.GetCurrentDirectory());

                // Выводим их по очереди.
                for (int i = 0; i < directoriesInLocalDirectory.Length; i++)
                {
                    WriteLine(directoriesInLocalDirectory[i]);
                }

                for (int i = 0; i < filesInLocalDirectory.Length; i++)
                {
                    WriteLine(filesInLocalDirectory[i]);
                }
            }
            else if (commandsParts.Length == 2)
            {
                // Тоже самое, что и для общего случая.
                string type = commandsParts[1];

                if (!(type == "(D)") && !(type == "(F)"))
                {
                    WriteLine("Incorrect command. Input 'help' to see the list of commands. ls");
                }
                else if (type == "(D)")
                {
                    string[] directoriesInLocalDirectory = Directory.GetDirectories(Directory.GetCurrentDirectory());
                    for (int i = 0; i < directoriesInLocalDirectory.Length; i++)
                    {
                        WriteLine(directoriesInLocalDirectory[i]);
                    }
                }
                else if (type == "(F)")
                {
                    string[] filesInLocalDirectory = Directory.GetFiles(Directory.GetCurrentDirectory());

                    for (int i = 0; i < filesInLocalDirectory.Length; i++)
                    {
                        WriteLine(filesInLocalDirectory[i]);
                    }
                }
            }
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. ls");
            }
        }

        /// <summary>
        /// Method for displaying file contents.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void ReadFile(string[] commandsParts, string coding = "UTF8")
        {
            // Пытаемся прочитать файл.
            if (commandsParts.Length >= 2)
            {
                try
                {
                    // Делаем оператор, который взависимоси от введённой кодировки, устанавливает её.
                    Encoding cod;
                    switch (coding)
                    {
                        case "Unicode":
                            cod = Encoding.Unicode;
                            break;
                        case "ASCII":
                            cod = Encoding.ASCII;
                            break;
                        default:
                            cod = Encoding.UTF8;
                            break;
                    }
                    // Считываем текст из файла в массив строк.
                    string[] allTextFromFile = File.ReadAllLines(commandsParts[1], cod);
                    // И выводим по строке.
                    for (int i = 0; i < allTextFromFile.Length; i++)
                    {
                        WriteLine(allTextFromFile[i]);
                    }
                }
                // Если считать не удалось, выводим ошибку.
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если количество аргументов не корректно, выдаём ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. copy");
            }
        }

        /// <summary>
        /// This method copy file to another directory.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void CopyFile(string[] commandsParts)
        {
            // Проверка на то, что строка нам подходит(передаётся верное количество параметров).
            if (commandsParts.Length == 3)
            {
                // Пытаемся скопировать файл.
                try
                {
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), commandsParts[1]), Path.Combine(Directory.GetCurrentDirectory(), commandsParts[2]));
                }
                // Если не удаётся, выдаём ошибку.
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если количество аргументов неверное, выдаём ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. copy");
            }
        }

        /// <summary>
        /// This method move file to another directory.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void MoveFile(string[] commandsParts)
        {
            // Проверяем, корректная ли команда.
            if (commandsParts.Length == 3)
            {
                // Пытаемся переместить файл.
                try
                {
                    File.Move(Path.Combine(Directory.GetCurrentDirectory(), commandsParts[1]), Path.Combine(Directory.GetCurrentDirectory(), commandsParts[2]));
                }
                // Если переместить не удаётся, выдаём ошибку.
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если количество аргументов не верно, выдаём ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. copy");
            }
        }

        /// <summary>
        /// This method delete file.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void DeleteFile(string[] commandsParts)
        {
            // Проверяем верное ли количество аргументов нам передано.
            if (commandsParts.Length == 2)
            {
                // Пытаемся удалить файл.
                try
                {
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), commandsParts[1]));
                }
                // Если удалить не удаётся, выводим ошибку.
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если колчисетво аргументов неверное, выводим ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. copy");
            }
        }

        /// <summary>
        /// This method creat new file and write there input strings.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void CreateFile(string[] commandsParts, string coding = "UTF8")
        {
            // Проверяем, верное ли количество аргументов нам передано.
            if (commandsParts.Length == 2)
            {
                // Пытаемся создать файл.
                try
                {
                    // Задаём нужную кодировку.
                    Encoding cod;
                    switch (coding)
                    {
                        case "Unicode":
                            cod = Encoding.Unicode;
                            break;
                        case "ASCII":
                            cod = Encoding.ASCII;
                            break;
                        default:
                            cod = Encoding.UTF8;
                            break;
                    }
                    TextWriter tw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), commandsParts[1]), true, cod);
                    string[] name_ = commandsParts[1].Split(Path.DirectorySeparatorChar);
                    tw.Close();
                }
                // Если создать не получилось, выдаём ошибку. (оу, ты читаешь каждый комментарий, а ты харош).
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если колчисетво аргументов неверное, выводим ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. create");
            }
        }

        /// <summary>
        /// This method concutinate 2 files and output their text into console.
        /// </summary>
        /// <param name="commandsParts"></param>
        public static void ConcutinateFiles(string[] commandsParts)
        {
            // Проверяем, что введено верное количество аргументов.
            if (commandsParts.Length >= 3)
            {
                // Пытаемся совместить 2 файла.
                try
                {
                    // Создаём список, чтобы хранить там текст всех файлов,
                    List<string> text = new List<string>();
                    // Сохраняем кодировку.
                    string cod = commandsParts[commandsParts.Length-1];
                    // Задаём нужную кодировку.
                    Encoding en;
                    switch (cod)
                    {
                        case "Unicode":
                            en = Encoding.Unicode;
                            break;
                        case "ASCII":
                            en = Encoding.ASCII;
                            break;
                        default:
                            en = Encoding.UTF8;
                            break;
                    }

                    // Создаём хранилище строк в одном файле.
                    string[] file;

                    // Записываем всё из файлов в лист.
                    for (int i = 1; i < commandsParts.Length; i++)
                    {
                        if (commandsParts[i] != cod)
                        {
                            file = File.ReadAllLines(commandsParts[i], en);
                            foreach (string str in file)
                            {
                                text.Add(str);
                            }
                        }
                    }

                    // Записываем всё в 1 файл.
                    TextWriter tw = new StreamWriter(commandsParts[1]);
                    foreach (string line in text)
                    {
                        tw.WriteLine(line);
                    }
                    tw.Close();

                    string[] stringA = {commandsParts[0], commandsParts[1] };

                    ReadFile(stringA);
                }
                // Если в процессе выполнения возникла ошибка, выводим её.
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }
            // Если колчисетво аргументов неверное, выводим ошибку.
            else
            {
                WriteLine("Incorrect command. Input 'help' to see the list of commands. con");
            }
        }

        /// <summary>
        /// This method choose method using comand.
        /// </summary>
        /// <param name="command"></param>
        public static void Controller(string command)
        {
            // Создаём массив строк, который будет являтся разыми частями введённой команды.
            string[] commandsParts = command.Split(' ');
            // Переменная содержащая "главную команду" (как ни странно).
            string mainCommand = commandsParts[0];

            // Перебор по главной команду и вызов нужного метода.
            switch (mainCommand)
            {
                case "help":
                    Instruction(commandsParts);
                    break;
                case "cd":
                    ChangeDirectory(commandsParts);
                    break;
                case "ls":
                    List(commandsParts);
                    break;
                case "read":
                    ReadFile(commandsParts);
                    break;
                case "copy":
                    CopyFile(commandsParts);
                    break;
                case "move":
                    MoveFile(commandsParts);
                    break;
                case "delete":
                    DeleteFile(commandsParts);
                    break;
                case "create":
                    CreateFile(commandsParts);
                    break;
                case "con":
                    ConcutinateFiles(commandsParts);
                    break;
                case "gd":
                    DrivesList(commandsParts);
                    break;
                case "clear":
                    Clear();
                    break;
            }
        }

        /// <summary>
        /// Prosto mian.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            FileManagerConsole();
        }
    }
}

//░░█▀░░░░░░░░░░░▀▀███████░░░░
//░░█▌░░░░░░░░░░░░░░░▀██████░░░
//░█▌░░░░░░░░░░░░░░░░███████▌░░
//░█░░░░░░░░░░░░░░░░░████████░░
//▐▌░░░░░░░░░░░░░░░░░▀██████▌░░
//░▌▄███▌░░░░▀████▄░░░░▀████▌░░
//▐▀▀▄█▄░▌░░░▄██▄▄▄▀░░░░████▄▄░
//▐░▀░░═▐░░░░░░══░░▀░░░░▐▀░▄▀▌▌
//▐░░░░░▌░░░░░░░░░░░░░░░▀░▀░░▌▌
//▐░░░▄▀░░░▀░▌░░░░░░░░░░░░▌█░▌▌
//░▌░░▀▀▄▄▀▀▄▌▌░░░░░░░░░░▐░▀▐▐░
//░▌░░▌░▄▄▄▄░░░▌░░░░░░░░▐░░▀▐░░
//░█░▐▄██████▄░▐░░░░░░░░█▀▄▄▀░░
//░▐░▌▌░░░░░░▀▀▄▐░░░░░░█▌░░░░░░
//░░█░░▄▀▀▀▀▄░▄═╝▄░░░▄▀░▌░░░░░░
//░░░▌▐░░░░░░▌░▀▀░░▄▀░░▐░░░░░░░
//░░░▀▄░░░░░░░░░▄▀▀░░░░█░░░░░░░
//░░░▄█▄▄▄▄▄▄▄▀▀░░░░░░░▌▌░░░░░░
//░░▄▀▌▀▌░░░░░░░░░░░░░▄▀▀▄░░░░░
//▄▀░░▌░▀▄░░░░░░░░░░▄▀░░▌░▀▄░░░
//░░░░▌█▄▄▀▄░░░░░░▄▀░░░░▌░░░▌▄▄
//░░░▄▐██████▄▄░▄▀░░▄▄▄▄▌░░░░▄░
//░░▄▌████████▄▄▄███████▌░░░░░▄
//░▄▀░██████████████████▌▀▄░░░░
//▀░░░█████▀▀░░░▀███████░░░▀▄░░
//░░░░▐█▀░░░▐░░░░░▀████▌░░░░▀▄░
//░░░░░░▌░░░▐░░░░▐░░▀▀█░░░░░░░▀
//░░░░░░▐░░░░▌░░░▐░░░░░▌░░░░░░░
//░╔╗║░╔═╗░═╦═░░░░░╔╗░░╔═╗░╦═╗░
//░║║║░║░║░░║░░░░░░╠╩╗░╠═╣░║░║░
//░║╚╝░╚═╝░░║░░░░░░╚═╝░║░║░╩═╝░