using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdaptiveTestingSystem.DLL.CScript
{
    /// <summary>
    /// Помощник в управлении объектами внутри DataGrid
    /// </summary>
    public static class DataGridHelper
    {
        /// <summary>
        /// Получение выделенной строки в DataGrid
        /// </summary>
        /// <param name="sender">DataGrid</param>
        /// <param name="e">MouseButtonEventArgs</param>
        /// <returns>Возвращает выделенную строку</returns>
        public static DataGridRow? GetRow(object sender, MouseButtonEventArgs e)
        {
            return ItemsControl.ContainerFromElement((DataGrid)sender,
                                                  e.OriginalSource as DependencyObject) as DataGridRow;
        }
    }
}
