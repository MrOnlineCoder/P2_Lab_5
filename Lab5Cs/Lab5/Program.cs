using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    class NewsRecord
    {
        public string Title;
        public string[] Keywords;

        public NewsRecord(string title, string[] keywords)
        {
            Title = title;
            Keywords = keywords;
        }

        public override string ToString()
        {
            return $"NewsRecord({Title}, {Keywords})";
        }
    }

    class TextNewsRecord : NewsRecord
    {
        public string Content;

        public TextNewsRecord(string title, string[] keywords, string content) : base(title, keywords)
        {
            Content = content;
        }
    }

    class VideoNewsRecord : NewsRecord
    {
        public string URL;

        public VideoNewsRecord(string title, string[] keywords, string url) : base(title, keywords)
        {
            URL = url;
        }
    }

    /*
     * Custom subscribing mechanism is implemented because we couldn't find
     * how to emit events via delegates in C# to 'specific' listeners,
     * in our case - text news with a topic
     * Delegates seem to emit event to everyone
     */
    struct NewsSubscriber
    {
        public string Type;
        public string Topic;
        public Action<NewsRecord> Handler;
    }

    class NewsStation
    {
        private List<NewsSubscriber> Subscribers;

        private List<TextNewsRecord> TextNews;
        private List<VideoNewsRecord> VideoNews;

        public NewsStation()
        {
            Subscribers = new List<NewsSubscriber>();
            TextNews = new List<TextNewsRecord>();
            VideoNews = new List<VideoNewsRecord>();
        }

        public void SubscribeToAllTextNews(Action<NewsRecord> handler)
        {
            Subscribers.Add(new NewsSubscriber {
                   Type = "text",
                   Topic = null,
                   Handler = handler
            });
        }

        public void SubscribeToAllVideoNews(Action<NewsRecord> handler)
        {
            Subscribers.Add(new NewsSubscriber
            {
                Type = "video",
                Topic = null,
                Handler = handler
            });
        }

        public void SubscribeToTopicTextNews(string topic, Action<NewsRecord> handler)
        {
            Subscribers.Add(new NewsSubscriber
            {
                Type = "text",
                Topic = topic,
                Handler = handler
            });
        }

        public void Publish(NewsRecord record)
        {
            string targetNewsType = "text";

            if (record is TextNewsRecord)
            {
                TextNews.Add(record as TextNewsRecord);
            } else
            {
                VideoNews.Add(record as VideoNewsRecord);
                targetNewsType = "video";
            }

            var fittingSubscribers = Subscribers.Where((subscriber) => subscriber.Type == targetNewsType).Where(subscriber =>
            {
                if (subscriber.Topic != null)
                {
                    return record.Keywords.Contains(subscriber.Topic);
                }
                else
                {
                    return true;
                }
            });

            foreach (var subscriber in fittingSubscribers)
            {
                subscriber.Handler(record);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            NewsStation station = new NewsStation();

            station.SubscribeToAllTextNews(record =>
            {
                Console.WriteLine($"[AllTextNews] {record.Title}");
            });

            station.SubscribeToAllVideoNews(record =>
            {
                Console.WriteLine($"[AllVideoNews] {record.Title}");
            });

            station.SubscribeToTopicTextNews("sport", record =>
            {
                Console.WriteLine($"[SportNews] {record.Title}");
            });

            station.Publish(new TextNewsRecord("Portal gun has been invented", new string [] { "science", "future" }, "Aperture science has invented working Portal gun prototype."));
            station.Publish(new VideoNewsRecord("Donald Trump visits UzhNU", new string[] { "education", "stars" }, "https://www.youtube.com/watch?v=dQw4w9WgXcQ."));
            station.Publish(new TextNewsRecord("Ukraine wins international football cup", new string[] { "national", "sport" }, "Beats Spain in finals with score 2 : 1"));
            Console.ReadKey();
        }
    }
}
