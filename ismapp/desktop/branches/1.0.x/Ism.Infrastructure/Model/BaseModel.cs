using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Validation;
using Newtonsoft.Json.Converters;
using Microsoft.Practices.ServiceLocation;
using Ism.Infrastructure.Services;

namespace Ism.Infrastructure.Model
{
    public class BaseModel : ValidatableBindableBase, IBaseModel
    {
        private int _id;
        private long _iid;
        private string _uuId;
        private int _active;
        private int _deleted;
        private DateTime _createDate;
        private DateTime _modifyDate;
        private int _status;
        private string _user;

        public BaseModel()
        {
            PropertyDeletegate = null;
            Active = 1;
            Deleted = 0;
        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [DefaultValue(-1)]
        [JsonProperty("iid")]
        public long Iid
        {
            get { return _iid; }
            set
            {
                SetProperty(ref _iid, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("uuid", DefaultValueHandling = DefaultValueHandling.Include)]
        public string UuId
        {
            get { return _uuId; }
            set
            {
                SetProperty(ref _uuId, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [DefaultValue(1)]
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public int Active
        {
            get { return _active; }
            set
            {
                SetProperty(ref _active, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [DefaultValue(0)]
        [JsonProperty("deleted", NullValueHandling = NullValueHandling.Ignore)]
        public int Deleted
        {
            get { return _deleted; }
            set
            {
                SetProperty(ref _deleted, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("crdate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                SetProperty(ref _createDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }


        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("mfdate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set
            {
                SetProperty(ref _modifyDate, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(0)]
        public int Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(0)]
        public string User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        [JsonIgnore]
        public Extensions.Extensions.SetPropertyDelegate PropertyDeletegate { get; set; }


        public override string ToString()
        {
            return $"{UuId?.ToString()}#{Iid}#{Id.ToString()}";
        }
    }
}
