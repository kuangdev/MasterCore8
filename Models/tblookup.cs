using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Data;

namespace MasterCore8.Models
{
    [Table("tblookup")]
    public class tblookup
    {
        [Key]
        public int id { get; set; }
        [MaxLength(50)]
        public string lookup_type { get; set; }
        [MaxLength(50)]
        public string lookup_code { get; set; }
        [MaxLength(200)]
        public string lookup_name { get; set; }
        [MaxLength(200)]
        public string lookup_name2 { get; set; }
        [MaxLength(200)]
        public string lookup_name3 { get; set; }
        [MaxLength(500)]
        public string lookup_text { get; set; }
        public int sortorder { get; set; }
        public bool isdeleted { get; set; }
        
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


    }
    
    //=============================== Lookup Concept ========================================

    // Class Const Lookup Type เพื่อระบุ Lookup type ทั้งหมดในระบบ
    public class LookupTypeList{
        public const string BasicCRUDCategory = "BasicCRUDCategory";
        public const string StudentCategory = "StudentCategory";
    }

    // สร้าง Class สืบทอด class จาก tblookup ตาม LookupType ทั้งหมดเพื่อง่ายต่อการ สร้าง Relation ใน ApplicationDBContext.cs
    public class LookupBasicCRUDCategory : tblookup {
    }

    public class LookupStudentCategory : tblookup {
    }
}