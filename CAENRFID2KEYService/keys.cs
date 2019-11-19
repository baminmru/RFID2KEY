
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
/*
[StructLayout( LayoutKind.Explicit )]
    public struct INPUT
    {
    [FieldOffset( 0 )]
    public int type;
    [FieldOffset( 4 )]
    public KEYBDINPUT ki;
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct KEYBDINPUT
    {
    public ushort wVk;
    public ushort wScan;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
    }

void Main () {
    string text = "abc123";                // text we will send
    Process notepad = Process.Start( "notepad" );    // launch notepad, we'll write into it
    Thread.Sleep( 2000 );                //give notepad some time to launch
    SetForegroundWindow( notepad.MainWindowHandle );// bring that notepad to the foreground
    short key = 0;        //variable where we will hold the value of each key

    // create an INPUT structure with default values
    INPUT input = new INPUT();
    input.type = INPUT_KEYBOARD;
    input.ki = new KEYBDINPUT();
    input.ki.dwExtraInfo = GetMessageExtraInfo();
    input.ki.dwFlags = 0;
    input.ki.time = 0;
    input.ki.wScan = 0;


    foreach ( char c in text ) {
        key = VkKeyScan( c );       //get the key for this character
        input.ki.wVk = (ushort) key;//update the input structure
        SendInput( 1, ref input, Marshal.SizeOf( input ) );//send the key to notepad
    }
    }

    [DllImport( "user32.dll" )]
    public static extern bool SetForegroundWindow (IntPtr hWnd);

    [DllImport( "user32.dll", SetLastError = true )]
    public static extern uint SendInput (uint nInputs, ref INPUT pInputs, int cbSize);
    public const int INPUT_KEYBOARD = 1;

    [DllImport( "user32.dll" )]
    public static extern IntPtr GetMessageExtraInfo ();

*/
public class KeysSender
{

    public static void PressTheKey(char ch)
    {
        byte vk = WindowsAPI.VkKeyScan(ch);
        ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);
        WindowsAPI.keybd_event(vk, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
        WindowsAPI.keybd_event(vk, 0, WindowsAPI.KEYEVENTF_KEYUP, 0);
    }

    public static void PressKey(char ch, bool press)
    {
        byte vk = WindowsAPI.VkKeyScan(ch);
        ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);

        if (press)
            KeyDown(scanCode);
        else
            KeyUp(scanCode);
    }

    public static void KeyDown(ushort scanCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
        inputs[0].ki.dwFlags = 0;
        inputs[0].ki.wScan = (ushort)(scanCode & 0xff);

        uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        if (intReturn != 1)
        {
            throw new Exception("Could not send key: " + scanCode);
        }
    }

    public static void KeyUp(ushort scanCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
        inputs[0].ki.wScan = scanCode;
        inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP;
        uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        if (intReturn != 1)
        {
            throw new Exception("Could not send key: " + scanCode);
        }
    }


}

      

 
