CREATE TABLE IF NOT EXISTS `ApplicationRole`
(
	`Id` INT NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(256) NOT NULL,
    `NormalizedName` NVARCHAR(256) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE (`Name`),
    UNIQUE (`NormalizedName`)
);

