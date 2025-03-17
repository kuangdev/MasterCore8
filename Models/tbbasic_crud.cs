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
    [Table("tbbasic_crud")]
    public class tbbasic_crud
    {        

        [Key]
        [MaxLength(36)]
        public string id { get; set; }
        [Required]
        [Display(Name = "RunNo.")]
        public int? run_no { get; set; } = 0;
        [MaxLength(50)]
        public string movie_code { get; set; }
        [Required]
        [MaxLength(200)]
        public string title { get; set; }
        [Required]
        [MaxLength(50)]
        public string category { get; set; }
        [MaxLength(2000)]
        public string detail { get; set; }
        [MaxLength(200)]
        public string img { get; set; }
        [Column(TypeName = "date")]
        public DateTime? publish_start_date { get; set; }
        [Column(TypeName = "date")]
        public DateTime? publish_end_date { get; set; }
        
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

        // relation ไปยัง LookupBasicCRUDCategory
        public virtual LookupBasicCRUDCategory lookup_category { get; set; }

        // construct
        public tbbasic_crud()
        {
            this.id =  !string.IsNullOrEmpty(this.id) ?  this.id : Guid.NewGuid().ToString();
            
        }

        // ======= Handle Method =============

        public static async Task<string> GenDocCode(ApplicationDbContext context, string id, bool updatedata = false){
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = null;
            if(context.Database.CurrentTransaction != null){
                transaction = null;
            }else{
                transaction = context.Database.BeginTransaction();
                
            }
            using var _transaction = transaction;
            try
            {               
                var mydata = await context.tbbasic_crud.FirstOrDefaultAsync(d=>d.id == id);
                
                //string[] statusActive = { "Preparing","Finished","Acknowledge", "Returned"  };
                var max_run_no = await context.tbbasic_crud
                    .Where(d=>d.category == mydata.category
                        && EF.Functions.DateDiffYear(d.create_date, DateTime.Now) == 0)
                    .AsNoTracking()
                    .MaxAsync(d=>d.run_no);
                    
                var category = mydata.category;
                var year = DateTime.Now.Year.ToString();
                var max_run_no_txt = "";
                var movie_code = "";
                int _max_run_no = max_run_no != null ? Convert.ToInt16(max_run_no) + 1 : 1;
                            
                max_run_no_txt = _max_run_no.ToString().PadLeft(5,'0');
                movie_code = $"{category}-{year}-{max_run_no_txt}";
                
                if(updatedata){
                    mydata.movie_code = movie_code;
                    mydata.run_no = _max_run_no;
                    await context.SaveChangesAsync();
                }
                if(_transaction != null){
                    transaction.Commit();
                }
                return movie_code;
            }
            catch (System.Exception)
            {
                if(_transaction == null){
                    throw;
                }
                transaction.Rollback();
                return "";
            }

        }
    }
}