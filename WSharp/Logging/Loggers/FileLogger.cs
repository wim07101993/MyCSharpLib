using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using Unity;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logs to a file.</summary>
    public class FileLogger : TextWriterLogger, IFileLogger
    {
        #region FIELDS

        private string _logDirectory;

        #endregion FIELDS

        #region CONSTRUCTORS

        /// <summary>Constructs a new instance of a <see cref="FileLogger"/>.</summary>
        [InjectionConstructor]
        public FileLogger()
        {
        }

        /// <summary>
        ///     Constructs a new instance of a <see cref="FileLogger"/> with a given stream to log to.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        public FileLogger(FileStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Writer = new StreamWriter(stream);
        }

        /// <summary>
        ///     Constructs a new instance of a <see cref="FileLogger"/> with a given writer to log to.
        /// </summary>
        /// <param name="writer">Writer to log to.</param>
        public FileLogger(TextWriter writer)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        ///     Constructs a new instance of a <see cref="FileLogger"/> with a given directory to log to.
        /// </summary>
        /// <param name="logDirectory"></param>
        [ResourceExposure(ResourceScope.Machine)]
        public FileLogger(string logDirectory)
        {
            LogDirectory = logDirectory;
        }

        #endregion CONSTRUCTORS

        #region PROPERTIES

        /// <summary>Directory to log to.</summary>
        public string LogDirectory
        {
            get => _logDirectory;
            set
            {
                lock (GlobalLock)
                {
                    _logDirectory = value;
                    if (!IsWriterNull)
                    {
                        Writer.Dispose();
                        Writer = null;
                    }
                    EnsureWriter();
                }
            }
        }

        #endregion PROPERTIES

        #region METHODS

        private static Encoding GetEncodingWithFallback(Encoding encoding)
        {
            // Clone it and set the "?" replacement fallback
            Encoding fallbackEncoding = (Encoding)encoding.Clone();
            fallbackEncoding.EncoderFallback = EncoderFallback.ReplacementFallback;
            fallbackEncoding.DecoderFallback = DecoderFallback.ReplacementFallback;

            return fallbackEncoding;
        }

        protected virtual string GenerateNewFileName()
        {
            var now = DateTime.UtcNow;
            return $"{now.Year}{now.Month}{now.Day}-{now.Hour}{now.Minute} - {Guid.NewGuid()}.log";
        }

        // This uses a machine resource, scoped by the fileName variable.
        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        protected override bool EnsureWriter()
        {
            lock (GlobalLock)
            {
                if (!IsWriterNull)
                    return true;

                // StreamWriter by default uses UTF8Encoding which will throw on invalid encoding
                // errors. This can cause the internal StreamWriter's state to be irrecoverable. It
                // is bad for tracing APIs to throw on encoding errors. Instead, we should provide a
                // "?" replacement fallback encoding to substitute illegal chars. For ex, In case of
                // high surrogate character D800-DBFF without a following low surrogate character DC00-DFFF
                // NOTE: We also need to use an encoding that does't emit BOM whic is StreamWriter's default
                Encoding noBOMwithFallback = GetEncodingWithFallback(new UTF8Encoding(false));

                if (!Directory.Exists(LogDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(LogDirectory);
                    }
                    catch (Exception e)
                    {
                        LogInternalException(e);
                        return false;
                    }
                }

                Exception exception = null;
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        var filePath = Path.Combine(LogDirectory, GenerateNewFileName());
                        Writer = new StreamWriter(filePath, true, noBOMwithFallback, 4096);
                        return true;
                    }
                    catch (IOException e)
                    {
                        exception = e;
                        continue;
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        //ERROR_ACCESS_DENIED, mostly ACL issues
                        exception = e;
                        break;
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        break;
                    }
                }

                LogInternalException(exception);
                return false;
            }
        }

        #endregion METHODS
    }
}