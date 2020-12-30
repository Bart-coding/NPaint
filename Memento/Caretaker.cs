using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPaint.Memento
{
    class Caretaker //przechowuje Mementa i umożliwia ich dodawanie do listy wraz z pobieraniem z niej
    {
        //private string FileName; //na sztywno; może final? 
        //ewentualnie listę eksportowaną/importowaną do pliku
        private List<Memento> CanvasFiles = new List<Memento>(); //List<String>
        public void AddMemento(Memento m)
        {

            CanvasFiles.Add(m);
        }

        public Memento GetMemento(int index) // można dodać GetLastMemento
        {
            return CanvasFiles[index];

        }
        public Memento GetLastMemento ()
        {
            return CanvasFiles.Last();
        }


    }
}
