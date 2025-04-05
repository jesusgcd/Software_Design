using NUnit.Framework;
using System;
using System.Linq;
using ExtendedDatabase;
using System.ComponentModel;

namespace DatabaseExtended.Tests
{
    

    [TestFixture]
    public class ExtendedDatabaseTests
    {

 /*
 • Add
    o Si ya hay usuarios con este nombre de usuario, se lanza una excepción InvalidOperationException.
    o Si ya hay usuarios con este id, se lanza una excepción InvalidOperationException.
 */

        [Test]
        [TestCase("John", 1, "John", 2)]
        [TestCase("Barti Kraus Junior", 2, "Barti Kraus Junior", 3)]
        // Verifica que el método Add lance una excepción InvalidOperationException si se intenta añadir un usuario con un nombre de usuario existente.
        public void MetodoAdd_AgregarUsuarioConNombreExistente_LanzaExcepcion(string username1, long id1, string username2, long id2)
        {
            // Arrange
            Database database = new Database();
            Person person1 = new Person(id1, username1);
            Person person2 = new Person(id2, username2);

            // Act
            database.Add(person1);

            // Assert
            Assert.Throws<InvalidOperationException>(() => database.Add(person2));
        }


        [Test]
        [TestCase("John", 1, "Jane", 1)]
        [TestCase("Barti Kraus", 2, "Barti Kraus Junior", 2)]
        // Verifica que el método Add lance una excepción InvalidOperationException si se intenta añadir un usuario con un ID existente.
        public void MetodoAdd_AgregarUsuarioConIdExistente_LanzaExcepcion(string username1, long id1, string username2, long id2)
        {
            // Arrange
            Database database = new Database();
            Person person1 = new Person(id1, username1);
            Person person2 = new Person(id2, username2);

            // Act
            database.Add(person1);

            // Assert
            Assert.Throws<InvalidOperationException>(() => database.Add(person2));
        }

/*
• Remove
*/

        [Test]
        // Verifica que el método Remove elimine correctamente un usuario de la base de datos.
        public void MetodoRemove_EliminarUsuarioExistente_FlujoNormal()
        {
            // Arrange
            Database database = new Database();
            Person person = new Person(1, "John");
            database.Add(person);

            // Act
            database.Remove();

            // Assert
            Assert.AreEqual(0, database.Count);
        }

        [Test]
        // Verifica que se lance una excepción InvalidOperationException al intentar eliminar un usuario de una base de datos vacía.
        public void MetodoRemove_EliminarUsuarioDeBaseDeDatosVacia_DebeLanzarExcepcion()
        {
            // Arrange
            Database database = new Database();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => database.Remove());
        }



/*
• FindByUsername
   o Si no hay un usuario presente por ese nombre de usuario, se lanza una excepción InvalidOperationException.
   o Si el parámetro de nombre de usuario es nulo, se lanza una excepción ArgumentNullException.
   o Los argumentos son CaseSensitive.
 */


        [Test]
        [TestCase("Charlie")]
        [TestCase("Eve")]
        [TestCase("Mallory")]
        // Verifica que se lance una excepción InvalidOperationException al intentar encontrar un usuario por nombre de usuario inexistente.
        public void MetodoFindByUsername_UsuarioNoPresente_DebeLanzarExcepcion(string username)
        {
            // Arrange
            Database database = new Database();
            database.Add(new Person(1, "Alice"));
            database.Add(new Person(2, "Bob"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => database.FindByUsername(username));
        }

        [Test]
        // Verifica que se lance una excepción ArgumentNullException al intentar encontrar un usuario con nombre de usuario nulo.
        public void MetodoFindByUsername_UsernameNulo_DebeLanzarExcepcion()
        {
            // Arrange
            Database database = new Database();
            database.Add(new Person(1, "Alice"));
            database.Add(new Person(2, "Bob"));

            string username = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => database.FindByUsername(username));
        }

