using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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

        /*
         * Main()
         * Hauptmethode des Programs
         */
        static void Main(string[] args)
        {

            sendMainMenu();
            getMainMenuInput();

            Console.ReadKey();
        }


        /* 
         * sendMainMenu()
         * Gibt das Hauptmenü mit allen verfügbaren Befehlen auf der Konsole aus.
         */
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
            Console.WriteLine("  export   <Dateiname>       -   Exportet den Dungeon in eine Textdatei (max. 15 Zeichen)");
            Console.WriteLine("  stop                       -   Beendet das Programm\n");

        }


        /* 
        * getMainMenuInput()
        * Liest die Benutzereingaben vom Hauptmenü ein.
        * Unterstützt die Befehle:
        *  - stop: Programm beenden
        *  - export: Dungeon exportieren
        *  - generate <breite> <höhe>: neuen Dungeon erstellen
        * Prüft Eingaben auf Gültigkeit und zeigt Fehlermeldungen bei falschen Eingaben.
        */
        static void getMainMenuInput()
        {

            bool successfulInput = false;

            Console.WriteLine("Bitte gib einen Befehl ein:");

            // Solange wiederholen bis irgendein Befehl ausgeführt wurde
            while (successfulInput == false)
            {

                // Eingabe des Benutzers
                Console.Write(" > ");
                string input = Console.ReadLine();

                // Den Input des Benutzers in Einzelteile teilen sodass man Breite und Höhe lesen kann
                string[] inputArray = input.ToLower().Split(' ');

                // Abfrage was eingegeben wurde
                switch (inputArray[0])
                {
                    // Stoppt den Generator
                    case "stop":

                        successfulInput = true;
                        sendSuccessfulMsg("Erfolgreich das Program gestoppt.");
                        Environment.Exit(0);
                        break;

                    // Exportiert den Dungeon in ein Textdokument auf den Desktop
                    case "export":

                        // Abfragen ob bisher ein Dungeon generiert wurde
                        if (dungeonGenerated)
                        {
                            successfulInput = true;

                            string dateiName;

                            string speicherText = "";

                            // Fragt ab ob ein Name eingegeben wurde
                            if (inputArray[1] == null)
                            {
                                dateiName = "Dungeon";
                            }
                            else
                            {

                                // Fragt ab ob der Name kurz genug ist
                                if (inputArray[1].Length > 15) 
                                {
                                    sendErrorMsg("Datei wurde in Dungeon umbenannt, weil der Dateiname zu lang war. (max. 15 Zeichen)");
                                    dateiName = "Dungeon";
                                }

                                // Wenn korz genug ist Name setzen
                                else
                                {
                                    dateiName = inputArray[1];
                                }
                            }


                            // Simuliert zweidimensionale for Schleife
                            for (int y = 0; y < dungeonHeight; y++)
                            {
                                for (int x = 0; x < dungeonWidth; x++)
                                {
                                    // Fügt derzeitige Koordinate zum Speichertext hinzu
                                    speicherText = speicherText + map[x, y];
                                }
                                speicherText = speicherText + "\n";

                            }

                            // Benutzt eine Methode von der Klasse Environment um den Desktoppath zu bekommen
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);    
                            
                            // Speichert den Speichertext in eine Textdatei auf dem Desktop
                            File.WriteAllText(Path.Combine(desktopPath, dateiName + ".txt"), speicherText);

                            sendSuccessfulMsg("Dungeon wurde erfolgreich auf dem Desktop mit dem Namen " + dateiName + " gespeichert.");

                            break;
                        }
                        else
                        {
                            sendErrorMsg("Es wurde bisher kein exportierbarer Dungeon erstellt.");
                            break;
                        }

                    // Generiert einen Dungeon
                    case "generate":

                        int dungeonBreite = 0;
                        int dungeonHöhe = 0;

                        // Fehlernachricht für eine unvollständige Eingabe
                        if (inputArray.Length <= 2)
                        {
                            sendErrorMsg("Du musst eine Breite und eine Höhe angeben.");
                            break;
                        }

                        // Abfrage ob die Eingabe ganze Zahlen sind
                        try
                        {
                            dungeonBreite = int.Parse(inputArray[1]);
                            dungeonHöhe = int.Parse(inputArray[2]);
                        }
                        catch
                        {
                            sendErrorMsg("Die Breite/Höhe des Dungeons konnte nicht konvertiert werden, bitte versuche es erneut.");
                            break;
                        }

                        // Abfrage ob die Breite nicht zu Hoch oder zu niedrig ist
                        if (dungeonBreite < 10 || dungeonBreite > 50)
                        {
                            sendErrorMsg("Die Breite muss mindestens 10 und höchstens 50 betragen.");
                            break;
                        }

                        // Abfrage ob die Höhe nicht zu Hoch oder zu niedrig ist
                        if (dungeonHöhe < 10 || dungeonHöhe > 25)
                        {
                            sendErrorMsg("Die Höhe muss mindestens 10 und höchstens 25 betragen.");
                            break;
                        }

                        // Speichert die Lokalen Variablen in die Globalen Variablen
                        dungeonHeight = dungeonHöhe;
                        dungeonWidth = dungeonBreite;

                        generateDungeon();
                        successfulInput = true;
                        break;

                    // Wenn irgendwas anderes eingegeben wird 
                    default:
                        sendErrorMsg("Einen falschen Befehl eingegeben, bitte versuche es erneut.");
                        break;

                }

            }

        }


        /* 
         * sendErrorMsg(string msg)
         * Gibt eine rote Fehlermeldung auf der Konsole aus.
         */
        static void sendErrorMsg(string msg)
        {
            // Zeichenfarbe zu rot ändern
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg + "\n");
            // Nach der Nachricht zurück ändern
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        /* 
         * sendSuccessfulMsg(string msg)
         * Gibt eine grüne Erfolgsmeldung auf der Konsole aus.
         */
        static void sendSuccessfulMsg(string msg)
        {
            // Zeichenfarbe zu grün ändern
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg + "\n");
            // Nach der Nachricht zurück ändern
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        /* 
        * generateDungeon()
        * Generiert einen neuen Dungeon:
        *  - Initialisiert die Map
        *  - Erstellt Startpunkt, Räume, Maze, Endpunkt und Objekte (Schätze/Fallen)
        *  - Füllt verbleibende leere Felder mit Wänden
        *  - Gibt den Dungeon auf der Konsole aus
        */
        static void generateDungeon()
        {
            // Setzt die Konsole so zurück sodass die Eingabe nicht mehr angezeigt wird
            Console.Clear();
            sendMainMenu();

            // Array Initialiesieren
            map = new char[dungeonWidth, dungeonHeight];

            // Ändert die Schriftfarbe auf dunkelrot schreibt die Überschrift und ändert die Farbe wieder auf grau
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" --- ZUFALLSDUNGEON ---");
            Console.ForegroundColor = ConsoleColor.Gray;

            // Methoden aufrufen zum generieren des Dungeons
            int[] startCoords = generateStart();
            generateRandomRooms(random.Next(4, 7));
            generateMaze(startCoords[0], startCoords[1]);
            generateEnd(startCoords[0], startCoords[1]);
            generateObjects();


            // Restlichen Wände hinzufügen
            // Zwei for Schleifen um ein Koordinatensystem zu simulieren
            for (int y = 0; y < dungeonHeight; y++)
            {
                for (int x = 0; x < dungeonWidth; x++)
                {

                    // Setzt den Rand
                    if (y == 0 || y == dungeonHeight - 1 || x == 0 || x == dungeonWidth - 1)
                    {
                        map[x, y] = '#';
                        continue;
                    }
                    // Überspringt die Koordinate wenn sie schon besetzt ist
                    if (map[x, y] != '\0')
                    {
                        continue;
                    }
                    // Füllt jede freie Koordinate mit einer Wand
                    map[x, y] = '#';

                }
            }

            // Methoden werden aufgerufen
            sendDungeon();
            sendSuccessfulMsg("Der Dungeon wurde erfolgreich erstellt");
            getMainMenuInput();

        }


        /* 
        * generateStart()
        * Wählt eine zufällige Startposition für den Dungeon und markiert sie mit S.
        * Gibt die Koordinaten x y zurück.
        */
        static int[] generateStart()
        {

            int startWidth = random.Next(1, dungeonWidth - 1);      // Erstellt eine Random Zahl zwischen 1 und der Dungeon Breite -1
            int startHeight = random.Next(1, dungeonHeight - 1);    // Erstellt eine Random Zahl zwischen 1 und der Dungeon Höhe -1
            map[startWidth, startHeight] = 'S';                     // Speichert diese Zahlen dann im Koordinatensystem und speichert S bei diesen Koordinaten 

            return new int[] { startWidth, startHeight };

        }


        /* 
        * generateRandomRooms(int roomCount)
        * Erstellt eine bestimmte Anzahl rechteckiger Räume im Dungeon.
        *  - Räume werden zufällig positioniert
        *  - Größe zwischen 3x3 und 6x6
        *  - Räume überschreiben nur leere Felder
        *  - Räume werden mit . markiert
        */
        static void generateRandomRooms(int roomCount)
        {

            for (int i = 0; i < roomCount; i++)
            {
                // Erstellt Random Zahlen zwischen 3 und 6 für die obere und Linke Wand des Raums
                int roomWidth = random.Next(3, 6);
                int roomHeight = random.Next(3, 6);

                // Erstellt Random Zahlen für die Breite und Höhe des Raums
                int roomX = random.Next(1, dungeonWidth - roomWidth - 1);
                int roomY = random.Next(1, dungeonHeight - roomHeight - 1);

                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    for (int x = roomX; x < roomX + roomWidth; x++)
                    {
                        // Überprüft ob die Koordinate schon besetzt ist
                        if (map[x, y] != '\0')
                        {
                            continue;
                        }
                        map[x, y] = '.';
                    }
                }
            }

        }


        /* 
        * generateMaze(int startX, int startY)
        * Erstellt ein Labyrinth ab einer Startposition.
        *  - Bewegt sich nur in vertikal/horizontal (keine Diagonalen)
        *  - Markiert den Pfad mit .
        */
        static void generateMaze(int startX, int startY)
        {
            // 4 mögliche Richtungen
            int[,] directions = new int[,]
            {          // x, y
                { 0, -2 }, // nach oben
                { 2, 0 },  // nach rechts
                { 0, 2 },  // nach unten
                { -2, 0 }  // nach links
            };

            // Direction Array zufällig anordnen
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
            
            // Führt den Code vier mal pro Methodenaufruf auf
            for (int i = 0; i < 4; i++)
            {
                // Berechnet wo der nächste Weg hingeht
                int targetX = startX + directions[i, 0];
                int targetY = startY + directions[i, 1];

                // Die nächste Koordinate darf nicht außerhalb des Dungeons sein
                if (targetX < 1 || targetX > dungeonWidth - 2 || targetY < 1 || targetY > dungeonHeight - 2)
                    continue;

                // Wenn die berechnete Koordinate nicht besetzt ist
                if (map[targetX, targetY] == '\0')
                {
                    // Berechnet die Zwischenkoordinate und setzt beide Koordinaten als Weg
                    int zwischenX = startX + directions[i, 0] / 2;
                    int zwischenY = startY + directions[i, 1] / 2;
                    map[zwischenX, zwischenY] = '.';
                    map[targetX, targetY] = '.';

                    // Führt solange aus bis der Weg nicht mehr weitergeführt werden kann
                    generateMaze(targetX, targetY);
                }
            }
        }


        /* 
        * generateEnd(int startX, int startY)
        * Wählt eine zufällige Endposition für den Dungeon und markiert sie mit E.
        * - Endpunkt wird nur gewählt wenn er mindestens minDistance Felder vom Start entfernt ist
        * - Gibt die Koordinaten x y zurück
        */
        static int[] generateEnd(int startX, int startY)
        {

            // Variablen Deklarieren
            int endX;
            int endY;
            int distance;
            int minDistance = 7; // Mindestabstand zwischen Start und Ende

            // Überprüft ob der Abstand zwischen Start und Ende minDistance beträgt
            do
            {
                // Erstellt Random Koordinaten für das Ende
                endX = random.Next(1, dungeonWidth - 1);
                endY = random.Next(1, dungeonHeight - 1);

                // Rechnet den Abstand zwischen Start und Ende aus
                distance = Math.Abs(endX - startX) + Math.Abs(endY - startY);

            } while (map[endX, endY] != '.' || distance < minDistance);

            map[endX, endY] = 'E';      // Speichert diese Zahlen dann im Koordinatensystem und speichert S bei diesen Koordinaten

            return new int[] { endX, endY };

        }


        /* 
        * generateObjects()
        * Verteilt zufällig Schatzkisten T und Fallen F im Dungeon.
        *  - Nur auf freien Feldern .
        *  - Wahrscheinlichkeit für Objekte liegt bei 5%
        *  - Für jedes gewählte Feld 50/50 Chance: T oder F
        */
        static void generateObjects()
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                for (int x = 0; x < dungeonWidth; x++)
                {
                    // Überspringt die Koordinate wenn sie schon belegt ist
                    if (map[x, y] != '.')
                        continue;

                    // Setzt die Chance der Objekte auf 5%
                    if (random.NextDouble() > 0.05)
                        continue;

                    // Entscheidet mit einer 50/50 Chance ob es eine Truhe oder eine Falle sein soll
                    if (random.NextDouble() > 0.5)
                    {
                        map[x, y] = 'T';
                    }
                    else
                    {
                        map[x, y] = 'F';
                    }
                }
            }
        }


        /* 
        * sendDungeon()
        * Gibt den aktuellen Dungeon auf der Konsole aus.
        * Farben werden verwendet:
        *  - Start/Ende = lila
        *  - Schatz = grün
        *  - Falle = rot
        *  - Alles andere = grau
        *  - Eine Legende der Karte
        */
        static void sendDungeon()
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                for (int x = 0; x < dungeonWidth; x++)
                {
                    // Ändert die Farbe für Start und Ziel auf Dunkel Magenta
                    if (map[x, y] == 'S' || map[x, y] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(map[x, y]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }

                    // Ändert die Farbe für Truhen auf Grün
                    if (map[x, y] == 'T')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(map[x, y]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }

                    // Ändert die Farbe für Fallen auf Rot
                    if (map[x, y] == 'F')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(map[x, y]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }

                    // Gibt die Wände und Gänge aus
                    Console.Write(map[x, y]);

                }
                Console.WriteLine();

                dungeonGenerated = true;

            }

            // Legende unter dem Dungeon
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("   S E ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("= Start/Endpunkt   ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("   T ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("= Schatzkiste   ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("   F ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("= Falle   ");
            Console.WriteLine("\n");

        }
    }
}
