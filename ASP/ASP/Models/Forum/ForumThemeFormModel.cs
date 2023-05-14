using Microsoft.AspNetCore.Mvc;

namespace ASP.Models.Forum
{
    public class ForumThemeFormModel
    {
        public String Title { get; set; } = null!;

        public String Description { get; set; } = null!;
        
        public string LogoId { get; set; } = null!;

        public String SectionId { get; set; } = null!;
    }
}
