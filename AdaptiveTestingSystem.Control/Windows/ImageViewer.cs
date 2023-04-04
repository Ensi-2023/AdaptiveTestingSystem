using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Control.Windows
{
    public class ImageViewer
    {
        public static void Show(byte[] questImage)
        {
            if (GetByteImage(questImage) != null)
            {
                WindowImageViewer imageWindow = new WindowImageViewer(questImage);
                imageWindow.ShowDialog();
            }
        }
        private static byte[] GetByteImage(byte[] questImage)
        {
            try
            {
                byte[] get = questImage;
                return get;
            }
            catch
            {
                return null;
            }
        }
    }
}
