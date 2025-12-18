using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    internal class Program
    {

        //  Variablen initialisieren und definieren
        private static bool dungeonGenerated = false;
        private static int dungeonWidth = 0;
        private static int dungeonHeight = 0;
        private static char[,] map = new char[dungeonWidth, dungeonHeight];

        static void Main(string[] args)
        {

            sendMainMenu();
            getMainMenuInput();

            Console.ReadKey();
        }

        static void generateDungeon()
        {

            Console.Clear();
            sendMainMenu();


            for (int height = 1; height <= dungeonHeight; height++)
            {

                for (int width = 1; width <= dungeonWidth; width++)
                {
                    if (height == 1 || height == dungeonHeight || width == 1 || width == dungeonWidth)
                    {
                        Console.Write("#");

                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();

            }

            Console.WriteLine("");

            getMainMenuInput();

        }

        /*
         *      HILFSMETHODEN
         */

        static void getMainMenuInput()
        {

            bool successfulInput = false;

            Console.WriteLine("Bitte gib einen Befehl ein:");

            // Solange wiederholen, bis irgendein Befehl ausgeführt wurde
            while (successfulInput == false)
            {

                Console.Write(" > ");
                string input = Console.ReadLine();

                // Den Input des Benutzers in Einzelteile teilen, sodass man Länge und Höhe lesen kann
                string[] inputArray = input.ToLower().Split(' ');

                switch (inputArray[0])
                {
                    case "stop":

                        successfulInput = true;
                        sendSuccessfulMsg("Erfolgreich das Program gestoppt.");
                        Environment.Exit(0);
                        break;

                    case "export":

                        // Abfragen, ob bisher ein Dungeon generiert wurde
                        if (dungeonGenerated)
                        {
                            successfulInput = true;
                            break;
                        }
                        else
                        {
                            sendErrorMsg("Es wurde bisher kein exportierbarer Dungeon erstellt.");
                            break;
                        }

                    case "generate":

                        int dungeonLänge = 0;
                        int dungeonHöhe = 0;

                        if (inputArray.Length <= 2)
                        {
                            sendErrorMsg("Du musst eine Länge und eine Höhe angeben.");
                            break;
                        }

                        try
                        {
                            dungeonLänge = int.Parse(inputArray[1]);
                            dungeonHöhe = int.Parse(inputArray[2]);
                        }
                        catch
                        {
                            sendErrorMsg("Die Länge/Höhe des Dungeons konnte nicht konvertiert werden, bitte versuche es erneut.");
                            break;
                        }

                        if (dungeonLänge < 10 || dungeonLänge > 50)
                        {
                            sendErrorMsg("Die Länge muss mindestens 10 und höchstens 50 betragen.");
                            break;
                        }

                        if (dungeonHöhe < 10 || dungeonHöhe > 25)
                        {
                            sendErrorMsg("Die Höhe muss mindestens 10 und höchstens 25 betragen.");
                            break;
                        }

                        dungeonHeight = dungeonHöhe;
                        dungeonWidth = dungeonLänge;

                        generateDungeon();
                        successfulInput = true;
                        break;

                    default:
                        sendErrorMsg("Einen falschen Befehl eingegeben, bitte versuche es erneut.");
                        break;

                }

            }

        }

        // Roter Error Message ausgeben lassen
        static void sendErrorMsg(string msg)
        {
            // Zeichenfarbe zu Rot ändern
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg + "\n");
            // Nach der Nachricht zurück ändern
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // Roter Error Message ausgeben lassen
        static void sendSuccessfulMsg(string msg)
        {
            // Zeichenfarbe zu Grüner ändern
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg + "\n");
            // Nach der Nachricht zurück ändern
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        // Main Menu in der Konsole ausgeben
        static void sendMainMenu()
        {

            // Console.WriteLines für das Main Menu
            Console.WriteLine("===========================================");
            Console.WriteLine("        RANDOM DUNGEON MAP GENERATOR       ");
            Console.WriteLine("===========================================\n");

            Console.WriteLine("Willkommen zum RDM-Generator!");
            Console.WriteLine("Mit diesem Programm kannst du zufällige Dungeon-Karten erstellen.\n");

            Console.WriteLine("Verfügbare Befehle:");
            Console.WriteLine("  generate <10-50> <10-25>   -   Erstellt einen neuen Dungeon");
            Console.WriteLine("  export                     -   Exportet densdf Dungeon in eine Textdatei");
            Console.WriteLine("  stop                       -   Beendet das Programm\n");

        }
    }
}