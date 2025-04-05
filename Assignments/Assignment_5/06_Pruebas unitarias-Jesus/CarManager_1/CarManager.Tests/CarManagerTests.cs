namespace CarManager.Tests
{
    using NUnit.Framework;
    using System;


    [TestFixture]
    public class CarManagerTests
    {

/********  Debe probar todos los constructores ********/

        [Test]
        // Verifica que el constructor privado sin parametros inicialice FuelAmount a cero.
        public void ConstructorPrivado_SinParametros_InicializaFuelAmountACero()
        {
            // Arrange & Act
            var privateConstructor = typeof(Car).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, Type.EmptyTypes, null);
            Assert.IsNotNull(privateConstructor, "Constructor privado no encontrado");

            var car = (Car)privateConstructor.Invoke(null);

            // Assert
            Assert.AreEqual(0.0, car.FuelAmount);
        }

        [Test]
        // Verifica que el constructor publico con parametros cree un objeto Car valido con los valores proporcionados.
        public void ConstructorPublico_ConParametrosValidos_CreaObjetoCar()
        {
            // Arrange
            string make = "Toyota";
            string model = "Corolla";
            double fuelConsumption = 6.5;
            double fuelCapacity = 50.0;

            // Act
            Car car = new Car(make, model, fuelConsumption, fuelCapacity);

            // Assert
            Assert.IsNotNull(car);
            Assert.AreEqual(make, car.Make);
            Assert.AreEqual(model, car.Model);
            Assert.AreEqual(fuelConsumption, car.FuelConsumption);
            Assert.AreEqual(0.0, car.FuelAmount); // FuelAmount debe inicializarse a 0
            Assert.AreEqual(fuelCapacity, car.FuelCapacity);
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar 'make' como nulo al constructor publico.
        public void ConstructorPublico_ConMakeNulo_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car(null, "Corolla", 6.5, 50.0));
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar 'make' como cadena vacia al constructor publico.
        public void ConstructorPublico_ConMakeVacio_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("", "Corolla", 6.5, 50.0));
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar fuelConsumption como 0 al constructor publico.
        public void ConstructorPublico_ConFuelConsumptionCero_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 0.0, 50.0));
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar fuelConsumption como valor negativo al constructor publico.
        public void ConstructorPublico_ConFuelConsumptionNegativo_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", -6.5, 50.0));
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar fuelCapacity como 0 al constructor publico.
        public void ConstructorPublico_ConFuelCapacityCero_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 6.5, 0.0));
        }

        [Test]
        // Verifica que se lance ArgumentException al proporcionar fuelCapacity como valor negativo al constructor publico.
        public void ConstructorPublico_ConFuelCapacityNegativo_LanzaArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 6.5, -50.0));
        }

/********  Debe probar todas las propiedades (getters y setters). ********/

        [Test]
        // Verifica que el getter de la propiedad Make devuelva el valor correcto despues de la inicializacion.
        public void Getter_Make_DevuelveValorCorrecto()
        {
            // Arrange
            string expectedMake = "Toyota";
            Car car = new Car(expectedMake, "Corolla", 6.5, 50.0);

            // Act
            string actualMake = car.Make;

            // Assert
            Assert.AreEqual(expectedMake, actualMake);
        }

        [Test]
        // Verifica que el getter de la propiedad Model devuelva el valor correcto despues de la inicializacion.
        public void Getter_Model_DevuelveValorCorrecto()
        {
            // Arrange
            string expectedModel = "Corolla";
            Car car = new Car("Toyota", expectedModel, 6.5, 50.0);

            // Act
            string actualModel = car.Model;

            // Assert
            Assert.AreEqual(expectedModel, actualModel);
        }

        [Test]
        // Verifica que el getter de la propiedad FuelConsumption devuelva el valor correcto despues de la inicializacion.
        public void Getter_FuelConsumption_DevuelveValorCorrecto()
        {
            // Arrange
            double expectedFuelConsumption = 6.5;
            Car car = new Car("Toyota", "Corolla", expectedFuelConsumption, 50.0);

            // Act
            double actualFuelConsumption = car.FuelConsumption;

            // Assert
            Assert.AreEqual(expectedFuelConsumption, actualFuelConsumption);
        }

        [Test]
        // Verifica que el getter de la propiedad FuelAmount devuelva el valor correcto despues de la inicializacion.
        public void Getter_FuelAmount_DevuelveValorCorrecto()
        {
            // Arrange
            double expectedFuelAmount = 0.0;
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);

            // Act
            double actualFuelAmount = car.FuelAmount;

            // Assert
            Assert.AreEqual(expectedFuelAmount, actualFuelAmount);
        }

        [Test]
        // Verifica que el getter de la propiedad FuelCapacity devuelva el valor correcto despues de la inicializacion.
        public void Getter_FuelCapacity_DevuelveValorCorrecto()
        {
            // Arrange
            double expectedFuelCapacity = 50.0;
            Car car = new Car("Toyota", "Corolla", 6.5, expectedFuelCapacity);

            // Act
            double actualFuelCapacity = car.FuelCapacity;

            // Assert
            Assert.AreEqual(expectedFuelCapacity, actualFuelCapacity);
        }


