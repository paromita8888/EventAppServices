using EventAppServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventAppServices.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Image()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    ConfigurationsController configCon = new ConfigurationsController();
                    //uncomment Below code before publish and comment next 2 lines.
                    //string filePath = Path.Combine(HttpContext.Server.MapPath(configCon.GetConfigvalue("APIURL") + configCon.GetConfigvalue("SpeakerImageURL")),
                    //                               Path.GetFileName(uploadFile.FileName));
                    string filePath = Path.Combine(HttpContext.Server.MapPath(configCon.GetConfigvalue("SpeakerImageURL")),
                                                   Path.GetFileName(file.FileName));
                    file.SaveAs(filePath);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a Image.";
            }
            return RedirectToAction("Image", "Image");
        }
        // [HttpPost]
        ////[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UploadImage(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        ConfigurationsController configCon = new ConfigurationsController();
        //        if (file.ContentLength > 0)
        //        {
        //            //string filePath = Path.Combine(HttpContext.Server.MapPath(configCon.GetConfigvalue("APIURL")+ configCon.GetConfigvalue("SpeakerImageURL")),
        //            //                               Path.GetFileName(uploadFile.FileName));
        //            string filePath = Path.Combine(HttpContext.Server.MapPath(configCon.GetConfigvalue("SpeakerImageURL")),
        //                                           Path.GetFileName(file.FileName));
        //            file.SaveAs(filePath);
        //        }
        //        ViewBag.msg = "Image Uploaded success";
        //        return RedirectToAction("Image", "Image");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.msg = "Fail to upload Image";
        //        throw ex;
        //    }


        //}
    }
}
