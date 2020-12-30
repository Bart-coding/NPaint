using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace NPaint.Memento
{
    class Memento // zawiera w sobie zapamiętywany obiekt
    { // może nim być sama nazwa pliku zamiast Canvas
        //Canvas State;
        String CanvasName;

        public Memento(String CanvasName)//public Memento(Canvas State)
        {
            this.CanvasName = CanvasName;
        }
        public void SetState(String CanvasName)
        {
            this.CanvasName = CanvasName;
        }

        public String GetState()
        {
            return CanvasName; //State
        }
    }
}
