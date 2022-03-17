namespace WinForms.Fluent.UI.Utilities.Classes
{
    public static class EasingFunctions
    {
        public static float Linear(float time, float startValue, float changeInValue, float duration)
        {
            return changeInValue * time / duration + startValue;
        }

        public static double EaseInExpo(float time, float startValue, float changeInValue, float duration)
        {
            return changeInValue * Math.Pow(2, 10 * (time/duration - 1)) + startValue;
        }

        public static double EaseOutExpo(float time, float startValue, float changeInValue, float duration)
        {
            return changeInValue * (-Math.Pow(2, -10 * time/duration) + 1) + startValue;
        }
    }
}
