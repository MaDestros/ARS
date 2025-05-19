using System;
using System.Configuration;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Threading;

namespace ARS
{
    internal class Program
    {
        static Program program = new Program();
        Process ServerProcess = new Process();
        static bool evented = false;
        static SoundPlayer player = new SoundPlayer();

        static void Main(string[] args)
        {
            program.ServerProcess.Exited += program.ServerProcess_Exited;
            program.ServerProcess.ErrorDataReceived += program.ServerProcess_ErrorDataReceived;

            Console.Title = "ARS - Auto Restart Script V:" + Assembly.GetExecutingAssembly().GetName().Version;
            Thread.Sleep(1000);
            Write("ARS - Auto Restart Script Premium", line: true, color: ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.DarkRed;

            string[] me = " _            ___  ___     ______          _             \r\n| |           |  \\/  |     |  _  \\        | |            \r\n| |__  _   _  | .  . | __ _| | | |___  ___| |_ _ __ ___  \r\n| '_ \\| | | | | |\\/| |/ _` | | | / _ \\/ __| __| '__/ _ \\ \r\n| |_) | |_| | | |  | | (_| | |/ /  __/\\__ \\ |_| | | (_) |\r\n|_.__/ \\__, | \\_|  |_/\\__,_|___/ \\___||___/\\__|_|  \\___/ \r\n        __/ |                                            \r\n       |___/                                             ".Split('\n');
            for (int i = 0; i < me.Length; i++)
            {
                Console.WriteLine(me[i]); Thread.Sleep(100);
            }

            Console.ForegroundColor = ConsoleColor.Red;

            string[] zippa = "            ,.~\\                                                  \r\n         ,-`    \\                                                 \r\n         \\       \\                                                \r\n          \\       \\                                               \r\n           \\       \\                                              \r\n            \\       \\                                             \r\n   _.-------.\\       \\                                            \r\n  (o| o o o | \\    .-`                                            \r\n __||o_o_o_o|_ad-``                                               \r\n|``````````````|'\r\n|              |\r\n|              |\r\n|              |\r\n|              |\r\n|______________|".Split('\n');     
            for (int i = 0; i < zippa.Length; i++)
            {
                Console.WriteLine(zippa[i]); Thread.Sleep(100);
            }

            Write("-==============================-", color: ConsoleColor.Blue);
            Console.Clear();

            if (args.Length > 0)
            {
                OnArgs(args);
            }

            if(ReadSetting("start") == "1" && args.Length == 0)
            {
                Write($">> Автозапуск включен.");
                Write($">> Запускаю сохранный файл: {ReadSetting("path")}");
                program.ServerStart(ReadSetting("path"));
                Console.ReadKey();
            }
            MainSwtich();
        }

        private static void OnArgs(string[] args)
        {
            Write(">> Обнаружены входные данные, обработано.", color: ConsoleColor.Yellow);
            Write(">> Записываю файл настроек.", color: ConsoleColor.Blue);
            AddUpdateAppSettings("path", args[0]);
            program.ServerStart(args[0]);
            evented = true;
        }

        static void Write(string text, bool line = true, int speed = 30, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < text.Length; ++i)
            {
                Console.Write(text[i]); Thread.Sleep(speed);
            }
            if (line == true)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor= ConsoleColor.Gray;
        }


        static string Command(string title = "Выполнить")
        {
            if(evented == false)
            {
                Write($"{title}: ", line: false, color: ConsoleColor.Red);
            }
            return Console.ReadLine().ToLower();
        }

