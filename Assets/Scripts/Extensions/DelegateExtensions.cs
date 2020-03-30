using System;

namespace Extensions
{
    public static class DelegateExtensions
    {
        public static void SafeInvoke(this Action @this)
        {
            @this?.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> @this, T param)
        {
            @this?.Invoke(param);
        }

        public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, T1 param1, T2 param2, T3 param3,
            T4 param4)
        {
            @this?.Invoke(param1, param2, param3, param4);
        }

        public static T SafeInvoke<T>(this Func<T> @this)
        {
            return @this != null ? @this.Invoke() : default(T);
        }
    }
}