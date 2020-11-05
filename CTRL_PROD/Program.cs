﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTRL_PROD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Diversos.PrevineDuplaExecucao(false))
                Environment.Exit(0);
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                SetUnhandledExceptionFilter(ptrToExceptionInfo =>
                {
                    var errorCode = "0x" + Marshal.GetExceptionCode().ToString("x2");
                    return 1;
                });

                //Start application
                Application.Run(Forms.MainForm);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // here you can log the exception ...
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // here you can log the exception ...
        }

        [DllImport("kernel32"), SuppressUnmanagedCodeSecurity]
        private static extern int SetUnhandledExceptionFilter(Callback cb);
        // This has to be an own non generic delegate because generic delegates cannot be marshalled to unmanaged code.
        private delegate uint Callback(IntPtr ptrToExceptionInfo);
    }
}
