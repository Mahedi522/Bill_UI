using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bill_UI.Models
{
    public class CustomersNames
    {
        public int Id { get; set; }
        public List<SelectListItem> CNameList { get; set; }
    }
}