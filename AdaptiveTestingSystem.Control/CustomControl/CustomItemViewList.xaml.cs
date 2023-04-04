using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdaptiveTestingSystem.Control.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для CustomItemViewList.xaml
    /// </summary>
    public partial class CustomItemViewList : UserControl
    {
        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

      
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(string), typeof(CustomItemViewList), new PropertyMetadata("0"));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set {
                SetValue(TitleProperty, value);
            }
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomItemViewList), new PropertyMetadata(string.Empty));


        public int GetIndex()
        {
            return int.Parse(Number.Replace("#", "")); 
        }

        public CustomItemViewList()
        {
            InitializeComponent();
        }

        public void SetColumnRange(int countColumn,bool gridLengthAuto=true)
        {
            for (int i = 0; i < countColumn; i++)
            {
                if(gridLengthAuto)
                    itemColumn.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto});
                else
                    itemColumn.ColumnDefinitions.Add(new ColumnDefinition() {});
            }
        }


        public void SetColumn(bool gridLengthAuto = true)
        {
           if (gridLengthAuto)
                    itemColumn.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                else
                    itemColumn.ColumnDefinitions.Add(new ColumnDefinition() { });          
        }


        public void SetTextToColumn(int index,string value,
            string styleForegroud = "DefaultTextForegroud",
            HorizontalAlignment horizontal = HorizontalAlignment.Left,
            VerticalAlignment vertical = VerticalAlignment.Center,
            double fontsize = 20)
        {

            if (itemColumn.ColumnDefinitions.Count < 3) return;

            SolidColorBrush buttonBrush = (SolidColorBrush)this.TryFindResource(styleForegroud);

            TextBlock text = new TextBlock()
            {
                Text = value.Replace("NULL",""),
                Foreground= buttonBrush,
                FontSize= fontsize,
                VerticalAlignment = vertical,
                HorizontalAlignment = horizontal,
                Margin = new Thickness(12,0,5,0)
            };

            Grid.SetColumn(text, index);           
            itemColumn.Children.Add(text);
        }

        public int GetCountColumn()
        {
            return itemColumn.ColumnDefinitions.Count;
        }

        public void DeleteColumn()
        {
            if (itemColumn.ColumnDefinitions.Count > 2)
            {

                for (int i=0;i<itemColumn.Children.Count;i++)
                {
                    var item = itemColumn.Children[i] as TextBlock;

                    if (item == null) continue;
                    if (item.Name == "number") continue;
                    if (item.Name == "value") continue;

                    itemColumn.Children.Remove(item);

                }
                
                itemColumn.ColumnDefinitions.RemoveRange(2, itemColumn.ColumnDefinitions.Count - 2); 
            }
        }
    }
}
