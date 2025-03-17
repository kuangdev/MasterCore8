using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterCore8.Models;

namespace MasterCore8.Data
{
    public class ApplicationDbContext : DbContext
    {

        public string databaseName { 
            get{
                try
                {                
                    return this.Database.GetDbConnection().Database.ToString();
                }
                catch (System.Exception)
                {
                    return "-";
                }
            } 
        }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<tbbasic_crud> tbbasic_crud { get; set; }
        public DbSet<tblookup> tblookup { get; set; }        
        public DbSet<tbstudent> tbstudent { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //  ============== Relation ================    
            builder.Entity<tbbasic_crud>().HasOne(b=>b.create).WithMany().HasForeignKey(b=>b.create_by);
            builder.Entity<tbbasic_crud>().HasOne(b=>b.update).WithMany().HasForeignKey(b=>b.update_by);
            builder.Entity<tbbasic_crud>().HasOne(b=>b.lookup_category).WithMany().HasForeignKey(b=>b.category).HasPrincipalKey(b=>b.lookup_code);
            
            builder.Entity<tbstudent>().HasOne(b=>b.create).WithMany().HasForeignKey(b=>b.create_by);
            builder.Entity<tbstudent>().HasOne(b=>b.update).WithMany().HasForeignKey(b=>b.update_by);

                  
            // ทำการ Setting Value ตามประเภท ของ lookup_type จาก Class ที่เราสร้างสืบทอด tblookup ไว้ที่ Models\tblookup.cs
            builder.Entity<tblookup>().HasDiscriminator<string>("lookup_type")
            .HasValue<LookupBasicCRUDCategory>(LookupTypeList.BasicCRUDCategory)
            .HasValue<LookupStudentCategory>(LookupTypeList.StudentCategory);
            

            
            //  ============== Seed Data ================            

            var createDate = DateTime.Now;
            var createBy = "016623";

            builder.Entity<AppUser>().HasData(new AppUser {
                id = "016623", empno = "016623", password  = "016623", name  = "Preedee P.", email  = "preedee@mail.connon",
                ext  = "1", dept  = "ICD", div  = "CPD", position  = "Sr.Programmer", status  = "active", roles  = "Admin",
                create_date  =createDate, create_by  = createBy, update_date  =createDate, update_by  = createBy
            });

            builder.Entity<tblookup>().HasData(
                new tblookup {  id=1,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Action", lookup_name="หนังบู๊", lookup_text="หนังบู๊ หรือ แอคชั่น จะมีเรื่องราวการดำเนินการคล้ายกับการผจญภัย ตัวเอกมักจะมีความเสี่ยงซึ่งจะนำไปสู่สถานการณ์ที่สุดอันตราย (รวมถึงการระเบิดฉากต่อสู้ ฉากการหลบหนี การแสดงความกล้าหาญ ฯลฯ ) บ่อยครั้งที่หนังมีสององค์ประกอบรวมอยู่ด้วยคือ “แอคชั่น” กับ “ผจญภัย” (บางครั้งก็เป็นเราก็เรียกมันว่า “แอ็คชั่นผจญภัย”) เพราะพวกเขามีโครงสร้างที่เหมือนกันมาก เรื่องราวต่างๆ ยกตัวอย่างเช่น เจมส์บอนด์", sortorder=1, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=2,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Adventure", lookup_name="หนังผจญภัย", lookup_text="เป็นเรื่องราวการผจญภัยที่เกี่ยวกับตัวเอกที่เดินทางไปยังสถานที่ที่ยิ่งใหญ่ หรือห่างไกลเพื่อทำบางสิ่งให้สำเร็จ อาจสามารถมีองค์ประกอบประเภทอื่นๆ อีกมากมายรวมอยู่ในนั้น เพราะมันเป็นประเภทที่เปิดกว้างมาก ตัวเอกมีภารกิจที่ต้องเผชิญหน้ากับอุปสรรคเพื่อไปให้ถึงจุดหมายปลายทางของพวกเขา นอกจากนี้เรื่องราวการผจญภัยมักจะมีเรื่องราวที่ลึกลับ ตัวละครก็จะมีพลังซ่อนอยู่", sortorder=2, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=3,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Comedy", lookup_name="หนังตลก", lookup_text="Comedy เป็นเรื่องราวที่เล่าถึงเหตุการณ์ตลกๆ หรือตลกที่ตั้งใจจะทำให้ผู้ชมหัวเราะ มันเป็นประเภทที่เปิดกว้างมาก และไม่ค่อยจะเหมือนกับกับประเภทอื่นๆ เช่น หนังตลกซอมบี้ หนังล้อเลียน หนังตลกโรแมนติก ที่ตัวเอกมักจะทำอะไรตลกๆ เพื่อสร้างเสียงหัวเราะให้กับผู้ชมตลอดทั้งเรื่อง", sortorder=3, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=4,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Crime", lookup_name="หนังอาชญากรรม", lookup_text="เป็นเรื่องราวอาชญากรรม ที่เกี่ยวข้องกับการกระทำที่ผิดกฎหมาย ตั้งแต่การปล้นธนาคาร ไปจนถึงการก่อการร้าย มันมักจะตกอยู่ในแนวแอ็คชั่นหรือแนวผจญภัย เช่น เรื่องราวของนักสืบ แก๊งมาเฟีย จอมโจร คดีฆาตกรรม ทั้งหมดนี้จัดอยู่ในภาพยนต์ประเภทอาชญากรรมทั้งสิ้น", sortorder=4, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=5,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Drama", lookup_name="หนังดราม่า", lookup_text="ละครเป็นประเภทของนิยายเล่าเรื่อง ที่ใช้ในภาพยนตร์ ละครโทรทัศน์ และวิทยุ (หรือกึ่งนิยาย) ที่เน้นนำเสนอเนื้อเรื่องที่สมจริงของตัวละคร ละครดราม่ามักจะถูกพิจารณาว่าเป็นสิ่งที่ตรงกันข้ามกับหนังตลก แต่อาจพิจารณาแยกจากงานอื่นๆ", sortorder=5, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=6,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Fantasy", lookup_name="หนังแฟนตาซี", lookup_text="เรื่องราวแฟนตาซีเป็นเรื่องเกี่ยวกับเวทมนตร์ หรือกองกำลังเหนือธรรมชาติมากกว่าเทคโนโลยี ถ้ามันเกิดขึ้นในยุคสมัยใหม่หรืออนาคต ขึ้นอยู่กับขอบเขตขององค์ประกอบอื่นๆ ยกตัวอย่าง Harry Potter จะมีข้อกำหนดของว่าเป็นหนังที่เกี่ยวข้องกับพ่อมด แต่มันถูกเรียกว่าเป็นซีรีย์แฟนตาซี", sortorder=6, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
                ,new tblookup {  id=7,lookup_type=LookupTypeList.BasicCRUDCategory, lookup_code="Horror", lookup_name="หนังผี", lookup_text="เป็นการบอกเล่าเรื่องราวสยองขวัญโดยเจตนาทำให้ตกใจ หรือทำให้ผู้ชมหวาดกลัวโดยผ่านความระแวงอย่างรุนแรงหรือตกใจ ยกตัวอย่างเช่นผลงานของ H. P. Lovecraft หรือ หนังผีบ้านเราที่หลายคนอาจจะคุ้นเคยมากที่สุดอย่างแน่นอน", sortorder=7, isdeleted=false, create_date=createDate, create_by=createBy, update_date=createDate, update_by=createBy }
            );

            
            
        }
            
    }
}