using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// This action is in charge of loading the home page
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This action is in charge of viewing the page where the user inputs the ID number of the customer
        /// </summary>
        /// <returns>View</returns>
        public ActionResult GetCustomer()
        {           
            return View();
        }
        /// <summary>
        /// This action is in charge of loading handling the updating of customer details and sending the input values to the API
        /// </summary>
        /// <param name="inputIDnumber"></param>
        /// <param name="inputFirstname"></param>
        /// <param name="inputlastname"></param>
        /// <param name="inputCell"></param>
        /// <param name="inputEmail"></param>
        /// <returns>View</returns>
        public ActionResult UpdateCustomer(string inputIDnumber, string inputFirstname, string inputlastname, string inputCell, string inputEmail)
        {
            var client = new RestClient("https://localhost:44385/api/Cust?inputIDnumber=" + inputIDnumber + "&inputFirstname=" + inputFirstname + "&inputlastname=" + inputlastname + "&inputCell=" + inputCell + "&inputEmail=" + inputEmail);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);
            string data = JsonConvert.DeserializeObject(response.Content).ToString();
            JObject rss = JObject.Parse(data);
            ViewBag.Message = (string)rss["message"];
            return View("ViewCustomer");
        }
        /// <summary>
        /// This action is in charge of viewing the customer details. It does a get request to the API
        /// </summary>
        /// <param name="inputIDnumber"></param>
        /// <returns>View</returns>
        public ActionResult ViewCustomer(string inputIDnumber)
        {            
                var client = new RestClient("https://localhost:44385/api/Cust?id=" + inputIDnumber);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                IRestResponse response = client.Execute(request);
            try
            {
                //Deserialize the received json 
                string data = JsonConvert.DeserializeObject(response.Content).ToString();
                JObject rss = JObject.Parse(data);
                string firstName = (string)rss["firstname"];
                string lastname = (string)rss["lastname"];
                string id = (string)rss["idNumber"];
                string mobile = (string)rss["mobile"];
                string email = (string)rss["email"];

                //These are viewbags that send the data from the controller to the view
                ViewBag.FirstName = firstName;
                ViewBag.lastname = lastname;
                ViewBag.Id = id;
                ViewBag.Mobile = mobile;
                ViewBag.Email = email;
                return View();
            }
            catch(Exception e)
            {
                return View();
            }
        }
        /// <summary>
        /// This action is in charge of creating the customer. It does a post request to the API
        /// </summary>
        /// <param name="inputIDnumber"></param>
        /// <param name="inputFirstname"></param>
        /// <param name="inputlastname"></param>
        /// <param name="inputCell"></param>
        /// <param name="inputEmail"></param>
        /// <returns>View</returns>
        public ActionResult CreateCustomer(string inputIDnumber, string inputFirstname, string inputlastname, string inputCell, string inputEmail)
        {
            if (inputIDnumber != null)
            {
                if(inputIDnumber.Length>13)
                {
                    ViewBag.Message = "Please enter valid ID Number";
                    return View();
                }
                else
                {
                    var client = new RestClient("https://localhost:44385/api/Cust?inputIDnumber=" + inputIDnumber + "&inputFirstname=" + inputFirstname + "&inputlastname=" + inputlastname + "&inputCell=" + inputCell + "&inputEmail=" + inputEmail);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    IRestResponse response = client.Execute(request);
                    string data = JsonConvert.DeserializeObject(response.Content).ToString();
                    JObject rss = JObject.Parse(data); 
                    ViewBag.Message = (string)rss["message"];
                }
            }
            return View();
        }       
    }
}