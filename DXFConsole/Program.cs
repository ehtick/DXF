using DXFLibrary;
using ShapeLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TracerLibrary;

namespace DXFConsole
{
    class Program
    {
        #region Fields

        static DXFLibrary.Document _dxf;
        public static bool isclosing = false;
        static private HandlerRoutine ctrlCHandler;

        #endregion
        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        #endregion
        #region Methods
        static void Main(string[] args)
        {
            // Read in specific configuration
            Debug.WriteLine("Enter Main()");

            ctrlCHandler = new HandlerRoutine(ConsoleCtrlCheck);
            SetConsoleCtrlHandler(ctrlCHandler, true);

            Parameter<string> appPath = new Parameter<string>("appPath","");
            Parameter<string> appName = new Parameter<string>("appName","dxf.cfg");

            // Required for the plot

            Parameter<string> filename = new Parameter<string>("filename","");
            Parameter<string> filePath = new Parameter<string>("filePath","");
            Parameter<string> outName = new Parameter<string>("outName","");

            filePath.Value = Environment.CurrentDirectory;
            filePath.Source = IParameter.SourceType.App;

            appPath.Value = System.Reflection.Assembly.GetExecutingAssembly().Location;
            int pos = appPath.Value.ToString().LastIndexOf(Path.DirectorySeparatorChar);
            if (pos > 0)
            {
                appPath.Value = appPath.Value.ToString().Substring(0, pos);
                appPath.Source = IParameter.SourceType.App;
            }

            Parameter<string> logPath = new Parameter<string>("logPath","");
            Parameter<string> logName = new Parameter<string>("logNamr","DXFConsole");
            logPath.Value = System.Reflection.Assembly.GetExecutingAssembly().Location;
            pos = logPath.Value.ToString().LastIndexOf(Path.DirectorySeparatorChar);
            if (pos > 0)
            {
                logPath.Value = logPath.Value.ToString().Substring(0, pos);
                logPath.Source = IParameter.SourceType.App;
            }

            Parameter<SourceLevels> traceLevels = new Parameter<SourceLevels>("traceLevels",TraceInternal.TraceLookup("CRITICAL"));
            traceLevels.Source = IParameter.SourceType.App;

            // Configure tracer options

            string logFilenamePath = logPath.Value.ToString() + Path.DirectorySeparatorChar + logName.Value.ToString() + ".log";
            FileStreamWithRolling dailyRolling = new FileStreamWithRolling(logFilenamePath, new TimeSpan(1, 0, 0, 0), FileMode.Append);
            TextWriterTraceListenerWithTime listener = new TextWriterTraceListenerWithTime(dailyRolling);
            System.Diagnostics.Trace.AutoFlush = true;
            TraceFilter fileTraceFilter = new System.Diagnostics.EventTypeFilter(SourceLevels.Verbose);
            listener.Filter = fileTraceFilter;

            // Trace to the console

            ConsoleTraceListener console = new ConsoleTraceListener();
            TraceFilter consoleTraceFilter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information);
            console.Filter = consoleTraceFilter;
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(listener);
            System.Diagnostics.Trace.Listeners.Add(console);

            // Check if the config file has been passed in and overwrite the registry

            // Read in configuration

            _dxf = new DXFLibrary.Document();

            // Read in the plot specific parameters

            string filenamePath = "";
            string extension = "";
            int items = args.Length;
            if (items == 1)
            {
                int index = 0;
                filenamePath = args[index].Trim('"');
                pos = filenamePath.LastIndexOf('.');
                if (pos > 0)
                {
                    extension = filenamePath.Substring(pos + 1, filenamePath.Length - pos - 1);
                    filenamePath = filenamePath.Substring(0, pos);
                }

                pos = filenamePath.LastIndexOf('\\');
                if (pos > 0)
                {
                    filePath.Value = filenamePath.Substring(0, pos);
                    filePath.Source = IParameter.SourceType.Command;
                    filename.Value = filenamePath.Substring(pos + 1, filenamePath.Length - pos - 1);
                    filename.Source = IParameter.SourceType.Command;
                }
                else
                {
                    filename.Value = filenamePath;
                    filename.Source = IParameter.SourceType.Command;
                }
                TraceInternal.TraceInformation("Use filename=" + filename.Value);
                TraceInternal.TraceInformation("Use filePath=" + filePath.Value);
            }
            else
            {
                for (int item = 0; item < items; item++)
                {
                    {
                        string lookup = args[item];
                        if (!lookup.StartsWith("/"))
                        {
                            lookup = lookup.ToLower();
                        }
                        switch (lookup)
                        {
                            case "/D":
                            case "--debug":
                                {
                                    string traceName = args[item + 1];
                                    traceName = traceName.TrimStart('"');
                                    traceName = traceName.TrimEnd('"');
                                    traceLevels.Value = TraceInternal.TraceLookup(traceName);
                                    traceLevels.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value Name=" + traceLevels);
                                    break;
                                }
                            case "/N":
                            case "--name":
                                {
                                    appName.Value = args[item + 1];
                                    appName.Value = appName.Value.ToString().TrimStart('"');
                                    appName.Value = appName.Value.ToString().TrimEnd('"');
                                    appName.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value Name=" + appName);
                                    break;
                                }
                            case "/P":
                            case "--path":
                                {
                                    appPath.Value = args[item + 1];
                                    appPath.Value = appPath.Value.ToString().TrimStart('"');
                                    appPath.Value = appPath.Value.ToString().TrimEnd('"');
                                    appPath.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value Path=" + appPath);
                                    break;
                                }
                            case "/n":
                            case "--logname":
                                {
                                    logName.Value = args[item + 1];
                                    logName.Value = logName.Value.ToString().TrimStart('"');
                                    logName.Value = logName.Value.ToString().TrimEnd('"');
                                    logName.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value logName=" + logName);
                                    break;
                                }
                            case "/p":
                            case "--logpath":
                                {
                                    logPath.Value = args[item + 1];
                                    logPath.Value = logPath.Value.ToString().TrimStart('"');
                                    logPath.Value = logPath.Value.ToString().TrimEnd('"');
                                    logPath.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value logPath=" + logPath);
                                    break;
                                }
                            case "/FN":
                            case "--filename":
                                {
                                    filename.Value = args[item + 1];
                                    filename.Value = filename.Value.ToString().TrimStart('"');
                                    filename.Value = filename.Value.ToString().TrimEnd('"');
                                    filename.Source = IParameter.SourceType.Command;
                                    pos = filename.Value.ToString().LastIndexOf('.');
                                    if (pos > 0)
                                    {
                                        extension = filename.Value.ToString().Substring(pos + 1, filename.Value.ToString().Length - pos - 1);
                                        filename.Value = filename.Value.ToString().Substring(0, pos);
                                    }
                                    TraceInternal.TraceVerbose("Use command value Filename=" + filename);
                                    break;
                                }
                            case "/O":
                            case "--output":
                                {
                                    outName.Value = args[item + 1];
                                    outName.Value = outName.Value.ToString().TrimStart('"');
                                    outName.Value = outName.Value.ToString().TrimEnd('"');
                                    outName.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value Output=" + outName);
                                    break;
                                }
                            case "/FP":
                            case "--filepath":
                                {
                                    filePath.Value = args[item + 1];
                                    filePath.Value = filePath.Value.ToString().TrimStart('"');
                                    filePath.Value = filePath.Value.ToString().TrimEnd('"');
                                    filePath.Source = IParameter.SourceType.Command;
                                    TraceInternal.TraceVerbose("Use command value Filename=" + filePath);
                                    break;
                                }
                        }
                    }
                }
            }

