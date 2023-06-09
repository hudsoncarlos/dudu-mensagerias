using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class Log
    {
        private static ILoggerFactory loggerFactory { get; set; }

        public static ILogger CriarLog()
        {
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });

            return loggerFactory.CreateLogger<Program>();
        }
    }

    public class LogWriter
    {
        // cria um modelo de dados para armazenar as informações de log. Como ele só será usado nesta classe pode ser definido dentro dela como interno.
        internal class LogEntry
        {
            public DateTime Date { get; set; }
            public string UserLogin { get; set; }
            public string Message { get; set; }
        }
        // Singleton, usado para acesso as funcionalidade dessa classe. Garante que só exista uma instancia por aplicação.
        private static LogWriter instance;
        public static LogWriter Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogWriter();
                return instance;
            }
        }
        // final da declaração do singleton

        private Queue<LogEntry> LogPool;
        private Thread thread;
        //Armazena a pasta atual da aplicação
        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

        public LogWriter()
        {
            // Cria a fila de entradas de log
            LogPool = new Queue<LogEntry>();
            // Cria a thread responsável por gravar log no arquivo  
            thread = new Thread(new ThreadStart(WriteOnDisk));

            thread.Start();
        }
        // Tarefa a ser executada pela thread
        private void WriteOnDisk()
        {
            while (true) // executa infinitamente
            {
                if (LogPool.Count > 0) // verfica se existem logs para gravar
                {
                    LogEntry entry = LogPool.Dequeue(); // retira a entrada de log da fila.
                    //Formata o caminho para o armazenamento do arquivo de log
                    string finalPath = Path.Combine(path, "Logs", entry.Date.Year.ToString(), entry.Date.Month.ToString(), entry.Date.Day.ToString() + ".log");
                    //cria a pasta caso ela não exista.
                    if (!Directory.Exists(Path.GetDirectoryName(finalPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(finalPath));

                    //grava o log
                    using (StreamWriter sw = File.AppendText(finalPath))
                    {
                        sw.WriteLine(string.Format("{0} - {1} - {2}", entry.Date, entry.UserLogin, entry.Message));
                    }
                }
            }
        }

        public void WriteLog(string userLogin, string message)
        {
            //Cria um objeto do tipo LogEntry
            LogEntry entry = new LogEntry { Date = DateTime.Now, UserLogin = userLogin, Message = message };
            //Adiciona entrada na fila
            LogPool.Enqueue(entry);
        }
    }
}
