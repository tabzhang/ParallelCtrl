using System.Runtime.InteropServices;

public class PortAccess
{
    [DllImport("inpout32.dll", EntryPoint = "Out32")]
    public static extern void Output(int PortAddress, int Data);

    [DllImport("inpout32.dll", EntryPoint = "Inp32")]
    public static extern int Input(int PortAddress);

    [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
    public static extern void Output_x64(int PortAddress, int Data);

    [DllImport("inpoutx64.dll", EntryPoint = "Inp32")]
    public static extern int Input_x64(int PortAddress);
}