using System;
using Agile.Diagnostics.Logging;

namespace Agile.Mobile.Gestures
{
    /// <summary>
    /// Encapsulates calculations and details about Fling Gestures
    /// </summary>
    /// <remarks>this may be Android specific...</remarks>
    public class FlingInfo
    {

        /// <summary>
        /// ctor
        /// </summary>
        public FlingInfo(float startX, float startY
            , float endX, float endY
            , float velocityX, float velocityY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            VelocityX = velocityX;
            VelocityY = velocityY;
        }

        protected const int SWIPE_THRESHOLD = 100;
        protected const int SWIPE_VELOCITY_THRESHOLD = 100;

        private FlingDirection? direction;

        public float StartX { get; private set; }
        public float StartY { get; private set; }

        public float EndX { get; private set; }
        public float EndY { get; private set; }

        public float DiffX {
            get { return EndX - StartX; }
        }

        public float DiffY
        {
            get { return EndY - StartY; }
        }
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        public FlingDirection Direction
        {
            get
            {
                if (! direction.HasValue)
                    direction = CalculateDirection();

                return direction.Value; 
            }
        }

        private FlingDirection CalculateDirection()
        {
            try
            {
                if (Math.Abs(DiffX) > Math.Abs(DiffY))
                {
                    if (Math.Abs(DiffX) > SWIPE_THRESHOLD && Math.Abs(VelocityX) > SWIPE_VELOCITY_THRESHOLD)
                    {
                        return DiffX > 0 
                            ? FlingDirection.Right 
                            : FlingDirection.Left;
                    }
                }
                else
                {
                    if (Math.Abs(DiffY) > SWIPE_THRESHOLD && Math.Abs(VelocityY) > SWIPE_VELOCITY_THRESHOLD)
                    {
                        return DiffY > 0 
                            ? FlingDirection.Down 
                            : FlingDirection.Up;
                    }
                }
                return FlingDirection.Unknown;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Scroller.OnFling");
                return FlingDirection.Unknown;
            }
        }
    }
}