        static void MainSwtich()
        {
        point:
            string[] CMD = Command().Split('#');
            switch (CMD[0])
            {
                case "help":
                    Write("help - Должна помогать вам, помогло ?");
                    Thread.Sleep(5000);

                    Write("Поискать обновление: ", line: false, color: ConsoleColor.Green);
                    Write("update", color: ConsoleColor.Yellow);

                    Write("Запуск чего либо: ", line: false);
                    Write ("start#путь-до-файла", color: ConsoleColor.Yellow);
                    Write("Запуск последнего файла: ", line: false);
                    Write("run", color: ConsoleColor.Yellow);

                    Write("Остановить или Перезапустить: ", line: false);
                    Write("stop | restart", color: ConsoleColor.Yellow);

                    Write("Очистка консоли: ", line: false);
                    Write("clear", color: ConsoleColor.Yellow);

                    Write("Нарисовать АРТ (Уникально): ", line: false);
                    Write("art", color: ConsoleColor.Yellow);

                    Write("Записать все доп настройки: ", line: false);
                    Write("save", color: ConsoleColor.Magenta);

                    break;
                case "save":
                    Write("Запись <(string) values> - Это параметры запуска.", color: ConsoleColor.Yellow);
                    AddUpdateAppSettings("values", "");
                    Write("Запись <(int) start> [0, 1] - Это параметр автостарта при запуске приложения.", color: ConsoleColor.Yellow);
                    AddUpdateAppSettings("start", "0");
                    break;
                case "start":
                    try
                    {
                        Write(">> Записываю файл настроек.", color: ConsoleColor.Blue);
                        AddUpdateAppSettings("path", CMD[1]);
                        program.ServerStart(CMD[1]);
                    }
                    catch (Exception ex)
                    {
                        Write(ex.Message);
                    }
                    evented = true;
                    break;
                case "run":
                    try
                    {
                        Write($">> Запускаю сохранный файл: {ReadSetting("path")}", color: ConsoleColor.Green);
                        program.ServerStart(ReadSetting("path"));
                    }
                    catch (Exception ex)
                    {
                        Write(ex.Message);
                    }
                    evented = true;
                    break;
                case "stop":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(">> Попытка остановки...");
                    program.ServerProcess.EnableRaisingEvents = false;
                    try
                    {
                        program.ServerProcess.Kill();
                        program.ServerProcess.Close();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(">> Процесс успешно остановлен.");
                    }
                    catch (Exception ex)
                    {
                        Write(ex.Message);
                    }

                    evented = false;
                    break;
                case "restart":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(">> Выполняю попытку рестарта.");
                    try
                    {
                        program.ServerProcess.Kill();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(">> Рестарт успешен.");
                    }
                    catch (Exception ex)
                    {
                        Write(ex.Message);
                    }
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "art":
                    Console.Clear();
                    player.Stream = Properties.Resources.Elephanz___Bullitt;
                    player.Play();

                    Write("  AYANAMI REI                           __.-\"..--,__\r\n                               __..---\"  | _|    \"-_\\\r\n                        __.---\"          | V|::.-\"-._D\r\n                   _--\"\".-.._   ,,::::::'\"\\/\"\"'-:-:/\r\n              _.-\"\"::_:_:::::'-8b---\"            \"'\r\n           .-/  ::::<  |\\::::::\"\\\r\n           \\/:::/::::'\\\\ |:::b::\\\r\n           /|::/:::/::::-::b:%b:\\|\r\n            \\/::::d:|8:::b:\"%%%%%\\\r\n            |\\:b:dP:d.:::%%%%%\"\"\"-,\r\n             \\:\\.V-/ _\\b%P_   /  .-._\r\n             '|T\\   \"%j d:::--\\.(    \"-.\r\n             ::d<   -\" d%|:::do%P\"-:.   \"-,\r\n             |:I _    /%%%o::o8P    \"\\.    \"\\\r\n              \\8b     d%%%%%%P\"\"-._ _ \\::.    \\\r\n              \\%%8  _./Y%%P/      .::'-oMMo    )\r\n                H\"'|V  |  A:::...:odMMMMMM(  ./\r\n                H /_.--\"JMMMMbo:d##########b/\r\n             .-'o      dMMMMMMMMMMMMMMP\"\"\r\n           /\" /       YMMMMMMMMM|\r\n         /   .   .    \"MMMMMMMM/\r\n         :..::..:::..  MMMMMMM:|\r\n          \\:/ \\::::::::JMMMP\":/\r\n           :Ao ':__.-'MMMP:::Y\r\n           dMM\"./:::::::::-.Y\r\n          _|b::od8::/:YM::/\r\n          I HMMMP::/:/\"Y/\"\r\n           \\'\"\"'  '':|\r\n            |    -::::\\\r\n            |  :-._ '::\\\r\n            |,.|    \\ _:\"o\r\n            | d\" /   \" \\_:\\.\r\n            \".Y. \\       \\::\\\r\n             \\ \\  \\      MM\\:Y\r\n              Y \\  |     MM \\:b\r\n              >\\ Y      .MM  MM\r\n              .IY L_    MP'  MP\r\n              |  \\:|   JM   JP\r\n              |  :\\|   MP   MM\r\n              |  :::  JM'  JP|\r\n              |  ':' JP   JM |\r\n              L   : JP    MP |\r\n              0   | Y    JM  |\r\n              0   |     JP\"  |\r\n              0   |    JP    |\r\n              m   |   JP     #\r\n              I   |  JM\"     Y\r\n              l   |  MP     :\"\r\n              |\\  :-       :|\r\n              | | '.\\      :|\r\n              | | \"| \\     :|\r\n               \\    \\ \\    :|\r\n               |  |  | \\   :|\r\n               |  |  |   \\ :|\r\n               |   \\ \\    | '.\r\n               |    |:\\   | :|\r\n               \\    |::\\..|  :\\\r\n                \". /::::::'  :||\r\n                  :|::/:::|  /:\\\r\n                  | \\/::|: \\' ::|\r\n                  |  :::||    ::|\r\n                  |   ::||    ::|\r\n                  |   ::||    ::|\r\n                  |   ::||    ::|\r\n                  |   ': |    .:|\r\n                  |    : |    :|\r\n                  |    : |    :|\r\n                  |    :||   .:|\r\n                  |   ::\\   .:|\r\n                 |    :::  .::|\r\n                /     ::|  :::|\r\n             __/     .::|   ':|\r\n    ...----\"\"        ::/     ::\r\n   /m_  AMm          '/     .:::\r\n   \"\"MmmMMM#mmMMMMMMM\"     .:::m\r\n      \"\"\"YMMM\"\"\"\"\"\"P        ':mMI\r\n               _'           _MMMM\r\n           _.-\"  mm   mMMMMMMMM\"\r\n          /      MMMMMMM\"\"\r\n          mmmmmmMMMM\"", speed: 50, color: ConsoleColor.Green);
                    
                    Write("\n\n\n>> Рисование окончено, нажмите любую клавишу для продолжения.", color: ConsoleColor.Yellow);
                    Console.ReadKey();
                    Console.Clear();
                    player.Stop();
                    break;
                case "update":
                    Write(">> Сейчас будет открыта web страница: boosty.to/madestro \nЗапуск через 5... 4... 3... 2... 1...");
                    Process.Start("https://boosty.to/madestro");
                    Write(">> Сайт был успешно открыт.");
                    break;
                default:
                    Write(">> Ошибка: Не допустимый запрос.", color: ConsoleColor.Cyan);
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            goto point;
        }

        private void ServerStart(string file)
        {
            Write(">> Задаю параметры ", false, color: ConsoleColor.Green);
            ServerProcess.EnableRaisingEvents = true;
            ServerProcess.StartInfo.RedirectStandardInput = true;
            ServerProcess.StartInfo.RedirectStandardOutput = false; // false
            ServerProcess.StartInfo.RedirectStandardError = true;
            if(ReadSetting("values") != "")
            {
                program.ServerProcess.StartInfo.Arguments = ReadSetting("values");
                Write(">> Найдены параметры запуска: ", false, color: ConsoleColor.Green);
                Console.WriteLine(ReadSetting("values"));
            }
            //ServerProcess.StartInfo.WorkingDirectory = false;
            ServerProcess.StartInfo.FileName = file;
            //ServerProcess.StartInfo.Arguments = false;
            ServerProcess.StartInfo.UseShellExecute = false;
            Thread.Sleep(1000);
            Write(">> OK <<", color: ConsoleColor.Yellow);

            try
            {
                if (!ServerProcess.Start())
                {
                    Console.WriteLine(">> Процесс запущен...");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(">> Процесс убит...");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    ServerProcess.Kill();
                    //CronTick.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Write(">> " + ex.Message, color: ConsoleColor.Red);
            }
        }

        void ServerProcess_Exited(object sender, EventArgs e)
        {
            ServerProcess.Start();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(">> Закрылся, перезапущен!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ServerProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ServerProcess.Kill();
        }


        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return null;
            }
        }

    }
}
