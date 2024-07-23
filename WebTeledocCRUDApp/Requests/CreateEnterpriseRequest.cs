using System.ComponentModel.DataAnnotations;

namespace WebTeledocCRUDApp.Requests
{
    public class CreateEnterpriseRequest
    {
        public string Name { get; set; }
        public string INN { get; set; }
        public bool IsIndividual { get; set; }
    }
}
