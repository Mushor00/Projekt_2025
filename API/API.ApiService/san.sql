-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Maj 10, 2025 at 03:34 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `san`
--
CREATE DATABASE IF NOT EXISTS `san` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
--
CREATE USER if not EXISTS `oskar`@'localhost' IDENTIFIED BY '1234';
--
GRANT ALL PRIVILEGES ON `san`.* TO 'oskar'@'localhost';
-- --------------------------------------------------------
USE `san`;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `osoby`
--

CREATE TABLE `osoby` (
  `ID` int(11) NOT NULL,
  `Login` char(255) NOT NULL,
  `Password` char(255) NOT NULL,
  `Email` char(255) NOT NULL,
  `Imie` char(50) NOT NULL,
  `Nazwisko` char(50) NOT NULL,
  `NrAlbumu` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;


--
-- Struktura tabeli dla tabeli `sale`
--

CREATE TABLE `sale` (
  `ID` int(11) NOT NULL,
  `Numer` int(11) NOT NULL,
  `Budynek` char(2) NOT NULL,
  `Nazwa` char(5) NOT NULL,
  `Piętro` tinyint(4) NOT NULL,
  `Pojemność` int(11) NOT NULL,
  `Dostępność` char(30) NOT NULL,
  `Projektor` int(5) NOT NULL,
  `Tablica` int(5) NOT NULL,
  `Klimatyzacja` tinyint(1) NOT NULL,
  `Komputerowa` tinyint(1) NOT NULL,
  `Ulica` char(255) NOT NULL,
  `Dla_niepelnosprawnych` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `sale`
--

INSERT INTO `sale` (`ID`, `Numer`, `Budynek`, `Nazwa`, `Piętro`, `Pojemność`, `Dostępność`, `Projektor`, `Tablica`, `Klimatyzacja`, `Komputerowa`, `Ulica`, `Dla_niepelnosprawnych`) VALUES
(1, 1, 'K', 'K21', 2, 15, 'wolna', 5, 2, 1, 1, 'Kilińskiego', 1),
(2, 2, 'K', 'K22', 2, 15, 'wolna', 4, 1, 1, 1, 'Kilińskiego', 1),
(3, 3, 'K', 'K23', 2, 15, 'wolna', 1, 2, 1, 1, 'Kilińskiego', 1),
(4, 1, 'K', 'K31', 3, 15, 'zajęta', 5, 3, 1, 1, 'Kilińskiego', 1),
(5, 2, 'K', 'K32', 3, 15, 'w konserwacji', 2, 2, 1, 1, 'Kilińskiego', 1),
(6, 3, 'K', 'K33', 3, 15, 'wolna', 4, 1, 1, 1, 'Kilińskiego', 1),
(7, 1, 'K', 'K41', 4, 15, 'wolna', 2, 4, 1, 1, 'Kilińskiego', 1),
(8, 2, 'K', 'K42', 4, 15, 'wolna', 7, 5, 1, 1, 'Kilińskiego', 1),
(9, 3, 'K', 'K43', 4, 15, 'wolna', 6, 3, 1, 1, 'Kilińskiego', 1),
(10, 1, 'P', 'P11', 1, 30, 'wolna', 6, 2, 1, 0, 'Kilińskiego', 1),
(11, 2, 'P', 'P12', 1, 30, 'wolna', 5, 1, 1, 0, 'Kilińskiego', 1),
(12, 3, 'P', 'P13', 1, 30, 'wolna', 3, 2, 1, 0, 'Kilińskiego', 1),
(13, 1, 'P', 'P21', 2, 30, 'wolna', 2, 2, 1, 0, 'Kilińskiego', 1),
(14, 2, 'P', 'P22', 2, 30, 'wolna', 1, 2, 1, 0, 'Kilińskiego', 1),
(15, 3, 'P', 'P23', 2, 30, 'wolna', 7, 1, 1, 0, 'Kilińskiego', 1),
(16, 1, 'P', 'P31', 3, 30, 'wolna', 5, 1, 1, 0, 'Kilińskiego', 1),
(17, 2, 'P', 'P32', 3, 30, 'wolna', 6, 2, 1, 0, 'Kilińskiego', 1),
(18, 3, 'P', 'P33', 3, 30, 'wolna', 4, 2, 1, 0, 'Kilińskiego', 1),
(19, 1, 'P', 'K41', 4, 30, 'wolna', 3, 2, 1, 0, 'Kilińskiego', 1),
(20, 2, 'P', 'K42', 4, 30, 'wolna', 5, 3, 1, 0, 'Kilińskiego', 1),
(21, 3, 'P', 'K43', 4, 30, 'wolna', 6, 2, 1, 0, 'Kilińskiego', 1),
(22, 1, 'P', 'A1', 1, 30, 'wolna', 7, 1, 1, 0, 'Kilińskiego', 1),
(23, 2, 'P', 'A2', 2, 45, 'wolna', 6, 3, 1, 0, 'Kilińskiego', 1),
(24, 3, 'K', 'A3', 2, 100, 'wolna', 5, 7, 1, 0, 'Kilińskiego', 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `sale_dostepnosc`
--

CREATE TABLE `sale_dostepnosc` (
  `ID` int(11) NOT NULL,
  `ID_sale` int(11) NOT NULL,
  `ID_osoby` int(11) NOT NULL,
  `Data` date NOT NULL,
  `Godzina_rozpoczecia` time NOT NULL,
  `Godzina_zakonczenia` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `osoby`
--
ALTER TABLE `osoby`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `sale`
--
ALTER TABLE `sale`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `sale_dostepnosc`
--
ALTER TABLE `sale_dostepnosc`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ID_sale` (`ID_sale`),
  ADD KEY `ID_osoby` (`ID_osoby`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `osoby`
--
ALTER TABLE `osoby`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `sale`
--
ALTER TABLE `sale`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `sale_dostepnosc`
--
ALTER TABLE `sale_dostepnosc`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `sale_dostepnosc`
--
ALTER TABLE `sale_dostepnosc`
  ADD CONSTRAINT `sale_dostepnosc_ibfk_1` FOREIGN KEY (`ID_sale`) REFERENCES `sale` (`ID`),
  ADD CONSTRAINT `sale_dostepnosc_ibfk_2` FOREIGN KEY (`ID_osoby`) REFERENCES `osoby` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
