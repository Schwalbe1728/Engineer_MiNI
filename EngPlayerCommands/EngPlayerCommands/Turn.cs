using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngPlayerCommands
{
    public class Turn : Command
    {
        private TurnDirection direction;
        private float turnDegree;

        public Turn(TurnDirection dir, float degree = 1f)
        {
            direction = dir;
            
            switch(direction)
            {
                case TurnDirection.Left:
                    turnDegree = -1 * degree;
                    break;

                case TurnDirection.Right:
                    turnDegree = 1 * degree;
                    break;

                case TurnDirection.None:
                    turnDegree = 0 * degree;
                    break;
            }
        }

        public float TurnDegree { get { return turnDegree; } }
        public float TurnDegreeClamped
        {
            get
            {
                return
                    (Direction == TurnDirection.Left) ?
                        Math.Max(-1, TurnDegree) :
                        Math.Min(1, TurnDegree);
            }
        }

        public TurnDirection Direction
        {
            get
            {
                return direction;
            }
        }
    }

    public enum TurnDirection
    {
        Left,
        Right,
        None
    }
}
