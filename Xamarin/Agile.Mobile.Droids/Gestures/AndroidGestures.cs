using Agile.Mobile.Gestures;
using Android.Views;

namespace Agile.Mobile.Droids.Gestures
{

    public static class AndroidGestures
    {
        public static FlingInfo GetFlingInfo(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var info = new FlingInfo(e1.GetX(), e1.GetY()
                , e2.GetX(), e2.GetY()
                , velocityX, velocityY);
            return info;
        }
    }
}