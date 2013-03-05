using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterReader.Models;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Linq;
using System.Configuration;

namespace TwitterReader.Controllers
{
    public class TwittesController : Controller
    {
        //
        // GET: /Twittes/
        public ActionResult Index()
        {
            var model = GetUserTwittes(ConfigurationManager.AppSettings["ScreenName"].ToString());
            SaveModelToDB(model);
            return View(model);
        }

        public ActionResult StatisticsPage()
        {
            return View(GetModelFromDB());
        }

        private void SaveModelToDB(Twittes model)
        {
            var mongo = new Mongo();
            //Mohammadk: 03/04/2013 : connect to localhost
            mongo.Connect();

            var db = mongo.GetDatabase("Twittes");
            var collection = db.GetCollection<Twittes>(ConfigurationManager.AppSettings["ScreenName"].ToString());

            collection.Remove(p => true);

            collection.Save(model);
        }

        private Twittes GetModelFromDB()
        {
            var mongo = new Mongo();
            //Mohammadk: 03/04/2013 : connect to localhost
            mongo.Connect();

            var db = mongo.GetDatabase("Twittes");
            var collection = db.GetCollection<Twittes>(ConfigurationManager.AppSettings["ScreenName"].ToString());
            var model = new Twittes();

            model.Tweets = (collection.Linq().ToList<Twittes>()[0]).Tweets;

            return model;
        }

        private Twittes GetUserTwittes(string screenName)
        {
            var model = new Twittes();

            RestClient client = new RestClient("http://api.twitter.com/1");
            JsonDeserializer jsonDeserializer = new JsonDeserializer();
          
            var request = new RestRequest(Method.GET);

            request.Resource = "statuses/user_timeline.json";

            request.Parameters.Add(new Parameter()
            {
                Name = "screen_name",
                Value = screenName,
                Type = ParameterType.GetOrPost
            });

            var response = client.Execute(request);

            model.Tweets =
              jsonDeserializer.Deserialize<List<Tweet>>(response);

           

            return model;

        }

    }
}
