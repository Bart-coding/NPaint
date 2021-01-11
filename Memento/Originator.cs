using System;

namespace NPaint.Memento
{
    class Originator //tworzy i odtwarza pamiątki; potrafi zmieniac ich stan
    {
        private String CanvasName;

        public void SetMemento (String CanvasName ) //Może powinno być SetState; wcześniej było (Memento m)
        {
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
