CREATE TABLE IF NOT EXISTS `books` (
`id` INT(10) AUTO_INCREMENT PRIMARY KEY,
`author` longtext,
`launch_date` datetime(6) NOT NULL,
`price` decimal(65,2) NOT NULL,
`title` longtext
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;