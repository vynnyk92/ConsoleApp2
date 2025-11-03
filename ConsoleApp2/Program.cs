using System.Runtime.InteropServices;

namespace ClickSolution
{
    internal class Program
    {
        private static object _lock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Insert monitor X position:");
            var xString = Console.ReadLine();
            Console.WriteLine("Insert monitor Y position:");
            var yString = Console.ReadLine();

            int.TryParse(xString, out var x);
            int.TryParse(yString, out var y);


            
            while (true) {
                lock (_lock)
                {
                    var isKeyDown = NativeKeyboard.IsKeyDown(KeyCode.Right);

                    if (isKeyDown)
                    {
                        Mouse.ExecuteClick(x, y);
                    }
                }
            }

            Console.ReadLine();
        }
    }

    [Flags]
    public enum MouseEventFlags
    {
        LEFTDOWN = 0x00000002,
        LEFTUP = 0x00000004,
        MIDDLEDOWN = 0x00000020,
        MIDDLEUP = 0x00000040,
        MOVE = 0x00000001,
        ABSOLUTE = 0x00008000,
        RIGHTDOWN = 0x00000008,
        RIGHTUP = 0x00000010
    }


    public static class Mouse
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void ExecuteClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event((int)MouseEventFlags.LEFTDOWN, x, y, 0, 0);
            mouse_event((int)MouseEventFlags.LEFTUP, x, y, 0, 0);
        }
    }

    /// <summary>
    /// Codes representing keyboard keys.
    /// </summary>
    /// <remarks>
    /// Key code documentation:
    /// http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx
    /// </remarks>
    internal enum KeyCode : int
    {
        /// <summary>
        /// The left arrow key.
        /// </summary>
        Left = 0x25,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        Up,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        Right,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        Down
    }

    /// <summary>
    /// Provides keyboard access.
    /// </summary>
    internal static class NativeKeyboard
    {
        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key pressed.
        /// </summary>
        private const int KeyPressed = 0x8000;

        /// <summary>
        /// Returns a value indicating if a given key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        /// <c>true</c> if the key is pressed, otherwise <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        /// <summary>
        /// Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
    }

}


