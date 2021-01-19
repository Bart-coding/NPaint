using System;

namespace NPaint.Memento
{
    class Originator //tworzy pamiątki z danym stanem i odtwarza stan wybranej pamiątki
    {
        public CanvasMemento CreateMemento(string CanvasName)
        {
            return new CanvasMemento(CanvasName);
        }

        public string RestoreFromMemento (CanvasMemento memento)
        {
            string CanvasName = memento.GetState();
            return CanvasName;
        }
    }
}
