// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var subreddit = Subreddit.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Subreddit
    {
        [JsonProperty("submission")]
        public Submission[] Submission { get; set; }
    }

    public partial class Submission
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("viewcount")]
        public object Viewcount { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("upvote")]
        public long Upvote { get; set; }

        [JsonProperty("downvote")]
        public long Downvote { get; set; }

        [JsonProperty("upratio")]
        public double Upratio { get; set; }

        [JsonProperty("subreddit")]
        public SubredditEnum Subreddit { get; set; }

        [JsonProperty("comments")]
        public Comment[] Comments { get; set; }
    }

    public partial class Comment
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("likes")]
        public long Likes { get; set; }

        [JsonProperty("upvote")]
        public long Upvote { get; set; }

        [JsonProperty("downvote")]
        public long Downvote { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }
    }

    public enum SubredditEnum { RGaming };

    public partial struct Comments
    {
        public Comment[] CommentArray;
        public long? Integer;

        public static implicit operator Comments(Comment[] CommentArray) => new Comments { CommentArray = CommentArray };
        public static implicit operator Comments(long Integer) => new Comments { Integer = Integer };
    }

    public partial class Subreddit
    {
        public static Subreddit FromJson(string json) => JsonConvert.DeserializeObject<Subreddit>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Subreddit self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                CommentsConverter.Singleton,
                SubredditEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
   
    internal class CommentsConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Comments) || t == typeof(Comments?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Comments { Integer = integerValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<Comment[]>(reader);
                    return new Comments { CommentArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type Comments");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Comments)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.CommentArray != null)
            {
                serializer.Serialize(writer, value.CommentArray);
                return;
            }
            throw new Exception("Cannot marshal type Comments");
        }

        public static readonly CommentsConverter Singleton = new CommentsConverter();
    }

    internal class SubredditEnumConverter : JsonConverter
    {
       public override bool CanConvert(Type t) => t == typeof(SubredditEnum) || t == typeof(SubredditEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "r/gaming")
            {
                return SubredditEnum.RGaming;
            }
            throw new Exception("Cannot unmarshal type SubredditEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SubredditEnum)untypedValue;
            if (value == SubredditEnum.RGaming)
            {
                serializer.Serialize(writer, "r/gaming");
                return;
            }
            throw new Exception("Cannot marshal type SubredditEnum");
        }

        public static readonly SubredditEnumConverter Singleton = new SubredditEnumConverter();
    }
}
