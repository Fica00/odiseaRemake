using System;
using Newtonsoft.Json;

[Serializable]
public class DatabaseData
{
    [JsonProperty("programas")]public DatabaseProgram[] Programs { get; set; }
    [JsonProperty("episodios")]public DatabaseEpisode[] Episodes { get; set; }
}