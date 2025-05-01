CREATE TABLE IF NOT EXISTS `user` (
`id` bigint(20) NOT NULL AUTO_INCREMENT,
`user_name` varchar(50) NOT NULL,
`password` varchar(130) NOT NULL,
`full_name` varchar(120) NOT NULL,
`refresh_token` varchar(500) NOT NULL,
`refresh_token_expiry_time` DATETIME NULL DEFAULT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY `user_name` (`user_name`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;