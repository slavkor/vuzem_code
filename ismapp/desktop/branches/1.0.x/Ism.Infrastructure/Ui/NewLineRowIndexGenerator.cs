using Ism.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls.Timeline;

namespace Ism.Infrastructure.Ui
{
    public class NewLineRowIndexGenerator : IItemRowIndexGenerator
    {
        public void GenerateRowIndexes(List<TimelineRowItem> dataItems)
        {
            var projects = dataItems.OrderBy(item => (item.DataItem as Project)?.Site?.Customer?.Name).Select((item, index) => new Foo(){ Index = index, Item = item }).ToList();

            //int ind = 0;
            //for (int i = 0; i < projects.Count; i++)
            //{

            //    if(i == projects.Count - 1)
            //    {
            //        projects[i].Index2 = ind;
            //        continue;
            //    }

            //    var p1 = projects[i].Item.DataItem as Project;
            //    var p2 = projects[i + 1].Item.DataItem as Project;
            //    //p => p.Start.Date <= data.Item2 && data.Item1 <= p.End.Date

            //    projects[i].Index2 = ind;

            //    var overlap = p1.Start.Date <= p2.End.Date && p2.Start.Date <= p1.End.Date;
            //    if (overlap) ind++;
            //}


            //var customers = dataItems.Select(i => (i.DataItem as Project)).GroupBy(g => g.Site?.Customer?.Name).Select((s, index) => new { Index = index, CustomerName = s.FirstOrDefault()?.Site?.Customer?.Name, Projects = s.ToList()}).OrderBy(o=> o.CustomerName).SelectMany((b, index) => b.Projects ).ToList();

            foreach (TimelineRowItem item in dataItems)
            {
                //var proj = (item.DataItem as Project);
                item.RowIndex = projects.Find(f => f.Item == item).Index;
            }
        }
        internal class Foo
        {
            public int Index { get; set; }
            public int Index2 { get; set; }
            public TimelineRowItem Item { get; set; }
        }
    }



}
