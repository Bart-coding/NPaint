using System.Collections.Generic;
using System.Linq;

namespace NPaint.Memento
{
    class Caretaker //przechowuje Mementa i umożliwia ich dodawanie do listy wraz z pobieraniem z niej
    {

        private readonly List<CanvasMemento> CanvasNames = new List<CanvasMemento>();


        public void AddMemento(CanvasMemento m)
        {
            CanvasNames.Add(m);
        }

        public CanvasMemento GetMemento(int index)
        {
            if (index >= CanvasNames.Count)
            {
                return null;
            }
            return CanvasNames[index];
        }

    }
}
