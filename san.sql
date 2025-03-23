-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 23, 2025 at 03:43 PM
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

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `osoby`
--

CREATE TABLE `osoby` (
  `ID` int(11) NOT NULL,
  `Login` char(255) NOT NULL,
  `Password` char(255) NOT NULL,
  `Email` char(255) NOT NULL
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
  `Dostępność` char(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `wyposażenie`
--

CREATE TABLE `wyposażenie` (
  `ID` int(11) NOT NULL,
  `ID_sale` int(11) NOT NULL,
  `Projektor_HDMI` tinyint(1) NOT NULL,
  `Projektor_VGA` tinyint(1) NOT NULL,
  `Tablica_multimedialna` tinyint(1) NOT NULL,
  `Tablica_suchościeralna` tinyint(1) NOT NULL,
  `Klimatyzacja` tinyint(1) NOT NULL,
  `Komputerowa` tinyint(1) NOT NULL
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
-- Indeksy dla tabeli `wyposażenie`
--
ALTER TABLE `wyposażenie`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `Połączenie_sale_wyposazenie` (`ID_sale`);

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
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `wyposażenie`
--
ALTER TABLE `wyposażenie`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `wyposażenie`
--
ALTER TABLE `wyposażenie`
  ADD CONSTRAINT `Połączenie_sale_wyposazenie` FOREIGN KEY (`ID_sale`) REFERENCES `sale` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
