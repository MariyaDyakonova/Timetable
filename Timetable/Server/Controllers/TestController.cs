using System.Data;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Server.Controllers
{
    public class TestController : ApiController
    {
        public DataTable GetAllHellos()
        {
            var http = new HttpClient();
            var uri = "http://localhost:49494/api/" + "typeofdiscipline" + "/";
            var response = http.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<DataTable>(response);
            return result;
        }
    }
}
