namespace Database.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class DatabaseTests
    {

        /********
        La capacidad del arreglo de almacenamiento debe ser exactamente de 16 enteros.
            #Si el tamanno del arreglo no es de 16 enteros, se lanza una excepción InvalidOperationException.
        ********/

        [Test]
        [Category("Constructor")]
        // Verifica que se lance una excepción si el tamaño del arreglo no es de 16 enteros.        
        public void Constructor_VerificaLanzamientoDeExcepcionSiElTamannoDelArregloEsMayorDeDieciseisEnteros()
        {
            // Arrange
            int[] initialData = Enumerable.Range(1, 17).ToArray();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new Database(initialData));
        }

        [Test]
        [Category("Constructor")]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 100 })]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 10, 20, 30, 40 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        // Verifica que el constructor almacene correctamente los datos proporcionados.
        public void Constructor_AlmacenamientoCorrectoDeDatos(int[] testData)
        {
            // Arrange

            // Act
            Database database = new Database(testData);

            // Assert
            Assert.AreEqual(testData.Length, database.Count);
            CollectionAssert.AreEqual(testData, database.Fetch());
        }

        /********
        La operación Add() debe annadir un elemento en la siguiente celda libre (como una pila).
            #Si hay 16 elementos en la Base de Datos y se intenta annadir un 17º, se lanza una excepción InvalidOperationException.
        ********/
        [Test]
        [Category("MedotoAdd")]
        // Verifica que se lance una excepción al intentar agregar más de 16 elementos a la base de datos.
        public void Add_VerificaLanzamientoDeExcepcionAlAgregarMasDeDieciseisElementos()
        {
            // Arrange
            int[] initialData = Enumerable.Range(1, 16).ToArray();
            Database db = new Database(initialData);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => db.Add(17));
        }

        [Test]
        [Category("MetodoAdd")]
        [TestCase(15)]
        [TestCase(10)]
        [TestCase(2)]
        // Verifica que se añada un elemento correctamente cuando hay espacio en la base de datos.
        public void Add_AniadeElementoExitosamente(int element)
        {
            // Arrange
            int[] initialData = Enumerable.Range(1, element).ToArray();
            Database database = new Database(initialData);
            int[] expectedArray = initialData.Append(element).ToArray();

            // Act
            database.Add(element);

            // Assert
            Assert.AreEqual(expectedArray, database.Fetch());
        }



        /********
        • La operación Remove() debe permitir solo la eliminación de un elemento en el último índice (como una pila).
            #Si intentas eliminar un elemento de una Base de Datos vacía, se lanza una excepción InvalidOperationException.
        ********/

        [Test]
        [Category("MetodoRemove")]
        [TestCase(15)]
        [TestCase(10)]
        [TestCase(2)]
        // Verifica que se pueda eliminar el último elemento de la base de datos (como una pila).
        public void Remove_EliminaUltimoElementoConExito(int element)
        {
            // Arrange
            int[] initialData = Enumerable.Range(1, element).ToArray();
            Database database = new Database(initialData);
            int[] expectedArray = initialData.Take(element - 1).ToArray();

            // Act
            database.Remove();

            // Assert
            Assert.AreEqual(expectedArray, database.Fetch());
        }

        [Test]
        [Category("MetodoRemove")]
        // Verifica que intentar eliminar de una base de datos vacía lance una excepción.
        public void Remove_BaseDeDatosVacia_LanzaExcepcion()
        {
            // Arrange
            Database database = new Database();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => database.Remove());
        }


        /******** • Los constructores deben tomar solo enteros y almacenarlos en el arreglo. ********/

        [Test]
        [Category("Constructor")]
        [TestCase(1, 2, 3)]
        [TestCase(10, 20, 30, 40)]
        [TestCase(100)]
        // Verifica que los constructores acepten solo enteros y los almacenen correctamente.
        public void Constructores_AceptanSoloEnterosYAlmacenanCorrectamente(params int[] data)
        {
            // Act
            Database database = new Database(data);

            // Assert
            Assert.AreEqual(data, database.Fetch());
        }

        /******** • El método Fetch() debe devolver los elementos como un arreglo. ********/

        [Test]
        [Category("MetodoFetch")]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 10, 20, 30, 40})]
        [TestCase(new int[] { 100 })]
        [TestCase(new int[] { })]
        // Verifica que el método Fetch() devuelva los elementos como un arreglo.
        public void MetodoFetch_DevuelveElementosComoArreglo( int[] data)
        {
            // Arrange
            Database database = new Database(data);

            // Act
            int[] result = database.Fetch();

            // Assert
            Assert.AreEqual(data.Length, result.Length);
            CollectionAssert.AreEqual(data, result);
        }
    }
}
