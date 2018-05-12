using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pjs1.Common.Migrations.MsSql1Db
{
    public partial class CreateMigrationDatabaseMsSql1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interlocutor",
                columns: table => new
                {
                    InterlocutorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InterlocutorType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProfileImageUrl = table.Column<string>(nullable: true),
                    StatusUnderName = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interlocutor", x => x.InterlocutorId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentData = table.Column<string>(nullable: true),
                    LogDatetime = table.Column<DateTimeOffset>(nullable: false),
                    PkId = table.Column<int>(nullable: false),
                    PreviousData = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(maxLength: 200, nullable: false),
                    LastName = table.Column<string>(maxLength: 200, nullable: false),
                    UserName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionTime = table.Column<DateTimeOffset>(nullable: false),
                    ContactReceiverId = table.Column<int>(nullable: false),
                    ContactSenderId = table.Column<int>(nullable: false),
                    ReceiverStatus = table.Column<int>(nullable: false),
                    SenderStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_Interlocutor_ContactReceiverId",
                        column: x => x.ContactReceiverId,
                        principalTable: "Interlocutor",
                        principalColumn: "InterlocutorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_Interlocutor_ContactSenderId",
                        column: x => x.ContactSenderId,
                        principalTable: "Interlocutor",
                        principalColumn: "InterlocutorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    ConversationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationReceiverId = table.Column<int>(nullable: false),
                    ConversationSenderId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    MessageDataType = table.Column<int>(nullable: false),
                    SendTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_Conversation_Interlocutor_ConversationReceiverId",
                        column: x => x.ConversationReceiverId,
                        principalTable: "Interlocutor",
                        principalColumn: "InterlocutorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_Interlocutor_ConversationSenderId",
                        column: x => x.ConversationSenderId,
                        principalTable: "Interlocutor",
                        principalColumn: "InterlocutorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_ContactReceiverId",
                table: "Contact",
                column: "ContactReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_ContactSenderId",
                table: "Contact",
                column: "ContactSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_ConversationReceiverId",
                table: "Conversation",
                column: "ConversationReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_ConversationSenderId",
                table: "Conversation",
                column: "ConversationSenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Interlocutor");
        }
    }
}
