using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziggy.ViewModels
{
    public static class ViewModelFactory
    {
        private static Func<Type, object> Factory { get; set; }
        public static void Initialize(Func<Type, object> factory)
        {
            Factory = factory;
        }
        public static T Create<T>()
        {
            return (T)Factory(typeof(T));
        }
    }
}
