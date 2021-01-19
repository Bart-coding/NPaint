using System;

namespace NPaint.Memento
{
    class Originator //tworzy pamiątki z danym stanem i odtwarza stan wybranej pamiątki
    {
        public CanvasMemento CreateMemento(string canvasName)
        {
            return new CanvasMemento(canvasName);
        }

        public string RestoreFromMemento (CanvasMemento memento)
        {
            return memento.GetState();
        }
    }
}
