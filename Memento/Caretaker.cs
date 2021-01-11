using System.Collections.Generic;
using System.Linq;

namespace NPaint.Memento
{
    class Caretaker //przechowuje Mementa i umożliwia ich dodawanie do listy wraz z pobieraniem z niej
    {
        //ewentualnie listę eksportowaną/importowaną do pliku
        private List<Memento> CanvasFiles = new List<Memento>();

        public void AddMemento(Memento m)
        {

            CanvasFiles.Add(m);
        }

        public Memento GetMemento(int index)
        {
            if (index >= CanvasFiles.Count)
            {
                return null;
            }
            return CanvasFiles[index];
        }

        public Memento GetLastMemento ()
        {
            return CanvasFiles.Last();
        }
    }
}
