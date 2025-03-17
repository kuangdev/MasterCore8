using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterCore8.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    empno = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    email = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    ext = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    dept = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    dept_full = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    div = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    position = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    roles = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    create_by = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    delete_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    delete_by = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblookup",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lookup_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    lookup_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    lookup_name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    lookup_name2 = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    lookup_name3 = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    lookup_text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    sortorder = table.Column<int>(type: "INTEGER", nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    create_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblookup", x => x.id);
                    table.UniqueConstraint("AK_tblookup_lookup_code", x => x.lookup_code);
                });

            migrationBuilder.CreateTable(
                name: "tbstudent",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    gender = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    class_room = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    img = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    active_date = table.Column<DateTime>(type: "date", nullable: true),
                    portfolio = table.Column<string>(type: "text", nullable: true),
                    signature = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    create_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbstudent", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbstudent_AppUser_create_by",
                        column: x => x.create_by,
                        principalTable: "AppUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbstudent_AppUser_update_by",
                        column: x => x.update_by,
                        principalTable: "AppUser",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbbasic_crud",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    run_no = table.Column<int>(type: "INTEGER", nullable: false),
                    movie_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    detail = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    img = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    publish_start_date = table.Column<DateTime>(type: "date", nullable: true),
                    publish_end_date = table.Column<DateTime>(type: "date", nullable: true),
                    create_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    update_by = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbasic_crud", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbbasic_crud_AppUser_create_by",
                        column: x => x.create_by,
                        principalTable: "AppUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbbasic_crud_AppUser_update_by",
                        column: x => x.update_by,
                        principalTable: "AppUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbbasic_crud_tblookup_category",
                        column: x => x.category,
                        principalTable: "tblookup",
                        principalColumn: "lookup_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "id", "create_by", "create_date", "delete_by", "delete_date", "dept", "dept_full", "div", "email", "empno", "ext", "name", "password", "position", "roles", "status", "update_by", "update_date" },
                values: new object[] { "016623", "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), null, null, "ICD", null, "CPD", "preedee@mail.connon", "016623", "1", "Preedee P.", "016623", "Sr.Programmer", "Admin", "active", "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) });

            migrationBuilder.InsertData(
                table: "tblookup",
                columns: new[] { "id", "create_by", "create_date", "isdeleted", "lookup_code", "lookup_name", "lookup_name2", "lookup_name3", "lookup_text", "lookup_type", "sortorder", "update_by", "update_date" },
                values: new object[,]
                {
                    { 1, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Action", "หนังบู๊", null, null, "หนังบู๊ หรือ แอคชั่น จะมีเรื่องราวการดำเนินการคล้ายกับการผจญภัย ตัวเอกมักจะมีความเสี่ยงซึ่งจะนำไปสู่สถานการณ์ที่สุดอันตราย (รวมถึงการระเบิดฉากต่อสู้ ฉากการหลบหนี การแสดงความกล้าหาญ ฯลฯ ) บ่อยครั้งที่หนังมีสององค์ประกอบรวมอยู่ด้วยคือ “แอคชั่น” กับ “ผจญภัย” (บางครั้งก็เป็นเราก็เรียกมันว่า “แอ็คชั่นผจญภัย”) เพราะพวกเขามีโครงสร้างที่เหมือนกันมาก เรื่องราวต่างๆ ยกตัวอย่างเช่น เจมส์บอนด์", "BasicCRUDCategory", 1, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 2, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Adventure", "หนังผจญภัย", null, null, "เป็นเรื่องราวการผจญภัยที่เกี่ยวกับตัวเอกที่เดินทางไปยังสถานที่ที่ยิ่งใหญ่ หรือห่างไกลเพื่อทำบางสิ่งให้สำเร็จ อาจสามารถมีองค์ประกอบประเภทอื่นๆ อีกมากมายรวมอยู่ในนั้น เพราะมันเป็นประเภทที่เปิดกว้างมาก ตัวเอกมีภารกิจที่ต้องเผชิญหน้ากับอุปสรรคเพื่อไปให้ถึงจุดหมายปลายทางของพวกเขา นอกจากนี้เรื่องราวการผจญภัยมักจะมีเรื่องราวที่ลึกลับ ตัวละครก็จะมีพลังซ่อนอยู่", "BasicCRUDCategory", 2, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 3, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Comedy", "หนังตลก", null, null, "Comedy เป็นเรื่องราวที่เล่าถึงเหตุการณ์ตลกๆ หรือตลกที่ตั้งใจจะทำให้ผู้ชมหัวเราะ มันเป็นประเภทที่เปิดกว้างมาก และไม่ค่อยจะเหมือนกับกับประเภทอื่นๆ เช่น หนังตลกซอมบี้ หนังล้อเลียน หนังตลกโรแมนติก ที่ตัวเอกมักจะทำอะไรตลกๆ เพื่อสร้างเสียงหัวเราะให้กับผู้ชมตลอดทั้งเรื่อง", "BasicCRUDCategory", 3, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 4, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Crime", "หนังอาชญากรรม", null, null, "เป็นเรื่องราวอาชญากรรม ที่เกี่ยวข้องกับการกระทำที่ผิดกฎหมาย ตั้งแต่การปล้นธนาคาร ไปจนถึงการก่อการร้าย มันมักจะตกอยู่ในแนวแอ็คชั่นหรือแนวผจญภัย เช่น เรื่องราวของนักสืบ แก๊งมาเฟีย จอมโจร คดีฆาตกรรม ทั้งหมดนี้จัดอยู่ในภาพยนต์ประเภทอาชญากรรมทั้งสิ้น", "BasicCRUDCategory", 4, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 5, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Drama", "หนังดราม่า", null, null, "ละครเป็นประเภทของนิยายเล่าเรื่อง ที่ใช้ในภาพยนตร์ ละครโทรทัศน์ และวิทยุ (หรือกึ่งนิยาย) ที่เน้นนำเสนอเนื้อเรื่องที่สมจริงของตัวละคร ละครดราม่ามักจะถูกพิจารณาว่าเป็นสิ่งที่ตรงกันข้ามกับหนังตลก แต่อาจพิจารณาแยกจากงานอื่นๆ", "BasicCRUDCategory", 5, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 6, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Fantasy", "หนังแฟนตาซี", null, null, "เรื่องราวแฟนตาซีเป็นเรื่องเกี่ยวกับเวทมนตร์ หรือกองกำลังเหนือธรรมชาติมากกว่าเทคโนโลยี ถ้ามันเกิดขึ้นในยุคสมัยใหม่หรืออนาคต ขึ้นอยู่กับขอบเขตขององค์ประกอบอื่นๆ ยกตัวอย่าง Harry Potter จะมีข้อกำหนดของว่าเป็นหนังที่เกี่ยวข้องกับพ่อมด แต่มันถูกเรียกว่าเป็นซีรีย์แฟนตาซี", "BasicCRUDCategory", 6, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) },
                    { 7, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016), false, "Horror", "หนังผี", null, null, "เป็นการบอกเล่าเรื่องราวสยองขวัญโดยเจตนาทำให้ตกใจ หรือทำให้ผู้ชมหวาดกลัวโดยผ่านความระแวงอย่างรุนแรงหรือตกใจ ยกตัวอย่างเช่นผลงานของ H. P. Lovecraft หรือ หนังผีบ้านเราที่หลายคนอาจจะคุ้นเคยมากที่สุดอย่างแน่นอน", "BasicCRUDCategory", 7, "016623", new DateTime(2025, 3, 17, 18, 42, 5, 204, DateTimeKind.Local).AddTicks(1016) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbbasic_crud_category",
                table: "tbbasic_crud",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_tbbasic_crud_create_by",
                table: "tbbasic_crud",
                column: "create_by");

            migrationBuilder.CreateIndex(
                name: "IX_tbbasic_crud_update_by",
                table: "tbbasic_crud",
                column: "update_by");

            migrationBuilder.CreateIndex(
                name: "IX_tbstudent_create_by",
                table: "tbstudent",
                column: "create_by");

            migrationBuilder.CreateIndex(
                name: "IX_tbstudent_update_by",
                table: "tbstudent",
                column: "update_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbbasic_crud");

            migrationBuilder.DropTable(
                name: "tbstudent");

            migrationBuilder.DropTable(
                name: "tblookup");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
