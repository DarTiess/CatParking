using Game;

namespace Infrastructure.EventsBus.Signals
{
    public struct BeginDraw
    {
        private Route _route;
        public Route Route => _route;

        public BeginDraw(Route route)
        {
            _route = route;
        }
    }
}