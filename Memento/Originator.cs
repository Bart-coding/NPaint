using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace NPaint.Memento
{
    class Originator
    {
        private Canvas state;
        public void SetMemento (Memento m)
        {
            state = m.GetState();
        }

        public Memento CreateMemento()
        {
            return new Memento(state);
        }
    }
}
