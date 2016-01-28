using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SAIS
{
   internal class Config
    {
       internal static string datasource = "";
       internal static string userid = "";
       internal static string password = "";
       [DllImport("KERNEL32.DLL")]
       private static extern uint GetPrivateProfileString(string AppName, string KeyName, string DefaultValue, StringBuilder sBuffer, int size, string Path);

       internal void GetConfig()
       {
       lb0: ;
       if (System.IO.File.Exists(Environment.CurrentDirectory + "\\config.cfg"))
       {
           StringBuilder sb = new StringBuilder(1024);
           GetPrivateProfileString("mssql", "server", "", sb, sb.MaxCapacity, Environment.CurrentDirectory + "\\config.cfg"); ;
           datasource = sb.ToString();
           sb.Clear();
           GetPrivateProfileString("mssql", "user", "", sb, sb.MaxCapacity, Environment.CurrentDirectory + "\\config.cfg"); ;
           userid = sb.ToString();
           sb.Clear();
           GetPrivateProfileString("mssql", "password", "", sb, sb.MaxCapacity, Environment.CurrentDirectory + "\\config.cfg"); ;
           password = sb.ToString();
           sb.Clear();

       }
       else
       {
           System.IO.StreamWriter sw = new System.IO.StreamWriter(Environment.CurrentDirectory + "\\config.cfg", true);
           sw.Write("[MSSQL]" + Environment.NewLine);
           sw.Write("server=(local)" + Environment.NewLine);
           sw.Write("user=sa" + Environment.NewLine);
           sw.Write("password=" + Environment.NewLine);
           sw.Close();
  	goto lb0;
       }
       }


    }
}
