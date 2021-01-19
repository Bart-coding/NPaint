using System;

namespace NPaint.Memento
{
    class CanvasMemento
    {
        private readonly string canvasName;

        public CanvasMemento(string canvasName)
        {
            this.canvasName = canvasName;
        }

        public string GetState()
        {
            return canvasName;
        }
    }
}
