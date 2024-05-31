using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bill_UI.Models
{
    public class ProductNames
    {
        public int Id { get; set; }
        public List<SelectListItem> PNameList { get; set; }
    }
}