            // Adjust the log location if it has been overridden in the registry


            if (logPath.Source == IParameter.SourceType.Command)
            {
                logFilenamePath = logPath.Value.ToString() + Path.DirectorySeparatorChar + logName.Value.ToString() + ".log";
            }

            if (logName.Source == IParameter.SourceType.Command)
            {
                logFilenamePath = logPath.Value.ToString() + Path.DirectorySeparatorChar + logName.Value.ToString() + ".log";
            }


            // Redirect the output

            listener.Flush();
            System.Diagnostics.Trace.Listeners.Remove(listener);
            listener.Close();
            listener.Dispose();

            dailyRolling = new FileStreamWithRolling(logFilenamePath, new TimeSpan(1, 0, 0, 0), FileMode.Append);
            listener = new TextWriterTraceListenerWithTime(dailyRolling);
            System.Diagnostics.Trace.AutoFlush = true;
            SourceLevels sourceLevels = TraceInternal.TraceLookup(traceLevels.Value.ToString());
            fileTraceFilter = new System.Diagnostics.EventTypeFilter(sourceLevels);
            listener.Filter = fileTraceFilter;
            System.Diagnostics.Trace.Listeners.Add(listener);

            TraceInternal.TraceInformation("Use Name=" + appName.Value);
            TraceInternal.TraceInformation("Use Path=" + appPath.Value);
            TraceInternal.TraceInformation("Use Filename=" + filename.Value);
            TraceInternal.TraceInformation("Use File Path=" + filePath.Value);
            TraceInternal.TraceInformation("Use Log Name=" + logName.Value);
            TraceInternal.TraceInformation("Use Log Path=" + logPath.Value);

            filenamePath = filePath.Value.ToString() + System.IO.Path.DirectorySeparatorChar + filename.Value.ToString() + ".shx";
            string output = filePath.Value.ToString() + System.IO.Path.DirectorySeparatorChar + filename.Value.ToString() + "_1.shp";


            if (outName.Value.ToString().Length == 0)
            {
                outName.Value = filename.Value;
            }

            // Read the plot data

            _dxf.Load(filePath.Value.ToString(), filename.Value.ToString());

            // Create the SHP file

            ShapeLibrary.Document shp = new ShapeLibrary.Document();

            // Need to iterate through the DXF Document

            foreach (Entity entity in _dxf.Entities)
            {
                if (entity.GetType() == typeof(DXFLibrary.Line))
                {
                    DXFLibrary.Line line = (DXFLibrary.Line)entity;
                    ShapeLibrary.Point p1 = shp.GetPoint((double)line.Start.X, (double)line.Start.Y, 0);
                    ShapeLibrary.Point p2 = shp.GetPoint((double)line.End.X, (double)line.End.Y, 0);
                    ShapeLibrary.Line l1 = new ShapeLibrary.Line(p1, p2);
                    shp.AddLine(l1);
                }
            }

            shp.Save(filePath.Value.ToString(), outName.Value.ToString());

            Debug.WriteLine("Exit Main()");
        }

        #endregion
        #region Private

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            Debug.WriteLine("Enter ConsoleCtrlCheck()");

            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    isclosing = true;
                    TraceInternal.TraceVerbose("CTRL+C received:");
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:
                    isclosing = true;
                    TraceInternal.TraceVerbose("CTRL+BREAK received:");
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:
                    isclosing = true;
                    TraceInternal.TraceVerbose("Program being closed:");
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    isclosing = true;
                    TraceInternal.TraceVerbose("User is logging off:");
                    break;

            }
            Debug.WriteLine("Exit ConsoleCtrlCheck()");

            Environment.Exit(0);

            return (true);

        }
        #endregion
    }
}

