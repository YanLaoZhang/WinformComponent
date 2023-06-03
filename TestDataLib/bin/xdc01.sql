CREATE TABLE change_mode (
    id              INTEGER       PRIMARY KEY AUTOINCREMENT,
    rn              VARCHAR (30)  NOT NULL,
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
