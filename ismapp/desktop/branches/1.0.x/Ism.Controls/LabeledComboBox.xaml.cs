using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Ism.Controls
{
    public partial class LabeledComboBox : UserControl
    {
        #region fields
        #region Label
        private static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(LabeledComboBox));

        private static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style), typeof(LabeledComboBox));
        #endregion Label

        #region ComboBox
        #region ComboBox
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.IsDropDownOpen dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.IsDropDownOpen dependency
        //     property.
        public static readonly DependencyProperty IsDropDownOpenProperty =
            ComboBox.IsDropDownOpenProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.IsEditable dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.IsEditable dependency
        //     property.
        public static readonly DependencyProperty IsEditableProperty =
            ComboBox.IsEditableProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.IsReadOnly dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.IsReadOnly dependency
        //     property.
        public static readonly DependencyProperty IsReadOnlyProperty =
            ComboBox.IsReadOnlyProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.MaxDropDownHeight dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.MaxDropDownHeight dependency
        //     property.
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            ComboBox.MaxDropDownHeightProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.SelectionBoxItemTemplate dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.SelectionBoxItemTemplate dependency
        //     property.
        public static readonly DependencyProperty SelectionBoxItemTemplateProperty =
            ComboBox.SelectionBoxItemTemplateProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.StaysOpenOnEdit dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.StaysOpenOnEdit dependency
        //     property.
        public static readonly DependencyProperty StaysOpenOnEditProperty =
            ComboBox.StaysOpenOnEditProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ComboBox.Text dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ComboBox.Text dependency property.
        public static readonly DependencyProperty TextProperty =
            ComboBox.TextProperty.AddOwner(typeof(LabeledComboBox));
        #endregion ComboBox

        #region Selector
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItem dependency
        //     property
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItem dependency
        //     property.
        public static readonly DependencyProperty IsSynchronizedWithCurrentItemProperty =
            ComboBox.IsSynchronizedWithCurrentItemProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Primitives.Selector.SelectedIndex dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Primitives.Selector.SelectedIndex dependency
        //     property.
        public static readonly DependencyProperty SelectedIndexProperty =
            ComboBox.SelectedIndexProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Primitives.Selector.SelectedItem dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Primitives.Selector.SelectedItem dependency
        //     property.
        public static readonly DependencyProperty SelectedItemProperty =
            ComboBox.SelectedItemProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Primitives.Selector.SelectedValuePath dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Primitives.Selector.SelectedValuePath dependency
        //     property.
        public static readonly DependencyProperty SelectedValuePathProperty =
            ComboBox.SelectedValuePathProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Primitives.Selector.SelectedValue dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Primitives.Selector.SelectedValue dependency
        //     property.
        public static readonly DependencyProperty SelectedValueProperty =
            ComboBox.SelectedValueProperty.AddOwner(typeof(LabeledComboBox));
        #endregion Selector

        #region ItemsControl
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.AlternationCount dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.AlternationCount
        //     dependency property.
        public static readonly DependencyProperty AlternationCountProperty =
            ComboBox.AlternationCountProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.DisplayMemberPath dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.DisplayMemberPath dependency
        //     property.
        public static readonly DependencyProperty DisplayMemberPathProperty =
            ComboBox.DisplayMemberPathProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.GroupStyleSelector dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.GroupStyleSelector dependency
        //     property.
        public static readonly DependencyProperty GroupStyleSelectorProperty =
            ComboBox.GroupStyleSelectorProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.HasItems dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.HasItems dependency
        //     property.
        public static readonly DependencyProperty HasItemsProperty =
            ComboBox.HasItemsProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.IsGrouping dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.IsGrouping dependency
        //     property.
        public static readonly DependencyProperty IsGroupingProperty =
            ComboBox.IsGroupingProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitive
        //     dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitive
        //     dependency property.
        public static readonly DependencyProperty IsTextSearchCaseSensitiveProperty =
            ComboBox.IsTextSearchCaseSensitiveProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.IsTextSearchEnabled dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.IsTextSearchEnabled dependency
        //     property.
        public static readonly DependencyProperty IsTextSearchEnabledProperty =
            ComboBox.IsTextSearchEnabledProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemBindingGroup dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemBindingGroup
        //     dependency property.
        public static readonly DependencyProperty ItemBindingGroupProperty =
            ComboBox.ItemBindingGroupProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemContainerStyle dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemContainerStyle dependency
        //     property.
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ComboBox.ItemContainerStyleProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemContainerStyleSelector dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemContainerStyleSelector dependency
        //     property.
        public static readonly DependencyProperty ItemContainerStyleSelectorProperty =
            ComboBox.ItemContainerStyleSelectorProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemsPanel dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemsPanel dependency
        //     property.
        public static readonly DependencyProperty ItemsPanelProperty =
            ComboBox.ItemsPanelProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemsSource dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemsSource dependency
        //     property.
        public static readonly DependencyProperty ItemsSourceProperty =
            ComboBox.ItemsSourceProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemStringFormat dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemStringFormat
        //     dependency property.
        public static readonly DependencyProperty ItemStringFormatProperty =
            ComboBox.ItemStringFormatProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemTemplate dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemTemplate dependency
        //     property.
        public static readonly DependencyProperty ItemTemplateProperty =
            ComboBox.ItemTemplateProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.ItemsControl.ItemTemplateSelector dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.ItemsControl.ItemTemplateSelector dependency
        //     property.
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ComboBox.ItemTemplateSelectorProperty.AddOwner(typeof(LabeledComboBox));
        #endregion ItemsControl

        #region Control
        /***
        // Summary:
        //     Identifies the System.Windows.Controls.Control.Background dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.Background dependency
        //     property.
        public static readonly DependencyProperty CmbBackgroundProperty =
                               DependencyProperty.Register("CmbBackground", typeof(Brush), typeof(LabeledComboBox));
        // Summary:
        //     Identifies the System.Windows.Controls.Control.BorderBrush dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.BorderBrush dependency
        //     property.
        public static readonly DependencyProperty CmbBorderBrushProperty =
                                ComboBox.BorderBrushProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.BorderThickness dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.BorderThickness dependency
        //     property.
        public static readonly DependencyProperty CmbBorderThicknessProperty =
                                ComboBox.BorderThicknessProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.FontFamily dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.FontFamily dependency
        //     property.
        public static readonly DependencyProperty CmbFontFamilyProperty =
                                ComboBox.FontFamilyProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.FontSize dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.FontSize dependency
        //     property.
        public static readonly DependencyProperty CmbFontSizeProperty =
                                ComboBox.FontSizeProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.FontStretch dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.FontStretch dependency
        //     property.
        public static readonly DependencyProperty CmbFontStretchProperty =
                                ComboBox.FontStretchProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.FontStyle dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.FontStyle dependency
        //     property.
        public static readonly DependencyProperty CmbFontStyleProperty =
                                ComboBox.FontStyleProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.FontWeight dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.FontWeight dependency
        //     property.
        public static readonly DependencyProperty CmbFontWeightProperty =
                                ComboBox.FontWeightProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.Foreground dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.Foreground dependency
        //     property.
        public static readonly DependencyProperty CmbForegroundProperty =
                                ComboBox.ForegroundProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.HorizontalContentAlignment dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.HorizontalContentAlignment dependency
        //     property.
        public static readonly DependencyProperty CmbHorizontalContentAlignmentProperty =
                                ComboBox.HorizontalContentAlignmentProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.IsTabStop dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.IsTabStop dependency
        //     property.
        public static readonly DependencyProperty CmbIsTabStopProperty =
                                ComboBox.IsTabStopProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.Padding dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.Padding dependency
        //     property.
        public static readonly DependencyProperty CmbPaddingProperty =
                                ComboBox.PaddingProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.TabIndex dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.TabIndex dependency
        //     property.
        public static readonly DependencyProperty CmbTabIndexProperty =
                                ComboBox.TabIndexProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.Template dependency property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.Template dependency
        //     property.
        public static readonly DependencyProperty CmbTemplateProperty =
                                ComboBox.TemplateProperty.AddOwner(typeof(LabeledComboBox));
        //
        // Summary:
        //     Identifies the System.Windows.Controls.Control.VerticalContentAlignment dependency
        //     property.
        //
        // Returns:
        //     The identifier for the System.Windows.Controls.Control.VerticalContentAlignment dependency
        //     property.
        public static readonly DependencyProperty CmbVerticalContentAlignmentProperty =
                                ComboBox.VerticalContentAlignmentProperty.AddOwner(typeof(LabeledComboBox));
     ***/
        #endregion Control
        #endregion ComboBox
        #endregion fields

        #region constructor
        public LabeledComboBox()
        {
            this.InitializeComponent();
        }
        #endregion constructor

        #region properties
        #region Label
        /// <summary>
        /// Declare a new LabelContent property that can be bound as well
        /// The ComboBoxWithLable.xaml will bind the Label's content to this
        /// </summary>
        public string LabelContent
        {
            get { return (string)GetValue(LabeledComboBox.LabelContentProperty); }
            set { SetValue(LabeledComboBox.LabelContentProperty, value); }
        }

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabeledComboBox.LabelStyleProperty); }
            set { SetValue(LabeledComboBox.LabelStyleProperty, value); }
        }
        #endregion Label

        #region Combobox
        #region Combobox
        //
        // Summary:
        //     Gets or sets a value that indicates whether the drop-down for a combo box
        //     is currently open.
        //
        // Returns:
        //     true if the drop-down is open; otherwise, false. The default is false.
        [Browsable(false)]
        [Category("Appearance")]
        [Bindable(true)]
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(LabeledComboBox.IsDropDownOpenProperty); }
            set { SetValue(LabeledComboBox.IsDropDownOpenProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a value that enables or disables editing of the text in text
        //     box of the System.Windows.Controls.ComboBox.
        //
        // Returns:
        //     true if the System.Windows.Controls.ComboBox can be edited; otherwise false.
        //     The default is false.
        public bool IsEditable
        {
            get { return (bool)GetValue(LabeledComboBox.IsEditableProperty); }
            set { SetValue(LabeledComboBox.IsEditableProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a value that enables selection-only mode, in which the contents
        //     of the combo box are selectable but not editable.
        //
        // Returns:
        //     true if the System.Windows.Controls.ComboBox is read-only; otherwise, false.
        //     The default is false.
        public bool IsReadOnly
        {
            get { return (bool)GetValue(LabeledComboBox.IsReadOnlyProperty); }
            set { SetValue(LabeledComboBox.IsReadOnlyProperty, value); }
        }
        //
        // Summary:
        //     Gets whether the System.Windows.Controls.ComboBox.SelectionBoxItem is highlighted.
        //
        // Returns:
        //     true if the System.Windows.Controls.ComboBox.SelectionBoxItem is highlighted;
        //     otherwise, false.
        //
        // Summary:
        //     Gets or sets the maximum height for a combo box drop-down.
        //
        // Returns:
        //     A double that represents the height that is retrieved or the height to set.
        //     The default value as defined to the property system is a calculated value
        //     based on taking a one-third fraction of the system max screen height parameters,
        //     but this default is potentially overridden by various control templates.
        [Bindable(true)]
        [TypeConverter(typeof(LengthConverter))]
        [Category("Layout")]
        public double MaxDropDownHeight
        {
            get { return (double)GetValue(LabeledComboBox.MaxDropDownHeightProperty); }
            set { SetValue(LabeledComboBox.MaxDropDownHeightProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets whether a System.Windows.Controls.ComboBox that is open and
        //     displays a drop-down control will remain open when a user clicks the System.Windows.Controls.TextBox.
        //
        // Returns:
        //     true to keep the drop-down control open when the user clicks on the text
        //     area to start editing; otherwise, false. The default is false.
        public bool StaysOpenOnEdit
        {
            get { return (bool)GetValue(LabeledComboBox.StaysOpenOnEditProperty); }
            set { SetValue(LabeledComboBox.StaysOpenOnEditProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the text of the currently selected item.
        //
        // Returns:
        //     The string of the currently selected item. The default is an empty string
        //     ("").
        public string Text
        {
            get { return (string)GetValue(LabeledComboBox.TextProperty); }
            set { SetValue(LabeledComboBox.TextProperty, value); }
        }
        #endregion Combobox

        #region Selector
        // Summary:
        //     Gets or sets a value that indicates whether a System.Windows.Controls.Primitives.Selector
        //     should keep the System.Windows.Controls.Primitives.Selector.SelectedItem
        //     synchronized with the current item in the System.Windows.Controls.ItemsControl.Items
        //     property.
        //
        // Returns:
        //     true if the System.Windows.Controls.Primitives.Selector.SelectedItem is always
        //     synchronized with the current item in the System.Windows.Controls.ItemCollection;
        //     false if the System.Windows.Controls.Primitives.Selector.SelectedItem is
        //     never synchronized with the current item; null if the System.Windows.Controls.Primitives.Selector.SelectedItem
        //     is synchronized with the current item only if the System.Windows.Controls.Primitives.Selector
        //     uses a System.Windows.Data.CollectionView. The default value is null.
        [TypeConverter("System.Windows.NullableBoolConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        [Bindable(true)]
        [Category("Behavior")]
        public bool? IsSynchronizedWithCurrentItem
        {
            get { return (bool)GetValue(LabeledComboBox.IsSynchronizedWithCurrentItemProperty); }
            set { SetValue(LabeledComboBox.IsSynchronizedWithCurrentItemProperty, value); }
        }

        /// <summary>
        /// MSDN Reference: http://msdn.microsoft.com/en-us/library/system.windows.controls.combobox.aspx
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(LabeledComboBox.SelectedIndexProperty); }
            set { SetValue(LabeledComboBox.SelectedIndexProperty, value); }
        }
        /// <summary>
        /// MSDN Reference: http://msdn.microsoft.com/en-us/library/system.windows.controls.combobox.aspx
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(LabeledComboBox.SelectedItemProperty); }
            set { SetValue(LabeledComboBox.SelectedItemProperty, value); }
        }

        /// <summary>
        /// MSDN Reference: http://msdn.microsoft.com/en-us/library/system.windows.controls.combobox.aspx
        /// </summary>
        public string SelectedValuePath
        {
            get { return (string)GetValue(LabeledComboBox.SelectedValuePathProperty); }
            set { SetValue(LabeledComboBox.SelectedValuePathProperty, value); }
        }

        /// <summary>
        /// MSDN Reference: http://msdn.microsoft.com/en-us/library/system.windows.controls.combobox.aspx
        /// </summary>
        public object SelectedValue
        {
            get { return (object)GetValue(LabeledComboBox.SelectedValueProperty); }
            set { SetValue(LabeledComboBox.SelectedValueProperty, value); }
        }
        #endregion Selector

        #region ItemsControl
        // Summary:
        //     Gets or sets the number of alternating item containers in the System.Windows.Controls.ItemsControl,
        //     which enables alternating containers to have a unique appearance.
        //
        // Returns:
        //     The number of alternating item containers in the System.Windows.Controls.ItemsControl.
        [Bindable(true)]
        public int AlternationCount
        {
            get { return (int)GetValue(LabeledComboBox.AlternationCountProperty); }
            set { SetValue(LabeledComboBox.AlternationCountProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a path to a value on the source object to serve as the visual
        //     representation of the object.
        //
        // Returns:
        //     The path to a value on the source object. This can be any path, or an XPath
        //     such as "@Name". The default is an empty string ("").
        [Bindable(true)]
        public string DisplayMemberPath
        {
            get { return (string)GetValue(LabeledComboBox.DisplayMemberPathProperty); }
            set { SetValue(LabeledComboBox.DisplayMemberPathProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a method that enables you to provide custom selection logic
        //     for a System.Windows.Controls.GroupStyle to apply to each group in a collection.
        //
        // Returns:
        //     A method that enables you to provide custom selection logic for a System.Windows.Controls.GroupStyle
        //     to apply to each group in a collection.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        public GroupStyleSelector GroupStyleSelector
        {
            get { return (GroupStyleSelector)GetValue(LabeledComboBox.GroupStyleSelectorProperty); }
            set { SetValue(LabeledComboBox.GroupStyleSelectorProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets a value that indicates whether case is a condition when searching
        //     for items.
        //
        // Returns:
        //     true if text searches are case-sensitive; otherwise, false.
        public bool IsTextSearchCaseSensitive
        {
            get { return (bool)GetValue(LabeledComboBox.IsTextSearchCaseSensitiveProperty); }
            set { SetValue(LabeledComboBox.IsTextSearchCaseSensitiveProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets a value that indicates whether System.Windows.Controls.TextSearch
        //     is enabled on the System.Windows.Controls.ItemsControl instance.
        //
        // Returns:
        //     true if System.Windows.Controls.TextSearch is enabled; otherwise, false.
        //     The default is false.
        public bool IsTextSearchEnabled
        {
            get { return (bool)GetValue(LabeledComboBox.IsTextSearchEnabledProperty); }
            set { SetValue(LabeledComboBox.IsTextSearchEnabledProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets the System.Windows.Data.BindingGroup that is copied to each
        //     item in the System.Windows.Controls.ItemsControl.
        //
        // Returns:
        //     The System.Windows.Data.BindingGroup that is copied to each item in the System.Windows.Controls.ItemsControl.
        [Bindable(true)]
        public BindingGroup ItemBindingGroup
        {
            get { return (BindingGroup)GetValue(LabeledComboBox.ItemBindingGroupProperty); }
            set { SetValue(LabeledComboBox.ItemBindingGroupProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets the System.Windows.Style that is applied to the container element
        //     generated for each item.
        //
        // Returns:
        //     The System.Windows.Style that is applied to the container element generated
        //     for each item. The default is null.
        [Bindable(true)]
        [Category("Content")]
        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(LabeledComboBox.ItemContainerStyleProperty); }
            set { SetValue(LabeledComboBox.ItemContainerStyleProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets custom style-selection logic for a style that can be applied
        //     to each generated container element.
        //
        // Returns:
        //     A System.Windows.Controls.StyleSelector object that contains logic that chooses
        //     the style to use as the System.Windows.Controls.ItemsControl.ItemContainerStyle.
        //     The default is null.
        [Bindable(true)]
        [Category("Content")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StyleSelector ItemContainerStyleSelector
        {
            get { return (StyleSelector)GetValue(LabeledComboBox.ItemContainerStyleSelectorProperty); }
            set { SetValue(LabeledComboBox.ItemContainerStyleSelectorProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets a collection used to generate the content of the System.Windows.Controls.ItemsControl.
        //
        // Returns:
        //     A collection that is used to generate the content of the System.Windows.Controls.ItemsControl.
        //     The default is null.
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(LabeledComboBox.ItemsSourceProperty); }
            set { SetValue(LabeledComboBox.ItemsSourceProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a composite string that specifies how to format the items in
        //     the System.Windows.Controls.ItemsControl if they are displayed as strings.
        //
        // Returns:
        //     A composite string that specifies how to format the items in the System.Windows.Controls.ItemsControl
        //     if they are displayed as strings.
        [Bindable(true)]
        public string ItemStringFormat
        {
            get { return (string)GetValue(LabeledComboBox.ItemStringFormatProperty); }
            set { SetValue(LabeledComboBox.ItemStringFormatProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets the System.Windows.DataTemplate used to display each item.
        //
        // Returns:
        //     A System.Windows.DataTemplate that specifies the visualization of the data
        //     objects. The default is null.
        [Bindable(true)]
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(LabeledComboBox.ItemTemplateProperty); }
            set { SetValue(LabeledComboBox.ItemTemplateProperty, value); }
        }

        //
        // Summary:
        //     Gets or sets the custom logic for choosing a template used to display each
        //     item.
        //
        // Returns:
        //     A custom System.Windows.Controls.DataTemplateSelector object that provides
        //     logic and returns a System.Windows.DataTemplate. The default is null.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(LabeledComboBox.ItemTemplateSelectorProperty); }
            set { SetValue(LabeledComboBox.ItemTemplateSelectorProperty, value); }
        }
        #endregion ItemsControl

        #region Control
        /***
        // Summary:
        //     Gets or sets a brush that describes the background of a control.
        //
        // Returns:
        //     The brush that is used to fill the background of the control. The default
        //     is System.Windows.Media.Brushes.Transparent.
        [Bindable(true)]
        [Category("Appearance")]
        public Brush CmbBackground
        {
          get { return (Brush)GetValue(LabeledComboBox.CmbBackgroundProperty); }
          set { SetValue(LabeledComboBox.CmbBackgroundProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a brush that describes the border background of a control.
        //
        // Returns:
        //     The brush that is used to fill the control's border; the default is System.Windows.Media.Brushes.Transparent.
        [Category("Appearance")]
        [Bindable(true)]
        public Brush CmbBorderBrush
        {
          get { return (Brush)GetValue(LabeledComboBox.BorderBrushProperty); }
          set { SetValue(LabeledComboBox.BorderBrushProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the border thickness of a control.
        //
        // Returns:
        //     A thickness value; the default is a thickness of 0 on all four sides.
        [Category("Appearance")]
        [Bindable(true)]
        public Thickness CmbBorderThickness
        {
          get { return (Thickness)GetValue(LabeledComboBox.BorderThicknessProperty); }
          set { SetValue(LabeledComboBox.BorderThicknessProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the font family of the control.
        //
        // Returns:
        //     A font family. The default is the system dialog font.
        [Bindable(true)]
        [Localizability(LocalizationCategory.Font)]
        [Category("Appearance")]
        public FontFamily CmbFontFamily
        {
          get { return (FontFamily)GetValue(LabeledComboBox.FontFamilyProperty); }
          set { SetValue(LabeledComboBox.FontFamilyProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the font size.
        //
        // Returns:
        //     The size of the text in the System.Windows.Controls.Control. The default
        //     is System.Windows.SystemFonts.MessageFontSize. The font size must be a positive
        //     number.
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        [Bindable(true)]
        [Category("Appearance")]
        public double CmbFontSize
        {
          get { return (double)GetValue(LabeledComboBox.FontSizeProperty); }
          set { SetValue(LabeledComboBox.FontSizeProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the degree to which a font is condensed or expanded on the screen.
        //
        // Returns:
        //     A System.Windows.FontStretch value. The default is System.Windows.FontStretches.Normal.
        [Category("Appearance")]
        [Bindable(true)]
        public FontStretch CmbFontStretch
        {
          get { return (FontStretch)GetValue(LabeledComboBox.FontStretchProperty); }
          set { SetValue(LabeledComboBox.FontStretchProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the font style.
        //
        // Returns:
        //     A System.Windows.FontStyle value. The default is System.Windows.FontStyles.Normal.
        [Category("Appearance")]
        [Bindable(true)]
        public FontStyle CmbFontStyle
        {
          get { return (FontStyle)GetValue(LabeledComboBox.FontStyleProperty); }
          set { SetValue(LabeledComboBox.FontStyleProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the weight or thickness of the specified font.
        //
        // Returns:
        //     A System.Windows.FontWeight value. The default is System.Windows.FontWeights.Normal.
        [Bindable(true)]
        [Category("Appearance")]
        public FontWeight CmbFontWeight
        {
          get { return (FontWeight)GetValue(LabeledComboBox.FontWeightProperty); }
          set { SetValue(LabeledComboBox.FontWeightProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a brush that describes the foreground color.
        //
        // Returns:
        //     The brush that paints the foreground of the control. The default value is
        //     the system dialog font color.
        [Bindable(true)]
        [Category("Appearance")]
        public Brush CmbForeground
        {
          get { return (Brush)GetValue(LabeledComboBox.ForegroundProperty); }
          set { SetValue(LabeledComboBox.ForegroundProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the horizontal alignment of the control's content.
        //
        // Returns:
        //     One of the System.Windows.HorizontalAlignment values. The default is System.Windows.HorizontalAlignment.Left.
        [Bindable(true)]
        [Category("Layout")]
        public HorizontalAlignment CmbHorizontalContentAlignment
        {
          get { return (HorizontalAlignment)GetValue(LabeledComboBox.HorizontalContentAlignmentProperty); }
          set { SetValue(LabeledComboBox.HorizontalContentAlignmentProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether a control is included in tab
        //     navigation.
        //
        // Returns:
        //     true if the control is included in tab navigation; otherwise, false. The
        //     default is true.
        [Bindable(true)]
        [Category("Behavior")]
        public bool CmbIsTabStop
        {
          get { return (bool)GetValue(LabeledComboBox.IsTabStopProperty); }
          set { SetValue(LabeledComboBox.IsTabStopProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the padding inside a control.
        //
        // Returns:
        //     The amount of space between the content of a System.Windows.Controls.Control
        //     and its System.Windows.FrameworkElement.Margin or System.Windows.Controls.Border.
        //     The default is a thickness of 0 on all four sides.
        [Category("Layout")]
        [Bindable(true)]
        public Thickness CmbPadding
        {
          get { return (Thickness)GetValue(LabeledComboBox.PaddingProperty); }
          set { SetValue(LabeledComboBox.PaddingProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a value that determines the order in which elements receive
        //     focus when the user navigates through controls by using the TAB key.
        //
        // Returns:
        //     A value that determines the order of logical navigation for a device. The
        //     default value is System.Int32.MaxValue.
        [Category("Behavior")]
        [Bindable(true)]
        public int CmbTabIndex
        {
          get { return (int)GetValue(LabeledComboBox.TabIndexProperty); }
          set { SetValue(LabeledComboBox.TabIndexProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets a control template.
        //
        // Returns:
        //     The template that defines the appearance of the System.Windows.Controls.Control.
        public ControlTemplate CmbTemplate
        {
          get { return (ControlTemplate)GetValue(LabeledComboBox.TemplateProperty); }
          set { SetValue(LabeledComboBox.TemplateProperty, value); }
        }
        //
        // Summary:
        //     Gets or sets the vertical alignment of the control's content.
        //
        // Returns:
        //     One of the System.Windows.VerticalAlignment values. The default is System.Windows.VerticalAlignment.Top.
        [Bindable(true)]
        [Category("Layout")]
        public VerticalAlignment CmbVerticalContentAlignment
        {
          get { return (VerticalAlignment)GetValue(LabeledComboBox.VerticalContentAlignmentProperty); }
          set { SetValue(LabeledComboBox.VerticalContentAlignmentProperty, value); }
        }
         ***/
        #endregion Control
        #endregion Combobox
        #endregion properties
    }
}
