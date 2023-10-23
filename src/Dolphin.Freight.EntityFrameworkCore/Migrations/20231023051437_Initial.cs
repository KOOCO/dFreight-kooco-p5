using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ImpersonatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImpersonatorUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ImpersonatorTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImpersonatorTenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Exceptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    JobArgs = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextTryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAbandoned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)15),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpClaimTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false),
                    Regex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RegexDescription = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpClaimTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatureValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatureValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLinkUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLinkUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpSecurityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSecurityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAccountGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountGroupName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAccountGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAirOtherCharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    companyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    showOnMAWB = table.Column<bool>(type: "bit", nullable: false),
                    showOnHAWB = table.Column<bool>(type: "bit", nullable: false),
                    chargeItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chargeItemSubitem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chargeRate = table.Column<int>(type: "int", nullable: false),
                    chargeRateUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    minPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirOtherCharges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAirports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AirportIataCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShowName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false),
                    Fid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ftype = table.Column<int>(type: "int", nullable: false),
                    IsMemo = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppContactPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRep = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailNotification = table.Column<bool>(type: "bit", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ContactTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ContactDivision = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ContactCellPhone = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    ContactFax = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    ContactEmailAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactRemark = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ContactGender = table.Column<int>(type: "int", nullable: true),
                    ContactMarriage = table.Column<int>(type: "int", nullable: true),
                    ContactSmokes = table.Column<int>(type: "int", nullable: true),
                    ContactDrink = table.Column<int>(type: "int", nullable: true),
                    ContactAge = table.Column<int>(type: "int", nullable: true),
                    ContactGarment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactHobby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInterest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactConstellation = table.Column<int>(type: "int", nullable: true),
                    ContactMemorialDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactBirthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContactCityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactStateCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContactPersons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppContainerDimensions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContainerLength = table.Column<double>(type: "float", nullable: false),
                    ContainerWidth = table.Column<double>(type: "float", nullable: false),
                    ContainerHeight = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContainerDimensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCreditLimitGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditLimitGroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    CreditTermType = table.Column<int>(type: "int", nullable: false),
                    CreditTermDays = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCreditLimitGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlphabetCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCurrencySetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyDepartment = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CustomerShortCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartingCurrency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    EndCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExChangeRate = table.Column<float>(type: "real", nullable: false),
                    EffectDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCurrencySetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppDisplaySettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompleteDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDisplaySettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGridPreference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Preference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferenceSrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGridPreference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppItNoRanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    EndNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppItNoRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppMemos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    FType = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Highlight = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMemos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppOffices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfficeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOffices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPortsManagements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubDiv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPort = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPortsManagements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppReportLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppReportLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSubstations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbbreviationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubstations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSysCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodeValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShowName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSysCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartnerAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentUploadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttachmentSize = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TPId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartnerAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TPType = table.Column<int>(type: "int", nullable: false),
                    TPCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TPName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: false),
                    TPNamePrint = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: false),
                    TPNameLocal = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    TPLocalAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CityCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    StateCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    TPPrintAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TPAliasName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    Ceo = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    CorpNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IataCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IataPrefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ScacCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    FirmsCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CbsaCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    DutyPaymentType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    OpenHours = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BussinessStatusType = table.Column<int>(type: "int", nullable: false),
                    CsCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesOfficeCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeOe = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeOi = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeAe = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeAi = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeCuOe = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeCuAe = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeCuOi = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SalesCodeCuAi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(72)", maxLength: 72, nullable: true),
                    TrackPayment = table.Column<bool>(type: "bit", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    CreditTermType = table.Column<int>(type: "int", nullable: false),
                    CreditTermDays = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<int>(type: "int", nullable: false),
                    BillToAgentCode = table.Column<bool>(type: "bit", nullable: false),
                    Clm = table.Column<bool>(type: "bit", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccountCurrencyCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankName2 = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    AccountNo2 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccountCurrencyCode2 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankName3 = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    AccountNo3 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccountCurrencyCode3 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsfSubmissionName = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    ImporterCodeType = table.Column<int>(type: "int", nullable: false),
                    ImporterNo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PopUpTips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditLimitGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClientUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ProtocolType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(type: "bit", nullable: false),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false),
                    AllowRememberConsent = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "bit", nullable: false),
                    RequirePkce = table.Column<bool>(type: "bit", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(type: "bit", nullable: false),
                    RequireRequestObject = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "bit", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
                    BackChannelLogoutUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
                    AllowOfflineAccess = table.Column<bool>(type: "bit", nullable: false),
                    IdentityTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccessTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    ConsentLifetime = table.Column<int>(type: "int", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "int", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "bit", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false),
                    AccessTokenType = table.Column<int>(type: "int", nullable: false),
                    EnableLocalLogin = table.Column<bool>(type: "bit", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "bit", nullable: false),
                    ClientClaimsPrefix = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserSsoLifetime = table.Column<int>(type: "int", nullable: true),
                    UserCodeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerDeviceFlowCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerDeviceFlowCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerPersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuditLogActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeType = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EntityTypeFullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => new { x.OrganizationUnitId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnitRoles_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantConnectionStrings",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantConnectionStrings", x => new { x.TenantId, x.Name });
                    table.ForeignKey(
                        name: "FK_AbpTenantConnectionStrings_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(196)", maxLength: 196, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => new { x.OrganizationUnitId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppContactPersonChilds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContactPersonChilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContactPersonChilds_AppContactPersons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "AppContactPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCountries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCountries_AppCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "AppCurrencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MblId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MblType = table.Column<int>(type: "int", nullable: false),
                    ContainerNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ContainerSizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SealNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PackageNum = table.Column<int>(type: "int", nullable: false),
                    PackageWeight = table.Column<double>(type: "float", nullable: false),
                    PackageMeasure = table.Column<double>(type: "float", nullable: false),
                    SealNo2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PicupNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDanger = table.Column<bool>(type: "bit", nullable: false),
                    StorageStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StorageEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureUnit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    VentilationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    GateInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CutOffDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoadedOnVesselDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCarrierRelease = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomsClearance = table.Column<bool>(type: "bit", nullable: false),
                    UvDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastFreeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YardLocation = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    ApptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GateOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreeDetentionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimateDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmptyReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAvailableForPickup = table.Column<bool>(type: "bit", nullable: false),
                    IsPP = table.Column<bool>(type: "bit", nullable: false),
                    IsCTF = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HblId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageWeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageMeasureUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VesselId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContainers_AppSysCodes_VentilationId",
                        column: x => x.VentilationId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppContainerSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContainerCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SizeDescription = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ContainerGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AmsTypeCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Teu = table.Column<double>(type: "float", nullable: false),
                    IsUseed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContainerSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContainerSizes_AppSysCodes_AmsTypeCodeId",
                        column: x => x.AmsTypeCodeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppContainerSizes_AppSysCodes_ContainerGroupId",
                        column: x => x.ContainerGroupId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppCurrencyTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ccy1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Ccy2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateInternal = table.Column<double>(type: "float", nullable: false),
                    RateInterna2 = table.Column<double>(type: "float", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCurrencyTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCurrencyTables_AppSysCodes_Ccy1Id",
                        column: x => x.Ccy1Id,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppCurrencyTables_AppSysCodes_Ccy2Id",
                        column: x => x.Ccy2Id,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppGlCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GlTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GlGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountingGlCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeposit = table.Column<bool>(type: "bit", nullable: false),
                    IsPayment = table.Column<bool>(type: "bit", nullable: false),
                    IsRevaluation = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGlCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppGlCodes_AppSysCodes_GlGroupId",
                        column: x => x.GlGroupId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppGlCodes_AppSysCodes_GlTypeId",
                        column: x => x.GlTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppPackageUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AmsNoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EManifestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPackageUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPackageUnits_AppSysCodes_AmsNoId",
                        column: x => x.AmsNoId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPackageUnits_AppSysCodes_EManifestId",
                        column: x => x.EManifestId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAwbNoRanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    EndNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAwbNoRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAwbNoRanges_AppTradePartners_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppCustomerPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivablesSources = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deposit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Invalid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    U2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    H2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCustomerPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCustomerPayment_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppCustomerPayment_AppTradePartners_ReceivablesSources",
                        column: x => x.ReceivablesSources,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShowPartyOnCheck = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Invalid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    U2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    H2T = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPayment_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppPayment_AppTradePartners_PaidTo",
                        column: x => x.PaidTo,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppTradeParties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartyType = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    TradePartyDescription = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetTradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradeParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTradeParties_AppTradePartners_TradePartnerId",
                        column: x => x.TradePartnerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartnerMemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Highlight = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartnerMemo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerMemo_AppTradePartners_TradePartnerId",
                        column: x => x.TradePartnerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceClaims",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceClaims", x => new { x.ApiResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceClaims_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceProperties",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceProperties", x => new { x.ApiResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceProperties_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceScopes",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceScopes", x => new { x.ApiResourceId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceScopes_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceSecrets",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceSecrets_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeClaims",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApiScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeClaims", x => new { x.ApiScopeId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "IdentityServerApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeProperties",
                columns: table => new
                {
                    ApiScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeProperties", x => new { x.ApiScopeId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeProperties_IdentityServerApiScopes_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "IdentityServerApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientClaims",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientClaims", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientClaims_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientCorsOrigins",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientCorsOrigins", x => new { x.ClientId, x.Origin });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientCorsOrigins_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientGrantTypes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientGrantTypes", x => new { x.ClientId, x.GrantType });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientGrantTypes_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientIdPRestrictions",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientIdPRestrictions", x => new { x.ClientId, x.Provider });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientIdPRestrictions_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientPostLogoutRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientPostLogoutRedirectUris", x => new { x.ClientId, x.PostLogoutRedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientProperties",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientProperties", x => new { x.ClientId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientProperties_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RedirectUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientRedirectUris", x => new { x.ClientId, x.RedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientRedirectUris_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientScopes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientScopes", x => new { x.ClientId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientScopes_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientSecrets",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientSecrets", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientSecrets_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceClaims",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdentityResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceClaims_IdentityServerIdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceProperties",
                columns: table => new
                {
                    IdentityResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceProperties", x => new { x.IdentityResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceProperties_IdentityServerIdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppAirImportHawbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MawbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HawbNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hsn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OPId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalDestination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalETA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trucker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastFreeDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StorageStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Freight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Package = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeWeightKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeWeightCBM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ITNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassOfEntry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ITDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ITIssuedLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrtRelease = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoReleasedto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoorDelivered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incoterms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTermStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTermTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PONo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillToId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomsBroker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirImportHawbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirImportHawbs_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportHawbs_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportHawbs_UserData_OPId",
                        column: x => x.OPId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportHawbs_UserData_SalesId",
                        column: x => x.SalesId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntityChangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PropertyTypeFullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppCountryDisplayName",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCountryDisplayName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCountryDisplayName_AppAirports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppCountryDisplayName_AppCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "AppCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCountryDisplayName_AppOffices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPorts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubDiv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPort = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPorts_AppCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "AppCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppBillingCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BillingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreditId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAR = table.Column<bool>(type: "bit", nullable: false),
                    IsAP = table.Column<bool>(type: "bit", nullable: false),
                    IsDC = table.Column<bool>(type: "bit", nullable: false),
                    IsGA = table.Column<bool>(type: "bit", nullable: false),
                    IsOceanImportMbl = table.Column<bool>(type: "bit", nullable: false),
                    IsOceanImportHbl = table.Column<bool>(type: "bit", nullable: false),
                    IsOceanExportMbl = table.Column<bool>(type: "bit", nullable: false),
                    IsOceanExportHbl = table.Column<bool>(type: "bit", nullable: false),
                    IsAirImportMbl = table.Column<bool>(type: "bit", nullable: false),
                    IsAirImportHbl = table.Column<bool>(type: "bit", nullable: false),
                    IsAirExportMbl = table.Column<bool>(type: "bit", nullable: false),
                    IsAirExportHbl = table.Column<bool>(type: "bit", nullable: false),
                    IsTkm = table.Column<bool>(type: "bit", nullable: false),
                    IsMsm = table.Column<bool>(type: "bit", nullable: false),
                    IsWhs = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPayroll = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBillingCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBillingCodes_AppGlCodes_CostId",
                        column: x => x.CostId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppBillingCodes_AppGlCodes_CreditId",
                        column: x => x.CreditId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppBillingCodes_AppGlCodes_DeitId",
                        column: x => x.DeitId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppBillingCodes_AppGlCodes_RevenueId",
                        column: x => x.RevenueId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppInv",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GlCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    BalanceAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    PaymentAmountTwd = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    InvoiceDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInv", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInv_AppGlCodes_GlCodeId",
                        column: x => x.GlCodeId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInv_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MblId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MawbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HawbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VesselScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShipToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttentionTo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    OutstandingAmount = table.Column<double>(type: "float", nullable: false),
                    PaidAmount = table.Column<double>(type: "float", nullable: false),
                    SystemNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CcyRateSourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IncotermsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memorandum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfitShare = table.Column<int>(type: "int", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    IsAmountConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GlCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    BalanceAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    PaymentAmountTwd = table.Column<decimal>(type: "decimal(20,10)", precision: 20, scale: 10, nullable: true),
                    InvoiceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HblNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReceiveed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalBeforeTax = table.Column<double>(type: "float", nullable: true),
                    TotalTax = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppGlCodes_GlCodeId",
                        column: x => x.GlCodeId,
                        principalTable: "AppGlCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppSysCodes_CcyRateSourceId",
                        column: x => x.CcyRateSourceId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppSysCodes_IncotermsId",
                        column: x => x.IncotermsId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppTradePartners_InvoiceCompanyId",
                        column: x => x.InvoiceCompanyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppInvoices_AppTradePartners_ShipToId",
                        column: x => x.ShipToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAirExportBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SoNoDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsHold = table.Column<bool>(type: "bit", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubAgentAwb = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierBkgNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermTypeFrom = table.Column<int>(type: "int", nullable: false),
                    SvcTermTypeTo = table.Column<int>(type: "int", nullable: false),
                    IncotermsType = table.Column<int>(type: "int", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActualShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TruckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureAndQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandlingInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DepatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DVCarriage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVCustomer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WtVal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Package = table.Column<double>(type: "float", nullable: false),
                    MawbPackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyingRate = table.Column<double>(type: "float", nullable: false),
                    BuyingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    SellingRate = table.Column<double>(type: "float", nullable: false),
                    SellingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    VolumeWeightKg = table.Column<double>(type: "float", nullable: false),
                    VolumeWeightCbm = table.Column<double>(type: "float", nullable: false),
                    GrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    GrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    GrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    DisplayUnit = table.Column<int>(type: "int", nullable: false),
                    SalesType = table.Column<int>(type: "int", nullable: false),
                    ShipType = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirExportBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppAirports_DepatureId",
                        column: x => x.DepatureId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppAirports_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppPackageUnits_PackageId",
                        column: x => x.PackageId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ActualShipperId",
                        column: x => x.ActualShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_BillToId",
                        column: x => x.BillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CargoPickupId",
                        column: x => x.CargoPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_NotifyId",
                        column: x => x.NotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_TruckerId",
                        column: x => x.TruckerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_UserData_HolderId",
                        column: x => x.HolderId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_UserData_SaleId",
                        column: x => x.SaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAirExportMawbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MawbCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssuingCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AwbType = table.Column<int>(type: "int", nullable: false),
                    MawbNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwbDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItnNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwbAcctCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MawbOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDirectMaster = table.Column<bool>(type: "bit", nullable: false),
                    DepatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans1ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans1DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans1FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans1CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans2ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans2DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans2FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans2CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans3Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans3ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans3DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans3FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans3CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DVCarriage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DVCustomer = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Insurance = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CarrierSpotNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    WtVal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingInfo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ShipperLoad = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Sci = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Package = table.Column<double>(type: "float", nullable: false),
                    MawbPackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyingRate = table.Column<double>(type: "float", nullable: false),
                    BuyingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    SellingRate = table.Column<double>(type: "float", nullable: false),
                    SellingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    VolumeWeightKg = table.Column<double>(type: "float", nullable: false),
                    VolumeWeightCbm = table.Column<double>(type: "float", nullable: false),
                    GrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    GrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    GrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    IncotermsType = table.Column<int>(type: "int", nullable: false),
                    ServiceTermTypeFrom = table.Column<int>(type: "int", nullable: false),
                    ServiceTermTypeTo = table.Column<int>(type: "int", nullable: false),
                    IsAwbCancelled = table.Column<bool>(type: "bit", nullable: false),
                    AwbCancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AwbCancelledOpId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReasonForCancel = table.Column<int>(type: "int", nullable: false),
                    BusinessReferredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsECom = table.Column<bool>(type: "bit", nullable: false),
                    DisplayUnit = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PONo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureAndQuantityOfGoods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManifestNatureAndQuantityOfGoods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandlingInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirExportMawbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppAirports_DepatureId",
                        column: x => x.DepatureId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppAirports_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppAirports_RouteTrans1Id",
                        column: x => x.RouteTrans1Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppAirports_RouteTrans2Id",
                        column: x => x.RouteTrans2Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppAirports_RouteTrans3Id",
                        column: x => x.RouteTrans3Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppPackageUnits_PackageId",
                        column: x => x.PackageId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_AwbAcctCarrierId",
                        column: x => x.AwbAcctCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_BusinessReferredId",
                        column: x => x.BusinessReferredId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_IssuingCarrierId",
                        column: x => x.IssuingCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_MawbCarrierId",
                        column: x => x.MawbCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_NotifyId",
                        column: x => x.NotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_RouteTrans1CarrierId",
                        column: x => x.RouteTrans1CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_RouteTrans2CarrierId",
                        column: x => x.RouteTrans2CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_RouteTrans3CarrierId",
                        column: x => x.RouteTrans3CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_UserData_AwbCancelledOpId",
                        column: x => x.AwbCancelledOpId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportMawbs_UserData_MawbOperatorId",
                        column: x => x.MawbOperatorId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAirImportMawbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MawbNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwbType = table.Column<int>(type: "int", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OverseaAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AwbAcctCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OPId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDirectMaster = table.Column<bool>(type: "bit", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans1ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans1DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans1FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans1CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans2ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans2DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans2FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans2CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans3Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteTrans3ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans3DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteTrans3FlightNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RouteTrans3CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FPOEDepature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FPOETrans1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FPOETrans2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FPOETrans3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FPOEDestination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreightLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StorageStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Package = table.Column<double>(type: "float", nullable: false),
                    MawbPackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MawbPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    GrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    VolumeWeightKg = table.Column<double>(type: "float", nullable: false),
                    VolumeWeightCbm = table.Column<double>(type: "float", nullable: false),
                    FreightType = table.Column<int>(type: "int", nullable: false),
                    IncotermsType = table.Column<int>(type: "int", nullable: false),
                    ServiceTermTypeFrom = table.Column<int>(type: "int", nullable: false),
                    ServiceTermTypeTo = table.Column<int>(type: "int", nullable: false),
                    BusinessReferredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsECom = table.Column<bool>(type: "bit", nullable: false),
                    DisplayUnit = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirImportMawbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppAirports_DepatureId",
                        column: x => x.DepatureId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppAirports_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppAirports_RouteTrans1Id",
                        column: x => x.RouteTrans1Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppAirports_RouteTrans2Id",
                        column: x => x.RouteTrans2Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppAirports_RouteTrans3Id",
                        column: x => x.RouteTrans3Id,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppPackageUnits_MawbPackageId",
                        column: x => x.MawbPackageId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_AwbAcctCarrierId",
                        column: x => x.AwbAcctCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_BillToId",
                        column: x => x.BillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_BusinessReferredId",
                        column: x => x.BusinessReferredId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_FreightLocationId",
                        column: x => x.FreightLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_NotifyId",
                        column: x => x.NotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_OverseaAgentId",
                        column: x => x.OverseaAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_RouteTrans1CarrierId",
                        column: x => x.RouteTrans1CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_RouteTrans2CarrierId",
                        column: x => x.RouteTrans2CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_RouteTrans3CarrierId",
                        column: x => x.RouteTrans3CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_UserData_OPId",
                        column: x => x.OPId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirImportMawbs_UserData_SalesId",
                        column: x => x.SalesId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppCargoManifests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PackageNum = table.Column<int>(type: "int", nullable: false),
                    PackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HtsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pcs = table.Column<int>(type: "int", nullable: false),
                    NetWeight = table.Column<double>(type: "float", nullable: false),
                    GrossWeight = table.Column<double>(type: "float", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCargoManifests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCargoManifests_AppPackageUnits_PackageUnitId",
                        column: x => x.PackageUnitId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppWarehouseReceipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    DimensionUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Pcs = table.Column<int>(type: "int", nullable: false),
                    PackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Measure = table.Column<double>(type: "float", nullable: false),
                    LoadPlanRemarks = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWarehouseReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppWarehouseReceipts_AppPackageUnits_PackageUnitId",
                        column: x => x.PackageUnitId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppWarehouseReceipts_AppSysCodes_DimensionUnitId",
                        column: x => x.DimensionUnitId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppOceanExportMbls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FilingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AmsNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MblCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblCarrierContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlAcctCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlAcctCarrierContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblOverseaAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblOverseaAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblNotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblNotifyContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsedCareOf = table.Column<bool>(type: "bit", nullable: false),
                    CareOfId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CareOfContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarrierContractNo = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    IsDirect = table.Column<bool>(type: "bit", nullable: false),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MblCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblCustomerContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblBillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblBillToContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblConsigneeContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblSalesTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmptyPickupContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreCarriageVesselNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrepreCarriageVoyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FreightTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayMblContainerSizeQtyInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OblTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PortCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VgmCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RailCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblReferralById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblReferralByContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBookingAgent = table.Column<bool>(type: "bit", nullable: false),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    MblReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OnBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubBlNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ServiceContactNo = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ForwardRefNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TransPort1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Trans1Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EctNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PrnNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsEcommerce = table.Column<bool>(type: "bit", nullable: false),
                    ColorRemarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomesticInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageWeightId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageMeasureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalAmountType = table.Column<bool>(type: "bit", nullable: false),
                    TotalPackage = table.Column<int>(type: "int", nullable: false),
                    TotalWeight = table.Column<double>(type: "float", nullable: false),
                    TotalMeasure = table.Column<double>(type: "float", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOceanExportMbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppPorts_TransPort1Id",
                        column: x => x.TransPort1Id,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_BlTypeId",
                        column: x => x.BlTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_ColorRemarkId",
                        column: x => x.ColorRemarkId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_FreightTermId",
                        column: x => x.FreightTermId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_MblSalesTypeId",
                        column: x => x.MblSalesTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_OblTypeId",
                        column: x => x.OblTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_PackageCategoryId",
                        column: x => x.PackageCategoryId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_PackageMeasureId",
                        column: x => x.PackageMeasureId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_PackageWeightId",
                        column: x => x.PackageWeightId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_PreCarriageVesselNameId",
                        column: x => x.PreCarriageVesselNameId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_BlAcctCarrierId",
                        column: x => x.BlAcctCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_CareOfId",
                        column: x => x.CareOfId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblBillToId",
                        column: x => x.MblBillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblCarrierId",
                        column: x => x.MblCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblConsigneeId",
                        column: x => x.MblConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblCustomerId",
                        column: x => x.MblCustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblNotifyId",
                        column: x => x.MblNotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblOverseaAgentId",
                        column: x => x.MblOverseaAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_MblReferralById",
                        column: x => x.MblReferralById,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_AppTradePartners_ShippingAgentId",
                        column: x => x.ShippingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_UserData_CancelById",
                        column: x => x.CancelById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_UserData_MblOperatorId",
                        column: x => x.MblOperatorId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_UserData_MblSaleId",
                        column: x => x.MblSaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportMbls_UserData_ReleaseById",
                        column: x => x.ReleaseById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppOceanImportMbls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FilingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AmsNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MblCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblCarrierContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlAcctCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlAcctCarrierContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblOverseaAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblOverseaAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblNotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblNotifyContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsedCareOf = table.Column<bool>(type: "bit", nullable: false),
                    CareOfId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CareOfContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarrierContractNo = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    IsDirect = table.Column<bool>(type: "bit", nullable: false),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MblCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblCustomerContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblBillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblBillToContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblConsigneeContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblSalesTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmptyPickupContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreCarriageVesselNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrepreCarriageVoyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FreightTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayMblContainerSizeQtyInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OblTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PortCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VgmCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RailCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblReferralById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblReferralByContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBookingAgent = table.Column<bool>(type: "bit", nullable: false),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    MblReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OnBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubBlNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ServiceContactNo = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ForwardRefNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TransPort1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Trans1Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EctNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PrnNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsEcommerce = table.Column<bool>(type: "bit", nullable: false),
                    ColorRemarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomesticInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageWeightId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageMeasureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalAmountType = table.Column<bool>(type: "bit", nullable: false),
                    TotalPackage = table.Column<int>(type: "int", nullable: false),
                    TotalWeight = table.Column<double>(type: "float", nullable: false),
                    TotalMeasure = table.Column<double>(type: "float", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AgentRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CyLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CfsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Etb = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOblReceived = table.Column<bool>(type: "bit", nullable: false),
                    OblReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessReferrerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LatestContainerEntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ItDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItLocation = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ReturnLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOceanImportMbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppPorts_TransPort1Id",
                        column: x => x.TransPort1Id,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_BlTypeId",
                        column: x => x.BlTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_ColorRemarkId",
                        column: x => x.ColorRemarkId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_FreightTermId",
                        column: x => x.FreightTermId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_MblSalesTypeId",
                        column: x => x.MblSalesTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_OblTypeId",
                        column: x => x.OblTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_PackageCategoryId",
                        column: x => x.PackageCategoryId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_PackageMeasureId",
                        column: x => x.PackageMeasureId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_PackageWeightId",
                        column: x => x.PackageWeightId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_PreCarriageVesselNameId",
                        column: x => x.PreCarriageVesselNameId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_BlAcctCarrierId",
                        column: x => x.BlAcctCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_BusinessReferrerId",
                        column: x => x.BusinessReferrerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_CareOfId",
                        column: x => x.CareOfId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_CfsLocationId",
                        column: x => x.CfsLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_CyLocationId",
                        column: x => x.CyLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblBillToId",
                        column: x => x.MblBillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblCarrierId",
                        column: x => x.MblCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblConsigneeId",
                        column: x => x.MblConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblCustomerId",
                        column: x => x.MblCustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblNotifyId",
                        column: x => x.MblNotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblOverseaAgentId",
                        column: x => x.MblOverseaAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblReferralById",
                        column: x => x.MblReferralById,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_MblShipperId",
                        column: x => x.MblShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_ReturnLocationId",
                        column: x => x.ReturnLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_AppTradePartners_ShippingAgentId",
                        column: x => x.ShippingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_UserData_CancelById",
                        column: x => x.CancelById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_UserData_MblOperatorId",
                        column: x => x.MblOperatorId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_UserData_MblSaleId",
                        column: x => x.MblSaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportMbls_UserData_ReleaseById",
                        column: x => x.ReleaseById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppVesselSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MblCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlAcctCarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblOverseaAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblNotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreightTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OblTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PortCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VgmCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RailCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnBoardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubBlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForwardRefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransPort1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Trans1Eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVesselSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppPorts_TransPort1Id",
                        column: x => x.TransPort1Id,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_BlTypeId",
                        column: x => x.BlTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_FreightTermId",
                        column: x => x.FreightTermId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_OblTypeId",
                        column: x => x.OblTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_BlAcctCarrierId",
                        column: x => x.BlAcctCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_MblCarrierId",
                        column: x => x.MblCarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_MblNotifyId",
                        column: x => x.MblNotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_MblOverseaAgentId",
                        column: x => x.MblOverseaAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppVesselSchedules_AppTradePartners_ShippingAgentId",
                        column: x => x.ShippingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartnerDefaultFreightAP",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    FreightCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreightDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Vol = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    AgentAmount = table.Column<double>(type: "float", nullable: false),
                    ShipModeFCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeLCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeFAK = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeBULK = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartnerDefaultFreightAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightAP_AppBillingCodes_FreightCode",
                        column: x => x.FreightCode,
                        principalTable: "AppBillingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightAP_AppTradePartners_TradePartnerId",
                        column: x => x.TradePartnerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartnerDefaultFreightAR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    FreightCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreightDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Vol = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    AgentAmount = table.Column<double>(type: "float", nullable: false),
                    ShipModeFCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeLCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeFAK = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeBULK = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartnerDefaultFreightAR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightAR_AppBillingCodes_FreightCode",
                        column: x => x.FreightCode,
                        principalTable: "AppBillingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightAR_AppTradePartners_TradePartnerId",
                        column: x => x.TradePartnerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppTradePartnerDefaultFreightDC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    FreightCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreightDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Vol = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    AgentAmount = table.Column<double>(type: "float", nullable: false),
                    ShipModeFCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeLCL = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeFAK = table.Column<bool>(type: "bit", nullable: true),
                    ShipModeBULK = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTradePartnerDefaultFreightDC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightDC_AppBillingCodes_FreightCode",
                        column: x => x.FreightCode,
                        principalTable: "AppBillingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppTradePartnerDefaultFreightDC_AppTradePartners_TradePartnerId",
                        column: x => x.TradePartnerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppInvoiceBills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPOrCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AmountToAgent = table.Column<double>(type: "float", nullable: false),
                    IsNonProfit = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Price = table.Column<int>(type: "int", nullable: true),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInvoiceBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInvoiceBills_AppInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "AppInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppAirExportHawbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MawbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HawbNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ITNNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActualShippedr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Consignee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverseaAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingCarrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trucker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OPId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubAgentAwb = table.Column<bool>(type: "bit", nullable: false),
                    DepartureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoPickup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DVCarriage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVCustoms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WTVAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Package = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyingRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyingRateUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellingRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellingRateUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeWeightKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeWeightCBM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightShprKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightShprLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightShprAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightCneeKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightCneeLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeightCneeAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightShprKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightShprLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightShprAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightCneeKG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightCneeLB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeableWeightCneeAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incoterms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTermStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTermEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AWBCancelled = table.Column<bool>(type: "bit", nullable: false),
                    AWBCancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanceledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonForCancel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PONo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureAndQuantityOfGoods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManifestNatureAndQuantityOfGoods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandlingInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirExportHawbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirExportHawbs_AppAirExportMawbs_MawbId",
                        column: x => x.MawbId,
                        principalTable: "AppAirExportMawbs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportHawbs_UserData_OPId",
                        column: x => x.OPId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportHawbs_UserData_SalesId",
                        column: x => x.SalesId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppOceanExportHbls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MblId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    B2bNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    QuotationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HblShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblBillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblNotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomsBrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TruckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSubAgentBl = table.Column<bool>(type: "bit", nullable: false),
                    ReceivingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FbaFcId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CargoPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForBuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForSalerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayHblContainerSizeQtyInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExpress = table.Column<bool>(type: "bit", nullable: false),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblSalesTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblWhCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyReturnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LcNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LcIssueBank = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LcIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    IsHold = table.Column<bool>(type: "bit", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    HblReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblReferralById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ShipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IncotermsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NraNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsEcommerce = table.Column<bool>(type: "bit", nullable: false),
                    CyCfsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRailwayCode = table.Column<bool>(type: "bit", nullable: false),
                    RailwayCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DoorDeliveryETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoorDeliveryATA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomClearance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomDeclaration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ColorRemarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomesticInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARSurplus = table.Column<double>(type: "float", nullable: false),
                    APSurplus = table.Column<double>(type: "float", nullable: false),
                    DCSurplus = table.Column<double>(type: "float", nullable: false),
                    SurplusType = table.Column<int>(type: "int", nullable: false),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOceanExportHbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppOceanExportMbls_MblId",
                        column: x => x.MblId,
                        principalTable: "AppOceanExportMbls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_CardColorId",
                        column: x => x.CardColorId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_ColorRemarkId",
                        column: x => x.ColorRemarkId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_FreightTermForBuyerId",
                        column: x => x.FreightTermForBuyerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_FreightTermForSalerId",
                        column: x => x.FreightTermForSalerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_IncotermsId",
                        column: x => x.IncotermsId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_MblSalesTypeId",
                        column: x => x.MblSalesTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_RailwayCodeId",
                        column: x => x.RailwayCodeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_ShipTypeId",
                        column: x => x.ShipTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_CargoPickupId",
                        column: x => x.CargoPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_CustomsBrokerId",
                        column: x => x.CustomsBrokerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_CyCfsLocationId",
                        column: x => x.CyCfsLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_FbaFcId",
                        column: x => x.FbaFcId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_HblBillToId",
                        column: x => x.HblBillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_HblConsigneeId",
                        column: x => x.HblConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_HblCustomerId",
                        column: x => x.HblCustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_HblNotifyId",
                        column: x => x.HblNotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_HblShipperId",
                        column: x => x.HblShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_MblReferralById",
                        column: x => x.MblReferralById,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_ReceivingAgentId",
                        column: x => x.ReceivingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_AppTradePartners_TruckerId",
                        column: x => x.TruckerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_UserData_CancelById",
                        column: x => x.CancelById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_UserData_HblOperatorId",
                        column: x => x.HblOperatorId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_UserData_HblSaleId",
                        column: x => x.HblSaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_UserData_HolderId",
                        column: x => x.HolderId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanExportHbls_UserData_ReleaseById",
                        column: x => x.ReleaseById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppOceanImportHbls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MblId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    B2bNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    QuotationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HblShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblBillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblNotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomsBrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TruckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSubAgentBl = table.Column<bool>(type: "bit", nullable: false),
                    ReceivingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FbaFcId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CargoPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForBuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForSalerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayHblContainerSizeQtyInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExpress = table.Column<bool>(type: "bit", nullable: false),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblSalesTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblWhCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyReturnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LcNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LcIssueBank = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LcIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    IsHold = table.Column<bool>(type: "bit", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    HblReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MblReferralById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ShipTypeId = table.Column<int>(type: "int", nullable: true),
                    IncotermsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NraNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsEcommerce = table.Column<bool>(type: "bit", nullable: false),
                    CyCfsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRailwayCode = table.Column<bool>(type: "bit", nullable: false),
                    RailwayCodeId = table.Column<int>(type: "int", nullable: true),
                    DoorDeliveryETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoorDeliveryATA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomClearance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomDeclaration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ColorRemarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomesticInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomesticInstructionsDelOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARSurplus = table.Column<double>(type: "float", nullable: false),
                    APSurplus = table.Column<double>(type: "float", nullable: false),
                    DCSurplus = table.Column<double>(type: "float", nullable: false),
                    SurplusType = table.Column<int>(type: "int", nullable: false),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AmsNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsfNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsfByThirdParty = table.Column<bool>(type: "bit", nullable: false),
                    IsfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RaiseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Demurrage = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ItDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItLocation = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessReferrerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomsDeclarationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomsDeclarationSendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOblReceived = table.Column<bool>(type: "bit", nullable: false),
                    OblReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPor = table.Column<bool>(type: "bit", nullable: false),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAn = table.Column<bool>(type: "bit", nullable: false),
                    AnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDo = table.Column<bool>(type: "bit", nullable: false),
                    DoDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NameAccount = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    GroupComm = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LineCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DoorMove = table.Column<bool>(type: "bit", nullable: false),
                    CClearance = table.Column<bool>(type: "bit", nullable: false),
                    CHold = table.Column<bool>(type: "bit", nullable: false),
                    Ror = table.Column<bool>(type: "bit", nullable: false),
                    IsfMatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lfd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryDocSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItIssuedLocation = table.Column<int>(type: "int", nullable: true),
                    Freight = table.Column<int>(type: "int", nullable: true),
                    EntryNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDoc = table.Column<bool>(type: "bit", nullable: false),
                    Available = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOceanImportHbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppOceanImportMbls_MblId",
                        column: x => x.MblId,
                        principalTable: "AppOceanImportMbls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_CardColorId",
                        column: x => x.CardColorId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_ColorRemarkId",
                        column: x => x.ColorRemarkId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_FreightTermForBuyerId",
                        column: x => x.FreightTermForBuyerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_FreightTermForSalerId",
                        column: x => x.FreightTermForSalerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_HblSalesTypeId",
                        column: x => x.HblSalesTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_IncotermsId",
                        column: x => x.IncotermsId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_BusinessReferrerId",
                        column: x => x.BusinessReferrerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_CargoPickupId",
                        column: x => x.CargoPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_CustomsBrokerId",
                        column: x => x.CustomsBrokerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_CyCfsLocationId",
                        column: x => x.CyCfsLocationId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_FbaFcId",
                        column: x => x.FbaFcId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_HblBillToId",
                        column: x => x.HblBillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_HblConsigneeId",
                        column: x => x.HblConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_HblCustomerId",
                        column: x => x.HblCustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_HblNotifyId",
                        column: x => x.HblNotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_HblShipperId",
                        column: x => x.HblShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_MblReferralById",
                        column: x => x.MblReferralById,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_ReceivingAgentId",
                        column: x => x.ReceivingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_AppTradePartners_TruckerId",
                        column: x => x.TruckerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_UserData_CancelById",
                        column: x => x.CancelById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_UserData_HblOperatorId",
                        column: x => x.HblOperatorId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_UserData_HblSaleId",
                        column: x => x.HblSaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_UserData_HolderId",
                        column: x => x.HolderId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOceanImportHbls_UserData_ReleaseById",
                        column: x => x.ReleaseById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppExportBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VesselScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SoNoDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SalespersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MblSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsHold = table.Column<bool>(type: "bit", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancelById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    HblReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierBkgNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IncotermsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HblAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voyage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContainerQtyInputText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PorEtd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolEtd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PodEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CargoReadyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FdestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FdestEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FbaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransPort1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Trans1Eta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EctNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrnNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferralById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContainerPickupNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TruckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmptyPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WhCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PortCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VgmCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RailCutOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyReturnDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreightTermForBuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForSalerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExportBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_DelId",
                        column: x => x.DelId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_FdestId",
                        column: x => x.FdestId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_PodId",
                        column: x => x.PodId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_PolId",
                        column: x => x.PolId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_PorId",
                        column: x => x.PorId,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppPorts_TransPort1Id",
                        column: x => x.TransPort1Id,
                        principalTable: "AppPorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_FreightTermForBuyerId",
                        column: x => x.FreightTermForBuyerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_FreightTermForSalerId",
                        column: x => x.FreightTermForSalerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_IncotermsId",
                        column: x => x.IncotermsId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_ShipModeId",
                        column: x => x.ShipModeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_SvcTermFromId",
                        column: x => x.SvcTermFromId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppSysCodes_SvcTermToId",
                        column: x => x.SvcTermToId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_BillToId",
                        column: x => x.BillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_CargoPickupId",
                        column: x => x.CargoPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_EmptyPickupId",
                        column: x => x.EmptyPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_FbaId",
                        column: x => x.FbaId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_HblAgentId",
                        column: x => x.HblAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_NotifyId",
                        column: x => x.NotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_ReferralById",
                        column: x => x.ReferralById,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_ShippingAgentId",
                        column: x => x.ShippingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppTradePartners_TruckerId",
                        column: x => x.TruckerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_AppVesselSchedules_VesselScheduleId",
                        column: x => x.VesselScheduleId,
                        principalTable: "AppVesselSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_UserData_CancelById",
                        column: x => x.CancelById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_UserData_HolderId",
                        column: x => x.HolderId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_UserData_MblSaleId",
                        column: x => x.MblSaleId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppExportBookings_UserData_ReleaseById",
                        column: x => x.ReleaseById,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_AuditLogId",
                table: "AbpAuditLogActions",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_ExecutionTime",
                table: "AbpAuditLogActions",
                columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_AuditLogId",
                table: "AbpEntityChanges",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
                table: "AbpFeatureValues",
                columns: new[] { "Name", "ProviderName", "ProviderKey" },
                unique: true,
                filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId",
                table: "AbpLinkUsers",
                columns: new[] { "SourceUserId", "SourceTenantId", "TargetUserId", "TargetTenantId" },
                unique: true,
                filter: "[SourceTenantId] IS NOT NULL AND [TargetTenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "RoleId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_Code",
                table: "AbpOrganizationUnits",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] { "TenantId", "Name", "ProviderName", "ProviderKey" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_NormalizedName",
                table: "AbpRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpSecurityLogs_TenantId_Action",
                table: "AbpSecurityLogs",
                columns: new[] { "TenantId", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSecurityLogs_TenantId_ApplicationName",
                table: "AbpSecurityLogs",
                columns: new[] { "TenantId", "ApplicationName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSecurityLogs_TenantId_Identity",
                table: "AbpSecurityLogs",
                columns: new[] { "TenantId", "Identity" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSecurityLogs_TenantId_UserId",
                table: "AbpSecurityLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
                table: "AbpSettings",
                columns: new[] { "Name", "ProviderName", "ProviderKey" },
                unique: true,
                filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_Name",
                table: "AbpTenants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "UserId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_RoleId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_Email",
                table: "AbpUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_NormalizedEmail",
                table: "AbpUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_NormalizedUserName",
                table: "AbpUsers",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_UserName",
                table: "AbpUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ActualShipperId",
                table: "AppAirExportBookings",
                column: "ActualShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_AgentId",
                table: "AppAirExportBookings",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_BillToId",
                table: "AppAirExportBookings",
                column: "BillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CargoPickupId",
                table: "AppAirExportBookings",
                column: "CargoPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CargoTypeId",
                table: "AppAirExportBookings",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CarrierId",
                table: "AppAirExportBookings",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CoLoaderId",
                table: "AppAirExportBookings",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ConsigneeId",
                table: "AppAirExportBookings",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CustomerId",
                table: "AppAirExportBookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DeliveryToId",
                table: "AppAirExportBookings",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DepatureId",
                table: "AppAirExportBookings",
                column: "DepatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DestinationId",
                table: "AppAirExportBookings",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ForwardingAgentId",
                table: "AppAirExportBookings",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_HolderId",
                table: "AppAirExportBookings",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_NotifyId",
                table: "AppAirExportBookings",
                column: "NotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_OfficeId",
                table: "AppAirExportBookings",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_PackageId",
                table: "AppAirExportBookings",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_SaleId",
                table: "AppAirExportBookings",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ShipperId",
                table: "AppAirExportBookings",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_TruckerId",
                table: "AppAirExportBookings",
                column: "TruckerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportHawbs_MawbId",
                table: "AppAirExportHawbs",
                column: "MawbId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportHawbs_OPId",
                table: "AppAirExportHawbs",
                column: "OPId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportHawbs_SalesId",
                table: "AppAirExportHawbs",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_AwbAcctCarrierId",
                table: "AppAirExportMawbs",
                column: "AwbAcctCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_AwbCancelledOpId",
                table: "AppAirExportMawbs",
                column: "AwbCancelledOpId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_BusinessReferredId",
                table: "AppAirExportMawbs",
                column: "BusinessReferredId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_CoLoaderId",
                table: "AppAirExportMawbs",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_ConsigneeId",
                table: "AppAirExportMawbs",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_DeliveryId",
                table: "AppAirExportMawbs",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_DepatureId",
                table: "AppAirExportMawbs",
                column: "DepatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_DestinationId",
                table: "AppAirExportMawbs",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_IssuingCarrierId",
                table: "AppAirExportMawbs",
                column: "IssuingCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_MawbCarrierId",
                table: "AppAirExportMawbs",
                column: "MawbCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_MawbOperatorId",
                table: "AppAirExportMawbs",
                column: "MawbOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_NotifyId",
                table: "AppAirExportMawbs",
                column: "NotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_OfficeId",
                table: "AppAirExportMawbs",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_PackageId",
                table: "AppAirExportMawbs",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans1CarrierId",
                table: "AppAirExportMawbs",
                column: "RouteTrans1CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans1Id",
                table: "AppAirExportMawbs",
                column: "RouteTrans1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans2CarrierId",
                table: "AppAirExportMawbs",
                column: "RouteTrans2CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans2Id",
                table: "AppAirExportMawbs",
                column: "RouteTrans2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans3CarrierId",
                table: "AppAirExportMawbs",
                column: "RouteTrans3CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteTrans3Id",
                table: "AppAirExportMawbs",
                column: "RouteTrans3Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_ShipperId",
                table: "AppAirExportMawbs",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportHawbs_ConsigneeId",
                table: "AppAirImportHawbs",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportHawbs_OPId",
                table: "AppAirImportHawbs",
                column: "OPId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportHawbs_SalesId",
                table: "AppAirImportHawbs",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportHawbs_ShipperId",
                table: "AppAirImportHawbs",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_AwbAcctCarrierId",
                table: "AppAirImportMawbs",
                column: "AwbAcctCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_BillToId",
                table: "AppAirImportMawbs",
                column: "BillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_BusinessReferredId",
                table: "AppAirImportMawbs",
                column: "BusinessReferredId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_CarrierId",
                table: "AppAirImportMawbs",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_CoLoaderId",
                table: "AppAirImportMawbs",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_ConsigneeId",
                table: "AppAirImportMawbs",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_CustomerId",
                table: "AppAirImportMawbs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_DepatureId",
                table: "AppAirImportMawbs",
                column: "DepatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_DestinationId",
                table: "AppAirImportMawbs",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_FreightLocationId",
                table: "AppAirImportMawbs",
                column: "FreightLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_MawbPackageId",
                table: "AppAirImportMawbs",
                column: "MawbPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_NotifyId",
                table: "AppAirImportMawbs",
                column: "NotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_OfficeId",
                table: "AppAirImportMawbs",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_OPId",
                table: "AppAirImportMawbs",
                column: "OPId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_OverseaAgentId",
                table: "AppAirImportMawbs",
                column: "OverseaAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans1CarrierId",
                table: "AppAirImportMawbs",
                column: "RouteTrans1CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans1Id",
                table: "AppAirImportMawbs",
                column: "RouteTrans1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans2CarrierId",
                table: "AppAirImportMawbs",
                column: "RouteTrans2CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans2Id",
                table: "AppAirImportMawbs",
                column: "RouteTrans2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans3CarrierId",
                table: "AppAirImportMawbs",
                column: "RouteTrans3CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteTrans3Id",
                table: "AppAirImportMawbs",
                column: "RouteTrans3Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_SalesId",
                table: "AppAirImportMawbs",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_ShipperId",
                table: "AppAirImportMawbs",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAwbNoRanges_CompanyId",
                table: "AppAwbNoRanges",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBillingCodes_CostId",
                table: "AppBillingCodes",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBillingCodes_CreditId",
                table: "AppBillingCodes",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBillingCodes_DeitId",
                table: "AppBillingCodes",
                column: "DeitId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBillingCodes_RevenueId",
                table: "AppBillingCodes",
                column: "RevenueId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCargoManifests_PackageUnitId",
                table: "AppCargoManifests",
                column: "PackageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContactPersonChilds_PersonId",
                table: "AppContactPersonChilds",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContainers_VentilationId",
                table: "AppContainers",
                column: "VentilationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContainerSizes_AmsTypeCodeId",
                table: "AppContainerSizes",
                column: "AmsTypeCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContainerSizes_ContainerGroupId",
                table: "AppContainerSizes",
                column: "ContainerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountries_CurrencyId",
                table: "AppCountries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryDisplayName_AirportId",
                table: "AppCountryDisplayName",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryDisplayName_CountryId",
                table: "AppCountryDisplayName",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryDisplayName_OfficeId",
                table: "AppCountryDisplayName",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCurrencyTables_Ccy1Id",
                table: "AppCurrencyTables",
                column: "Ccy1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppCurrencyTables_Ccy2Id",
                table: "AppCurrencyTables",
                column: "Ccy2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomerPayment_OfficeId",
                table: "AppCustomerPayment",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomerPayment_ReceivablesSources",
                table: "AppCustomerPayment",
                column: "ReceivablesSources");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_BillToId",
                table: "AppExportBookings",
                column: "BillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CancelById",
                table: "AppExportBookings",
                column: "CancelById");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CargoPickupId",
                table: "AppExportBookings",
                column: "CargoPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CargoTypeId",
                table: "AppExportBookings",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CarrierId",
                table: "AppExportBookings",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CoLoaderId",
                table: "AppExportBookings",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ConsigneeId",
                table: "AppExportBookings",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_CustomerId",
                table: "AppExportBookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_DelId",
                table: "AppExportBookings",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_DeliveryToId",
                table: "AppExportBookings",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_EmptyPickupId",
                table: "AppExportBookings",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_FbaId",
                table: "AppExportBookings",
                column: "FbaId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_FdestId",
                table: "AppExportBookings",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ForwardingAgentId",
                table: "AppExportBookings",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_FreightTermForBuyerId",
                table: "AppExportBookings",
                column: "FreightTermForBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_FreightTermForSalerId",
                table: "AppExportBookings",
                column: "FreightTermForSalerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_HblAgentId",
                table: "AppExportBookings",
                column: "HblAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_HolderId",
                table: "AppExportBookings",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_IncotermsId",
                table: "AppExportBookings",
                column: "IncotermsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_MblSaleId",
                table: "AppExportBookings",
                column: "MblSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_NotifyId",
                table: "AppExportBookings",
                column: "NotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_OfficeId",
                table: "AppExportBookings",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_PodId",
                table: "AppExportBookings",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_PolId",
                table: "AppExportBookings",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_PorId",
                table: "AppExportBookings",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ReferralById",
                table: "AppExportBookings",
                column: "ReferralById");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ReleaseById",
                table: "AppExportBookings",
                column: "ReleaseById");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ShipModeId",
                table: "AppExportBookings",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ShipperId",
                table: "AppExportBookings",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_ShippingAgentId",
                table: "AppExportBookings",
                column: "ShippingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_SvcTermFromId",
                table: "AppExportBookings",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_SvcTermToId",
                table: "AppExportBookings",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_TransPort1Id",
                table: "AppExportBookings",
                column: "TransPort1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_TruckerId",
                table: "AppExportBookings",
                column: "TruckerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_VesselScheduleId",
                table: "AppExportBookings",
                column: "VesselScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlCodes_GlGroupId",
                table: "AppGlCodes",
                column: "GlGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGlCodes_GlTypeId",
                table: "AppGlCodes",
                column: "GlTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInv_CustomerId",
                table: "AppInv",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInv_GlCodeId",
                table: "AppInv",
                column: "GlCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoiceBills_InvoiceId",
                table: "AppInvoiceBills",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_CcyRateSourceId",
                table: "AppInvoices",
                column: "CcyRateSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_CustomerId",
                table: "AppInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_GlCodeId",
                table: "AppInvoices",
                column: "GlCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_IncotermsId",
                table: "AppInvoices",
                column: "IncotermsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_InvoiceCompanyId",
                table: "AppInvoices",
                column: "InvoiceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInvoices_ShipToId",
                table: "AppInvoices",
                column: "ShipToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_AgentId",
                table: "AppOceanExportHbls",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CancelById",
                table: "AppOceanExportHbls",
                column: "CancelById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CardColorId",
                table: "AppOceanExportHbls",
                column: "CardColorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CargoPickupId",
                table: "AppOceanExportHbls",
                column: "CargoPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CargoTypeId",
                table: "AppOceanExportHbls",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ColorRemarkId",
                table: "AppOceanExportHbls",
                column: "ColorRemarkId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CustomsBrokerId",
                table: "AppOceanExportHbls",
                column: "CustomsBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_CyCfsLocationId",
                table: "AppOceanExportHbls",
                column: "CyCfsLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_DelId",
                table: "AppOceanExportHbls",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_DeliveryToId",
                table: "AppOceanExportHbls",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_EmptyPickupId",
                table: "AppOceanExportHbls",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_FbaFcId",
                table: "AppOceanExportHbls",
                column: "FbaFcId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_FdestId",
                table: "AppOceanExportHbls",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ForwardingAgentId",
                table: "AppOceanExportHbls",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_FreightTermForBuyerId",
                table: "AppOceanExportHbls",
                column: "FreightTermForBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_FreightTermForSalerId",
                table: "AppOceanExportHbls",
                column: "FreightTermForSalerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblBillToId",
                table: "AppOceanExportHbls",
                column: "HblBillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblConsigneeId",
                table: "AppOceanExportHbls",
                column: "HblConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblCustomerId",
                table: "AppOceanExportHbls",
                column: "HblCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblNotifyId",
                table: "AppOceanExportHbls",
                column: "HblNotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblOperatorId",
                table: "AppOceanExportHbls",
                column: "HblOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblSaleId",
                table: "AppOceanExportHbls",
                column: "HblSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HblShipperId",
                table: "AppOceanExportHbls",
                column: "HblShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_HolderId",
                table: "AppOceanExportHbls",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_IncotermsId",
                table: "AppOceanExportHbls",
                column: "IncotermsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_MblId",
                table: "AppOceanExportHbls",
                column: "MblId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_MblReferralById",
                table: "AppOceanExportHbls",
                column: "MblReferralById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_MblSalesTypeId",
                table: "AppOceanExportHbls",
                column: "MblSalesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_PodId",
                table: "AppOceanExportHbls",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_PolId",
                table: "AppOceanExportHbls",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_PorId",
                table: "AppOceanExportHbls",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_RailwayCodeId",
                table: "AppOceanExportHbls",
                column: "RailwayCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ReceivingAgentId",
                table: "AppOceanExportHbls",
                column: "ReceivingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ReleaseById",
                table: "AppOceanExportHbls",
                column: "ReleaseById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ShipModeId",
                table: "AppOceanExportHbls",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_ShipTypeId",
                table: "AppOceanExportHbls",
                column: "ShipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_SvcTermFromId",
                table: "AppOceanExportHbls",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_SvcTermToId",
                table: "AppOceanExportHbls",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportHbls_TruckerId",
                table: "AppOceanExportHbls",
                column: "TruckerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_BlAcctCarrierId",
                table: "AppOceanExportMbls",
                column: "BlAcctCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_BlTypeId",
                table: "AppOceanExportMbls",
                column: "BlTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_CancelById",
                table: "AppOceanExportMbls",
                column: "CancelById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_CareOfId",
                table: "AppOceanExportMbls",
                column: "CareOfId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_CargoTypeId",
                table: "AppOceanExportMbls",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_CoLoaderId",
                table: "AppOceanExportMbls",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_ColorRemarkId",
                table: "AppOceanExportMbls",
                column: "ColorRemarkId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_DelId",
                table: "AppOceanExportMbls",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_DeliveryToId",
                table: "AppOceanExportMbls",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_EmptyPickupId",
                table: "AppOceanExportMbls",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_FdestId",
                table: "AppOceanExportMbls",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_ForwardingAgentId",
                table: "AppOceanExportMbls",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_FreightTermId",
                table: "AppOceanExportMbls",
                column: "FreightTermId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblBillToId",
                table: "AppOceanExportMbls",
                column: "MblBillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblCarrierId",
                table: "AppOceanExportMbls",
                column: "MblCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblConsigneeId",
                table: "AppOceanExportMbls",
                column: "MblConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblCustomerId",
                table: "AppOceanExportMbls",
                column: "MblCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblNotifyId",
                table: "AppOceanExportMbls",
                column: "MblNotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblOperatorId",
                table: "AppOceanExportMbls",
                column: "MblOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblOverseaAgentId",
                table: "AppOceanExportMbls",
                column: "MblOverseaAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblReferralById",
                table: "AppOceanExportMbls",
                column: "MblReferralById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblSaleId",
                table: "AppOceanExportMbls",
                column: "MblSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_MblSalesTypeId",
                table: "AppOceanExportMbls",
                column: "MblSalesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_OblTypeId",
                table: "AppOceanExportMbls",
                column: "OblTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_OfficeId",
                table: "AppOceanExportMbls",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PackageCategoryId",
                table: "AppOceanExportMbls",
                column: "PackageCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PackageMeasureId",
                table: "AppOceanExportMbls",
                column: "PackageMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PackageWeightId",
                table: "AppOceanExportMbls",
                column: "PackageWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PodId",
                table: "AppOceanExportMbls",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PolId",
                table: "AppOceanExportMbls",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PorId",
                table: "AppOceanExportMbls",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_PreCarriageVesselNameId",
                table: "AppOceanExportMbls",
                column: "PreCarriageVesselNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_ReleaseById",
                table: "AppOceanExportMbls",
                column: "ReleaseById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_ShipModeId",
                table: "AppOceanExportMbls",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_ShippingAgentId",
                table: "AppOceanExportMbls",
                column: "ShippingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_SvcTermFromId",
                table: "AppOceanExportMbls",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_SvcTermToId",
                table: "AppOceanExportMbls",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanExportMbls_TransPort1Id",
                table: "AppOceanExportMbls",
                column: "TransPort1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_AgentId",
                table: "AppOceanImportHbls",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_BusinessReferrerId",
                table: "AppOceanImportHbls",
                column: "BusinessReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CancelById",
                table: "AppOceanImportHbls",
                column: "CancelById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CardColorId",
                table: "AppOceanImportHbls",
                column: "CardColorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CargoPickupId",
                table: "AppOceanImportHbls",
                column: "CargoPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CargoTypeId",
                table: "AppOceanImportHbls",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ColorRemarkId",
                table: "AppOceanImportHbls",
                column: "ColorRemarkId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CustomsBrokerId",
                table: "AppOceanImportHbls",
                column: "CustomsBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_CyCfsLocationId",
                table: "AppOceanImportHbls",
                column: "CyCfsLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_DelId",
                table: "AppOceanImportHbls",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_DeliveryToId",
                table: "AppOceanImportHbls",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_EmptyPickupId",
                table: "AppOceanImportHbls",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_FbaFcId",
                table: "AppOceanImportHbls",
                column: "FbaFcId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_FdestId",
                table: "AppOceanImportHbls",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ForwardingAgentId",
                table: "AppOceanImportHbls",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_FreightTermForBuyerId",
                table: "AppOceanImportHbls",
                column: "FreightTermForBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_FreightTermForSalerId",
                table: "AppOceanImportHbls",
                column: "FreightTermForSalerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblBillToId",
                table: "AppOceanImportHbls",
                column: "HblBillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblConsigneeId",
                table: "AppOceanImportHbls",
                column: "HblConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblCustomerId",
                table: "AppOceanImportHbls",
                column: "HblCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblNotifyId",
                table: "AppOceanImportHbls",
                column: "HblNotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblOperatorId",
                table: "AppOceanImportHbls",
                column: "HblOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblSaleId",
                table: "AppOceanImportHbls",
                column: "HblSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblSalesTypeId",
                table: "AppOceanImportHbls",
                column: "HblSalesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HblShipperId",
                table: "AppOceanImportHbls",
                column: "HblShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_HolderId",
                table: "AppOceanImportHbls",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_IncotermsId",
                table: "AppOceanImportHbls",
                column: "IncotermsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_MblId",
                table: "AppOceanImportHbls",
                column: "MblId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_MblReferralById",
                table: "AppOceanImportHbls",
                column: "MblReferralById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_PodId",
                table: "AppOceanImportHbls",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_PolId",
                table: "AppOceanImportHbls",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_PorId",
                table: "AppOceanImportHbls",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ReceivingAgentId",
                table: "AppOceanImportHbls",
                column: "ReceivingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ReleaseById",
                table: "AppOceanImportHbls",
                column: "ReleaseById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ShipModeId",
                table: "AppOceanImportHbls",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_SvcTermFromId",
                table: "AppOceanImportHbls",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_SvcTermToId",
                table: "AppOceanImportHbls",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_TruckerId",
                table: "AppOceanImportHbls",
                column: "TruckerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_BlAcctCarrierId",
                table: "AppOceanImportMbls",
                column: "BlAcctCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_BlTypeId",
                table: "AppOceanImportMbls",
                column: "BlTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_BusinessReferrerId",
                table: "AppOceanImportMbls",
                column: "BusinessReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CancelById",
                table: "AppOceanImportMbls",
                column: "CancelById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CareOfId",
                table: "AppOceanImportMbls",
                column: "CareOfId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CargoTypeId",
                table: "AppOceanImportMbls",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CfsLocationId",
                table: "AppOceanImportMbls",
                column: "CfsLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CoLoaderId",
                table: "AppOceanImportMbls",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ColorRemarkId",
                table: "AppOceanImportMbls",
                column: "ColorRemarkId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_CyLocationId",
                table: "AppOceanImportMbls",
                column: "CyLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_DelId",
                table: "AppOceanImportMbls",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_DeliveryToId",
                table: "AppOceanImportMbls",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_EmptyPickupId",
                table: "AppOceanImportMbls",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_FdestId",
                table: "AppOceanImportMbls",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ForwardingAgentId",
                table: "AppOceanImportMbls",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_FreightTermId",
                table: "AppOceanImportMbls",
                column: "FreightTermId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblBillToId",
                table: "AppOceanImportMbls",
                column: "MblBillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblCarrierId",
                table: "AppOceanImportMbls",
                column: "MblCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblConsigneeId",
                table: "AppOceanImportMbls",
                column: "MblConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblCustomerId",
                table: "AppOceanImportMbls",
                column: "MblCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblNotifyId",
                table: "AppOceanImportMbls",
                column: "MblNotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblOperatorId",
                table: "AppOceanImportMbls",
                column: "MblOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblOverseaAgentId",
                table: "AppOceanImportMbls",
                column: "MblOverseaAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblReferralById",
                table: "AppOceanImportMbls",
                column: "MblReferralById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblSaleId",
                table: "AppOceanImportMbls",
                column: "MblSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblSalesTypeId",
                table: "AppOceanImportMbls",
                column: "MblSalesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_MblShipperId",
                table: "AppOceanImportMbls",
                column: "MblShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_OblTypeId",
                table: "AppOceanImportMbls",
                column: "OblTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_OfficeId",
                table: "AppOceanImportMbls",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PackageCategoryId",
                table: "AppOceanImportMbls",
                column: "PackageCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PackageMeasureId",
                table: "AppOceanImportMbls",
                column: "PackageMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PackageWeightId",
                table: "AppOceanImportMbls",
                column: "PackageWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PodId",
                table: "AppOceanImportMbls",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PolId",
                table: "AppOceanImportMbls",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PorId",
                table: "AppOceanImportMbls",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_PreCarriageVesselNameId",
                table: "AppOceanImportMbls",
                column: "PreCarriageVesselNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ReleaseById",
                table: "AppOceanImportMbls",
                column: "ReleaseById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ReturnLocationId",
                table: "AppOceanImportMbls",
                column: "ReturnLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ShipModeId",
                table: "AppOceanImportMbls",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_ShippingAgentId",
                table: "AppOceanImportMbls",
                column: "ShippingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_SvcTermFromId",
                table: "AppOceanImportMbls",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_SvcTermToId",
                table: "AppOceanImportMbls",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportMbls_TransPort1Id",
                table: "AppOceanImportMbls",
                column: "TransPort1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppPackageUnits_AmsNoId",
                table: "AppPackageUnits",
                column: "AmsNoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPackageUnits_EManifestId",
                table: "AppPackageUnits",
                column: "EManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPackageUnits_PackageCode",
                table: "AppPackageUnits",
                column: "PackageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppPayment_OfficeId",
                table: "AppPayment",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPayment_PaidTo",
                table: "AppPayment",
                column: "PaidTo");

            migrationBuilder.CreateIndex(
                name: "IX_AppPorts_CountryId",
                table: "AppPorts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradeParties_TradePartnerId",
                table: "AppTradeParties",
                column: "TradePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightAP_FreightCode",
                table: "AppTradePartnerDefaultFreightAP",
                column: "FreightCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightAP_TradePartnerId",
                table: "AppTradePartnerDefaultFreightAP",
                column: "TradePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightAR_FreightCode",
                table: "AppTradePartnerDefaultFreightAR",
                column: "FreightCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightAR_TradePartnerId",
                table: "AppTradePartnerDefaultFreightAR",
                column: "TradePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightDC_FreightCode",
                table: "AppTradePartnerDefaultFreightDC",
                column: "FreightCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerDefaultFreightDC_TradePartnerId",
                table: "AppTradePartnerDefaultFreightDC",
                column: "TradePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTradePartnerMemo_TradePartnerId",
                table: "AppTradePartnerMemo",
                column: "TradePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_BlAcctCarrierId",
                table: "AppVesselSchedules",
                column: "BlAcctCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_BlTypeId",
                table: "AppVesselSchedules",
                column: "BlTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_CoLoaderId",
                table: "AppVesselSchedules",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_DelId",
                table: "AppVesselSchedules",
                column: "DelId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_DeliveryToId",
                table: "AppVesselSchedules",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_EmptyPickupId",
                table: "AppVesselSchedules",
                column: "EmptyPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_FdestId",
                table: "AppVesselSchedules",
                column: "FdestId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_ForwardingAgentId",
                table: "AppVesselSchedules",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_FreightTermId",
                table: "AppVesselSchedules",
                column: "FreightTermId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_MblCarrierId",
                table: "AppVesselSchedules",
                column: "MblCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_MblNotifyId",
                table: "AppVesselSchedules",
                column: "MblNotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_MblOverseaAgentId",
                table: "AppVesselSchedules",
                column: "MblOverseaAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_OblTypeId",
                table: "AppVesselSchedules",
                column: "OblTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_OfficeId",
                table: "AppVesselSchedules",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_PodId",
                table: "AppVesselSchedules",
                column: "PodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_PolId",
                table: "AppVesselSchedules",
                column: "PolId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_PorId",
                table: "AppVesselSchedules",
                column: "PorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_ShipModeId",
                table: "AppVesselSchedules",
                column: "ShipModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_ShippingAgentId",
                table: "AppVesselSchedules",
                column: "ShippingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_SvcTermFromId",
                table: "AppVesselSchedules",
                column: "SvcTermFromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_SvcTermToId",
                table: "AppVesselSchedules",
                column: "SvcTermToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVesselSchedules_TransPort1Id",
                table: "AppVesselSchedules",
                column: "TransPort1Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppWarehouseReceipts_DimensionUnitId",
                table: "AppWarehouseReceipts",
                column: "DimensionUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AppWarehouseReceipts_PackageUnitId",
                table: "AppWarehouseReceipts",
                column: "PackageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_DeviceCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_Expiration",
                table: "IdentityServerDeviceFlowCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_Expiration",
                table: "IdentityServerPersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogActions");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpClaimTypes");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpFeatureValues");

            migrationBuilder.DropTable(
                name: "AbpLinkUsers");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSecurityLogs");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpTenantConnectionStrings");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AppAccountGroups");

            migrationBuilder.DropTable(
                name: "AppAirExportBookings");

            migrationBuilder.DropTable(
                name: "AppAirExportHawbs");

            migrationBuilder.DropTable(
                name: "AppAirImportHawbs");

            migrationBuilder.DropTable(
                name: "AppAirImportMawbs");

            migrationBuilder.DropTable(
                name: "AppAirOtherCharges");

            migrationBuilder.DropTable(
                name: "AppAttachments");

            migrationBuilder.DropTable(
                name: "AppAwbNoRanges");

            migrationBuilder.DropTable(
                name: "AppCargoManifests");

            migrationBuilder.DropTable(
                name: "AppContactPersonChilds");

            migrationBuilder.DropTable(
                name: "AppContainerDimensions");

            migrationBuilder.DropTable(
                name: "AppContainers");

            migrationBuilder.DropTable(
                name: "AppContainerSizes");

            migrationBuilder.DropTable(
                name: "AppCountryDisplayName");

            migrationBuilder.DropTable(
                name: "AppCreditLimitGroups");

            migrationBuilder.DropTable(
                name: "AppCurrencySetting");

            migrationBuilder.DropTable(
                name: "AppCurrencyTables");

            migrationBuilder.DropTable(
                name: "AppCustomerPayment");

            migrationBuilder.DropTable(
                name: "AppDisplaySettings");

            migrationBuilder.DropTable(
                name: "AppExportBookings");

            migrationBuilder.DropTable(
                name: "AppGridPreference");

            migrationBuilder.DropTable(
                name: "AppInv");

            migrationBuilder.DropTable(
                name: "AppInvoiceBills");

            migrationBuilder.DropTable(
                name: "AppItNoRanges");

            migrationBuilder.DropTable(
                name: "AppLanguages");

            migrationBuilder.DropTable(
                name: "AppMemos");

            migrationBuilder.DropTable(
                name: "AppOceanExportHbls");

            migrationBuilder.DropTable(
                name: "AppOceanImportHbls");

            migrationBuilder.DropTable(
                name: "AppPayment");

            migrationBuilder.DropTable(
                name: "AppPortsManagements");

            migrationBuilder.DropTable(
                name: "AppReportLog");

            migrationBuilder.DropTable(
                name: "AppTradeParties");

            migrationBuilder.DropTable(
                name: "AppTradePartnerAttachments");

            migrationBuilder.DropTable(
                name: "AppTradePartnerDefaultFreightAP");

            migrationBuilder.DropTable(
                name: "AppTradePartnerDefaultFreightAR");

            migrationBuilder.DropTable(
                name: "AppTradePartnerDefaultFreightDC");

            migrationBuilder.DropTable(
                name: "AppTradePartnerMemo");

            migrationBuilder.DropTable(
                name: "AppWarehouseReceipts");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerClientClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "IdentityServerClientGrantTypes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerClientRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerPersistedGrants");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpUsers");

            migrationBuilder.DropTable(
                name: "AppAirExportMawbs");

            migrationBuilder.DropTable(
                name: "AppContactPersons");

            migrationBuilder.DropTable(
                name: "AppOffices");

            migrationBuilder.DropTable(
                name: "AppVesselSchedules");

            migrationBuilder.DropTable(
                name: "AppInvoices");

            migrationBuilder.DropTable(
                name: "AppOceanExportMbls");

            migrationBuilder.DropTable(
                name: "AppOceanImportMbls");

            migrationBuilder.DropTable(
                name: "AppBillingCodes");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResources");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClients");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResources");

            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "AppAirports");

            migrationBuilder.DropTable(
                name: "AppPackageUnits");

            migrationBuilder.DropTable(
                name: "AppPorts");

            migrationBuilder.DropTable(
                name: "AppSubstations");

            migrationBuilder.DropTable(
                name: "AppTradePartners");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "AppGlCodes");

            migrationBuilder.DropTable(
                name: "AppCountries");

            migrationBuilder.DropTable(
                name: "AppSysCodes");

            migrationBuilder.DropTable(
                name: "AppCurrencies");
        }
    }
}
