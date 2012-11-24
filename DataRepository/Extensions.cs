using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public static class Extensions
    {
        public static void ApplyTo<T>(this T arg, params Action<T>[] actions) {
            Array.ForEach(actions, action => action(arg));
        }
    }
}
