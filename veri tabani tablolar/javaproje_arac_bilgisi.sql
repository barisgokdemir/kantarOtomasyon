-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: localhost    Database: javaproje
-- ------------------------------------------------------
-- Server version	8.0.18

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `arac_bilgisi`
--

DROP TABLE IF EXISTS `arac_bilgisi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `arac_bilgisi` (
  `plaka` varchar(45) NOT NULL,
  `sofor` varchar(45) NOT NULL,
  `giris_tarihi` varchar(45) NOT NULL,
  `kayit_eden` varchar(45) NOT NULL,
  `giris_agirligi` varchar(45) NOT NULL,
  `cikis_agirligi` varchar(45) NOT NULL,
  PRIMARY KEY (`plaka`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `arac_bilgisi`
--

LOCK TABLES `arac_bilgisi` WRITE;
/*!40000 ALTER TABLE `arac_bilgisi` DISABLE KEYS */;
INSERT INTO `arac_bilgisi` VALUES ('21421aa','baris','29.12.2019','baris','1500','0'),('33258bnc ','baris','29.12.2019','baris','15820','0'),('33abz45','baris','29.12.2019','baris','1124','0'),('33cnzla','baris','29.12.2019','baris','310003','0'),('35325','baris','29.12.2019','baris','1500','0'),('354asd','baris','29.12.2019','baris','14520','0'),('3asda','baris','29.12.2019','baris','1500','0'),('78asdsad78','baris','29.12.2019','baris','1124241','0');
/*!40000 ALTER TABLE `arac_bilgisi` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-12-29 23:41:56