/******** Debe probar todos los metodos y validaciones dentro de la clase.  ********/

        [Test]
        // Prueba de refuel: Annadir combustible dentro de la capacidad.
        public void Refuel_AgregarCombustibleDentroCapacidad_Correcto()
        {
            // Arrange
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);
            double initialFuelAmount = car.FuelAmount;

            // Act
            double fuelToAdd = 20.0;
            car.Refuel(fuelToAdd);

            // Assert
            double expectedFuelAmount = initialFuelAmount + fuelToAdd;
            Assert.AreEqual(expectedFuelAmount, car.FuelAmount);
        }

        [Test]
        // Prueba de refuel: Annadir combustible excediendo la capacidad.
        public void Refuel_AgregarCombustibleExcediendoCapacidad_LimitaAlMaximo()
        {
            // Arrange
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);
            car.Refuel(40.0); // Llenar el tanque al 80%

            // Act
            double fuelToAdd = 20.0;
            car.Refuel(fuelToAdd);

            // Assert
            Assert.AreEqual(car.FuelCapacity, car.FuelAmount);
        }

        [Test]
        // Prueba de refuel: Intentar Annadir combustible negativo (debe fallar).
        public void Refuel_AgregarCombustibleNegativo_DebeLanzarExcepcion()
        {
            // Arrange
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);

            // Act y Assert
            Assert.Throws<ArgumentException>(() => car.Refuel(-10.0));
        }

        [Test]
        // Prueba de drive: Conducir sin suficiente combustible (debe fallar).
        public void Drive_ConducirSinSuficienteCombustible_DebeLanzarExcepcion()
        {
            // Arrange
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);

            // Act y Assert
            Assert.Throws<InvalidOperationException>(() => car.Drive(1000.0));
        }

        [Test]
        // Prueba de drive: Conducir con suficiente combustible.
        public void Drive_ConducirConSuficienteCombustible_Correcto()
        {
            // Arrange
            Car car = new Car("Toyota", "Corolla", 6.5, 50.0);
            car.Refuel(30.0); // Llenar el tanque al 60%

            // Act
            double distanceToDrive = 300.0;
            car.Drive(distanceToDrive);

            // Assert
            double expectedFuelAmount = 30.0 - (distanceToDrive / 100.0 * car.FuelConsumption);
            Assert.AreEqual(expectedFuelAmount, car.FuelAmount);
        }

        [Test]
        // Prueba de constructor: Intentar crear un auto con marca nula (debe fallar).
        public void Constructor_CrearAutoConMarcaNula_DebeLanzarExcepcion()
        {
            // Act y Assert
            Assert.Throws<ArgumentException>(() => new Car(null, "Corolla", 6.5, 50.0));
        }

        [Test]
        // Prueba de constructor: Intentar crear un auto con modelo nulo (debe fallar).
        public void Constructor_CrearAutoConModeloNulo_DebeLanzarExcepcion()
        {
            // Act y Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", null, 6.5, 50.0));
        }

        [Test]
        // Prueba de constructor: Intentar crear un auto con consumo de combustible cero (debe fallar).
        public void Constructor_CrearAutoConConsumoCombustibleCero_DebeLanzarExcepcion()
        {
            // Act y Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 0.0, 50.0));
        }

        [Test]
        // Prueba de constructor: Intentar crear un auto con capacidad de combustible cero (debe fallar).
        public void Constructor_CrearAutoConCapacidadCombustibleCero_DebeLanzarExcepcion()
        {
            // Act y Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 6.5, 0.0));
        }


    }
}