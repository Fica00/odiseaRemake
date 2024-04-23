using System;

[Serializable]
public class DatabasePanel
{
    public int ID;
    public int Index;
    public int Theme;
    public string InfoES;
    public string InfoPT;
    public string SynopsisES;
    public string SynopsisPT;
    public string ThumbnailES;
    public string ThumbnailPT;
    public DatabaseChapter[] Chapters;
}