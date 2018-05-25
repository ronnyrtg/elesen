using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Users
    {

        public int UsersID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FullName { get; set; }

        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string Username { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string Email { get; set; }

        [StringLength(32)]
        [Column(TypeName = "VARCHAR2")]
        public string Password { get; set; }
        public int? RoleTemplateID { get; set; }
        public int? DepartmentID { get; set; }

        public int Locked { get; set; }
        public bool Active { get; set; }
        public Users()
        {
            Active = true;
        }

        public virtual RoleTemplate RoleTemplate { get; set; }
        public virtual Department Department { get; set; }

    }
}
