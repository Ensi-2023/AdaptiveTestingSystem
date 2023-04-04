using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.Control.Themes
{
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_CollectionList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_Wotemark", Type = typeof(TextBlock))]
    public class ComboTextBox : TextBox
    {



        public PlacementMode Placment
        {
            get { return (PlacementMode)GetValue(PlacmentProperty); }
            set { SetValue(PlacmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placment.  This enables animation, styling, binding, etc...




        public double MaxPopupHeight
        {
            get { return (double)GetValue(MaxPopupHeightProperty); }
            set { SetValue(MaxPopupHeightProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set
            {
                if (Items.Count != 0) SetValue(IsOpenProperty, value);
            }
        }

        public string Wotemark
        {
            get { return (string)GetValue(WotemarkProperty); }
            set { SetValue(WotemarkProperty, value); }
        }

        public bool WotemarkVisible
        {
            get { return (bool)GetValue(WotemarkVisibleProperty); }
            set { SetValue(WotemarkVisibleProperty, value); }
        }

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }


        public double WotemarkFontSize
        {
            get { return (double)GetValue(WotemarkFontSizeProperty); }
            set { SetValue(WotemarkFontSizeProperty, value); }
        }

        public List<PopupItemControl> Items
        {
            get { return (List<PopupItemControl>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public bool OnSearch
        {
            get { return (bool)GetValue(OnSearchProperty); }
            set { SetValue(OnSearchProperty, value); }
        }


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }



        public bool IsNumberOnly
        {
            get { return (bool)GetValue(IsNumberOnlyProperty); }
            set { SetValue(IsNumberOnlyProperty, value); }
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }


        public Visibility VisibilityOpenButton
        {
            get { return (Visibility)GetValue(VisibilityOpenButtonProperty); }
            set { SetValue(VisibilityOpenButtonProperty, value); }
        }

        public static readonly DependencyProperty OnSearchProperty;
        public static readonly DependencyProperty SelectedIndexProperty;
        public static readonly DependencyProperty WotemarkFontSizeProperty;
        public static readonly DependencyProperty IsEditableProperty;
        public static readonly DependencyProperty WotemarkVisibleProperty;
        public static readonly DependencyProperty WotemarkProperty;
        public static readonly DependencyProperty IsOpenProperty;
        public static readonly DependencyProperty ItemsProperty;
        public static readonly DependencyProperty MaxPopupHeightProperty;
        public static readonly DependencyProperty PlacmentProperty;
        public static readonly DependencyProperty VisibilityOpenButtonProperty;
        public static readonly DependencyProperty IsValidProperty;
        public static readonly DependencyProperty IsNumberOnlyProperty;




        



        static ComboTextBox()
        {
            OnSearchProperty = DependencyProperty.Register("OnSearch", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(false));
            SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ComboTextBox), new PropertyMetadata(-1));
            ItemsProperty = DependencyProperty.Register("Items", typeof(List<PopupItemControl>), typeof(ComboTextBox), new PropertyMetadata(new List<PopupItemControl>()));
            WotemarkFontSizeProperty = DependencyProperty.Register("WotemarkFontSize", typeof(double), typeof(ComboTextBox), new PropertyMetadata(15.0));
            IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(true));
            WotemarkVisibleProperty = DependencyProperty.Register("WotemarkVisible", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(true));
            WotemarkProperty = DependencyProperty.Register("Wotemark", typeof(string), typeof(ComboTextBox), new PropertyMetadata(string.Empty));
            IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(false));
            MaxPopupHeightProperty = DependencyProperty.Register("MaxPopupHeight", typeof(double), typeof(ComboTextBox), new PropertyMetadata(250.0));
            PlacmentProperty = DependencyProperty.Register("Placment", typeof(PlacementMode), typeof(ComboTextBox), new PropertyMetadata(PlacementMode.Top));
            VisibilityOpenButtonProperty = DependencyProperty.Register("VisibilityOpenButton", typeof(Visibility), typeof(ComboTextBox), new PropertyMetadata(Visibility.Visible));
            IsValidProperty =  DependencyProperty.Register("IsValid", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(false));
            IsNumberOnlyProperty = DependencyProperty.Register("IsNumberOnly", typeof(bool), typeof(ComboTextBox), new PropertyMetadata(false));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboTextBox), new FrameworkPropertyMetadata(typeof(ComboTextBox)));
        }
        Button? _button;
        Popup? _popup;
        ListBox? _box;
        List<PopupItemControl> _items = new List<PopupItemControl>();
        bool _isPopupMouse = false;

        Brush _brushBorder;
        Thickness _borderThickness;

        public ComboTextBox()
        {
            Items = new List<PopupItemControl>();
            this._borderThickness = this.BorderThickness;
            this._brushBorder = this.BorderBrush;

        }

        ~ComboTextBox() 
        {
            this.MouseUp -= PTextBox_MouseUp;
            this.MouseEnter -= PTextBox_MouseEnter;
            this.MouseLeave -= PTextBox_MouseLeave;
            if (_box != null)
            {
                this._box.PreviewMouseLeftButtonUp -= _box_PreviewMouseLeftButtonUp;
                this._box.SelectionChanged -= _box_SelectionChanged;
            }

            if (_popup != null)
            {
                this._popup.Closed -= Popup_Closed;
                this._popup.MouseEnter -= Popup_MouseEnter;
            }
            if (_button != null)
            {
                this._button.Click -= OpenCollection_Click;
            }
        }

        public void Insert(PopupItemControl item)
        {
            item.MinWidth= 50;
            
            this.Items.Add(item);
            this.UpdateDefaultStyle();
            //this._box.ItemsSource = this.Items;
            //Logger.Debug($"{Items.Count}");
        }

        public void Delete(PopupItemControl item)
        {
            this.Items.Remove(item);
            this.UpdateDefaultStyle();
        }


        Popup _errorpopup;
        public bool IsError { get; set; } = false;
        private string TextError { get; set; } = string.Empty;
        public void Error(string error)
        {

            if (_errorpopup != null)
            {
                if (_errorpopup.IsOpen)
                {
                   return;
                }
  
            }

            IsError = true;

            Border border = new Border()
            {
                Background = (Brush)this.TryFindResource("ElipseBar_AnswerNotCorrect"),
                CornerRadius = new CornerRadius(5),
                BorderThickness = new Thickness(1),
                BorderBrush = (Brush)this.TryFindResource("NotificationButtonIcon"),
                Width=150,
                MinHeight=50
            };

            TextBlock text = new TextBlock()
            {
                Text = error,
                TextWrapping= TextWrapping.Wrap,
                Foreground = (Brush)this.TryFindResource("DefaultTextForegroud"),
                FontSize = 15,
                HorizontalAlignment= HorizontalAlignment.Center,
                VerticalAlignment= VerticalAlignment.Center,

            };

            TextError = error;

            border.Child = text;

            _errorpopup = new Popup()
            {
                AllowsTransparency= true,
                Child= border,
                VerticalOffset = 0,
                HorizontalOffset= 5,
                StaysOpen= false,
            };

            _errorpopup.PlacementTarget = this;
            _errorpopup.Placement = PlacementMode.Right;
            _errorpopup.IsOpen= true;

            this.BorderBrush = Brushes.Red;
            this.BorderThickness = new Thickness(1);

        }

        public PopupItemControl? ReturnSelectItem()
        {
            if(_box==null) return null;

            return _box.SelectedItem as PopupItemControl;
        }

        public void CloseError()
        {
            IsError = false;
            this.BorderBrush = _brushBorder;
            this.BorderThickness = _borderThickness;

            if (_errorpopup!=null) _errorpopup.IsOpen = false;
        }

        public void ClearItems()
        {
            this.Items.Clear();
            this.Text = string.Empty;
            this.UpdateDefaultStyle();
     
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = GetTemplateChild("PART_Button") as Button;
            if (button != null)
            {
                button.Click += OpenCollection_Click;
                _button= button;
            }

            var popup = GetTemplateChild("PART_Popup") as Popup;
            if (popup != null)
            {
               
                popup.Closed += Popup_Closed;
                popup.MouseEnter += Popup_MouseEnter;
                _popup = popup;
            }

            var collection = GetTemplateChild("PART_CollectionList") as ListBox;
            if (collection != null)
            {
                this._box = collection;
                this._box.PreviewMouseLeftButtonUp += _box_PreviewMouseLeftButtonUp;
                this._box.SelectionChanged += _box_SelectionChanged;
            }

            this.MouseUp += PTextBox_MouseUp;
            this.MouseEnter += PTextBox_MouseEnter;
            this.MouseLeave += PTextBox_MouseLeave;




        }

        private void _box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_box != null)
            {
                this.SelectedIndex = _box.SelectedIndex;
         
            }
        }

        private void Popup_MouseEnter(object sender, MouseEventArgs e)
        {
            _isPopupMouse = true;
        }

        private void _box_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (this._box == null) return;

            if (IsOpen)
            {
                if (this._box.SelectedItem == null) return;
                this.Text = ((PopupItemControl)_box.SelectedItem).Caption.ToString();
                this.Focus();
               // this.SelectionStart = this.Text.Length;

                IsOpen = false;
            }
            else IsOpen = true;



        }


        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (IsNumberOnly)
            {
                if (!Char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                }

            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

    

            if (IsEditable) 
            {
                if (IsValid) this.ValidData(Text);
                this.StartSearch(); 
            }

         

        }

        private void ValidData(string text,bool viewPopup=true)
        {
                  
            var item = Items.Find(o => (o.Caption.ToLower().Trim() == text.ToLower().Trim()));
            if (item == null)
            {
                if(viewPopup) Error($"Не корректные данные");
                this.BorderBrush = Brushes.Red;
                this.BorderThickness = new Thickness(1);
                IsError = true;

            }
            else { CloseError(); }
        }

        public void SetErrorNoMessage()
        {
            ValidData(Text,false);
        
        }

        private void PTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Stays(false);
        }

        private void Stays(bool value)
        {
            if (OnSearch || IsEditable == true) return;
            if (_popup != null) _popup.StaysOpen = value;
        }

        private void PTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Stays(true);
        }

        private void PTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEditable) return;
            if (_isPopupMouse) return;
            IsOpen = !IsOpen;


        }
        private void Popup_Closed(object? sender, EventArgs e)
        {
            IsOpen = false;
            _isPopupMouse = false;
        }
        private void OpenCollection_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = !IsOpen;
        }

        private readonly List<Key> _key = new()
        {
            Key.Left,
            Key.Down,
            Key.Right,
            Key.Up
        };
        private bool SearchKey(KeyEventArgs e)
        {
            foreach (var item in this._key)
            {
                if (item == e.Key) { return true; }
            }
            return false;
        }


        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);


            if (this._popup == null) return;
            if (this._box == null) return;
            if (this._popup.IsOpen)
            {
                if (SearchKey(e))
                {
                    if (this._box.IsFocused) return;
                    this._box.Focus();
                    if (this._box.SelectedIndex == -1) return;
                    ((ListBoxItem)this._box.ItemContainerGenerator.ContainerFromIndex(this._box.SelectedIndex)).Focus();
                }

                if (e.Key == Key.Enter)
                {
                    this.Text = ((PopupItemControl)_box.SelectedItem).Caption.ToString();
                    this.Focus();
                    this.SelectionStart = this.Text.Length;

                

                    if (this._popup == null) return;
                    IsOpen = false;
                }
            }
            else
            {
                if (IsEditable == false && SearchKey(e)) IsOpen = true;
            }
        }


        private void StartSearch()
        {
            if (OnSearch == false) return;

            if ((this._popup == null) || (this._box == null)) return;
            if (this.Text.Trim().Length == 0)
            {
                this.IsOpen = false;
                return;
            }

            if (GetItems(this.Text[..SelectionStart]))
            {
                IsOpen = true;
                if (this._popup.Placement == PlacementMode.Top)
                {
                    this._box.SelectedIndex = _box.Items.Count - 1;
                }
                else
                {
                    this._box.SelectedIndex = 0;
                }
            }
            else
                IsOpen = false;
        }


        private bool GetItems(string inputText)
        {
            if (inputText.Trim().Length == 0)
            {
                return false;
            }

            if (_box == null) return false;
            this._box.ItemsSource = null;
            var _items = this.Items.Where(i => (i.Caption.ToLower().Contains(inputText.ToLower())) || (i.HiddenField.ToLower().Contains(inputText.ToLower())));
            this._box.ItemsSource = _items;

            return this._box.Items.Count > 0 ? true : false;
        }
    }
}
