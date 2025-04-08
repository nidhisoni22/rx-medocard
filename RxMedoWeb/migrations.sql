CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Memberships` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FullName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Phone` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Address` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `DateOfBirth` datetime(6) NOT NULL,
    `MembershipType` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `ApplicationDate` datetime(6) NOT NULL,
    `IsApproved` tinyint(1) NOT NULL,
    `MembershipNumber` varchar(20) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Memberships` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250407123941_InitialCreate', '8.0.13');

COMMIT;

START TRANSACTION;

CREATE TABLE `DiagnosticBookings` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PatientName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `PatientAge` int NOT NULL,
    `PatientGender` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `TestType` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `PreferredDate` datetime(6) NOT NULL,
    `ContactNumber` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `BookingDate` datetime(6) NOT NULL,
    `IsConfirmed` tinyint(1) NOT NULL,
    CONSTRAINT `PK_DiagnosticBookings` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250407212410_AddDiagnosticBookings', '8.0.13');

COMMIT;

