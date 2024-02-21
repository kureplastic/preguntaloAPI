-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 13-02-2024 a las 17:56:18
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `preguntalo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `administradores`
--

CREATE TABLE `administradores` (
  `id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Nombre` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `consultas`
--

CREATE TABLE `consultas` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `UsuarioId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Titulo` varchar(255) NOT NULL,
  `Texto` varchar(255) NOT NULL,
  `Resuelto` tinyint(1) NOT NULL,
  `RespuestaSeleccionada` int(8) UNSIGNED ZEROFILL NOT NULL,
  `PuntuacionPositiva` int(255) NOT NULL,
  `PuntuacionNegativa` int(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `documentosvalidaciones`
--

CREATE TABLE `documentosvalidaciones` (
  `id` int(11) NOT NULL,
  `ValidacionId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `RutaDocumento` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ratings`
--

CREATE TABLE `ratings` (
  `id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `PuntuacionPositiva` int(255) NOT NULL,
  `PuntuacionNegativa` int(255) NOT NULL,
  `RespuestasElegidas` int(255) NOT NULL,
  `RespuestasContestadas` int(255) NOT NULL,
  `ConsultasRealizadas` int(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `relacionesconsultas-categorias`
--

CREATE TABLE `relacionesconsultas-categorias` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `CategoriaId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `ConsultaId` int(10) UNSIGNED ZEROFILL NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `respuestas`
--

CREATE TABLE `respuestas` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `UsuarioId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Texto` varchar(255) NOT NULL,
  `PuntuacionPositiva` int(255) NOT NULL,
  `PuntuacionNegativa` int(255) NOT NULL,
  `ConsultaId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `RespuestaId` int(8) UNSIGNED ZEROFILL NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `RatingId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `FotoPerfil` varchar(255) DEFAULT NULL,
  `ValidacionId` int(8) UNSIGNED ZEROFILL NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `validaciones`
--

CREATE TABLE `validaciones` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `Titulo` varchar(255) NOT NULL,
  `EntidadOtorgante` varchar(255) NOT NULL,
  `Confirmada` tinyint(1) NOT NULL,
  `Descripcion` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `administradores`
--
ALTER TABLE `administradores`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `consultas`
--
ALTER TABLE `consultas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UsuarioId` (`UsuarioId`),
  ADD KEY `RespuestaSeleccionada` (`RespuestaSeleccionada`);

--
-- Indices de la tabla `documentosvalidaciones`
--
ALTER TABLE `documentosvalidaciones`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ValidacionId` (`ValidacionId`);

--
-- Indices de la tabla `ratings`
--
ALTER TABLE `ratings`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `relacionesconsultas-categorias`
--
ALTER TABLE `relacionesconsultas-categorias`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ConsultaId` (`ConsultaId`),
  ADD KEY `CategoriaId` (`CategoriaId`);

--
-- Indices de la tabla `respuestas`
--
ALTER TABLE `respuestas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `respuestas_ibfk_1` (`RespuestaId`),
  ADD KEY `ConsultaId` (`ConsultaId`),
  ADD KEY `UsuarioId` (`UsuarioId`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ValidacionId` (`ValidacionId`),
  ADD KEY `RatingId` (`RatingId`) USING BTREE;

--
-- Indices de la tabla `validaciones`
--
ALTER TABLE `validaciones`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `administradores`
--
ALTER TABLE `administradores`
  MODIFY `id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `consultas`
--
ALTER TABLE `consultas`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `documentosvalidaciones`
--
ALTER TABLE `documentosvalidaciones`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `ratings`
--
ALTER TABLE `ratings`
  MODIFY `id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `relacionesconsultas-categorias`
--
ALTER TABLE `relacionesconsultas-categorias`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `respuestas`
--
ALTER TABLE `respuestas`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `validaciones`
--
ALTER TABLE `validaciones`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `consultas`
--
ALTER TABLE `consultas`
  ADD CONSTRAINT `consultas_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `consultas_ibfk_2` FOREIGN KEY (`RespuestaSeleccionada`) REFERENCES `respuestas` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `documentosvalidaciones`
--
ALTER TABLE `documentosvalidaciones`
  ADD CONSTRAINT `documentosvalidaciones_ibfk_1` FOREIGN KEY (`ValidacionId`) REFERENCES `validaciones` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `ratings`
--
ALTER TABLE `ratings`
  ADD CONSTRAINT `ratings_ibfk_1` FOREIGN KEY (`id`) REFERENCES `usuarios` (`RatingId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `relacionesconsultas-categorias`
--
ALTER TABLE `relacionesconsultas-categorias`
  ADD CONSTRAINT `relacionesconsultas-categorias_ibfk_1` FOREIGN KEY (`ConsultaId`) REFERENCES `consultas` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `relacionesconsultas-categorias_ibfk_2` FOREIGN KEY (`CategoriaId`) REFERENCES `categorias` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `respuestas`
--
ALTER TABLE `respuestas`
  ADD CONSTRAINT `respuestas_ibfk_1` FOREIGN KEY (`RespuestaId`) REFERENCES `respuestas` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `respuestas_ibfk_2` FOREIGN KEY (`ConsultaId`) REFERENCES `consultas` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `respuestas_ibfk_3` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`ValidacionId`) REFERENCES `validaciones` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
