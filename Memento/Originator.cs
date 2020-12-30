using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace NPaint.Memento
{
    class Originator //tworzy i odtwarza pamiątki; potrafi zmieniac ich stan
    {
        //private Canvas state;
        private String CanvasName;
        public void SetMemento (String CanvasName ) //Może powinno być SetState; wcześniej było (Memento m)
        {
            //this.CanvasName = m.GetState();
            this.CanvasName = CanvasName;
        }

        public Memento CreateMemento()
        {
            return new Memento(CanvasName);
        }

        public String restoreFromMemento (Memento m)
        {
            CanvasName = m.GetState();
            return CanvasName;
        }
    }
}
