//using GtechTask.DAL;
//using GtechTask.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;

//namespace GtechTask.Controllers
//{
//    public class ApplicationController : Controller
//    {
//        private readonly User_DL _userDl;

//        public ApplicationController(IConfiguration configuration)
//        {
//            _userDl = new User_DL(configuration);
//        }

//        public IActionResult Index()
//        {
//            var eMIUser = new EMIUser();
//            return View(eMIUser);
//        }
//        [HttpPost]
//        public IActionResult CalculateEMI(EMIUser eMIUser)
//        {
//            double emiAmount = (double)(eMIUser.LoanAmount + (eMIUser.LoanAmount * (eMIUser.ROI / 100))) / eMIUser.Tenure;
//            var emiSchedule = GenerateEMISchedule(eMIUser);
//            ViewData["EMIAmount"] = emiAmount;
//            ViewData["EMISchedule"]=emiSchedule;
//            return View("Index", eMIUser);
//        }
//        private EMI[] GenerateEMISchedule(EMIUser eMIUser)
//        {
//            var emiSchedule =new EMI[eMIUser.Tenure];
//            DateTime dueDate = eMIUser.LoanDate.AddMonths(1);
//            double emiAmount = (double)(eMIUser.LoanAmount + (eMIUser.LoanAmount * (eMIUser.ROI / 100))) / eMIUser.Tenure;
//            for(int i =0; i< eMIUser.Tenure; i++)
//            {
//                emiSchedule[i] = new EMI
//                {
//                    EmiNo = i + 1,
//                    DueDate = dueDate.AddMonths(i),
//                    EmiAmount = emiAmount
//                };
//            }
//            return emiSchedule;
//        }



//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(Users users)
//        {
//            if (ModelState.IsValid)
//            {
//                _userDl.InsertUserPlan(users);
//                return RedirectToAction("Index");
//            }
//            return View(users);
//        }
//    }
//}
using GtechTask.DAL;
using GtechTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GtechTask.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly User_DL _userDl;

        public ApplicationController(IConfiguration configuration)
        {
            _userDl = new User_DL(configuration);
        }

        //public IActionResult Index()
        //{
        //    var plans = _userDl.GetAllPlans();
        //    ViewBag.Plans = plans;
        //    var eMIUser = new EMIUser();
        //    return View(eMIUser);
        //}
        public IActionResult Index()
        {
            var plans = _userDl.GetAllPlans();
            if (plans != null)
            {
                ViewBag.Plans = new SelectList(plans, "PlaneName", "PlaneName");
            }
            else
            {
                ViewBag.Plans = new SelectList(new List<Users>(), "PlaneName", "PlaneName");
            }

            var eMIUser = new EMIUser();
            return View(eMIUser);
        }


        [HttpPost]
        public IActionResult CalculateEMI(EMIUser eMIUser)
        {
            //var emiSchedule = GenerateEMISchedule(eMIUser);
            var emiSchedule = GenerateEMISchedule(eMIUser, false);

            eMIUser.EMISchedule = emiSchedule;
            return View("Index", eMIUser);
        }
        [HttpPost]
        public IActionResult GenerateSchedule(EMIUser eMIUser)
        {
            var emiSchedule = GenerateEMISchedule(eMIUser, true);
            eMIUser.EMISchedule = emiSchedule;
            eMIUser.ScheduleGenerated = true; 
            return View("Index", eMIUser);
        }

        [HttpGet]
        public IActionResult GetPlanDetails(string planeName)
        {
            var plan = _userDl.GetPlanDetails(planeName);
            if (plan != null)
            {
                return Json(new { tenureMonths = plan.TenureMonths, roi = plan.ROI });
            }
            return Json(null);
        }

        //private List<EMI> GenerateEMISchedule(EMIUser eMIUser)
        //{
        //    var emiSchedule = new List<EMI>();
        //    double monthlyInterestRate = (double)eMIUser.ROI / 12 / 100;
        //    double emiAmount = eMIUser.LoanAmount * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, eMIUser.Tenure) / (Math.Pow(1 + monthlyInterestRate, eMIUser.Tenure) - 1);

        //    for (int i = 0; i < eMIUser.Tenure; i++)
        //    {
        //        DateTime dueDate = eMIUser.LoanDate.AddMonths(i + 1);
        //        emiSchedule.Add(new EMI
        //        {
        //            EmiNo = i + 1,
        //            DueDate = dueDate,
        //            EmiAmount = emiAmount
        //        });
        //    }

        //    return emiSchedule;
        //}

        private List<EMI> GenerateEMISchedule(EMIUser eMIUser, bool generateFullSchedule)
        {
            var emiSchedule = new List<EMI>();
            double monthlyInterestRate = (double)eMIUser.ROI / 12 / 100;
            double emiAmount = eMIUser.LoanAmount * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, eMIUser.Tenure) / (Math.Pow(1 + monthlyInterestRate, eMIUser.Tenure) - 1);

            if (generateFullSchedule)
            {
                for (int i = 0; i < eMIUser.Tenure; i++)
                {
                    DateTime dueDate = eMIUser.LoanDate.AddMonths(i + 1);
                    emiSchedule.Add(new EMI
                    {
                        EmiNo = i + 1,
                        DueDate = dueDate,
                        EmiAmount = emiAmount
                    });
                }
            }
            else
            {
                
                emiSchedule.Add(new EMI
                {
                    EmiNo = 1,
                    DueDate = eMIUser.LoanDate.AddMonths(1),
                    EmiAmount = emiAmount
                });
            }

            return emiSchedule;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Users users)
        {
            if (ModelState.IsValid)
            {
                _userDl.InsertUserPlan(users);
                return RedirectToAction("Index");
            }
            return View(users);
        }
    }
}

