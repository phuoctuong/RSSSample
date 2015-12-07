using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using AngleSharp;
using System.Collections;
using System.Threading.Tasks;

namespace RSS.Controllers
{
    public class RSSController : ApiController
    {
        ArrayList date_arr = new ArrayList();
        ArrayList title_arr = new ArrayList();
       
        [HttpGet]
        public async Task<Rss20FeedFormatter> Get()
        {
            // Setup the configuration to support document loading
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            // Load the names of all The Big Bang Theory episodes from Wikipedia
            var address = "http://www.fit.hcmus.edu.vn/vn/Default.aspx?tabid=53";
            // Asynchronously get the document in a new context using the configuration
            var document = await BrowsingContext.New(config).OpenAsync(address);
            // This CSS selector gets the desired content
            var cellSelector_title = ".post_title";
            var cellSelector_date = ".day_month";
            // Perform the query to get all cells with the content
            var cells_title = document.QuerySelectorAll(cellSelector_title);
            var cells_date = document.QuerySelectorAll(cellSelector_date);

            Thread.Sleep(5000);
            //var cells_date = document.QuerySelectorAll(cellSelector_date);
            // We are only interested in the text - select it with LINQ
            // var titles = cells.Select(m => m.TextContent);

            foreach (var dat in cells_date)
            {
                date_arr.Add(dat.TextContent);
            }
            foreach (var tit in cells_title)
            {
                title_arr.Add(tit.Children[0].Attributes["title"].Value);
            }
           
            

                List<SyndicationItem> items = new List<SyndicationItem>();

                var feed = new SyndicationFeed("Khoa hoc tu nhien CNTT feed", "This is a test", new Uri("http://www.fit.hcmus.edu.vn/vn/Default.aspx?tabid=53"));
                feed.Authors.Add(new SyndicationPerson("phuoctuong285@gmail.com"));
                feed.Categories.Add(new SyndicationCategory("Categories Feed"));
                feed.Description = new TextSyndicationContent("This is KHOA HOC TU NHIEN_CNTT news feed");


                for (int i = 0; i < title_arr.Count; i++)
                {
                    SyndicationItem item = new SyndicationItem(
                        title_arr[i].ToString(),
                       "Date: "+date_arr[i].ToString() ,
                        new Uri("http://www.fit.hcmus.edu.vn/vn/Default.aspx?tabid=53")
                        );

                    items.Add(item);
                }
                feed.Items = items;
                return new Rss20FeedFormatter(feed);
            
            }
            
               
           
        }

    }

