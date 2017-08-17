using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

public class Tl
{
    public const int CallStackDepth = 12; //int.MaxValue;
    public const int MaxArrayReflectLength = 5;
    public static volatile bool CombineThreads = true;
    private static readonly object locker = new object();

    private static readonly Hashtable visited = new Hashtable();
    private static readonly string home;
    private static readonly Dictionary<string, int> entered = new Dictionary<string, int>();
    private static readonly Dictionary<string, int> leaved = new Dictionary<string, int>();

    static Tl()
    {
        home = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!home.EndsWith(Path.DirectorySeparatorChar.ToString()) && !home.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            home += Path.DirectorySeparatorChar;
    }

    public static string Reflect(object thing)
    {
        if (thing == null)
        {
            return "null";
        }
        Type type = thing.GetType();
        if (thing is string)
        {
            return "\"" + thing + "\"";
        }
        if (type.IsEnum || type.IsPrimitive || thing is DateTime)
        {
            return thing.ToString();
        }
        if (visited.ContainsKey(thing))
        {
            return "(loop ref)";
        }
        visited.Add(thing, null);
        StringBuilder result = new StringBuilder();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (PropertyInfo property in properties)
        {
            if (property.GetIndexParameters().Length > 0 || property.Name == "SyncRoot")
            {
                continue;
            }
            try
            {
                object value = property.GetValue(thing, null);
                if (value != null && value != thing)
                {
                    string reflected = Reflect(value);
                    if (result.Length > 0)
                    {
                        result.Append(", ");
                    }
                    result.Append(property.Name).Append(":").Append(reflected);
                }
            }
            catch
            {
                // Ignore inner exceptions in property code
            }
        }
        foreach (FieldInfo field in fields)
        {
            try
            {
                object value = field.GetValue(thing);
                if (value != null && value != thing)
                {
                    string reflected = Reflect(value);
                    if (result.Length > 0)
                    {
                        result.Append(", ");
                    }
                    result.Append(field.Name).Append(":").Append(reflected);
                }
            }
            catch
            {
                // Ignore inner exceptions in property code
            }
        }
        result.Insert(0, ":{").Insert(0, type.Name).Append("}");
        if (type.IsArray || thing is System.Collections.IEnumerable)
        {
            List<string> list = new List<string>();
            int count = -1;
            if (thing is System.Collections.ICollection)
            {
                count = ((System.Collections.ICollection)thing).Count;
            }
            if (type.IsArray)
            {
                count = ((Array)thing).Length;
            }
            bool more = false;
            foreach (object item in (System.Collections.IEnumerable)thing)
            {
                string reflected = Reflect(item);
                if (list.Count < MaxArrayReflectLength)
                {
                    list.Add(reflected);
                }
                else
                {
                    more = true;
                    break;
                }
            }
            //if (count >= 0)
            //{
            //    result.Append(" [").Append(count).Append("] ");
            //}
            result.Append("[").Append(string.Join(", ", list.ToArray()));
            if (more)
            {
                result.Append(",.. ");
            }
            result.Append("]");
        }
        visited.Remove(thing);
        return result.ToString();
    }

    private static bool dumped = false;

    public static void DumpAssemblies()
    {
        if (dumped)
            return;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<string> lines = new List<string>();
        foreach (Assembly a in assemblies)
        {
            string line = a.FullName;
            if (a.GlobalAssemblyCache)
                line += " GAC";
            else if (a is System.Reflection.Emit.AssemblyBuilder || a.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                line += " dyn";
            else
                line += " " + a.CodeBase;
            lines.Add(line);
        }
        lines.Sort();
        lines.Insert(0, CallerMethodName());
        Log(string.Join("\n", lines.ToArray()));
        dumped = true;
        //yield return paths ? a.Location : a.FullName;
    }

    public static void Trace(bool stack, string message)
    {
            //@"D:\Home\cgafe\";
        System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        System.Threading.Thread thread = System.Threading.Thread.CurrentThread;
        string logFile = CombineThreads ?
            string.Format("{0}.{1}_{2}.log", home, process.ProcessName, process.Id) :
            string.Format("{0}.{1}_{2}_{3}.log", home, process.ProcessName, process.Id, thread.ManagedThreadId);
        string dt = DateTime.Now.ToString("HH:mm:ss.ff");
        message = (string.IsNullOrEmpty(message) ? string.Empty : " " + message) + "\n";
        string threadId = CombineThreads ? " [" + thread.ManagedThreadId.ToString() + "]" : string.Empty;
        message = dt + threadId + message;
        if (stack)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string[] trace = stackTrace.ToString().Replace(" at ", string.Empty).Split('\n');
            int count = 0;
            for (int i = 0; i < trace.Length; i++)
            {
                trace[i] = trace[i].Trim();
                if (trace[i].StartsWith("Tl.Log(") || trace[i].StartsWith("Tl.Trace(") || ++count > CallStackDepth)
                {
                    trace[i] = string.Empty;
                }
            }
            message += string.Join("\n", trace).Trim() + "\n";
        }
        if (CombineThreads)
            lock(locker)
                System.IO.File.AppendAllText(logFile, message);
        else
            System.IO.File.AppendAllText(logFile, message);
    }

    public static void TraceFormat(bool stack, string format, params object[] args)
    {
        string message = string.Format(format, args);
        Trace(stack, message);
    }

    public static void Trace(bool stack)
    {
        Trace(stack, "***");
    }

    public static void Trace(string message)
    {
        Trace(true, message);
    }

    public static void TraceFormat(string format, params object[] args)
    {
        TraceFormat(true, format, args);
    }

    public static void TraceObjects(params object[] args)
    {
        Trace(true, string.Join(" ", args));
    }

    public static void Trace()
    {
        Trace(true);
    }

    public static void Log(string message)
    {
        Trace(false, message);
    }

    public static void LogFormat(string format, params object[] args)
    {
        TraceFormat(false, format, args);
    }

    public static void LogObjects(params object[] args)
    {
        Trace(false, string.Join(" ", args));
    }

    public static void Log()
    {
        Trace(false);
    }

    public static void Enter()
    {
        Enter(null);
    }

    public static void Enter(string message)
    {
        string method = CallerMethodName();
        Log(string.Join(" ", new[] { method, "enter", message}));
        if (!entered.ContainsKey(method))
        {
            entered.Add(method, 1);
        }
        else
        {
            entered[method] = entered[method] + 1;
        }
    }

    public static void Leave()
    {
        string method = CallerMethodName();
        Log(method + " leave");
        if (!leaved.ContainsKey(method))
        {
            leaved.Add(method, 1);
        }
        else
        {
            leaved[method] = leaved[method] + 1;
        }
    }

    private static string CallerMethodName()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        string[] trace = stackTrace.ToString().Replace(" at ", string.Empty).Split('\n');
        for (int i = 0; i < trace.Length; i++)
        {
            trace[i] = trace[i].Trim();
            if (!trace[i].StartsWith("Tl.") && !trace[i].StartsWith("Tl."))
            {
                return trace[i];
            }
        }
        throw new InvalidOperationException("Can\'t find caller method in stack trace.");
    }
}
