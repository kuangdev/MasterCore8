using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterCore8.Models
{
    public class AppUser
    {
        [Key]
        [MaxLength(50)]
        public string id { get; set; }
        [Required]
        [MaxLength(10)]
        public string empno { get; set; }
        [MaxLength(50)]
        [MinLength(4)]
        public string password { get; set; }        
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)]
        public string email { get; set; }
        [MaxLength(50)]
        public string ext { get; set; }
        [MaxLength(50)]
        public string dept { get; set; }
        [MaxLength(50)]
        public string dept_full { get; set; }
        [MaxLength(50)]
        public string div { get; set; }
        [MaxLength(50)]
        public string position { get; set; }
        [MaxLength(50)]
        public string status { get; set; } //[active, deleted]
        [Required]
        [MaxLength(20)]
        public string roles { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? create_date { get; set; }
        [MaxLength(100)]
        public string create_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? update_date { get; set; }
        [MaxLength(100)]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? delete_date { get; set; }
        [MaxLength(100)]
        public string delete_by { get; set; }



        // public virtual string password_confirm { get; set; }

        public virtual ICollection<string> rolelist { 
            get {
                return roles?.Split(",").ToList() ?? new List<string>();
            } 
        }


        public virtual Dictionary<string, string> lookupRoleList {
            get {
                var data = new Dictionary<string, string>();
                data.Add("Admin","Admin");
                data.Add("Approver","Approver");
                data.Add("User","User");

                return data;
            }
        }
    }
}