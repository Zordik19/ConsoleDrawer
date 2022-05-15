using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ConsoleApp7
{
    [Serializable]
    public sealed class SavingData
    {
        public SavingData(char[,] Field, ConsoleColor Color)
        {
            this.Field = Field;
            this.Color = Color;
        }
        public char[,] Field { get; set; }
        public ConsoleColor Color { get; set; }
    }
    internal class Program
    {
        private static char[,] Field = new char[29, 120];
        private static int CursorX = 0;
        private static int CursorY = 0;
        private static int? PaintX = null;
        private static int? PaintY = null;
        private static ConsoleColor[] Colors = { ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green };
        private static int CurrentConsoleColorIndex = 0;
        static void SaveData()
        {
            Console.Clear();
            Console.Write("Path : ");
            string path = Console.ReadLine();
            Field[CursorY, CursorX] = ' ';
            try
            {
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    SavingData data = new SavingData(Field, Colors[CurrentConsoleColorIndex]);
                    formatter.Serialize(stream, data);
                }
            }
            catch 
            {
                Console.WriteLine("Path is not found!");
                Console.ReadLine();
            }
        }
        static void OpenData()
        {
            Console.Clear();
            Console.Write("Path : ");
            string path = Console.ReadLine();
            Field[CursorY, CursorX] = ' ';
            CursorX = 0;
            CursorY = 0;
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    stream.Position = 0;
                    BinaryFormatter formatter = new BinaryFormatter();
                    SavingData data = (SavingData)formatter.Deserialize(stream);
                    Field = data.Field;
                    Console.ForegroundColor = data.Color;
                }
            }
            catch
            {
                Console.WriteLine("File is not found!");
                Console.ReadLine();
            }
        }
        static void ReadKey()
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                if (CursorY > 0)
                {
                    CursorY--;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (CursorY < Field.GetLength(0) - 1)
                {
                    CursorY++;
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (CursorX < Field.GetLength(1) - 1)
                {
                    CursorX++;
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (CursorX > 0)
                {
                    CursorX--;
                }
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                PaintX = CursorX;
                PaintY = CursorY;
            }
            else if (key.Key == ConsoleKey.S)
            {
                SaveData();
            }
            else if (key.Key == ConsoleKey.O)
            {
                OpenData();
            }
            else if (key.Key == ConsoleKey.C)
            {
                CurrentConsoleColorIndex++;
                if (CurrentConsoleColorIndex >= Colors.Length)
                {
                    CurrentConsoleColorIndex = 0;
                }
                Console.ForegroundColor = Colors[CurrentConsoleColorIndex];
            }
        }
        static void Main(string[] args)
        {
            Field[CursorY, CursorX] = '+';           
            foreach (var i in Field)
                Console.Write(i);
            Field[CursorY, CursorX] = ' ';
            while (true)
            {
                ReadKey();
                Field[CursorY, CursorX] = '+';
                Console.Clear();
                foreach (var i in Field)
                    Console.Write(i);
                Field[CursorY, CursorX] = ' ';
                if (PaintX != null && PaintY != null)
                {
                    Field[PaintY.Value, PaintX.Value] = '*';
                    PaintX = null;
                    PaintY = null;
                }
            }
        }
    }
}
