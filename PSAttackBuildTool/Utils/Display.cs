using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PSAttackBuildTool.Utils
{
    class Display
    {
        public string dashboard = @"
  __________________________________________________________
 /              __                                          \
 |    _____ ____\ \  _____ _____ _____ _____ _____ _____    |
 |   |  _  |   __\ \|  _  |_   _|_   _|  _  |     |  |  |   |
 |   |   __|__   |> |     | | |   | | |     |   --|    -|   |
 |   |__|  |_____/ /|__|__| |_|   |_| |__|__|_____|__|__|   |
 |              /_/                   BUILD TOOL v{0}       |
 \__________________________________________________________/

 Stage: {1}
 Status: {2}

 {3}

 {4}
";

        public int stageTop = 10;
        public int stageLeft = 8;
        public int statusTop = 11;
        public int statusLeft = 9;
        public int messageTop = 13;
        public int messageLeft = 1;
        public int secondaryMessageTop = 15;
        public int secondaryMessageLeft = 1;

        public Display()
        {
            Console.Clear();
            Console.Write(this.dashboard, Strings.version, "","","","");
        }

        public void updateStage(string value)
        {
            Console.CursorTop = stageTop;
            Console.CursorLeft = stageLeft;
            string clear = String.Concat(Enumerable.Repeat(" ",(Console.WindowWidth - stageLeft)));
            Console.Write(clear);
            Console.CursorTop = stageTop;
            Console.CursorLeft = stageLeft;
            Console.Write(value);
        }

        public void updateStatus(string value)
        {
            Console.CursorTop = statusTop;
            Console.CursorLeft = statusLeft;
            string clear = String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth - statusLeft)));
            string clearWholeLine = String.Concat(Enumerable.Repeat(" ", Console.WindowWidth));
            Console.Write(clear);
            while (Console.CursorTop < messageTop)
            {
                Console.Write(clearWholeLine);
                Console.CursorTop += 1;
            }
            Console.CursorTop = statusTop;
            Console.CursorLeft = statusLeft;
            Console.Write(value);
        }

        public void updateMessage(string value)
        {
            Console.CursorTop = messageTop;
            Console.CursorLeft = messageLeft;
            string clear = String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth - messageLeft)));
            string clearWholeLine = String.Concat(Enumerable.Repeat(" ", Console.WindowWidth));
            Console.Write(clear);
            int cursorTop = Console.CursorTop;
            int windowHeight = Console.WindowHeight;
            while (Console.CursorTop < (Console.WindowHeight - 1))
            {
                cursorTop = Console.CursorTop;
                windowHeight = Console.WindowHeight;
                Console.Write(clearWholeLine);
            }
            Console.CursorTop = messageTop;
            Console.CursorLeft = messageLeft;
            Console.Write(value);
        }

        public void updateSecondaryMessage(string value)
        {
            Console.CursorTop = secondaryMessageTop;
            Console.CursorLeft = secondaryMessageLeft;
            string clear = String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth - secondaryMessageLeft)));
            string clearWholeLine = String.Concat(Enumerable.Repeat(" ", Console.WindowWidth));
            Console.Write(clear);
            while (Console.CursorTop < (Console.WindowHeight - 1))
            {
                Console.Write(clearWholeLine);
            }
            Console.CursorTop = secondaryMessageTop;
            Console.CursorLeft = secondaryMessageLeft;
            Console.Write(value);
        }
    }
}
