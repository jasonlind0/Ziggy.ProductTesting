using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Ziggy.ViewModels
{
    public interface IBindingOperations
    {
        void EnableCollectionSynchronization(IEnumerable collection);
    }
}
