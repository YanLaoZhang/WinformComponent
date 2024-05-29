# 创建数据库
CREATE DATABASE IF NOT EXISTS `erp_info`;

USE erp_info;
# 创建开发用户
CREATE USER IF NOT EXISTS 'rckerp'@'%' IDENTIFIED BY '2023erp';
GRANT SELECT, INSERT, UPDATE, DELETE ON erp_info.* TO 'rckerp'@'%';
FLUSH PRIVILEGES;

# 创建查询用户
CREATE USER IF NOT EXISTS 'erpread'@'%' IDENTIFIED BY '2024readerp';
GRANT SELECT ON erp_info.* TO 'erpread'@'%';
FLUSH PRIVILEGES;

CREATE TABLE IF NOT EXISTS `system_order_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL COMMENT '订单创建时间',
  `order_number` varchar(255) DEFAULT '' COMMENT '系统订单号',
  `purchase_order` varchar(255) DEFAULT '' COMMENT '客户订单',
  `customer_id` varchar(255) DEFAULT '' COMMENT '客户ID',
  `material_name` varchar(255) DEFAULT '' COMMENT '物料名称',
  `qty` int(11) DEFAULT NULL COMMENT '数量',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29792 DEFAULT CHARSET=utf8mb4 COMMENT='系统订单';

CREATE TABLE IF NOT EXISTS `customer_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `customer_id` varchar(255) DEFAULT '' COMMENT '客户ID',
  `customer_number` varchar(255) DEFAULT '' COMMENT '客户编号',
  `customer_name` varchar(255) DEFAULT '' COMMENT '客户名称',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2857 DEFAULT CHARSET=utf8mb4 COMMENT='客户列表';

CREATE TABLE IF NOT EXISTS `manufacturing_order_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order_number` varchar(255) DEFAULT '' COMMENT '工单号',
  `create_date` date NOT NULL COMMENT '工单创建时间',
  `plan_start_date` date DEFAULT NULL COMMENT '计划开始时间',
  `plan_finish_date` date DEFAULT NULL COMMENT '计划完成时间',
  `start_date` date DEFAULT NULL COMMENT '开始时间',
  `finish_date` date DEFAULT NULL COMMENT '完成时间',
  `close_date` date DEFAULT NULL COMMENT '完工时间',
  `system_order` varchar(255) DEFAULT '' COMMENT '系统订单',
  `purchase_order` varchar(255) DEFAULT '' COMMENT '客户订单',
  `sale_outstock_order` varchar(255) DEFAULT '' COMMENT '出库销售单',
  `receive_order` varchar(255) DEFAULT '' COMMENT '应收单',
  `invoice_number` varchar(255) DEFAULT '' COMMENT '发票号',
  `customer_id` varchar(255) DEFAULT '',
  `customer_name` varchar(255) DEFAULT '',
  `closed` varchar(255) DEFAULT '' COMMENT '是否完成',
  `material` varchar(255) DEFAULT '' COMMENT '物料',
  `quantity` int(11) DEFAULT NULL COMMENT '数量',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=40020 DEFAULT CHARSET=utf8mb4 COMMENT='生产订单（工单）';

CREATE TABLE IF NOT EXISTS `material_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL COMMENT '物料创建时间',
  `material_number` varchar(255) DEFAULT '' COMMENT '料号',
  `name` varchar(255) DEFAULT '' COMMENT '物料名称',
  `specification` varchar(255) DEFAULT '' COMMENT '物料规格',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13875 DEFAULT CHARSET=utf8mb4 COMMENT='物料列表';

CREATE TABLE IF NOT EXISTS `receive_order_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL COMMENT '创建时间',
  `order_number` varchar(255) DEFAULT '' COMMENT '应收单号',
  `sale_outstock_order` varchar(255) DEFAULT '' COMMENT '出库销售单',
  `system_order` varchar(255) DEFAULT '' COMMENT '系统订单',
  `invoice_number` varchar(255) DEFAULT '' COMMENT '发票号',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=112151 DEFAULT CHARSET=utf8mb4 COMMENT='应收单';

CREATE TABLE IF NOT EXISTS `sale_outstock_order_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL COMMENT '创建时间',
  `order_number` varchar(255) DEFAULT '' COMMENT '出库销售单号',
  `purchase_order` varchar(255) DEFAULT '' COMMENT '客户订单',
  `system_order` varchar(255) DEFAULT '' COMMENT '系统订单',
  `material` varchar(255) DEFAULT '',
  `customer_id` varchar(255) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=42122 DEFAULT CHARSET=utf8mb4 COMMENT='出库销售单';

CREATE TABLE IF NOT EXISTS `subcontract_order_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order_number` varchar(255) DEFAULT '' COMMENT '委外订单号',
  `create_date` date NOT NULL COMMENT '委外订单创建时间',
  `plan_start_date` date DEFAULT NULL COMMENT '计划开始时间',
  `plan_finish_date` date DEFAULT NULL COMMENT '计划完成时间',
  `convey_date` date DEFAULT NULL COMMENT '开始时间',
  `finish_date` date DEFAULT NULL COMMENT '完成时间',
  `close_date` date DEFAULT NULL COMMENT '完工时间',
  `plan_order` varchar(255) DEFAULT '' COMMENT '计划订单',
  `system_order` varchar(255) DEFAULT '' COMMENT '系统订单',
  `purchase_order` varchar(255) DEFAULT '' COMMENT '客户订单',
  `sale_outstock_order` varchar(255) DEFAULT '' COMMENT '出库销售单',
  `receive_order` varchar(255) DEFAULT '' COMMENT '应收单',
  `invoice_number` varchar(255) DEFAULT '' COMMENT '发票号',
  `closed` varchar(255) DEFAULT '' COMMENT '是否完成',
  `material` varchar(255) DEFAULT '' COMMENT '物料',
  `quantity` int(11) DEFAULT NULL COMMENT '数量',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=97147 DEFAULT CHARSET=utf8mb4 COMMENT='委外订单（工单）';