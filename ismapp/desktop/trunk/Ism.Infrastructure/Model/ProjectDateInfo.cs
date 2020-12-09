using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Extensions;
using Prism.Commands;

namespace Ism.Infrastructure.Model
{
    public class ProjectDateInfo
    {
        public ProjectDateInfo() : this(DateTime.Now) {}
        public ProjectDateInfo(DateTime date):this(date, 0){}
        public ProjectDateInfo(DateTime date, int noOfWorkers) : this(date, noOfWorkers, 0) { }
        public ProjectDateInfo(DateTime date, int noOfWorkers, int max) : this(date, noOfWorkers, max, null) { }

        public ProjectDateInfo(DateTime date, int noOfWorkers, int max, WorkPlace workplace)
        {
            Date = date;
            NoOfWorkers = noOfWorkers;
            Max = max;
            WorkPlace = workplace;
        }
        public WorkPlace WorkPlace { get; set; }
        public DateTime Date { get; set; }
        public int NoOfWorkers { get; set; }
        public int Max { get; set; }

        public int Limit { get { return NoOfWorkers >= Max ? Max : NoOfWorkers; } }
        public int LimitOverflow { get { return NoOfWorkers >= Max ? NoOfWorkers - Max : 0; } }

    }

    public class WorkPlaceDateInfo
    {
        public WorkPlaceDateInfo() : this(DateTime.Now) { }
        public WorkPlaceDateInfo(DateTime date) : this(date, 0) { }
        public WorkPlaceDateInfo(DateTime date, int noOfWorkers) : this(date, noOfWorkers, 0) { }
        public WorkPlaceDateInfo(DateTime date, int noOfWorkers, int max) : this(date, noOfWorkers, max, null) { }

        public WorkPlaceDateInfo(DateTime date, int noOfWorkers, int max, WorkPlace workplace)
        {
            Date = date;
            NoOfWorkers = noOfWorkers;
            Max = max;
            WorkPlace = workplace;
        }
        public WorkPlace WorkPlace { get; set; }
        public DateTime Date { get; set; }
        public int NoOfWorkers { get; set; }
        public int Max { get; set; }

        public int Limit { get { return NoOfWorkers >= Max ? Max : NoOfWorkers; } }
        public int LimitOverflow { get { return NoOfWorkers >= Max ? NoOfWorkers - Max : 0; } }
        public override string ToString()
        {
            return $"{Date} : no: {NoOfWorkers}, awailable: {Max}, missing: {LimitOverflow}";
        }
    }

    public class WorkPlacePeriod
    {
        public WorkPlacePeriod() : this(DateTime.Now) { }
        public WorkPlacePeriod(DateTime start) : this(start, DateTime.Now ) { }
        public WorkPlacePeriod(DateTime start, DateTime end) : this(start, end, null) { }
        public WorkPlacePeriod(DateTime start, DateTime end, WorkPlace workplace): this(start, end, workplace, 0) { }
        public WorkPlacePeriod(DateTime start, DateTime end, WorkPlace workplace, int noOfWorkers) {
            Start = start;
            End = end;
            WorkPlace = workplace;
            NoOfWorkers = noOfWorkers;
        }

        public WorkPlace WorkPlace { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NoOfWorkers { get; set; }

        public List<WorkPlaceDateInfo> Days => (End - Start).WorkPlaceDays(Start.Date, NoOfWorkers, WorkPlace);
    }


    public class WorkPlaceInfo : BaseModel
    {
        public WorkPlaceInfo() : this(null) { }
        public WorkPlaceInfo(WorkPlace workplace) : this(workplace, null) { }
        public WorkPlaceInfo(WorkPlace workplace, List<WorkPlaceDateInfo> dates)
        {
            WorkPlace = workplace;
            Dates = dates;

        }
        public WorkPlaceInfo (ref DateTime minDate, ref DateTime maxDate, ref DateTime visStart, ref DateTime visEnd, ref DateTime selStart, ref DateTime selEnd)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            VisibleStart = visStart;
            VisibleEnd = visEnd;
            SelectionStart = selStart;
            SelectionEnd = selEnd;
        }


        public WorkPlace WorkPlace { get; set; }

        public List<WorkPlaceDateInfo> Dates { get; set; }
        public DelegateCommand<object> SelectionChangedEventCommand { get; set; }
        public Action<string, DateTime> DateCallbackAction { get; set; }

        private DateTime _visibleStart;

        public DateTime VisibleStart
        {
            get { return _visibleStart; }
            set
            {
                SetProperty(ref _visibleStart, value);
                DateCallbackAction?.Invoke(nameof(VisibleStart), value);
            }
        }

        
        private DateTime _visibleEnd;


        public DateTime VisibleEnd
        {
            get { return _visibleEnd; }
            set
            {
                SetProperty(ref _visibleEnd, value);
                DateCallbackAction?.Invoke(nameof(VisibleEnd), value);

            }
        }

        private DateTime _selectionStart;

        public DateTime SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                SetProperty(ref _selectionStart, value);
                DateCallbackAction?.Invoke(nameof(SelectionStart), value);

            }
        }

        private DateTime _selectionEnd;

        public DateTime SelectionEnd
        {
            get { return _selectionEnd; }
            set
            {
                SetProperty(ref _selectionEnd, value);
                DateCallbackAction?.Invoke(nameof(SelectionEnd), value);

            }
        }


        private DateTime _minDate;

        public DateTime MinDate
        {
            get { return _minDate; }
            set
            {
                SetProperty(ref _minDate, value);
                DateCallbackAction?.Invoke(nameof(MinDate), value);

            }
        }

        private DateTime _maxDate;

        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                SetProperty(ref _maxDate, value);
                DateCallbackAction?.Invoke(nameof(MaxDate), value);

            }
        }
    }


}
