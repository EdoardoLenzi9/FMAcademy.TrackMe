using System;
using System.Threading;
using System.Threading.Tasks;

namespace FactoryMind.TrackMe.Business.Events
{
    public class PositionEngine
    {
        public event EventHandler<EventArgs> PositionEvent;
        private float _x;
        private float _y;
        private static PositionEngine _instance;
        
        public static PositionEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PositionEngine();
                }
                return _instance;
            }
        }

        public PositionEngine()
        {
            var random = new Random();
            _x = (float)(random.NextDouble() * 90);
            _y = (float)(random.NextDouble() * 90);
            Start();
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    PositionEvent?.Invoke(_instance, new EventArgs { X = _x, Y = _y });
                    Thread.Sleep(3000);//ogni 3s aggiorna posizione
                }
            });
        }
    }

    public class EventArgs : System.EventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}