using System;
using System.IO;
using System.Runtime.Versioning;

namespace WSharp.Services.Logging.Loggers
{
    public abstract class TextWriterLogger : ALogger, ITextLogger
    {
        #region FIELDS

        private TextWriter _writer;

        #endregion FIELDS


        #region CONSTRUCTORS

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.Diagnostics.TextWriterTraceListener'/> class with
        /// <see cref='System.IO.TextWriter'/> 
        /// as the output recipient.</para>
        /// </devdoc>
        protected TextWriterLogger()
        {
        }

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.Diagnostics.TextWriterTraceListener'/> class with the 
        ///    specified name and using the stream as the recipient of the debugging and tracing output.</para>
        /// </devdoc>
        protected TextWriterLogger(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _writer = new StreamWriter(stream);
        }

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.Diagnostics.TextWriterTraceListener'/> class with the 
        ///    specified name and using the specified writer as recipient of the tracing or
        ///    debugging
        ///    output.</para>
        /// </devdoc>
        protected TextWriterLogger(TextWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
        
        #endregion CONSTRUCTORS


        #region PROPERTIES

        /// <devdoc>
        ///    <para> Indicates the text writer that receives the tracing
        ///       or debugging output.</para>
        /// </devdoc>
        public TextWriter Writer
        {
            get
            {
                EnsureWriter();
                return _writer;
            }
            set => _writer = value;
        }

        protected bool IsWriterNull => _writer == null;

        #endregion PROPERTIES


        #region METHODS

        public override void Dispose(bool isDisposing)
        {
            try
            {
                // clean up resources
                if (_writer != null)
                {
                    try
                    {
                        _writer.Close();
                    }
                    catch (ObjectDisposedException) { }
                }
                _writer = null;
            }
            finally
            {
                base.Dispose(isDisposing);
            }
        }

        public override void InternalLog(ILogEntry logEntry)
        {
            if (!EnsureWriter())
                return;

            try
            {
                _writer.Write(logEntry);
            }
            catch (ObjectDisposedException) { }
        }
        
        // This uses a machine resource, scoped by the fileName variable.
        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        protected abstract bool EnsureWriter();

        #endregion METHODS
    }
}
