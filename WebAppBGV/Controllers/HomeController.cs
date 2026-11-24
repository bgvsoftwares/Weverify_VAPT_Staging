using BAL.HRRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.HRViewModels;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using BAL;

namespace WebAppBGV.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        ITeamDepartmentRepository repoTeamDept;
        public HomeController(ITeamDepartmentRepository _repoTeamDept)
        {
            repoTeamDept = _repoTeamDept;
        }
        
        // GET: Home
        [SessionExpireFilterAttribute]
        [AuthenticateUser]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult SelectDesignation()
        {
            try
            {
                var list = repoTeamDept.GetTeamDepartmentsWithDesignationsById(Convert.ToInt16(Session["TeamMemberRowID"])).ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].EncTeamDepartmentRowID = clsCommonMethods.PEncrypt(list[i].TeamDepartmentRowID.ToString());
                }
                ViewBag.TeamDepartments = list;
            }
            catch (Exception Ex)
            {
                TempData["ErrorMsg"] = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
           
            return View();
        }

        // GET: Home
        [SessionExpireFilterAttribute]
        [ValidateSessionFilterSpokLoginInternal]
        public ActionResult Index()
        {            
            return View();
        }

        [SessionExpireFilterAttribute]
        public ActionResult RedirectToDashboard(string teamdeptid = "")
        {
            try
            {
                short teamdeptiD = 0;
                if (teamdeptid != "")
                {
                    teamdeptiD = Convert.ToInt16(clsCommonMethods.PDecrypt(teamdeptid));
                }

                var model = repoTeamDept.GetDepartmentWithDesignationById(teamdeptiD);
                if (model != null)
                {
                    Session["DepartmentRowID"] = model.DepartmentRowID;
                    Session["Department"] = model.DepartmentName.Trim();
                    Session["DesignationRowID"] = model.DesignationRowID;
                    Session["Designation"] = model.DesignationName.Trim();

                    if (model.DepartmentName == "Admin" || Session["Department"].ToString() == "HR")
                    {
                        return RedirectToAction("Index");
                    }
                    else if (model.DepartmentName == "Data Entry")
                    {
                        return RedirectToAction("Index", "ProvidedInfo");
                    }
                    else if (model.DesignationName == "Team Member-Edu-Research")
                    {
                        return RedirectToAction("ManageResearchEducationChecks", "ProvidedInfo");
                    }
                    else if (model.DesignationName == "Team Member-Emp-Research")
                    {
                        return RedirectToAction("ManageResearchEmploymentChecks", "ProvidedInfo");
                    }
                    else if (model.DepartmentName == "Address" || model.DepartmentName == "Education" || model.DepartmentName == "Employment" || model.DepartmentName == "Reference" || model.DepartmentName == "Criminal" || model.DepartmentName == "National Identity" || model.DepartmentName == "Special Check")
                    {
                        return RedirectToAction("Index", "Verification");
                    }
                    else if (model.DepartmentName == "Physical Verification")
                    {
                        return RedirectToAction("Index", "Verification");
                    }
                    else if (model.DepartmentName == "Report QC")
                    {
                        return RedirectToAction("Index", "ReportQC");
                    }
                    else if (model.DepartmentName == "Client Servicing")
                    {
                        return RedirectToAction("Index", "ClientServicing");
                    }
                    else if (model.DepartmentName == "Pending Case")
                    {
                        return RedirectToAction("InsuffYTRFundsPending", "ClientServicing");                        
                    }
                    else if (model.DepartmentName == "Annexure and Invoicing")
                    {
                        return RedirectToAction("AnnexureInvoicing", "ClientServicing");
                    }
                    else if (model.DepartmentName == "Sales Department")
                    {
                        return RedirectToAction("Index", "Sales");
                    }

                    else
                    {
                        return RedirectToAction("Index");
                    }
                }                     
               
            }
            catch (Exception Ex)
            {
                TempData["ErrorMsg"] = Ex.Message;
                clsCommonMethods.ErrorLog(Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }
            return RedirectToAction("SelectDesignation");
        }

        
        [HttpGet]
        public ActionResult delete(int personalRowId,int subCheckId,string gtr,string doctype,string fileName, int VerifiedUploadRowID)
        {
            ClientCustomize cc = new ClientCustomize();
            cc.deleteUploadDoc(personalRowId, subCheckId, doctype, fileName, VerifiedUploadRowID);
            return Redirect(gtr);

        }

      

    }
}