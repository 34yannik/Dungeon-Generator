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

            Console.ReadKey();
        }

        static void generateDungeon()
        {

        }

        /*
         *      HILFSMETHODEN
         */

        static void getMainMenuInput()
        {

            bool successfulInput = false;

            // Solange wiederholen, bis irgendein Befehl ausgeführt wurde
            while (successfulInput == false)
            {

                Console.Write(" > ");
                string input = Console.ReadLine();

                // Den Input des Benutzers in Einzelteile teilen, sodass man Länge und Höhe lesen kann
                string[] inputArray = input.ToLower().Split(' ');

                // Wenn der Befehl 'stop' war, Program beenden
                if (inputArray[0] == "stop")
                {
                    successfulInput = true;
                    sendSuccessfulMsg("Erfolgreich das Program gestoppt.");
                    Environment.Exit(0);

                }

                // Wenn der Befehl 'export' war, anfangen, den Dungeon zu exportieren
                if (inputArray[0] == "export")
                {
                    // Abfragen, ob bisher ein Dungeon generiert wurde
                    if (dungeonGenerated)
                    {
                        successfulInput = true;
                    }
                    else
                    {
                        sendErrorMsg("Es wurde bisher kein exportierbarer Dungeon erstellt.");
                        continue;
                    }
                }

                // Wenn der Befehl 'generate' war, den Dungeon generieren
                if (inputArray[0] == "generate")
                {
                    int dungeonLänge = 0;
                    int dungeonHöhe = 0;

                    if (inputArray.Length < 3)
                    {
                        sendErrorMsg("Du musst eine Länge und eine Höhe angeben.");
                        continue;
                    }

                    try
                    {
                        dungeonLänge = int.Parse(inputArray[1]);
                        dungeonHöhe = int.Parse(inputArray[2]);
                    }
                    catch
                    {
                        sendErrorMsg("Die Länge/Höhe des Dungeons konnte nicht konvertiert werden, bitte versuche es erneut.");
                        continue;
                    }

                    if (dungeonLänge < 10 || dungeonLänge > 50)
                    {
                        sendErrorMsg("Die Länge muss mindestens 10 und höchstens 50 betragen.");
                        continue;
                    }

                    if (dungeonHöhe < 10 || dungeonHöhe > 25)
                    {
                        sendErrorMsg("Die Höhe muss mindestens 10 und höchstens 25 betragen.");
                        continue;
                    }

                    dungeonWidth = dungeonHöhe;
                    dungeonWidth = dungeonLänge;


                }

                sendErrorMsg("Einen falschen Befehl eingegeben, bitte versuche es erneut.");

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
            Console.WriteLine("  generate <länge> <höhe>   -   Erstellt einen neuen Dungeon");
            Console.WriteLine("  export                    -   Exportet den Dungeon in eine Textdatei");
            Console.WriteLine("  stop                      -   Beendet das Programm\n");

            Console.WriteLine("Bitte gib einen Befehl ein:");
            getMainMenuInput();

        }
    }
}