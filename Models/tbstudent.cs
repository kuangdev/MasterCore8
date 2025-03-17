using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MasterCore8.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MasterCore8.Models
{
    [Table("tbstudent")]
    public class tbstudent
    {
        [Key]
        [MaxLength(36)]
        public string id { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(50)]
        public string name { get; set; }
        [MaxLength(2000)]
        public string address { get; set; }
        [MaxLength(50)]
        public string gender { get; set; }
        [MaxLength(50)]
        public string class_room { get; set; }
        [MaxLength(200)]
        public string img { get; set; }
        [Column(TypeName = "date")]
        public DateTime? active_date { get; set; }
        [Column(TypeName = "text")]
        public string portfolio { get; set; }
        [MaxLength(200)]
        public string signature { get; set; }
        
        // ======= create & update =========
        [MaxLength(50)]
        [Display(Name = "CreateBy")]
        public string create_by { get; set; }
        [Column(TypeName = "datetime")]
        [Display(Name = "CreateDate")]
        public DateTime? create_date { get; set; }
        [MaxLength(50)]
        [Display(Name = "UpdateBy")]
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        [Display(Name = "UpdateDate")]
        public DateTime? update_date { get; set; }

        // relation
        public virtual AppUser create { get; set; }
        public virtual AppUser update { get; set; }

    }
}