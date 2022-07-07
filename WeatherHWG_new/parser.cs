using System;
using System.Collections.Generic;
using System.Text;

namespace Weather_HWG
{
    public class Coord_Response
    {
        public string name { get; set; }
        public Local_names local_names { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }

    public class Local_names
    {
        public string en { get; set; }
        public string ru { get; set; }
    }
    
    public class Weather_response
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string basee { get; set; }
        public Main_W main { get; set; }
        public float visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public double dt { get; set; }
        public Sys sys { get; set; }
        public double timezone { get; set; }
        public Int32 id { get; set; }
        public string name { set; get; }
        public int cod { get; set; }
    }
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }

    }
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main_W
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public float pressure { get; set; }
        public float humidity { get; set; }

    }
    public class Wind
    {
        public float speed { get; set; }
        public float deg { get; set; }
        public float gust { get; set; }

    }
    public class Clouds
    {
        public float all { get; set; }
    }
    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public double sunrise { get; set; }
        public double sunset { get; set; }
    }
}
