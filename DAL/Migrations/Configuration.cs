namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.DataContext context)
        {
            #region Add Master Category

            //MasterCategory objCat1 = new DAL.MasterCategory() { CategoryName = "Permanent", Status = 1 };
            //MasterCategory objCat2 = new DAL.MasterCategory() { CategoryName = "Temporary", Status = 1 };
            //MasterCategory objCat3 = new DAL.MasterCategory() { CategoryName = "Contract Basis", Status = 1 };
            //MasterCategory objCat4 = new DAL.MasterCategory() { CategoryName = "Trainee", Status = 1 };
            //MasterCategory objCat5 = new DAL.MasterCategory() { CategoryName = "Part Time", Status = 1 };
            //MasterCategory objCat6 = new DAL.MasterCategory() { CategoryName = "Full Time", Status = 1 };
            //MasterCategory objCat7 = new DAL.MasterCategory() { CategoryName = "Temporary Period", Status = 1 };
            //MasterCategory objCat8 = new DAL.MasterCategory() { CategoryName = "Management Trainee", Status = 1 };
            //MasterCategory objCat9 = new DAL.MasterCategory() { CategoryName = "N/A", Status = 1 };

            //context.MasterCategories.Add(objCat1);
            //context.MasterCategories.Add(objCat2);
            //context.MasterCategories.Add(objCat3);
            //context.MasterCategories.Add(objCat4);
            //context.MasterCategories.Add(objCat5);
            //context.MasterCategories.Add(objCat6);
            //context.MasterCategories.Add(objCat7);
            //context.MasterCategories.Add(objCat8);
            //context.MasterCategories.Add(objCat9);

            #endregion

            #region Add Master BillingCycle

            //MasterBillingCycle objMBC1 = new DAL.MasterBillingCycle() { BillingCycle = "Anually", Status = 1 };
            //MasterBillingCycle objMBC2 = new DAL.MasterBillingCycle() { BillingCycle = "Half Yearly", Status = 1 };
            //MasterBillingCycle objMBC3 = new DAL.MasterBillingCycle() { BillingCycle = "Quaterly", Status = 1 };
            //MasterBillingCycle objMBC4 = new DAL.MasterBillingCycle() { BillingCycle = "Monthly", Status = 1 };
            //MasterBillingCycle objMBC5 = new DAL.MasterBillingCycle() { BillingCycle = "Weekly", Status = 1 };
            //MasterBillingCycle objMBC6 = new DAL.MasterBillingCycle() { BillingCycle = "Daily", Status = 1 };
            //MasterBillingCycle objMBC7 = new DAL.MasterBillingCycle() { BillingCycle = "Once in two months", Status = 1 };
            //MasterBillingCycle objMBC8 = new DAL.MasterBillingCycle() { BillingCycle = "Five months", Status = 1 };

            //context.MasterBillingCycles.Add(objMBC1);
            //context.MasterBillingCycles.Add(objMBC2);
            //context.MasterBillingCycles.Add(objMBC3);
            //context.MasterBillingCycles.Add(objMBC4);
            //context.MasterBillingCycles.Add(objMBC5);
            //context.MasterBillingCycles.Add(objMBC6);
            //context.MasterBillingCycles.Add(objMBC7);
            //context.MasterBillingCycles.Add(objMBC8);

            #endregion

            #region Add Master Antecedent Type

            //MasterAntecedentType objAT1 = new DAL.MasterAntecedentType() { AntecedentTypeName = "BGV Stated", Status = 1 };
            //MasterAntecedentType objAT2 = new DAL.MasterAntecedentType() { AntecedentTypeName = "V Verified", Status = 1 };
            //MasterAntecedentType objAT3 = new DAL.MasterAntecedentType() { AntecedentTypeName = "BGV Stated & V Verified", Status = 1 };
            //MasterAntecedentType objAT4 = new DAL.MasterAntecedentType() { AntecedentTypeName = "System Generated", Status = 1 };

            //context.MasterAntecedentTypes.Add(objAT1);
            //context.MasterAntecedentTypes.Add(objAT2);
            //context.MasterAntecedentTypes.Add(objAT3);
            //context.MasterAntecedentTypes.Add(objAT4);

            #endregion

            #region Add Severity Grid

            //MasterSeverityGrid SGrid1 = new DAL.MasterSeverityGrid() { SeverityGrid = "Major Discrepant", ColorName = "Red", ColorCode = "#ff0000", Status = 1 };
            //MasterSeverityGrid SGrid2 = new DAL.MasterSeverityGrid() { SeverityGrid = "Minor Discrepant", ColorName = "Yellow", ColorCode = "#ffff00", Status = 1 };
            //MasterSeverityGrid SGrid3 = new DAL.MasterSeverityGrid() { SeverityGrid = "Insufficient Information", ColorName = "Orange", ColorCode = "#ffa500", Status = 1 };
            //MasterSeverityGrid SGrid4 = new DAL.MasterSeverityGrid() { SeverityGrid = "No response received", ColorName = "Amber", ColorCode = "#ff9400", Status = 1 };
            //MasterSeverityGrid SGrid5 = new DAL.MasterSeverityGrid() { SeverityGrid = "Unable to Verify", ColorName = "Magenta", ColorCode = "#ff00ff", Status = 1 };
            //MasterSeverityGrid SGrid6 = new DAL.MasterSeverityGrid() { SeverityGrid = "Verified Clear", ColorName = "Green", ColorCode = "#008000", Status = 1 };
            //MasterSeverityGrid SGrid7 = new DAL.MasterSeverityGrid() { SeverityGrid = "Stop / Reject", ColorName = "Blue", ColorCode = "#0000a0", Status = 1 };

            //context.MasterSeverityGrids.Add(SGrid1);
            //context.MasterSeverityGrids.Add(SGrid2);
            //context.MasterSeverityGrids.Add(SGrid3);
            //context.MasterSeverityGrids.Add(SGrid4);
            //context.MasterSeverityGrids.Add(SGrid5);
            //context.MasterSeverityGrids.Add(SGrid6);
            //context.MasterSeverityGrids.Add(SGrid7);

            #endregion

            #region Add Department Master

            //MasterDepartment objDept1 = new MasterDepartment() { DepartmentName = "Address", Status = 1 };
            //MasterDepartment objDept2 = new MasterDepartment() { DepartmentName = "Admin", Status = 1 };
            //MasterDepartment objDept3 = new MasterDepartment() { DepartmentName = "Audit", Status = 1 };
            //MasterDepartment objDept4 = new MasterDepartment() { DepartmentName = "Billing", Status = 1 };
            //MasterDepartment objDept5 = new MasterDepartment() { DepartmentName = "Client Servicing", Status = 1 };
            //MasterDepartment objDept6 = new MasterDepartment() { DepartmentName = "Collection", Status = 1 };
            //MasterDepartment objDept7 = new MasterDepartment() { DepartmentName = "Criminal", Status = 1 };
            //MasterDepartment objDept8 = new MasterDepartment() { DepartmentName = "Data Entry", Status = 1 };
            //MasterDepartment objDept9 = new MasterDepartment() { DepartmentName = "Education", Status = 1 };
            //MasterDepartment objDept10 = new MasterDepartment() { DepartmentName = "Employment", Status = 1 };
            //MasterDepartment objDept11 = new MasterDepartment() { DepartmentName = "HR", Status = 1 };
            //MasterDepartment objDept12 = new MasterDepartment() { DepartmentName = "IT", Status = 1 };
            //MasterDepartment objDept13 = new MasterDepartment() { DepartmentName = "National Identity", Status = 1 };
            //MasterDepartment objDept14 = new MasterDepartment() { DepartmentName = "Partner", Status = 1 };
            //MasterDepartment objDept15 = new MasterDepartment() { DepartmentName = "Physical Verification", Status = 1 };
            //MasterDepartment objDept16 = new MasterDepartment() { DepartmentName = "Reference", Status = 1 };
            //MasterDepartment objDept17 = new MasterDepartment() { DepartmentName = "Report QC", Status = 1 };
            //MasterDepartment objDept18 = new MasterDepartment() { DepartmentName = "Sales", Status = 1 };

            //context.MasterDepartments.Add(objDept1);
            //context.MasterDepartments.Add(objDept2);
            //context.MasterDepartments.Add(objDept3);
            //context.MasterDepartments.Add(objDept4);
            //context.MasterDepartments.Add(objDept5);
            //context.MasterDepartments.Add(objDept6);
            //context.MasterDepartments.Add(objDept7);
            //context.MasterDepartments.Add(objDept8);
            //context.MasterDepartments.Add(objDept9);
            //context.MasterDepartments.Add(objDept10);
            //context.MasterDepartments.Add(objDept11);
            //context.MasterDepartments.Add(objDept12);
            //context.MasterDepartments.Add(objDept13);
            //context.MasterDepartments.Add(objDept14);
            //context.MasterDepartments.Add(objDept15);
            //context.MasterDepartments.Add(objDept16);
            //context.MasterDepartments.Add(objDept17);
            //context.MasterDepartments.Add(objDept18);

            #endregion
        }
    }
}
