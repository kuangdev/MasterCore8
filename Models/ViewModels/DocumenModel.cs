using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCore8.Models.ViewModels
{
    public class DocumentModel
    {
        [Required]
        [MaxLength(20)]
        public string TestString { get; set; }

        [Required]
        [MaxLength(20)]
        public string TestString2 { get; set; }

        [Required]
        [MaxLength(5)]
        public int TestNumber { get; set; }


        public DateTime TestDateTime { get; set; }

        public string Ttext { get; set; }
        public DateTime Tdate { get ; set; } = DateTime.Now;
        public DateTime Tdatetime { get ; set; } = DateTime.Now;
        public DateTime Tmonth { get ; set; } = DateTime.Now;
        public DateTime Ttime { get ; set; } = DateTime.Now;
        public DateTime Tdaterange { get ; set; } = DateTime.Now;
        public string Tfile { get; set; }
        public string Ttextarea { get; set; }
        public string Teditor { get; set; }
        public string Tcheckbox { get; set; }
        public string Tradio { get; set; }
        public string Tselect { get; set; }
        public string Tsignature { get; set; }

        public List<string> TestList {
            get {
                return new List<string>() {
                    "Test01",
                    "Test02",
                    "Test03",
                    "Test04",
                    "Test05",
                    "Test06",
                };
            }
        }
    }
}