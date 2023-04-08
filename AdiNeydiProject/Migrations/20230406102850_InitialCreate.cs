using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdiNeydiProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character(50)", fixedLength: true, maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "character(255)", fixedLength: true, maxLength: 255, nullable: true),
                    Post_ID = table.Column<int>(type: "integer", nullable: true),
                    User_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Audio_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character(255)", fixedLength: true, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Text = table.Column<string>(type: "text", nullable: true),
                    trueComment = table.Column<bool>(type: "boolean", nullable: true),
                    Post_ID = table.Column<int>(type: "integer", nullable: true),
                    User_ID = table.Column<int>(type: "integer", nullable: true),
                    CommentOrder = table.Column<int>(type: "integer", nullable: true),
                    RepliedComment_ID = table.Column<int>(type: "integer", nullable: true),
                    Created_Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Ip_Address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Comment_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Post_ID = table.Column<int>(type: "integer", nullable: true),
                    User_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true),
                    User_ID = table.Column<int>(type: "integer", nullable: true),
                    Post_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Picture_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Title = table.Column<string>(type: "character(255)", fixedLength: true, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type_ID = table.Column<int>(type: "integer", nullable: true),
                    Category_ID = table.Column<int>(type: "integer", nullable: true),
                    User_ID = table.Column<int>(type: "integer", nullable: true),
                    Is_Approved = table.Column<string>(type: "text", nullable: true),
                    Created_Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Post_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character(255)", fixedLength: true, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Type_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    FirstName = table.Column<string>(type: "character(50)", fixedLength: true, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character(50)", fixedLength: true, maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "character(30)", fixedLength: true, maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character(80)", fixedLength: true, maxLength: 80, nullable: true),
                    Password = table.Column<string>(type: "character(80)", fixedLength: true, maxLength: 80, nullable: true),
                    Is_Active = table.Column<bool>(type: "boolean", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserType_ID = table.Column<int>(type: "integer", nullable: true),
                    Is_Phone_Verificated = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    Created_Time = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserType_pkey", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audio");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserType");
        }
    }
}
