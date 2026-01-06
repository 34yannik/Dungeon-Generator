using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Generator
{
    internal class Program
    {

        //  Variablen initialisieren und definieren
        private static bool dungeonGenerated = false;
        private static int dungeonWidth = 0;
        private static int dungeonHeight = 0;
        private static char[,] map;

        private static Random random = new Random();

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

            map = new char[dungeonWidth, dungeonHeight];

            // Schätze (T), Falle (F) 
            // Weg vom Start bis Ende

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" --- ZUFALLSDUNGEON ---");
            Console.ForegroundColor = ConsoleColor.Gray;

            int[] startCoords = generateStart();
            generateRandomRooms(random.Next(4, 7));
            generateMaze(startCoords[0], startCoords[1]);
            generateEnd(startCoords[0], startCoords[1]);


            // Restlichen Wände hinzufügen
            for (int y = 0; y < dungeonHeight; y++)
            {
                for (int x = 0; x < dungeonWidth; x++)
                {

                    if (y == 0 || y == dungeonHeight - 1 || x == 0 || x == dungeonWidth - 1)
                    {
                        map[x, y] = '#';
                        continue;
                    }

                    if (map[x, y] != '\0')
                    {
                        continue;
                    }
                    map[x, y] = '#';

                }
            }

            sendDungeon();

            dungeonGenerated = true;

            getMainMenuInput();

        }

        static void sendDungeon()
        {

            for (int y = 0; y < dungeonHeight; y++)
            {

                for (int x = 0; x < dungeonWidth; x++)
                {

                    Console.Write(map[x, y]);

                }
                Console.WriteLine();

            }

        }

        static void generateMaze(int startX, int startY)
        {
            // 4 mögliche Richtungen
            int[,] directions = new int[,]
            {   // x, y
        { 0, -2 }, // nach oben
        { 2, 0 },  // nach rechts
        { 0, 2 },  // nach unten
        { -2, 0 }  // nach links
            };

            // Shuffle-directions in-place
            for (int i = 3; i > 0; i--)
            {
                int j = random.Next(i + 1); // 0 bis i
                                            // Tausche i und j
                int tempX = directions[i, 0];
                int tempY = directions[i, 1];
                directions[i, 0] = directions[j, 0];
                directions[i, 1] = directions[j, 1];
                directions[j, 0] = tempX;
                directions[j, 1] = tempY;
            }

            for (int i = 0; i < 4; i++)
            {
                int targetX = startX + directions[i, 0];
                int targetY = startY + directions[i, 1];

                if (targetX < 1 || targetX > dungeonWidth - 2 || targetY < 1 || targetY > dungeonHeight - 2)
                    continue;

                if (map[targetX, targetY] == '\0')
                {
                    int zwischenX = startX + directions[i, 0] / 2;
                    int zwischenY = startY + directions[i, 1] / 2;
                    map[zwischenX, zwischenY] = '.';
                    map[targetX, targetY] = '.';

                    generateMaze(targetX, targetY);
                }
            }
        }

        static int[] generateStart()
        {

            int startWidth = random.Next(1, dungeonWidth - 1);
            int startHeight = random.Next(1, dungeonHeight - 1);
            map[startWidth, startHeight] = 'S';

            return new int[] { startWidth, startHeight };

        }

        static int[] generateEnd(int startX, int startY)
        {

            int endX;
            int endY;
            int distance;
            int minDistance = 5; // Mindestabstand zwischen Start und Ende

            do
            {
                endX = random.Next(1, dungeonWidth - 1);
                endY = random.Next(1, dungeonHeight - 1);

                distance = Math.Abs(endX - startX) + Math.Abs(endY - startY);

            } while (map[endX, endY] != '.' || distance < minDistance);

            map[endX, endY] = 'E';

            return new int[] { endX, endY };

        }

        static void generateRandomRooms(int roomCount)
        {

            for (int i = 0; i < roomCount; i++)
            {
                int roomWidth = random.Next(3, 6);
                int roomHeight = random.Next(3, 6);

                int roomX = random.Next(1, dungeonWidth - roomWidth - 1);
                int roomY = random.Next(1, dungeonHeight - roomHeight - 1);

                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    for (int x = roomX; x < roomX + roomWidth; x++)
                    {
                        if (map[x, y] != '\0')
                        {
                            continue;
                        }
                        map[x, y] = '.';
                    }
                }
            }

        }

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

                    case "solution":



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
            Console.WriteLine("  solution                   -   Zeigt dir den Weg vom Start zum Ende");

        }
    }
}
