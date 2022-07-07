using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Weather_HWG;

namespace WeatherHWG_new
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {

        HttpClient Client = new HttpClient();
        APIHelper aPIHelper = new APIHelper();

        private string Location { get; set; } = "Vladivostok";
        private double Lat { get; set; }
        private double Lon { get; set; }

        CancellationTokenSource cts;

        private async void GetCoord()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Lat = location.Latitude;
                    Lon = location.Longitude;

                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void Propeller_anim(bool x, float speed)
        {
            if (!x) { return; }

            int final_speed = (int)speed * 10000;
            while (x)
            {
                await propellerImg.RelRotateTo(360, (uint)final_speed);
            }
        }
        private async void WindArr_rotater(float deg = 0)
        {
            float needed = deg - 45;
            await windImg.RelRotateTo(deg, 0);
        }
        private async void check_connection()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await DisplayAlert("Success", "INTERNET IS ON", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "INTANATA NET", "neOk");
            }
        }
        private string Wind_Direction(float wind)
        {
            if (wind >= 20 && wind < 30)
            {
                return "NNE";
            }
            else if (wind >= 30 && wind < 50)
            {
                return "NE";
            }
            else if (wind >= 50 && wind < 70)
            {
                return "ENE";
            }
            else if (wind >= 70 && wind < 100)
            {
                return "E";
            }
            else if (wind >= 100 && wind < 120)
            {
                return "ESE";
            }
            else if (wind >= 120 && wind < 140)
            {
                return "SE";
            }
            else if (wind >= 140 && wind < 160)
            {
                return "SSE";
            }
            else if (wind >= 160 && wind < 190)
            {
                return "S";
            }
            else if (wind >= 190 && wind < 210)
            {
                return "SSW";
            }
            else if (wind >= 210 && wind < 230)
            {
                return "SW";
            }
            else if (wind >= 230 && wind < 250)
            {
                return "WSW";
            }
            else if (wind >= 250 && wind < 280)
            {
                return "W";
            }
            else if (wind >= 280 && wind < 300)
            {
                return "WNW";
            }
            else if (wind >= 300 && wind < 320)
            {
                return "NW";
            }
            else if (wind >= 320 && wind < 340)
            {
                return "NNW";
            }
            else if (wind >= 340 && wind < 360 || wind < 20)
            {
                return "N";
            }
            return "ERROR";
        }
        private async void Get_response(double lat = 0, double lon = 0)
        {
            string result = await aPIHelper.Get_response(Location, lat, lon);

            Weather_response weather_Responses = JsonConvert.DeserializeObject<Weather_response>(result);
            tempTxt.Text = Math.Round(weather_Responses.main.temp - 273.15, 1).ToString();
            humidiryTxt.Text = weather_Responses.main.humidity.ToString() + "%💧";

            windTxt.Text = weather_Responses.wind.speed.ToString() + " " + Wind_Direction(weather_Responses.wind.deg);
            WindArr_rotater(weather_Responses.wind.deg);
            Propeller_anim(true, weather_Responses.wind.speed);

            pressureTxt.Text = weather_Responses.main.pressure.ToString() + " hpa";
            cloudsTxt.Text = weather_Responses.clouds.all.ToString() + "%";
            cityTxt.Text = weather_Responses.name.ToString();
            mainTxt.Text = weather_Responses.weather[0].main.ToString();

            string data = weather_Responses.dt.ToString();
            //Console.WriteLine(data);
            var date = (new DateTime(1970, 1, 1)).AddSeconds(double.Parse(data));
            var correct_date = date.Date;
            //Другой способ
            //var dateAndTime = DateTime.Now;
            //var datee = dateAndTime.Date;

            dateTxt.Text = date.ToString("dd/MM/yyyy");

            string pics_url = "https://openweathermap.org/img/wn/";

            mainImg.Source = pics_url + weather_Responses.weather[0].icon.ToString() + ".png";
        }

        public WeatherPage()
        {
            InitializeComponent();

            ToolbarItem sharing = new ToolbarItem
            {
                Text = "Share",
                Order = ToolbarItemOrder.Primary,
                Priority = 1,
                Icon = new FileImageSource
                {
                    File = "shareicon.png"
                }

            };

            ToolbarItems.Add(sharing);
            //check_connection();

            sharing.Clicked += async (s, e) =>
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = $"Погода в городе {cityTxt.Text}",
                    Text = $"На данный момент температура в городе {cityTxt.Text} равна: {tempTxt.Text}. \n Влажность: {humidiryTxt.Text}. \n Скорость ветра: {windTxt.Text}. \n Направление ветра: {"SSS"}. \n Состояние погоды: {mainTxt.Text}."
                });
            };

            GetCoord();
            Get_response(Lat, Lon);
            Device.StartTimer(new TimeSpan(0, 0, 300), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Get_response(Lat, Lon);
                });

                return true;
            });
        }
    }
}