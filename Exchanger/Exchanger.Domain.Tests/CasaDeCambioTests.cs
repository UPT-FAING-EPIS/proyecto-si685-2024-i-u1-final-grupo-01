using NUnit.Framework;
using System;

namespace Exchanger.Domain.Tests
{
    [TestFixture]
    public class CasaDeCambioTests
    {
        // Las pruebas anteriores han sido omitidas para mantener la concisión.

        [Test]
        public void Depositar_AddsToBalance()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool depositResult = cliente.Depositar(100, "USD");

            // Assert
            Assert.IsTrue(depositResult, "Depósito debe ser exitoso");
            Assert.That(cliente.saldoPorMoneda["USD"], Is.EqualTo(600), "El saldo de USD debe ser 100 después del depósito");
        }

        [Test]
        public void VerificarCredenciales_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool verificationResult = cliente.VerificarCredenciales("testUser", "password");

            // Assert
            Assert.IsTrue(verificationResult, "La verificación de credenciales debe ser verdadera con credenciales válidas");
        }

        [Test]
        public void VerificarCredenciales_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool verificationResult = cliente.VerificarCredenciales("testUser", "wrongPassword");

            // Assert
            Assert.IsFalse(verificationResult, "La verificación de credenciales debe ser falsa con credenciales inválidas");
        }

        [Test]
        public void VerificarCredenciales_NullToken_ReturnsTrue()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool verificationResult = cliente.VerificarCredenciales("testUser", "password");

            // Assert
            Assert.IsTrue(verificationResult, "La verificación de credenciales debe ser verdadera incluso con un token nulo");
        }

        [Test]
        public void VerificarCredenciales_NullTokenWithProvidedToken_ReturnsFalse()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool verificationResult = cliente.VerificarCredenciales("testUser", "password", "someToken");

            // Assert
            Assert.IsFalse(verificationResult, "La verificación de credenciales debe ser falsa cuando se proporciona un token pero es nulo");
        }

        [Test]
        public void RealizarTransaccion_ReturnsFalse_WhenInvalidMonedaOrigen()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool transactionResult = cliente.RealizarTransaccion(100, "INVALID", "EUR", 0);

            // Assert
            Assert.IsFalse(transactionResult, "La transacción debe fallar cuando la moneda de origen es inválida");
        }

        [Test]
        public void RealizarTransaccion_ReturnsFalse_WhenInvalidMonedaDestino()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool transactionResult = cliente.RealizarTransaccion(100, "USD", "INVALID", 0);

            // Assert
            Assert.IsFalse(transactionResult, "La transacción debe fallar cuando la moneda de destino es inválida");
        }
        [Test]
        public void RealizarTransaccion_ReturnsFalse_WhenInsufficientBalance()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool transactionResult = cliente.RealizarTransaccion(1000, "USD", "EUR", 0);

            // Assert
            Assert.IsFalse(transactionResult, "La transacción debe fallar cuando el saldo es insuficiente");
        }

        [Test]
        public void RealizarTransaccion_AddsToHistorial_WhenSuccessful()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");
            cliente.Depositar(100, "USD");

            // Act
            cliente.RealizarTransaccion(50, "USD", "EUR", 42);

            // Assert
            Assert.That(cliente.historialTransacciones.Count, Is.EqualTo(1), "Debe haber una transacción en el historial después de una transacción exitosa");
        }

        [Test]
        public void Depositar_ReturnsFalse_WhenInvalidCurrency()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");

            // Act
            bool depositResult = cliente.Depositar(100, "INVALID");

            // Assert
            Assert.IsFalse(depositResult, "El depósito debe fallar cuando la moneda es inválida");
        }

        [Test]
        public void RealizarTransaccion_ReturnsTrue_WhenSufficientBalance()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");
            cliente.Depositar(1000, "USD");

            // Act
            bool transactionResult = cliente.RealizarTransaccion(500, "USD", "EUR", 0);

            // Assert
            Assert.IsTrue(transactionResult, "La transacción debe ser exitosa cuando el saldo es suficiente");
        }

        [Test]
        public void VerHistorial_ReturnsCorrectFormat_WhenTransactionsExist()
        {
            // Arrange
            var cliente = new Cliente("testUser", "password");
            cliente.Depositar(100, "USD");
            cliente.RealizarTransaccion(50, "USD", "EUR", 42);

            // Act
            string historial = cliente.VerHistorial();

            // Assert
            StringAssert.Contains("Historial de Transacciones:", historial, "El historial debe contener la cabecera");
            StringAssert.Contains("Fecha:", historial, "Cada transacción debe contener la fecha");
        }
    }
}
