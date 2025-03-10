using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GestionaleLibreria.Data.Logging
{
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.txt");

        static Logger()
        {
            // Assicura che la cartella Logs esista
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        }

        /// <summary>
        /// Registra un messaggio di log generale
        /// </summary>
        public static void LogInfo(string className, string methodName, string message)
        {
            WriteLog("INFO", className, methodName, message);
        }

        /// <summary>
        /// Registra un errore con stack trace
        /// </summary>
        public static void LogError(string className, string methodName, Exception ex)
        {
            WriteLog("ERROR", className, methodName, $"{ex.Message}\n{ex.StackTrace}");
        }

        private static void WriteLog(string logType, string className, string methodName, string message)
        {
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Se il file esiste, controlliamo la dimensione
            if (File.Exists(logFilePath))
            {
                FileInfo fileInfo = new FileInfo(logFilePath);
                long fileSizeInMB = fileInfo.Length / (1024 * 1024); // Converti in MB

                if (fileSizeInMB >= 5) // Limite di 5 MB
                {
                    string archiveLogPath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                    File.Move(logFilePath, archiveLogPath); // Rinominare il file attuale
                }
            }

            // Scrive nel nuovo file di log
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logType}] {className}.{methodName} - {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

    }
}