        [Test]
        [TestCase("alice")]
        [TestCase("AlIcE")]
        [TestCase("ALICE")]
        // Verifica que los argumentos de búsqueda sean CaseSensitive.
        public void MetodoFindByUsername_CaseSensitive(string username)
        {
            // Arrange
            Database database = new Database();
            database.Add(new Person(1, "Alice"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => database.FindByUsername(username));
        }

        [Test]
        public void MetodoFindByUsername_DebeRetornarPersonaExistente()
        // Verifica que el método FindByUsername retorne la persona correspondiente cuando existe un usuario con ese nombre.
        {
            // Arrange
            string expectedName = "Alice";
            long expectedId = 1;
            Database database = new Database();
            database.Add(new Person(expectedId, expectedName));

            // Act
            Person foundPerson = database.FindByUsername(expectedName);

            // Assert
            Assert.NotNull(foundPerson);  // Verifica que la persona encontrada no sea nula
            Assert.AreEqual(expectedName, foundPerson.UserName);  // Verifica que el nombre de usuario sea el esperado
            Assert.AreEqual(expectedId, foundPerson.Id);  // Verifica que el ID de la persona sea el esperado
        }
        /*
        • FindById
            o Si no hay un usuario presente por ese id, se lanza una excepción InvalidOperationException.
            o Si se encuentran ids negativos, se lanza una excepción ArgumentOutOfRangeException. 
         */


         [Test]
         [TestCase(100)]
         [TestCase(200)]
         [TestCase(300)]
         [TestCase(400)]
         [TestCase(500)]
         // Verifica que el método FindById lance una excepción si no hay un usuario con el ID proporcionado.
         public void MetodoFindById_DebeLanzarExcepcionSiNoHayUsuarioConId(long nonExistingId)
            {
             // Arrange
             Database database = new Database();
             database.Add(new Person(1, "Alice"));
             database.Add(new Person(2, "Bob"));
             database.Add(new Person(3, "Charlie"));

             // Act & Assert
             Assert.Throws<InvalidOperationException>(() => database.FindById(nonExistingId));
         }


        [Test]
        [TestCase(-1)] 
        [TestCase(-100)] 
        // Verifica que el método FindById lance una excepción si se proporciona un ID negativo.
        public void MetodoFindById_DebeLanzarExcepcionSiIdEsNegativo(long negativeId)
        {
            // Arrange
            Database database = new Database();
            database.Add(new Person(1, "Alice"));

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => database.FindById(negativeId));
        }

/* No olvide probar los constructores ¡también son métodos!  */

        [Test]
        // Verifica que el constructor sin argumentos inicialice la base de datos vacía.
        public void Constructor_SinArgumentos_DebeInicializarBaseDeDatosVacia()
        {
            // Arrange
            Database database = new Database();

            // Act
            int count = database.Count;

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        // Verifica que el constructor inicialice la base de datos con personas dadas.
        public void Constructor_ConPersonas_DebeInicializarBaseDeDatosConPersonasDadas()
        {
            // Arrange
            Person[] initialPersons = new Person[]
            {
                new Person(1, "Alice"),
                new Person(2, "Bob"),
                new Person(3, "Charlie")
            };

            // Act
            Database database = new Database(initialPersons);

            // Assert
            Assert.AreEqual(initialPersons.Length, database.Count);
        }

        [Test]
        // Verifica que el constructor con un número mayor a 16 personas lance una excepción.
        public void Constructor_ConMasDeDieciseisPersonas_DebeLanzarExcepcion()
        {
            // Arrange
            Person[] tooManyPersons = new Person[17];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Database(tooManyPersons));
        }

        [Test]
        // Verifica que el constructor inicialice correctamente un objeto Person con un id y un username.
        public void Constructor_ConIdYUsername_DebeInicializarPersonaCorrectamente()
        {
            // Arrange
            long id = 1;
            string username = "Alice";

            // Act
            Person person = new Person(id, username);

            // Assert
            Assert.AreEqual(id, person.Id);
            Assert.AreEqual(username, person.UserName);
        }
    }
}