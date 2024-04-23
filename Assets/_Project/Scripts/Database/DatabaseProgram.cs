using System;
using Newtonsoft.Json;

[Serializable]
public class DatabaseProgram
{
    [JsonProperty("id_programa")]public int Id;
    [JsonProperty("title_es")]public string TitleEs;
    [JsonProperty("title_pt")]public string TitlePt;
    [JsonProperty("description_es")]public string DescriptionEs;
    [JsonProperty("description_pt")]public string DescriptionPt;
    [JsonProperty("category")]public int? Category;
    [JsonProperty("poster_es")]public string PosterEs;
    [JsonProperty("poster_pt")]public string PosterPt;
    [JsonProperty("active")]public int? Active;
    [JsonProperty("item_order")]public int? ItemOrder;
}