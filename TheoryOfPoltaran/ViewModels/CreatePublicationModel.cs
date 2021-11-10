using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheoryOfPoltaran.ViewModels
{
    public class CreatePublicationModel
    {
        public string PublicationJson { get; set; }
        public PublicationViewModel GetModel()
        {
            return JsonConvert.DeserializeObject<PublicationViewModel>(PublicationJson);
        }
    }
}
