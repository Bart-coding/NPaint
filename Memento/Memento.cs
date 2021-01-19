using System;

namespace NPaint.Memento
{
    class CanvasMemento
    {
        private readonly string CanvasName;

        public CanvasMemento(string CanvasName)
        {
            this.CanvasName = CanvasName;
        }

        public string GetState()
        {
            return CanvasName;
        }
    }
}
