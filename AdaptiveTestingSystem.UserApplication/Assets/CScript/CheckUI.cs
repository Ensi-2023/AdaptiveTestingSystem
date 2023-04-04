using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class CheckUI
    {
        public static UIElement? GetMainBodyUI()
        {             
            return (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0];
        }
    }
}
