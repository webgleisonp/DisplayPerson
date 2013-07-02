using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DisplayPerson.Core;
using DisplayPerson.DAL;
using DisplayPerson.Models;

namespace DisplayPerson.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ContextDAL();
            var repository = new Repository<Person>(context);

            try
            {
                return View(repository.List().OrderBy(x => x.Id).AsEnumerable());
            }
            catch (DbUnexpectedValidationException ex)
            {
                var errors = new List<string>();

                errors.Add(ex.Message);

                ViewBag.Errors = errors;

                return View();
            }
            finally
            {
                repository = null;
                context = null;
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            var context = new ContextDAL();
            var repository = new Repository<Person>(context);

            try
            {
                var filePath = HttpContext.Server.MapPath("~/Files/secondProblem.txt");

                using (var sr = new StreamReader(filePath))
                {
                    var line = string.Empty;

                    var errors = new List<string>();

                    while ((line = sr.ReadLine()) != null)
                    {
                        var newPerson = new Person(line);

                        var modelValidationResults = Common.Validate<Person>(newPerson);

                        if (modelValidationResults.Count() == 0)
                        {
                            repository.Add(newPerson);
                        }

                        foreach (var validationResult in modelValidationResults)
                        {
                            if (!string.IsNullOrEmpty(validationResult.Message))
                            {
                                errors.Add(validationResult.Message);
                            }
                        }
                    }

                    ViewBag.Errors = errors;

                    context.SaveChanges();
                }

                return View(repository.List().OrderBy(x => x.Id).AsEnumerable());
            }
            catch (DbUnexpectedValidationException ex)
            {
                var errors = new List<string>();

                errors.Add(ex.Message);

                ViewBag.Errors = errors;

                return View();
            }
            finally
            {
                repository = null;
                context = null;
            }
        }
    }
}
