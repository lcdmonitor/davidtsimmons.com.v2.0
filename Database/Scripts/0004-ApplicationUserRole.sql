CREATE TABLE IF NOT EXISTS `ApplicationUserRole` (
  `UserId` int NOT NULL,
  `RoleId` int NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  CONSTRAINT `ApplicationUserRole_ApplicationUser_Id` FOREIGN KEY (`UserId`) REFERENCES `ApplicationUser` (`Id`),
  CONSTRAINT `ApplicationUserRole_ApplicationRole_Id` FOREIGN KEY (`RoleId`) REFERENCES `ApplicationRole` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;