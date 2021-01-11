using System;

namespace NPaint.Memento
{
    class Memento // zawiera w sobie zapamiętywany obiekt
    {
        String CanvasName;

        public Memento(String CanvasName)
        {
            this.CanvasName = CanvasName;
        }

        public void SetState(String CanvasName)//być może niepotrzebne
        {
            this.CanvasName = CanvasName;
        }

        public String GetState()
        {
            return CanvasName; //State
        }
    }
}
