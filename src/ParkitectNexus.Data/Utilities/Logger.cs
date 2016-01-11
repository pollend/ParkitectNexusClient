﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Threading;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Logging utility.
    /// </summary>
    public class Logger : ILogger
    {
        private StreamWriter _streamWriter;
        private LogLevel _logLevel = LogLevel.Debug;
        private string _loggingPath;

        /// <summary>
        ///     Gets or sets the minimum log level.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; }

        /// <summary>
        ///     Gets the logging path.
        /// </summary>
        public string LoggingPath { get  { return _loggingPath;}  }

        /// <summary>
        ///     Gets a value indicating whether this instance is opened.
        /// </summary>
        public bool IsOpened {  get{return _streamWriter != null;} }

        /// <summary>
        ///     Opens the logging stream at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Open(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (IsOpened) Close();

            var overwrite = File.Exists(path) && new FileInfo(path).Length > 10 * 1024;
            
            try
            {
                _loggingPath = path;
                _streamWriter = overwrite ? new StreamWriter(File.Open(path, FileMode.Create)) : File.AppendText(path);
                _streamWriter.AutoFlush = true;
            }
            catch
            {
                _loggingPath = null;
                _streamWriter = null;
            }
        }

        /// <summary>
        ///     Closes the logging stream.
        /// </summary>
        public void Close()
        {
            if (IsOpened)
            {
                _loggingPath = null;
                _streamWriter.Flush();
                _streamWriter.Dispose();
                _streamWriter = null;
            }
        }

        /// <summary>
        ///     Writes the specified message to the log at log level <see cref="LogLevel.Debug" />.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteLine(string message)
        {
            WriteLine(message, LogLevel.Debug);
        }

        /// <summary>
        ///     Writes the specified message to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        public void WriteLine(string message, LogLevel logLevel)
        {
            if (logLevel >= MinimumLogLevel)
                _streamWriter.Log(message, logLevel);
        }

        /// <summary>
        ///     Writes the specified exception to the log at log level <see cref="LogLevel.Fatal" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void WriteException(Exception exception)
        {
            WriteException(exception, LogLevel.Fatal);
        }

        /// <summary>
        ///     Writes the specified exception to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        public void WriteException(Exception exception, LogLevel logLevel)
        {
            WriteLine("Exception: " + exception.Message, logLevel);
            WriteLine("StrackTrace: " + exception.StackTrace, logLevel);
            if (exception.InnerException != null && exception.InnerException != exception)
            {
                WriteLine("InnerException:", logLevel);
                WriteException(exception.InnerException, logLevel);
            }
        }


        public void Dispose()
        {
            Close();
        }
    }
}