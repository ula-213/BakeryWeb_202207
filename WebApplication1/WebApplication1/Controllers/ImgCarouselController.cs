using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class ImgCarouselController : BaseController
    {
        private readonly ImgCarouselDBService imgCarouselService = new ImgCarouselDBService();
        
        public ActionResult Index()
        {
            ImgCarouselViewModel Data = new ImgCarouselViewModel();
            Data.DataList = imgCarouselService.GetDataList();
            return View(Data);
        }
    }
}