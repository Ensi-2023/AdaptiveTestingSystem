using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestingUserMeny.xaml
    /// </summary>
    public partial class GUI_TestingUserMeny : UserControl
    {
        public int IndexTest { get; private set; }
        public int CountQuest { get; private set; }


        public GUI_TestingUserMeny(int indexTest,string nameTest, int countQuestTest)
        {
            InitializeComponent();
            IndexTest= indexTest;
            testName.Text = nameTest;
            CountQuest= countQuestTest;

            SetProgressBarValue(countQuestTest);


            Description.Text = "Адаптивный тест - это прохождение теста со случайными вопросами по сложности на основе ваших предыдущих оценок за тесты";
        }

        private void SetProgressBarValue(int count)
        {
       
            if (count >= 20 && count < 40)
            {
                countQuest.Minimum = 15;
                countQuest.Maximum = count;
                countQuest.Value = 15;
            }
            else

                if (count > 40)
            {
                countQuest.Minimum = 20;
                countQuest.Maximum = count;
                countQuest.Value = 20;
            }
            else
            {
                countQuest.Minimum = 5;
                countQuest.Maximum = count;
                countQuest.Value = 5;
            }
        }

        private void btStartTest_Click(object sender, RoutedEventArgs e)
        {
            GUI_TestReady.Instance.SetUI(new GUI_TestingRun(IndexTest, countQuest.Value) { IsAdaptive = _rbAdaptiveYes.IsChecked});
        }
    }
}
