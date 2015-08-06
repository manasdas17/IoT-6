using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using ST.IoT.Data.Interfaces;
using ST.IoT.Services.Minions.Data.Interfaces;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Data.STNeo
{
    public class MinionsSeamlessThingiesNeo4JDataFacade : IMinionsDataService
    {
        private IThingsDataFacade _thingsDataFacade;

        public MinionsSeamlessThingiesNeo4JDataFacade(IThingsDataFacade thingsDataFacade)
        {
            _thingsDataFacade = thingsDataFacade;
        }

        public MinionsResponseMessage PutMinion(MinionsRequestMessage message)
        {
            // TODO: make sure all of the minions data is proper

            try
            {
                var request = JObject.Parse(message.Request);
                var name = request["name"].ToString();
                var content = request["content"].ToString();

                try
                {
                    var check = JObject.Parse(content);
                }
                catch (Exception ex)
                {
                    throw new Exception("Content is not valied json");
                }

                // TODO: convert minions data to ST Things data format

                var template = File.ReadAllText("messages/neo_put_minion_template.json");
                var json = template.Replace("{id}", name).Replace("{content}", content);

                _thingsDataFacade.Put(json);

                return new MinionsResponseMessage(
                    HttpStatusCode.OK,
                    json);
            }
            catch (AggregateException aex)
            {
                return new MinionsResponseMessage(HttpStatusCode.InternalServerError,
                    aex.InnerExceptions.First().Message);
            }
            catch (Exception ex)
            {
                return new MinionsResponseMessage(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public MinionsResponseMessage GetMinion(MinionsRequestMessage message)
        {
            // TODO: make sure all of the minions data is proper

            try
            {
                var request = JObject.Parse(message.Request);
                var name = request["name"].ToString();

                // TODO: convert minions data to ST Things data format

                var template = File.ReadAllText("messages/neo_get_minion_quotes_template.json");
                var json = template.Replace("{id}", name);

                var jo = JObject.Parse(json);

                if (request["Meta"]["Paging"] != null)
                {
                    jo["Meta"]["Paging"] = JObject.Parse(request["Meta"]["Paging"].ToString());
                }

                var result = _thingsDataFacade.Get(jo.ToString());

                var response_jo = JObject.Parse(result);
                if (!response_jo["Results"].Any())
                {
                    return new MinionsResponseMessage(HttpStatusCode.NotFound, result);
                }


                return new MinionsResponseMessage(
                    HttpStatusCode.OK,
                    result);
            }
            catch (AggregateException aex)
            {
                return new MinionsResponseMessage(HttpStatusCode.InternalServerError,
                    aex.InnerExceptions.First().Message);
            }
            catch (Exception ex)
            {
                return new MinionsResponseMessage(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
