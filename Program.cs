using System;
using System.IO;

namespace ParallelCtrl
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //	PortAccess.Input( 888 );        //从888（即0x378）端口读取数据
            //	PortAccess.Output( 888, 4 );    //把4从888端口输出

            string version = Environment.Is64BitProcess ? "64" : "32";
            if (version == "64")
            {
                if (!File.Exists(@"inpoutx64.dll"))
                {
                    byte[] Save = Properties.Resource.inpoutx64;
                    using (var fsObj = new FileStream(@"inpoutx64.dll", FileMode.CreateNew))
                    {
                        fsObj.Write(Save, 0, Save.Length);
                        fsObj.Close();
                    }
                }
            }
            else
            {
                if (!File.Exists(@"inpout32.dll"))
                {
                    byte[] Save = Properties.Resource.inpout32;
                    using (var fsObj = new FileStream(@"inpout32.dll", FileMode.CreateNew))
                    {
                        fsObj.Write(Save, 0, Save.Length);
                        fsObj.Close();
                    }
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
                switch (args[0].ToUpper())
                {
                    case "-I" when args.Length == 2:
                        //string version = Environment.Is64BitProcess ? "64" : "32";
                        //将十六进制“10”转换为十进制i
                        //int i = Convert.ToInt32("10", 16);

                        if (version == "64")
                        {
                            int input = PortAccess.Input_x64(Convert.ToInt32(args[1], 16));
                            Console.ForegroundColor = ConsoleColor.DarkGreen;   //设置前景色，即字体颜色
                            Console.WriteLine(input.ToString());
                            Console.ForegroundColor = ConsoleColor.White;       //设置前景色，即字体颜色
                        }
                        else
                        {
                            int input = PortAccess.Input(Convert.ToInt32(args[1], 16));
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine(input.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        break;

                    case "-O" when args.Length == 3:
                        if (version == "64")
                        {
                            PortAccess.Output_x64(Convert.ToInt32(args[1], 16), int.Parse(args[2]));
                        }
                        else
                        {
                            PortAccess.Output(Convert.ToInt32(args[1], 16), int.Parse(args[2]));
                        }
                        break;

                    default:
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
        private static void HelpOut()
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
        private static void RecordErrorLog(string msg)
        {
            using (var writer = File.AppendText(@"Error.log"))
            {
                try
                {
                    writer.WriteLine($"{DateTime.Now:yyyy/MM/dd hh:mm:ss} {msg}");
                    writer.Flush();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}