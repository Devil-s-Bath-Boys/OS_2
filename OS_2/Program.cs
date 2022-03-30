using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading;


namespace OS_2
{
    internal class Program
    {
        static string[] initialHashes = { "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad", "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b", "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f" };
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

        void divisionOfLabor(int start, int finish, string[] decryptedPassPull, int decryptedPassPullMarker)
        {
            string decryption = "";

            for (int initialHashesMarker = 0; initialHashesMarker < 3; initialHashesMarker++)
            {
                for (int counter = start; counter < finish; counter++)
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] sourceBytes = Encoding.UTF8.GetBytes(passPull[counter]);
                        byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                        string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                        if (hash == initialHashes[initialHashesMarker])
                        {
                            decryption = passPull[counter];
                            decryptedPassPull[decryptedPassPullMarker] = decryption;
                            decryptedPassPullMarker++;

                            break;
                        }
                    }

                }
            }

        }
        static void Second()
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();
            string[] decryptedPassPull = new string[3];
            for (int initialHashesMarker = 0; initialHashesMarker < 3; initialHashesMarker++)
            {
                decryptedPassPull[initialHashesMarker] = passHashing(initialHashes[initialHashesMarker]);
            }
            Console.WriteLine("\n" + "Пароль, соответсвтующий первому хешу: " + decryptedPassPull[0] + "\n" +
                              "Пароль, соответсвтующий второму хешу: " + decryptedPassPull[1] + "\n" +
                              "Пароль, соответсвтующий третьему хешу: " + decryptedPassPull[2]);
            swatch.Stop();
            Console.WriteLine("\n" + "Время перебора в однопоточном режиме: " + swatch.Elapsed);
        }
        /*
                void Third()
                {
                    Console.WriteLine("Введите количество потоков:\n");
                    int threadsAmount = int.Parse(Console.ReadLine());

                    int difference = size / threadsAmount;
                    int start = 0;
                    int finish = difference;

                    string[] decryptedPassPull = new string[3];
                    int decryptedPassPullMarker = 0;

                    for (int threadsCounter = 0; threadsCounter < threadsAmount; threadsCounter++)
                    {
                        ThreadStart ts = new ThreadStart();
                        Thread t = new Thread(ts);
                        t.Start();
                        start = start + difference;
                        finish = finish + difference;

                    }

                }
        */

        static void Main(string[] args)
        {
            passPullForming();
            while (true)
            {
                Console.WriteLine("Выход из программы - '1'.\n" +
                                  "Однопоточный режим работы - '2'.\n" +
                                  "Многопоточный режим - '3'.");

                char choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':
                        Environment.Exit(0);
                        break;

                    case '2':
                        Second();
                        break;

                    case '3':
                        //Third();
                        break;
                    default:
                        {
                            Console.WriteLine("\n" + "Пожалуйста, выберите один из предложенных пунктов 1 - 3.\n");
                            continue;
                        }

                }

            }

        }
    }
}










