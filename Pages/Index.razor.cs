using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.Win32.SafeHandles;


namespace homepage.Pages;
using static Util.Util;

public partial class Index 
{
    public (string, string)[] Sites = new []{
        ("https://github.com/mbrc12", "github.svg"),
        ("mailto:mbrc12@gmail.com", "email.svg"),
        ("assets/docs/Mriganka_AcademicCV.pdf", "cv.svg"),
        ("assets/docs/Mriganka_IndustrialCV.pdf", "cv.svg")
    };

    public MarkupString About = new("");
    public MarkupString Extra = new("");
    
    protected override void OnInitialized()
    {

        About = Parse("About.md");
        Extra = Parse("Extra.md");
    }
}