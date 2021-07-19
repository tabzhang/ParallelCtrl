using System;
using System.IO;

namespace ParallelCtrl
{
    class Program
    {
        //系统版本号判断
        //using System.Text;
        //using System.Collections.Generic;
        //using System.Diagnostics;
        //using System.Runtime.InteropServices;
        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public static void Main(string[] args)
        {
            //public static extern void Output(int adress, int value);
            //public static extern int Input(int adress);

            //	PortAccess.Input( 888 );//从888（即0x378）端口读取数据
            //	PortAccess.Output( 888, 4 );//把4从888端口输出

            string version = Environment.Is64BitProcess ? "64" : "32";
            if (version == "64")
            {
                if (!File.Exists(@"inpoutx64.dll"))
                {
                    byte[] Save = Properties.Resource.inpoutx64;
                    FileStream fsObj = new FileStream(@"inpoutx64.dll", FileMode.CreateNew);
                    fsObj.Write(Save, 0, Save.Length);
                    fsObj.Close();
                }
            }
            else
            {
                if (!File.Exists(@"inpout32.dll"))
                {
                    byte[] Save = Properties.Resource.inpout32;
                    FileStream fsObj = new FileStream(@"inpout32.dll", FileMode.CreateNew);
                    fsObj.Write(Save, 0, Save.Length);
                    fsObj.Close();
                }
            }

            if (args.Length < 2 || args.Length > 3)
            {
                //判断参数数量，输出Help
                Console.WriteLine("\n\tPlease Run With Parameters Below:\n");
                HelpOut();
                return;
            }

            try
            {
                if (args[0].ToUpper() == "-I" && args.Length == 2)
                {
                    //string version = Environment.Is64BitProcess ? "64" : "32";

                    //将十六进制“10”转换为十进制i
                    //int i = Convert.ToInt32("10", 16);

                    if (version == "64")
                    {
                        int input = PortAccess.Input_x64(Convert.ToInt32(args[1], 16));
                        Console.ForegroundColor = ConsoleColor.DarkGreen; //设置前景色，即字体颜色
                        Console.WriteLine(input.ToString());
                        Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    }
                    else
                    {
                        int input = PortAccess.Input(Convert.ToInt32(args[1], 16));
                        Console.ForegroundColor = ConsoleColor.DarkGreen; //设置前景色，即字体颜色
                        Console.WriteLine(input.ToString());
                        Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    }

                }
                else if (args[0].ToUpper() == "-O" && args.Length == 3)
                {
                    //string version = Environment.Is64BitProcess ? "64" : "32";
                    if (version == "64")
                    {
                        PortAccess.Output_x64(Convert.ToInt32(args[1], 16), int.Parse(args[2]));
                    }
                    else
                    {
                        PortAccess.Output(Convert.ToInt32(args[1], 16), int.Parse(args[2]));
                    }
                }
                else
                {
                    //判断参数数量，输出Help
                    Console.WriteLine("\n\tPlease Run With Parameters Below:\n");
                    HelpOut();
                    return;
                }
            }
            catch (Exception ex)
            {
                RecordErrorLog(ex.Message);
            }
        }

        //输出帮助
        static void HelpOut()
        {
            Console.WriteLine("\t==Syntax:\n");
            Console.WriteLine("\tParallel Write:\tParallel -w Port Data");
            Console.WriteLine("\tParallel Read:\tParallel -r Port\n");

            //将十六进制“10”转换为十进制i
            //int i = Convert.ToInt32("10", 16);
            //将十进制i转换为十六进制s
            //string s = string.Format("{0:X}", i);

            Console.WriteLine("\t==Example:\n");
            Console.WriteLine("\tParallel -o[O] x378 4\t\tWrite 4 To Port x378)");
            Console.WriteLine("\tParallel -i[I] x378\t\tRead Data From Port x378");
        }

        //简单的写LOG方法
        static void RecordErrorLog(String msg)
        {
            StreamWriter writer = null;
            try
            {
                writer = File.AppendText(@"Error.log");
                writer.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), msg);
                writer.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

    }
}