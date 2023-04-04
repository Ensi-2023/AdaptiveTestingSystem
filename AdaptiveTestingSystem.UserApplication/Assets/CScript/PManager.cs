using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class PManager : PageManager
    {
        public PManager(Grid contentWindow) : base(contentWindow) { }     

        public override void Next(UserControl? next)
        {
            base.Next(next);
        }
     
    }
}
