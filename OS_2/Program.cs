using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


namespace OS_2
{
    internal class Program
    {
        static bool trigger = false;
        static string initialHashes1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
        static string initialHashes2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
        static string initialHashes3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";
        static int size = 11881376;
        static string alphabet = "abcdefghijklmnopqrstuvwxyz";
        static char[] hashDecrypt = new char[5];
        static string[] passPull = new string[size];

        static int counter = 0;

        static void passPullForming()
        {
            for (int counter1 = 0; counter1 < 26; counter1++)
            {
                hashDecrypt[0] = alphabet[counter1];
                for (int counter2 = 0; counter2 < 26; counter2++)
                {
                    hashDecrypt[1] = alphabet[counter2];
                    for (int counter3 = 0; counter3 < 26; counter3++)
                    {
                        hashDecrypt[2] = alphabet[counter3];
                        for (int counter4 = 0; counter4 < 26; counter4++)
                        {
                            hashDecrypt[3] = alphabet[counter4];
                            for (int counter5 = 0; counter5 < 26; counter5++)
                            {
                                hashDecrypt[4] = alphabet[counter5];
                                passPull[counter++] = new string(hashDecrypt);
                            }
                        }
                    }
                }
            }
        }

        static string passHashing(string incomeHash)
        {

            string decryption = "";

            for (int passPullMarker = 0; passPullMarker < size; passPullMarker++)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(passPull[passPullMarker]);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                    if (hash == incomeHash)
                    {
                        decryption = passPull[passPullMarker];
                        break;
                    }
                }
            }
            return decryption;
        }

        static string divisionOfLabor(int start, int finish, string incomeHash)
        {
            string decryption = "";

            for (int counter = start; counter < finish; counter++)
            {
                if (trigger == true)
                {
                    break;
                }
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(passPull[counter]);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                    if (hash == incomeHash)
                    {
                        decryption = passPull[counter];
                        trigger = true;
                        break;
                    }
                }

            }

            return decryption;
        }

        static string Second(string initialHash)
        {
            return passHashing(initialHash);
        }

        static string Third(string incomeHash, int threadsAmount)
        {
            int difference = size / threadsAmount;
            int remains = size % threadsAmount;
            int start = 0;
            int finish = 0;
            string trueSaveHouse = "";

            int[] iteration = new int[threadsAmount];

            Thread[] threads = new Thread[threadsAmount];

            for (int counter = 0; counter < threadsAmount; counter++)
            {
                iteration[counter] = difference;

                if (remains != 0)
                {
                    iteration[counter]++;
                    remains--;
                }

                if (counter != 0) start = finish;
                finish = start + iteration[counter];

                int from = start;
                int to = finish;

                threads[counter] = new Thread(() =>
                {
                    string saveHouse = System.String.Empty;
                    saveHouse = divisionOfLabor(from, to, incomeHash);
                    if (!String.IsNullOrEmpty(saveHouse)) trueSaveHouse = saveHouse;
                });
                threads[counter].Start();
            }

            for (int counter = 0; counter < threadsAmount; counter++)
            {
                threads[counter].Join();
                threads[counter].Interrupt();
            }

            start = 0;
            finish = 0;
            trigger = false;

            return trueSaveHouse;
        }



        static void Main(string[] args)
        {
            int marker = 0;
            passPullForming();
            while (true)
            {
                Console.WriteLine("\n" + "Выход из программы - '1'.\n" +
                                  "Однопоточный режим работы - '2'.\n" +
                                  "Многопоточный режим - '3'.");

                char choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':
                        Environment.Exit(0);
                        break;

                    case '2':
                        Stopwatch swatchSecond = new Stopwatch();
                        swatchSecond.Start();
                        Console.WriteLine("\n" + "Пароль, соответсвтующий первому хешу: " + Second(initialHashes1) +
                                          "\n" + "Пароль, соответсвтующий второму хешу: " + Second(initialHashes2) +
                                          "\n" + "Пароль, соответсвтующий третьему хешу: " + Second(initialHashes3));

                        swatchSecond.Stop();
                        Console.WriteLine("\n" + "Время перебора в однопоточном режиме: " + swatchSecond.Elapsed);
                        break;

                    case '3':
                        Stopwatch swatchThird = new Stopwatch();
                        swatchThird.Start();

                        Console.WriteLine("\nВведите количество потоков:\n");
                        int threadsAmount = int.Parse(Console.ReadLine());

                        Console.WriteLine("\n" + "Пароль, соответсвтующий первому хешу: " + Third(initialHashes1, threadsAmount));
                        Console.WriteLine("\n" + "Пароль, соответсвтующий второму хешу: " + Third(initialHashes2, threadsAmount));
                        Console.WriteLine("\n" + "Пароль, соответсвтующий третьему хешу: " + Third(initialHashes3, threadsAmount));

                        swatchThird.Stop();
                        Console.WriteLine("\n" + "Время перебора в многопоточном режиме: " + swatchThird.Elapsed);
                        break;
                    default:
                        {
                            Console.WriteLine("\n" + "Пожалуйста, выберите один из предложенных пунктов 1 - 3.\n");
                            continue;
                        }

                }

                while (marker == 0)
                {
                    Console.WriteLine("\n" + "Желаете отчистить консоль? y / n");
                    char answer = Console.ReadKey().KeyChar;

                    switch (answer)
                    {
                        case 'y':
                            Console.Clear();
                            marker = 1;
                            break;

                        case 'n':
                            marker = 1;
                            break;
                        default:
                            {
                                Console.WriteLine("\n" + "Пожалуйста, введите y / n.\n");
                                continue;
                            }
                    }
                }
                marker = 0;
            }
        }
    }
}









