using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDepartmentsGonA.D.ClassFolder
{
    class VariableClass
    {
        public static int UserID { get; set; }
        public static int StudentID { get; set; }
        public static int CuratorID { get; set; }


        public static int pioPassoprtID { get; set; }
        public static bool pioPassportNewCreated = false;

        public static int CityID { get; set; }
        public static bool CityNewCreated = false;

        public static int RegionID { get; set; }
        public static bool RegionNewCreated = false;

        public static int StreetID { get; set; }
        public static bool StreetNewCreated = false;


        public static int AdressID { get; set; }
        public static bool AdressNewCreated = false;
        public static bool AddAdressIiUsing = false;

        public static bool EditAdreesIsUsing = false;
        public static bool AdreesWasEdit = false;


        public static int GroupID { get; set; }
        public static bool GroupNewCreated = false;

        public static int SpecialityID { get; set; }
        public static bool SpecialityNewCreated = false;

        public static int StatusID { get; set; }
        public static bool StatusNewCreated = false;

        

    }
}
