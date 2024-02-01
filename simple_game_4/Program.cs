using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static int screenWidth = 20; // Reduced window size
    static int playerPosition = screenWidth / 2;
    static char playerChar = 'A';
    static char fallingObjectChar = '*';
    static List<FallingObject> fallingObjects = new List<FallingObject>();
    static int score = 0;

    static void Main()
    {
        Console.Title = "Dodge the Falling Objects";
        Console.CursorVisible = false;

        Console.WriteLine("Use Left and Right arrows to move. Press Q to quit.");
        Console.WriteLine("Dodge '*' to survive!");

        while (true)
        {
            Console.Clear();

            // Display borders
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");

                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("|");
            }

            // Display player and score
            Console.SetCursorPosition(playerPosition, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(playerChar);
            Console.ResetColor();

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Score: {score}");
            Console.ResetColor();

            // Display falling objects
            foreach (var obj in fallingObjects)
            {
                Console.SetCursorPosition(obj.X, obj.Y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(fallingObjectChar);
                Console.ResetColor();
            }

            // Move player based on input
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                ProcessInput(key);
            }

            // Move falling objects and check for collisions
            MoveObjects();
            CheckCollisions();

            // Wait a moment
            Thread.Sleep(1); // Reduced delay to 50 milliseconds

            // Check for game over (quit condition)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q)
                {
                    ShowGameOverScreen();
                    break;
                }
            }
        }
    }

    static void ProcessInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.LeftArrow:
                if (playerPosition > 1) // Adjusted boundary to accommodate borders
                    playerPosition--;
                break;
            case ConsoleKey.RightArrow:
                if (playerPosition < screenWidth - 2) // Adjusted boundary to accommodate borders
                    playerPosition++;
                break;
        }
    }

    static void MoveObjects()
    {
        Random random = new Random();

        // Move existing falling objects
        foreach (var obj in fallingObjects.ToList())
        {
            obj.Y++;

            // Remove objects that have reached the bottom
            if (obj.Y >= Console.WindowHeight)
            {
                fallingObjects.Remove(obj);
                score++;
            }
        }

        // Add a new falling object with a chance
        if (random.Next(0, 10) < 2)
        {
            fallingObjects.Add(new FallingObject(random.Next(1, screenWidth - 1), 1)); // Adjusted X boundary
        }
    }

    static void CheckCollisions()
    {
        foreach (var obj in fallingObjects)
        {
            if (obj.X == playerPosition && obj.Y == Console.WindowHeight - 1)
            {
                // Game over if the player collides with an object
                ShowGameOverScreen();
                break;
            }
        }
    }

    static void ShowGameOverScreen()
    {
        Console.Clear();
        Console.WriteLine($"Game Over! Your score: {score}");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
        Environment.Exit(0);
    }
}

class FallingObject
{
    public int X { get; set; }
    public int Y { get; set; }

    public FallingObject(int x, int y)
    {
        X = x;
        Y = y;
    }
}
