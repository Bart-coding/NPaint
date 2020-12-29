using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace NPaint.Memento
{
    class Memento
    {
        Canvas State;

        public Memento(Canvas State)
        {
            this.State = State;
        }
        public void SetState(Canvas State)
        {
            this.State = State;
        }

        public Canvas GetState()
        {
            return State;
        }
    }
}
