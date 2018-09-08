using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class TutorialModel
    {
        //Normal integer variable
        public int TUTORIAL_ID { get; set; }
        
        //Nullable integer variable
        public int? BT_ID { get; set; }

        //Nullable decimal point variables i.e 0.50
        public float? LUAS { get; set; }

        //Nullable date or time variable, requires System namespace
        public DateTime? TARIKH { get; set; }

        //Text or varchar variable
        [Display(Name = "Some Description")] //Requires System.ComponentModel.DataAnnotations namespace
        [Required(ErrorMessage = "Please enter some description")]
        public string T_DESC { get; set; }

        //Boolean but in Oracle is actually integer of size 1 with values 0 or 1
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }

        //Custom List type variable where the function name is Select2ListItem, requires System.Collections.Generic namespace
        //The function Select2ListItem can be found at TradingLicense.Model.ApplicationModel
        public List<Select2ListItem> exampleList { get; set; }
        }
}
