using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

public class ScreenScanner
{


    // Import necessary Windows API functions and constants
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern int GetPixel(IntPtr hdc, int x, int y);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
   
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;

    public Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;
    public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
    public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
    public const int QKEY = 0x51; // Key Q 
    public const int WKEY = 0x57; // Key W 
    public const int EKEY = 0x45; // Key E 
    public const int SKEY = 0x53; // Key S 
    public const int SPACEBAR = 0x20; // SPACE
    public const int PLAYERSTATSX = 1574; // pixel status of player with no addon
    public const int PLAYERSTATSY = 470; // pixel status of player with no addon
    public const int RANGEX = 1662; // pixel status of player with no addon
    public const int RANGEY = 6003; // pixel status of player with no addon
    public const int FACINGX = 1577; // pixel status of player with no addon
    public const int FACINGY = 715; // pixel status of player with no addon
    public const int ONE = 0x31; // Key 1 
    public const int TWO = 0x32; // Key 2 
    public const int THREE = 0x33; // Key 3 
    public const int FOUR = 0x34; // Key 4 
    public const int FIVE = 0x35; // Key 5 
    public const int SIX = 0x36; // Key 6 
    public const int SEVEN = 0x37; // Key 7 
    public const int EIGHT = 0x38; // Key 8 
    public const int NINE = 0x39; // Key 9 
    public const int ZERO = 0x30; // Key 0 
    public const int N1 = 0x61; // Numpad key 1 
    public const int N2 = 0x62; // Numpad key 2 
    public const int N3 = 0x63; // Numpad Key 3 
    public const int N4 = 0x64; // Numpad Key 4 
    public const int N5 = 0x65; // Numpad Key 5 
    public const int N6 = 0x66; // Numpad Key 6 
    public const int N7 = 0x67; // Numpad Key 7 
    public const int N8 = 0x68; // Numpad Key 8 
    public const int N9 = 0x69; // Numpad Key 9 
    public const int N0 = 0x60; // Numpad Key 0
    private Cursor Cursor;

    public static bool IsRedColor(int r, int g, int b)
    {
        // Check if the color falls within the range of red shades
        return r >= 200 && g <= 50 && b <= 50;
    }
    public static bool IsBrightGreen(int r, int g, int b)
{
        return r <= 50 && g >= 200 && b <= 50;

    }
    public static void SendKeyForDuration(Keys key, int durationMilliseconds)
    {
        SendKeys.Send(key.ToString());
        Thread.Sleep(durationMilliseconds);
        SendKeys.SendWait(key.ToString());
    }
    public static void wait(int milliseconds) // Wait in number of miliseconds. 
    {
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds <= milliseconds)
        {
            Application.DoEvents();
        }
    }
    public static void FocusGame() // Focus game window
    {
        var prc = Process.GetProcessesByName("Game.exe");
        if (prc.Length > 0)
        {
            SetForegroundWindow(prc[0].MainWindowHandle);
        }
        else
            MessageBox.Show("Game window not found");
        
    }
    public static void PressKey(byte key, int time = 50) // it will send coded key, and keep it pressed for number of miliseconds in argument 2, 50 miliseconds will be default. 
    {
        FocusGame(); // this is the focusGame method above
        if (time != 2) keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
        if (time != 2) wait(time); // this is wait method above 
        if (time > 0) keybd_event(key, 0, KEYEVENTF_KEYUP, 0); //Release Key 
    }

    /**/
    public static void MouseClick(int button = 2) // argument is button, 1 or 2, 1 is default
    {
        //Call the imported function with the cursor's current position
        int X = Cursor.Position.X;
        int Y = Cursor.Position.Y;
        if (button == 1) mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        else if (button == 2)
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
    }
    public static void MouseMove(int x, int y)
    {
        Cursor.Position = new Point(x, y);
    }
    public static void Click(int x, int y, int button = 2)
    {
        MouseMove(x, y);
        MouseClick(button); 
    }
    public static Color GetColorAt(int x, int y)
    {
        Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        Rectangle bounds = new Rectangle(x, y, 1, 1);
        using (Graphics g = Graphics.FromImage(bmp))
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        return bmp.GetPixel(0, 0);
    }
    /**/

    //not used yet as there are no color objects to call with, this will handle routing
    public static int ColorToInt(Color color)
    {
        Console.WriteLine((int)(Math.Pow(256, 2) * color.R + 256 * color.G + color.B));
        return (int)(Math.Pow(256, 2) * color.R + 256 * color.G + color.B);
    }

    public static int Combat()
    {
        //method to handle combat key bindings, dependent on game this can be changed. 
        //it will return 1 to move it backwards in state 
        PressKey(ONE, 50);
        PressKey(TWO, 50);
        PressKey(THREE, 5000);
        PressKey(FOUR, 500);
        PressKey(THREE, 500);
        PressKey(TWO, 50);
        Thread.Sleep(24850); //allow for combat to happen
        PressKey(FIVE, 50);
        PressKey(TWO, 200);

        return 1; 
    }
    
    public static void Main()
    {
        // Get the dimensions of the primary display
        int screenWidth = GetSystemMetrics(SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SM_CYSCREEN);

        // Calculate the center coordinates
        int dotX = screenWidth / 2;
        int dotY = (screenHeight / 2)-45;
        int arX = screenWidth / 2;
        int arY = screenHeight / 2;
        // Get the pixel color at the center coordinates
        IntPtr hdc = GetDC(IntPtr.Zero);
        int pixel = GetPixel(hdc, dotX, dotY);
        ReleaseDC(IntPtr.Zero, hdc);

        // Extract RGB values from the pixel
        int pixelColor = (int)(pixel & 0x00FFFFFF);
        int pixelR = (int)(pixelColor & 0xFF);
        int pixelG = (int)((pixelColor >> 8) & 0xFF);
        int pixelB = (int)((pixelColor >> 16) & 0xFF);


        bool isRedColor = ScreenScanner.IsRedColor(pixelR, pixelG, pixelB);
        int step = 0; 
     
        while (isRedColor)
        {
            Console.WriteLine("Solid red color detected at the center of the screen!");
            

            int rightX = ScreenScanner.GetSystemMetrics(ScreenScanner.SM_CXSCREEN) - 1;
            int rightY = ScreenScanner.GetSystemMetrics(ScreenScanner.SM_CYSCREEN) / 2;

            // Move cursor 
            //Point mousePosition = Control.MousePosition;

            //this will become the location method
            //this checks an arrow at 0,0 that will change based on desired location pre-defined within lua
            //checking for any type of green if it is not the correct value it will turn pressing q or e to turn until it is green 
            IntPtr hdcd = GetDC(IntPtr.Zero);
            int Arpixel = GetPixel(hdc, arX, arY);
            ReleaseDC(IntPtr.Zero, hdcd);
            int ArpixelColor = (int)(Arpixel & 0x00FFFFFF);
            int ArpixelR = (int)(ArpixelColor & 0xFF);
            int ArpixelG = (int)((ArpixelColor >> 8) & 0xFF);
            int ArpixelB = (int)((ArpixelColor >> 16) & 0xFF);

            Color decide = ScreenScanner.GetColorAt(0, 0);
            int col = ColorToInt(decide);
            Console.WriteLine(col); //we need to change this to retrieve current facing direction, 

            bool isGreenColor = ScreenScanner.IsBrightGreen(ArpixelR, ArpixelG, ArpixelB); 

            //while not green press a or d until green then w
            while (isGreenColor)
            {
                Combat();
            }
            if (!isGreenColor)
            {
                PressKey(QKEY, 10);
            }

        }
        
    }

  
}
