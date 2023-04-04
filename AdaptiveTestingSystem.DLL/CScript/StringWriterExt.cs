#nullable disable
using System.ComponentModel;
using System.IO;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class StringWriterExt : StringWriter
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void FlushedEventHandler(object sender, EventArgs args);
        public event FlushedEventHandler Flushed;
        public virtual bool AutoFlush { get; set; }

        public StringWriterExt() : base() { }
        public StringWriterExt(bool autoflush) : base() { this.AutoFlush = autoflush; }
        protected void OnFlush()
        {

            var eh = Flushed;
            if (eh != null)
            {
                eh(this, EventArgs.Empty);
                this.GetStringBuilder().Clear();
            }

        }

        public override void Flush()
        {
            base.Flush();
            OnFlush();
        }


        public override void Write(char value)
        {
            base.Write(value);
            if (AutoFlush) Flush();

        }
        public override void Write(string value)
        {
            base.Write(value);
            if (AutoFlush) Flush();

        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            if (AutoFlush) Flush();

        }

    }
}
