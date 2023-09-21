using System.Data;
using System.Formats.Tar;
using System.Text;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace homepage.Util;

public class PaperRaw {
    public String Title = "";
    public String[] With = {};
    public String? Link;
    public String Year = "";
    public String Blurb = "";
}

public class Paper {
    public String Title = new("");
    public String With = new("");
    public String? Link; 
    public String Year = new("");
    public MarkupString Blurb = new("");
}

public class TopicRaw {
    public String Title = new("");
    public String Blurb = new("");
}

public class Topic {
    public String Title = new("");
    public MarkupString Blurb = new("");
}

public class Util {
    public static MarkdownPipeline Pipeline { get; private set; } = 
        new MarkdownPipelineBuilder().UseMathematics().Build();

    public static String Slurp(String path) {
        var sr = new StreamReader(path);
        return sr is not null ? sr.ReadToEnd() : "";
    }

    public static MarkupString ParseMD(String md) {
        var html = Markdown.ToHtml(md, Pipeline);
        var htmlSafe = new MarkupString(html);
        return htmlSafe;
    }

    public static MarkupString Parse(string fileName) {
        return ParseMD(Slurp($"wwwroot/assets/content/{fileName}")); 
    }

    public static MarkupString GetSVG(string path) {
        var svgSafe = new MarkupString(Slurp($"wwwroot/assets/images/{path}"));
        return svgSafe; 
    }

    public static String Fake() {
        return String.Concat(Faker.Lorem.Paragraphs(20));
    }

    public static Paper ParsePaper(PaperRaw pr)
    { 
        var blurb = ParseMD(Slurp($"wwwroot/assets/content/Papers/blurbs/{pr.Blurb}"));
        var with = "";
        var N = pr.With.Length;
        if (N == 1) {
            with = pr.With[0];
        } else if (N > 1) {
            StringBuilder sb = new("joint work with ");
            for (int i = 0; i < N; i++) {
                sb.Append(pr.With[i]);
                if (i == N - 2) {
                    sb.Append(" and ");
                } else if (i == N - 1) {
                    sb.Append(".");
                } else {
                    sb.Append(", ");
                } 
            }
            with = sb.ToString();
        }

        return new Paper{ Title = pr.Title, With = with, Year = pr.Year, Link = pr.Link, Blurb = blurb };
    }

    public static Paper[] GetPapers() 
    {
        var json = Slurp("wwwroot/assets/content/Papers/index.json");
        var papers = JsonConvert.DeserializeObject<PaperRaw[]>(json)!;
        return papers.Select(p => ParsePaper(p)).ToArray();
    }

    public static Topic ParseTopic(TopicRaw tr)
    { 
        var blurb = ParseMD(Slurp($"wwwroot/assets/content/Topics/{tr.Blurb}"));
        return new Topic { Title = tr.Title, Blurb = blurb };
    }

    public static Topic[] GetTopics()
    {
        var json = Slurp("wwwroot/assets/content/Topics/index.json");
        var topics = JsonConvert.DeserializeObject<TopicRaw[]>(json)!;
        return topics.Select(p => ParseTopic(p)).ToArray();
    }
}