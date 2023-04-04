using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.MVVMLibraly
{
    public interface ICommandMVVM
    {
        public ICommand FirstPage { get; }
        public ICommand LastPage { get; }
        public ICommand NextPage { get; }
        public ICommand PrevPage { get; }
        public ICommand RemoveItems { get; }
        public ICommand CancelFilter { get; }
        public ICommand ViewInformation { get; }
    }
}
