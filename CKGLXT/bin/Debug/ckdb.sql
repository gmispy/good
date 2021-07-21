/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50096
Source Host           : localhost:3306
Source Database       : ckdb

Target Server Type    : MYSQL
Target Server Version : 50096
File Encoding         : 65001

Date: 2021-07-19 20:32:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for huwubiao
-- ----------------------------
DROP TABLE IF EXISTS `huwubiao`;
CREATE TABLE `huwubiao` (
  `HWName` varchar(50) character set gbk default NULL,
  `HWType` varchar(30) character set gbk default NULL,
  `HWZhuanTai` int(11) default NULL,
  `HWWeiZhi` varchar(100) character set gbk default NULL,
  `HWTime` datetime default NULL,
  `HWKYCount` int(11) default NULL,
  `HWJieChuCount` int(11) default NULL,
  `HWWHCount` int(11) default NULL,
  `HWBFCount` int(11) default NULL,
  `HWBYYCount` int(11) default NULL,
  `HWCount` int(11) default NULL,
  `HWDanHao` varchar(50) NOT NULL,
  PRIMARY KEY  (`HWDanHao`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of huwubiao
-- ----------------------------
INSERT INTO `huwubiao` VALUES ('货物A', '其他类', '1', 'XB3', '2021-07-18 22:52:53', '1744', '6', '100', '150', '0', '2000', 'bianhao1');

-- ----------------------------
-- Table structure for jiechubiao
-- ----------------------------
DROP TABLE IF EXISTS `jiechubiao`;
CREATE TABLE `jiechubiao` (
  `JCID` int(11) NOT NULL auto_increment,
  `JCHWID` int(11) default NULL,
  `JCRen` varchar(30) character set gbk default NULL,
  `JCTime` datetime default NULL,
  `JCState` int(11) default NULL,
  `JCGHTime` datetime default NULL,
  `JCCount` int(11) default NULL,
  `JCGHCount` int(11) default NULL,
  `JCBuMen` varchar(30) character set gbk default NULL,
  `JCGongHao` varchar(30) character set gbk default NULL,
  `HWDanHaoJ` varchar(30) default NULL,
  PRIMARY KEY  (`JCID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of jiechubiao
-- ----------------------------
INSERT INTO `jiechubiao` VALUES ('4', '0', 'zaizai', '2021-07-19 19:45:58', '2', '2021-07-19 19:47:59', '0', null, '研发部', '123456', 'bianhao1');
INSERT INTO `jiechubiao` VALUES ('5', '0', 'sss', '2021-07-19 20:13:58', '1', null, '5', null, 'ss', '123456', 'bianhao1');
INSERT INTO `jiechubiao` VALUES ('6', '0', '11', '2021-07-19 20:15:20', '1', null, '1', null, '2221', '234567', 'bianhao1');

-- ----------------------------
-- Table structure for yuangongbiao
-- ----------------------------
DROP TABLE IF EXISTS `yuangongbiao`;
CREATE TABLE `yuangongbiao` (
  `YGID` int(11) NOT NULL auto_increment,
  `YGDLM` varchar(30) character set gbk default NULL,
  `YGMM` varchar(30) character set gbk default NULL,
  `YGDXM` varchar(30) character set gbk default NULL,
  `YGIsUse` int(11) default NULL,
  `YGIsZhiWei` int(11) default '1',
  PRIMARY KEY  (`YGID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of yuangongbiao
-- ----------------------------
INSERT INTO `yuangongbiao` VALUES ('1', 'admin', 'admin123', '管理员', '1', '2');
INSERT INTO `yuangongbiao` VALUES ('2', 'putong', 'putong123', '一般员工', '1', '1');

-- ----------------------------
-- Table structure for yuyuebiao
-- ----------------------------
DROP TABLE IF EXISTS `yuyuebiao`;
CREATE TABLE `yuyuebiao` (
  `YuID` int(11) NOT NULL auto_increment,
  `YuHWID` int(11) default NULL,
  `YuRen` varchar(30) character set gbk default NULL,
  `YuTime` datetime default NULL,
  `YuCount` int(255) default NULL,
  `YuBuMen` varchar(30) character set gbk default NULL,
  `YuGongHao` varchar(30) character set gbk default NULL,
  `HWDanHaoY` varchar(30) default NULL,
  PRIMARY KEY  (`YuID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of yuyuebiao
-- ----------------------------
