using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using Ism.Infrastructure.Model;

namespace Ism.Controls.TimeLine
{
    public class TimelineControl : Control
    {
        #region Fields
        public const string PART_TREEVIEW = "PART_TreeView";
        public const string PART_LISTBOX = "PART_ListBox";
        #endregion

        #region Properties



        public bool InvalideMathes
        {
            get { return (bool)GetValue(InvalideMathesProperty); }
            set {
                SetValue(InvalideMathesProperty, value);
                GetMatches();
            }
        }

        // Using a DependencyProperty as the backing store for InvalideMathes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvalideMathesProperty =
            DependencyProperty.Register("InvalideMathes", typeof(bool), typeof(TimelineControl));



        public static readonly DependencyProperty DataProviderProperty =
            DependencyProperty.Register("DataProvider", typeof(ITimelineDataProvider), typeof(TimelineControl));
        public ITimelineDataProvider DataProvider
        {
            get {
                return (ITimelineDataProvider)GetValue(DataProviderProperty);
            }
            set {
                SetValue(DataProviderProperty, value);
            }
        }

        public static readonly DependencyProperty HierarchicalTemplateProperty =
            DependencyProperty.Register("HierarchicalTemplate", typeof(HierarchicalDataTemplate), typeof(TimelineControl));
        public HierarchicalDataTemplate HierarchicalTemplate
        {
            get
            {
                return (HierarchicalDataTemplate)GetValue(HierarchicalTemplateProperty);
            }
            set
            {
                SetValue(HierarchicalTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
    DependencyProperty.Register("SelectedItem", typeof(object), typeof(TimelineControl));
        public object SelectedItem
        {
            get
            {
                return (object) GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }




        public TreeView TreeView { get; set; }
        public ListBox ListBox { get; set; }
        #endregion

        #region Constructor
        static TimelineControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineControl), new FrameworkPropertyMetadata(typeof(TimelineControl)));
        }

        public TimelineControl() { }
        #endregion

        #region Public Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TreeView = Template.FindName(PART_TREEVIEW, this) as TreeView;
            ListBox = Template.FindName(PART_LISTBOX, this) as ListBox;

            if (TreeView != null)
            {
                TreeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                TreeView.MouseLeftButtonUp += TreeView_MouseLeftButtonUp;
                TreeView.ItemsSource = DataProvider.GetFilters();
            }

            if(null != ListBox)
            {
                ListBox.SelectionChanged += ListBox_SelectionChanged;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = e.AddedItems.Count == 0 ? null : e.AddedItems[0];
        }

        private void TreeView_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetMatches();
        }

        private void GetMatches()
        {
            DataProvider.GetMatchesAsync((HierarchyData)TreeView.SelectedItem, OnGetMatchesCallback);
        }

        private void OnGetMatchesCallback(IEnumerable list)
        {
            ListBox.ItemsSource = list;
        }

        #endregion

        #region Event Handlers
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            HierarchyData newValue = e.NewValue as HierarchyData;
            HierarchyData oldValue = e.OldValue as HierarchyData;

            if (newValue == null)
                return;

            if (newValue.HasChildren)
                newValue.IsExpanded = true;

            if (oldValue != null)
            {
                if (oldValue.Level == newValue.Level)
                    oldValue.IsExpanded = false;
                else
                {
                    bool sameParent = false;
                    if (newValue.Level > oldValue.Level)
                        sameParent = oldValue == GetParentOf(newValue, oldValue.Level);
                    else
                        sameParent = newValue == GetParentOf(oldValue, newValue.Level);

                    if (!sameParent)
                        CollapseNodes(GetParentOf(oldValue, 1));
                }
            }
        }
        #endregion

        #region Helper Methods
        private void CollapseNodes(HierarchyData hierarchyBase)
        {
            if (hierarchyBase != null)
            {
                hierarchyBase.IsExpanded = false;
                if (hierarchyBase.Children != null)
                {
                    foreach (HierarchyData child in hierarchyBase.Children)
                    {
                        CollapseNodes(child);
                    }
                }
            }
        }

        private HierarchyData GetParentOf(HierarchyData oldValue, int level)
        {
            if (oldValue == null || level < 1)
                return null;

            HierarchyData parent = oldValue;
            while (parent.Level > level)
            {
                parent = parent.Parent;
            }

            return parent;
        }
        #endregion
    }
}
