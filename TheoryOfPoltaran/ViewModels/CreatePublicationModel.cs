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
        public PublicationModel GetModel()
        {
            return JsonConvert.DeserializeObject<PublicationModel>(PublicationJson);
        }
    }
    public class PublicationModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
    }
}
