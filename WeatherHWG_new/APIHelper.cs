using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Weather_HWG
{
    public class APIHelper
    {
        HttpClient Client = new HttpClient();

        public async Task<string> Get_response(string city, double Lat = 0, double Lon = 0)
        {
            string appid = "2f5c4fab286fa279ae7141338e7e967a";
            List<Coord_Response> coords;
            double lat;
            double lon;

            if (Lat == 0 && Lon == 0)
            {
                coords = JsonConvert.DeserializeObject<List<Coord_Response>>(await Get_coord(city));
                lat = coords[0].lat;
                lon = coords[0].lon;
            }
            else
            {
                lat = Lat;
                lon = Lon;
            }

            string curr_weather_rq = "https://api.openweathermap.org/data/2.5/weather?lat="+lat.ToString()+"&lon="+lon.ToString()+"&appid="+appid;

            string responseTask = await Client.GetStringAsync(curr_weather_rq);

            return responseTask;
        }

        public async Task<string> Get_coord(string city = "Moscow")
        {
            string appid = "2f5c4fab286fa279ae7141338e7e967a";

            string coord_rq = "https://api.openweathermap.org/geo/1.0/direct?q=" + city + "&limit=1&appid=" + appid;

            string get_coord = await Client.GetStringAsync(coord_rq);

            return get_coord;
        }
    }

    public class APIResponse
    {
        public bool Success => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }
}
