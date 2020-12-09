using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Behaviors
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interactivity;
    using Telerik.Windows.Controls;
    using Telerik.Windows.Controls.GridView;
    using Telerik.Windows.Controls.TreeView;
    using Telerik.Windows.DragDrop;
    
    using DragEventArgs = Telerik.Windows.DragDrop.DragEventArgs;

    public class DragDropBehavior : Behavior<RadGridView>
    {
        public ObservableCollection<object> Collection { get; set; }
        public int DropIndex { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.RowLoaded += this.AssociatedObjectRowLoaded;
            DragDropManager.AddDropHandler(this.AssociatedObject, AssociatedObjectDragEnter);
            DragDropManager.AddDragOverHandler(this.AssociatedObject, this.OnRowDragOver, true);
        }

        private void AssociatedObjectRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.Row is GridViewHeaderRow || e.Row is GridViewNewRow || e.Row is GridViewFooterRow)
            {
                return;
            }

            var row = e.Row as GridViewRow;
            this.InitializeRowDragAndDrop(row);
        }

        private void InitializeRowDragAndDrop(GridViewRow row)
        {
            if (row == null)
            {
                return;
            }

            DragDropManager.RemoveDragOverHandler(row, this.OnRowDragOver);
            DragDropManager.AddDragOverHandler(row, this.OnRowDragOver);
            DragDropManager.AddPreviewDropHandler(row, this.OnPreviewDrop);
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            int a = 0;
        }

        private void AssociatedObjectDragEnter(object sender, DragEventArgs e)
        {
            var dataFromObject = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;

            if (dataFromObject != null)
            {
                IEnumerable<object> draggedItems = dataFromObject.DraggedItems.ToList();
                /*
                Forach(var item in draggedItems )
                this.AssociatedObject.Items.AddNewItem(item);
                */
                //AddItemToList();// Add your custom code to do the injection of dragged items to grid.
            }
        }

        private void OnRowDragOver(object sender, DragEventArgs e)
        {
            // the below line of codes will force the control to accept even the disallowed objects.

            var options = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;
            if (options != null)
            {
                options.DropAction = DropAction.Move;
                var dragVisual = options.DragVisual as TreeViewDragVisual;
                if (dragVisual != null)
                {
                    dragVisual.IsDropPossible = true;
                }
            }

            //Now we will find the and update the index of dropped item.
            var row = sender as GridViewRow;
            if (row == null)
            {
                return;
            }

            int dropIndex = (this.AssociatedObject.Items as IList).IndexOf(row.DataContext);
            DropPosition dropPositionFromPoint = this.GetDropPositionFromPoint(e.GetPosition(row), row);

            if (dropIndex >= row.GridViewDataControl.Items.Count - 1 &&
                dropPositionFromPoint == DropPosition.After)
            {
                return;
            }

            DropIndex = dropIndex;
        }

        private DropPosition GetDropPositionFromPoint(Point absoluteMousePosition, GridViewRow row)
        {
            if (row != null)
            {
                return absoluteMousePosition.Y < row.ActualHeight / 2 ? DropPosition.Before : DropPosition.After;
            }

            return DropPosition.Inside;
        }
    }
}
