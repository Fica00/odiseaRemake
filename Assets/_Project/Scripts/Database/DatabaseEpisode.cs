using System;
using Newtonsoft.Json;

[Serializable]
public class DatabaseEpisode
{
    [JsonProperty("id_episodio")]public int Id;
    [JsonProperty("program_id")]public int? ProgramId;
    [JsonProperty("title_es")]public string TitleEs;
    [JsonProperty("title_pt")]public string TitlePt;
    [JsonProperty("description_es")]public string DescriptionEs;
    [JsonProperty("description_pt")]public string DescriptionPt;
    [JsonProperty("season_number")]public int? SeasonNumber;
    [JsonProperty("episode_number")]public int? EpisodeNumber;
    [JsonProperty("date_start")]public DateTime? DateStart;
    [JsonProperty("date_end")]public DateTime? DateEnd;
    [JsonProperty("url_h264_es")]public string UrlH264ES;
    [JsonProperty("url_h264_pt")]public string UrlH264Pt;
    [JsonProperty("url_h265_es")]public string UrlH265ES;
    [JsonProperty("url_h265_pt")]public string UrlH265Pt;
    [JsonProperty("poster_es")]public string PosterES;
    [JsonProperty("poster_pt")] public string PosterPt;
    [JsonProperty("active")]public int? Active;
    [JsonProperty("item_order")] public int? ItemOrder;
    [JsonProperty("stereoscopic")]public int? Stereoscopic;
}