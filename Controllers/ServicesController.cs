using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YarielPrueba.Models;

namespace YarielPrueba.Controllers
{
    public class ServicesController : Controller 
    {  
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {

                db.Configuration.ProxyCreationEnabled = false;
                List<services> serList = db.services.ToList<services>();
                return Json(new { data = serList }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.services.Where(s => s.id == id).FirstOrDefault<services>());
                }
            }

        }
        [HttpPost]
        public ActionResult AddOrEdit(services services)
        {
            using (DBModel db = new DBModel())
            {
                if (services.id == 0)
                {
                    db.services.Add(services);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado Satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(services).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Actualizado Satisfactoriamente." }, JsonRequestBehavior.AllowGet);

                }

            }
        }

        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                services services = db.services.Where(p => p.id == id).FirstOrDefault<services>();
                db.services.Remove(services);
                db.SaveChanges();
                return Json(new { success = true, message = "Eliminado Satisfactoriamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            using (DBModel db = new DBModel())
            {
                try
                {
                    string[] ids = formCollection["ID"].Split(new char[] { ',' });
                    foreach (string id in ids)
                    {
                        var services = db.services.Find(int.Parse(id));
                        db.services.Remove(services);
                        db.SaveChanges();
                    }
                    return Json(new { success = true, message = "Eliminados Satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return Json(new { success = false, message = "Debe seleccionar un registro." }, JsonRequestBehavior.AllowGet);
                }
                
            }
            
        }
    }
}