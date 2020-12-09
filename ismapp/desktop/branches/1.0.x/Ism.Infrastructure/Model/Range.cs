using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ism.Infrastructure.Model
{
    public class Range: BaseModel
    {
        private Day _from;
        private Day _to;

        private BaseModel _origin;
        private BaseModel _destination;

        public Range() : this(DateTime.Now) {}

        public Range(DateTime from): this(from, from){}

        public Range(DateTime from, DateTime to)
        {
            From = new Day(from);
            To = new Day(to);
        }

        [JsonProperty("from")]
        public Day From
        {
            get { return _from; }
            set
            {
                SetProperty(ref _from, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonProperty("to")]
        public Day To
        {
            get { return _to; }
            set
            {
                SetProperty(ref _to, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("origin")]
        public BaseModel Origin
        {
            get { return _origin; }
            set
            {
                SetProperty(ref _origin, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("destination")]
        public BaseModel Destination
        {
            get { return _destination; }
            set
            {
                SetProperty(ref _destination, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }
}
