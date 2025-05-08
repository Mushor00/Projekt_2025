-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Maj 06, 2025 at 09:32 PM
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
CREATE DATABASE IF NOT EXISTS `san` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
--
CREATE USER if not EXISTS `oskar`@`localhost`;
--
GRANT ALL PRIVILEGES ON `san`.* TO 'oskar'@'localhost' IDENTIFIED BY '1234';
-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `osoby`
--
USE `san`;
CREATE TABLE `osoby` (
  `ID` int(11) NOT NULL,
  `Login` char(255) NOT NULL,
  `Password` char(255) NOT NULL,
  `Email` char(255) NOT NULL,
  `Imie` char(50) NOT NULL,
  `Nazwisko` char(50) NOT NULL,
  `NrAlbumu` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

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
  `ProjektorHDMI` int(1) NOT NULL,
  `ProjektorVGA` int(1) NOT NULL,
  `TablicaMultimedialna` int(1) NOT NULL,
  `TablicaSuchoscieralna` int(1) NOT NULL,
  `Klimatyzacja` int(1) NOT NULL,
  `Komputerowa` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;




CREATE TABLE `sale_dostepnosc` (
  `ID` INT NOT NULL AUTO_INCREMENT , 
  `ID_sale` INT NOT NULL , 
  `ID_osoby` INT NOT NULL , 
  `Data` DATE NOT NULL , 
  `Godzina_rozpoczecia` TIME NOT NULL , 
  `Godzina_zakonczenia` TIME NOT NULL , 
  PRIMARY KEY (`ID`),
    FOREIGN KEY (`ID_sale`) REFERENCES sale(ID),
    FOREIGN KEY (`ID_osoby`) REFERENCES osoby(ID)
  ) ENGINE = InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
--
-- Dumping data for table `sale`
--

INSERT INTO `sale` (`ID`, `Numer`, `Budynek`, `Nazwa`, `Piętro`, `Pojemność`, `Dostępność`, `ProjektorHDMI`, `ProjektorVGA`, `TablicaMultimedialna`, `TablicaSuchoscieralna`, `Klimatyzacja`, `Komputerowa`) VALUES
(2, 0, '[v', '[valu', 0, 0, '[value-7]', 0, 0, 0, 0, 0, 0);

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
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `osoby`
--
ALTER TABLE `osoby`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sale`
--
ALTER TABLE `sale`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
