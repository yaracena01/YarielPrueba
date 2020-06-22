using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YarielPrueba.Models;

namespace YarielPrueba.Controllers
{
    public class PersonaController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Persona> perList = db.Persona.ToList<Persona>();
                return Json(new { data = perList }, JsonRequestBehavior.AllowGet);
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
                    return View(db.Persona.Where(p => p.ID == id).FirstOrDefault<Persona>());
                }
            }

        }
        [HttpPost]
        public ActionResult AddOrEdit(Persona persona)
        {
            using (DBModel db = new DBModel())
            {
                if (persona.ID == 0)
                {
                    db.Persona.Add(persona);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado Satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(persona).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Actualizado Satisfactoriamente." }, JsonRequestBehavior.AllowGet);

                }

            }
        }

        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Persona persona = db.Persona.Where(p => p.ID == id).FirstOrDefault<Persona>();
                db.Persona.Remove(persona);
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
                        var persona = db.Persona.Find(int.Parse(id));
                        db.Persona.Remove(persona);
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