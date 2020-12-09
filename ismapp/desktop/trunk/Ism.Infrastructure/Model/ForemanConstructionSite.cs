using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class ForemanConstructionSite: BaseModel
    {
        private ConstructionSite _constructionSite;
        private IList<Project> _project;

        public ForemanConstructionSite()
        {
        }

        [JsonProperty("site")]
        public ConstructionSite ConstructionSite
        {
            get { return _constructionSite; }
            set
            {
                SetProperty(ref _constructionSite, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
        [JsonProperty("projects")]
        public IList<Project> Projects
        {
            get { return _project; }
            set
            {
                SetProperty(ref _project, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }



    public class ForemanProject : BaseModel
    {
        private ConstructionSite _constructionSite;
        private Project _project;


        public ConstructionSite ConstructionSite
        {
            get { return _constructionSite; }
            set
            {
                SetProperty(ref _constructionSite, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

        public Project Project
        {
            get { return _project; }
            set
            {
                SetProperty(ref _project, value);
                PropertyDeletegate?.Invoke(this);
            }
        }

    }
}
