using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class FixedNamingEverywhere_6try_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_Users_ID",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OrganizerID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Tools_Tool_ID",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_UserID",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_LendingPartners_Users_PartnerID",
                table: "LendingPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_LendingPartners_Users_UserID",
                table: "LendingPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrows_Orders_ID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderID",
                table: "ProductBorrows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Tools_ToolID",
                table: "ProductBorrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Borrows_Borrows_ID",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tools_ToolID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Tools_Tools_ID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityDeposits_Tools_Tools_ID",
                table: "SecurityDeposits");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityDeposits_Users_UserID",
                table: "SecurityDeposits");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolAccessories_Tools_Tool_ID",
                table: "ToolAccessories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Users_Users_ID",
                table: "Tools");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tools",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Users_ID",
                table: "Tools",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Tools_Users_ID",
                table: "Tools",
                newName: "IX_Tools_UsersId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ToolAccessories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Tool_ID",
                table: "ToolAccessories",
                newName: "ToolId");

            migrationBuilder.RenameColumn(
                name: "Quantity_Available",
                table: "ToolAccessories",
                newName: "QuantityAvailable");

            migrationBuilder.RenameIndex(
                name: "IX_ToolAccessories_Tool_ID",
                table: "ToolAccessories",
                newName: "IX_ToolAccessories_ToolId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "SecurityDeposits",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SecurityDeposits",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Users_ID",
                table: "SecurityDeposits",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "Tools_ID",
                table: "SecurityDeposits",
                newName: "ToolsId");

            migrationBuilder.RenameColumn(
                name: "Refund_Date",
                table: "SecurityDeposits",
                newName: "RefundDate");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityDeposits_UserID",
                table: "SecurityDeposits",
                newName: "IX_SecurityDeposits_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityDeposits_Tools_ID",
                table: "SecurityDeposits",
                newName: "IX_SecurityDeposits_ToolsId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Schedules",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Tools_ID",
                table: "Schedules",
                newName: "ToolsId");

            migrationBuilder.RenameColumn(
                name: "Available_to",
                table: "Schedules",
                newName: "AvailableTo");

            migrationBuilder.RenameColumn(
                name: "Available_from",
                table: "Schedules",
                newName: "AvailableFrom");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_Tools_ID",
                table: "Schedules",
                newName: "IX_Schedules_ToolsId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ToolID",
                table: "Reviews",
                newName: "ToolId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ToolID",
                table: "Reviews",
                newName: "IX_Reviews_ToolId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Returns",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Borrows_ID",
                table: "Returns",
                newName: "BorrowsId");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_Borrows_ID",
                table: "Returns",
                newName: "IX_Returns_BorrowsId");

            migrationBuilder.RenameColumn(
                name: "ToolID",
                table: "ProductBorrows",
                newName: "ToolId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "ProductBorrows",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ProductBorrows",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Tools_ID",
                table: "ProductBorrows",
                newName: "ToolsId");

            migrationBuilder.RenameColumn(
                name: "Orders_ID",
                table: "ProductBorrows",
                newName: "OrdersId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBorrows_ToolID",
                table: "ProductBorrows",
                newName: "IX_ProductBorrows_ToolId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBorrows_OrderID",
                table: "ProductBorrows",
                newName: "IX_ProductBorrows_OrderId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Orders_ID",
                table: "Payments",
                newName: "OrdersId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_Orders_ID",
                table: "Payments",
                newName: "IX_Payments_OrdersId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Notifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Users_ID",
                table: "Notifications",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "Sent_At",
                table: "Notifications",
                newName: "SentAt");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "LendingPartners",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PartnerID",
                table: "LendingPartners",
                newName: "PartnerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "LendingPartners",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Users_Id",
                table: "LendingPartners",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "Start_Date",
                table: "LendingPartners",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Partnership_Type",
                table: "LendingPartners",
                newName: "PartnershipType");

            migrationBuilder.RenameColumn(
                name: "Partners_Id",
                table: "LendingPartners",
                newName: "PartnersId");

            migrationBuilder.RenameColumn(
                name: "Partner_Contract",
                table: "LendingPartners",
                newName: "PartnerContract");

            migrationBuilder.RenameIndex(
                name: "IX_LendingPartners_UserID",
                table: "LendingPartners",
                newName: "IX_LendingPartners_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LendingPartners_PartnerID",
                table: "LendingPartners",
                newName: "IX_LendingPartners_PartnerId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Histories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Histories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Tool_ID",
                table: "Histories",
                newName: "ToolId");

            migrationBuilder.RenameColumn(
                name: "Added_at",
                table: "Histories",
                newName: "AddedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_UserID",
                table: "Histories",
                newName: "IX_Histories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_Tool_ID",
                table: "Histories",
                newName: "IX_Histories_ToolId");

            migrationBuilder.RenameColumn(
                name: "OrganizerID",
                table: "Events",
                newName: "OrganizerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Events",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Organizers_ID",
                table: "Events",
                newName: "OrganizersId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerID",
                table: "Events",
                newName: "IX_Events_OrganizerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Borrows",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Users_ID",
                table: "Borrows",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_Users_ID",
                table: "Borrows",
                newName: "IX_Borrows_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Tools_ToolId",
                table: "Histories",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LendingPartners_Users_PartnerId",
                table: "LendingPartners",
                column: "PartnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LendingPartners_Users_UserId",
                table: "LendingPartners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrows_OrdersId",
                table: "Payments",
                column: "OrdersId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderId",
                table: "ProductBorrows",
                column: "OrderId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Tools_ToolId",
                table: "ProductBorrows",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Borrows_BorrowsId",
                table: "Returns",
                column: "BorrowsId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tools_ToolId",
                table: "Reviews",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Tools_ToolsId",
                table: "Schedules",
                column: "ToolsId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityDeposits_Tools_ToolsId",
                table: "SecurityDeposits",
                column: "ToolsId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityDeposits_Users_UserId",
                table: "SecurityDeposits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolAccessories_Tools_ToolId",
                table: "ToolAccessories",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Users_UsersId",
                table: "Tools",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Tools_ToolId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_LendingPartners_Users_PartnerId",
                table: "LendingPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_LendingPartners_Users_UserId",
                table: "LendingPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrows_OrdersId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderId",
                table: "ProductBorrows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Tools_ToolId",
                table: "ProductBorrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Borrows_BorrowsId",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tools_ToolId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Tools_ToolsId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityDeposits_Tools_ToolsId",
                table: "SecurityDeposits");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityDeposits_Users_UserId",
                table: "SecurityDeposits");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolAccessories_Tools_ToolId",
                table: "ToolAccessories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Users_UsersId",
                table: "Tools");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tools",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Tools",
                newName: "Users_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Tools_UsersId",
                table: "Tools",
                newName: "IX_Tools_Users_ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ToolAccessories",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ToolId",
                table: "ToolAccessories",
                newName: "Tool_ID");

            migrationBuilder.RenameColumn(
                name: "QuantityAvailable",
                table: "ToolAccessories",
                newName: "Quantity_Available");

            migrationBuilder.RenameIndex(
                name: "IX_ToolAccessories_ToolId",
                table: "ToolAccessories",
                newName: "IX_ToolAccessories_Tool_ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SecurityDeposits",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SecurityDeposits",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "SecurityDeposits",
                newName: "Users_ID");

            migrationBuilder.RenameColumn(
                name: "ToolsId",
                table: "SecurityDeposits",
                newName: "Tools_ID");

            migrationBuilder.RenameColumn(
                name: "RefundDate",
                table: "SecurityDeposits",
                newName: "Refund_Date");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityDeposits_UserId",
                table: "SecurityDeposits",
                newName: "IX_SecurityDeposits_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityDeposits_ToolsId",
                table: "SecurityDeposits",
                newName: "IX_SecurityDeposits_Tools_ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Schedules",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ToolsId",
                table: "Schedules",
                newName: "Tools_ID");

            migrationBuilder.RenameColumn(
                name: "AvailableTo",
                table: "Schedules",
                newName: "Available_to");

            migrationBuilder.RenameColumn(
                name: "AvailableFrom",
                table: "Schedules",
                newName: "Available_from");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ToolsId",
                table: "Schedules",
                newName: "IX_Schedules_Tools_ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ToolId",
                table: "Reviews",
                newName: "ToolID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ToolId",
                table: "Reviews",
                newName: "IX_Reviews_ToolID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Returns",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "BorrowsId",
                table: "Returns",
                newName: "Borrows_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_BorrowsId",
                table: "Returns",
                newName: "IX_Returns_Borrows_ID");

            migrationBuilder.RenameColumn(
                name: "ToolId",
                table: "ProductBorrows",
                newName: "ToolID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ProductBorrows",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductBorrows",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ToolsId",
                table: "ProductBorrows",
                newName: "Tools_ID");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "ProductBorrows",
                newName: "Orders_ID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBorrows_ToolId",
                table: "ProductBorrows",
                newName: "IX_ProductBorrows_ToolID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBorrows_OrderId",
                table: "ProductBorrows",
                newName: "IX_ProductBorrows_OrderID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "Payments",
                newName: "Orders_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_OrdersId",
                table: "Payments",
                newName: "IX_Payments_Orders_ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notifications",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Notifications",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Notifications",
                newName: "Users_ID");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "Notifications",
                newName: "Sent_At");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LendingPartners",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "PartnerId",
                table: "LendingPartners",
                newName: "PartnerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LendingPartners",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "LendingPartners",
                newName: "Users_Id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "LendingPartners",
                newName: "Start_Date");

            migrationBuilder.RenameColumn(
                name: "PartnershipType",
                table: "LendingPartners",
                newName: "Partnership_Type");

            migrationBuilder.RenameColumn(
                name: "PartnersId",
                table: "LendingPartners",
                newName: "Partners_Id");

            migrationBuilder.RenameColumn(
                name: "PartnerContract",
                table: "LendingPartners",
                newName: "Partner_Contract");

            migrationBuilder.RenameIndex(
                name: "IX_LendingPartners_UserId",
                table: "LendingPartners",
                newName: "IX_LendingPartners_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_LendingPartners_PartnerId",
                table: "LendingPartners",
                newName: "IX_LendingPartners_PartnerID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Histories",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Histories",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ToolId",
                table: "Histories",
                newName: "Tool_ID");

            migrationBuilder.RenameColumn(
                name: "AddedAt",
                table: "Histories",
                newName: "Added_at");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_UserId",
                table: "Histories",
                newName: "IX_Histories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_ToolId",
                table: "Histories",
                newName: "IX_Histories_Tool_ID");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Events",
                newName: "OrganizerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Events",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "OrganizersId",
                table: "Events",
                newName: "Organizers_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                newName: "IX_Events_OrganizerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Borrows",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Borrows",
                newName: "Users_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows",
                newName: "IX_Borrows_Users_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_Users_ID",
                table: "Borrows",
                column: "Users_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Tools_Tool_ID",
                table: "Histories",
                column: "Tool_ID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_UserID",
                table: "Histories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LendingPartners_Users_PartnerID",
                table: "LendingPartners",
                column: "PartnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LendingPartners_Users_UserID",
                table: "LendingPartners",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrows_Orders_ID",
                table: "Payments",
                column: "Orders_ID",
                principalTable: "Borrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderID",
                table: "ProductBorrows",
                column: "OrderID",
                principalTable: "Borrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Tools_ToolID",
                table: "ProductBorrows",
                column: "ToolID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Borrows_Borrows_ID",
                table: "Returns",
                column: "Borrows_ID",
                principalTable: "Borrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tools_ToolID",
                table: "Reviews",
                column: "ToolID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Tools_Tools_ID",
                table: "Schedules",
                column: "Tools_ID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityDeposits_Tools_Tools_ID",
                table: "SecurityDeposits",
                column: "Tools_ID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityDeposits_Users_UserID",
                table: "SecurityDeposits",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolAccessories_Tools_Tool_ID",
                table: "ToolAccessories",
                column: "Tool_ID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Users_Users_ID",
                table: "Tools",
                column: "Users_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
