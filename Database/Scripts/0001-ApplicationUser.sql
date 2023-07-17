CREATE TABLE IF NOT EXISTS `ApplicationUser` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserName` VARCHAR(255) NOT NULL,
    `NormalizedUserName` VARCHAR(255) NOT NULL,
    `Email` VARCHAR(255) NULL,
    `NormalizedEmail` VARCHAR(255) NULL,
    `EmailConfirmed` BOOLEAN NOT NULL DEFAULT FALSE,
    `PasswordHash` TEXT NULL,
    `PhoneNumber` VARCHAR(255) NULL,
    `PhoneNumberConfirmed` BOOLEAN NOT NULL DEFAULT FALSE,
    `TwoFactorEnabled` BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (`Id`),
    UNIQUE (`UserName`),
    UNIQUE (`NormalizedUserName`)
) ENGINE = InnoDB;