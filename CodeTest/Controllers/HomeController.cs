﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using Quote.Contracts;
using Quote.Models;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }

        public ActionResult Test3(string content = "")
        {
            ViewBag.ResultadpApi = content;
            return View();

        }

        [HttpPost]
        public ActionResult ObtenerRespuestaApi(string selectedItem)
        {
            string apiUrl = "https://refactored-pancake.free.beeceptor.com/margin/";

            if(selectedItem == null) selectedItem = string.Empty;


            string url = string.IsNullOrEmpty(selectedItem) ? apiUrl : $"{apiUrl}{selectedItem}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // se añade para poder acceder debido al framework 
                    client.DefaultRequestHeaders.Clear(); //Limpia cualquier encabezado

                    //btengo el response y lo guardo en un HttpResponseMessage
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Valido un status 200
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Se guarda sí se consume con exito
                        string content = response.Content.ReadAsStringAsync().Result;


                        //ViewBag.ResultadoAPI = content;
                        return RedirectToAction("Test3", new { content = content });
                    }
                    else
                    {
                        //Diferente a 200.
                        string content = "{ \"margin\": 0.0 }";
                        //ViewBag.ResultadoAPI = content;

                        return RedirectToAction("Test3", new { content = content });
                    }
                }
            }
            catch (Exception ex)
            {
                string content = "Algo ocurrió mal, intentar más tarde.";
                return RedirectToAction("Test3", new { content = content });
            }
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            return View(result.TourQuotes);
        }
    }
}