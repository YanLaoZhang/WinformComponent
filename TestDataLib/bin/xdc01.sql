CREATE TABLE change_mode (
    id              INTEGER       PRIMARY KEY AUTOINCREMENT,
    sn              VARCHAR (30)  NOT NULL,
    firmware        VARCHAR (16)  DEFAULT (''),
    start_test_time DATETIME      DEFAULT (datetime('now', 'localtime') ) 
                                  NOT NULL,
    end_test_time   DATETIME      DEFAULT (datetime('now', 'localtime') ) 
                                  NOT NULL,
    test_result     VARCHAR (30)  DEFAULT ('') 
                                  NOT NULL,
    wifi_mode       VARCHAR (16)  DEFAULT (''),
    test_file       VARCHAR (16)  DEFAULT ('') 
);

INSERT INTO change_mode (rn, firmware, start_test_time, end_test_time, test_result, wifi_mode, test_file)
VALUES ('rn', 'firmware', 'start_test_time', 'end_test_time', 'test_result', 'wifi_mode', 'test_file');


CREATE TABLE change_mode (
	`id` INT (11) NOT NULL AUTO_INCREMENT,
	`sn` VARCHAR (30) NOT NULL,
	`firmware` VARCHAR (30) DEFAULT '' COMMENT 'FW版本',
	`start_test_time` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`end_test_time` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	`test_result` VARCHAR (30) NOT NULL COMMENT '测试结果',
	`wifi_mode` VARCHAR (30) NOT NULL,
	`test_file` VARCHAR (30) NOT NULL,
	PRIMARY KEY (`id`)
) ENGINE = INNODB DEFAULT CHARSET = utf8;

CREATE USER 'rckxdc01'@'%' IDENTIFIED BY '2022xdc01';
GRANT SELECT, INSERT ON xdc01_management.* TO 'rckxdc01'@'%';