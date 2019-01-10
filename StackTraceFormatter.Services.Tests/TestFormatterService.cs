using System;
using StrackTraceFormatter.Services;
using Xunit;

namespace StackTraceFormatter.Services.Tests
{
    public class TestFormatterService
    {
        private FormatterService _target;

        public TestFormatterService ()
        {
            _target = new FormatterService();
        }

        [Fact]
        public void TestRemoveSuperflousLineEndings()
        {
            //Arrange
            var input =
                @"      System.Data.SqlClient.SqlException : Netzwerkbezogener oder instanzspezifischer Fehler beim Herstellen einer Verbi
ndung mit SQL Server. Der Server wurde nicht gefunden, oder auf ihn kann nicht zugegriffen werden. Überprüfen Sie, ob de
r Instanzname richtig ist und ob SQL Server Remoteverbindungen zulässt. (provider: SQL Network Interfaces, error: 26 - F
ehler beim Bestimmen des angegebenen Servers/der angegebenen Instanz)
      Stack Trace:
           bei System.Data.SqlClient.SqlInternalConnectionTds..ctor(DbConnectionPoolIdentity identity, SqlConnectionStri
ng connectionOptions, SqlCredential credential, Object providerInfo, String newPassword, SecureString newSecurePassword,
 Boolean redirectedUserInstance, SqlConnectionString userConnectionOptions, SessionData reconnectSessionData, DbConnecti
onPool pool, String accessToken, Boolean applyTransientFaultHandling, SqlAuthenticationProviderManager sqlAuthProviderMa
nager)
           bei System.Data.SqlClient.SqlConnectionFactory.CreateConnection(DbConnectionOptions options, DbConnectionPool
Key poolKey, Object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions use
rOptions)";

            //Act
            var actual = _target.RemoveSuperflousNewLines(input);

            //Assert
            Assert.Equal(
@"      System.Data.SqlClient.SqlException : Netzwerkbezogener oder instanzspezifischer Fehler beim Herstellen einer Verbindung mit SQL Server. Der Server wurde nicht gefunden, oder auf ihn kann nicht zugegriffen werden. Überprüfen Sie, ob der Instanzname richtig ist und ob SQL Server Remoteverbindungen zulässt. (provider: SQL Network Interfaces, error: 26 - Fehler beim Bestimmen des angegebenen Servers/der angegebenen Instanz)
      Stack Trace:
           bei System.Data.SqlClient.SqlInternalConnectionTds..ctor(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, SqlCredential credential, Object providerInfo, String newPassword, SecureString newSecurePassword, Boolean redirectedUserInstance, SqlConnectionString userConnectionOptions, SessionData reconnectSessionData, DbConnectionPool pool, String accessToken, Boolean applyTransientFaultHandling, SqlAuthenticationProviderManager sqlAuthProviderManager)
           bei System.Data.SqlClient.SqlConnectionFactory.CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, Object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)", actual);
        }

        [Fact]
        public void TestAddLinebreaks()
        {
            //Arrange
            const string input = @"      System.ArgumentException : Das Format der Initialisierungszeichenfolge stimmt nicht mit der Spezifikation überein, die bei Index '0' beginnt.      Stack Trace:           bei System.Data.Common.DbConnectionOptions.GetKeyValuePair(String connectionString, Int32 currentPosition, StringBuilder buffer, Boolean useOdbcRules, String& keyname, String& keyvalue)           bei System.Data.Common.DbConnectionOptions.ParseInternal(Hashtable parsetable, String connectionString, Boolean buildChain, Hashtable synonyms, Boolean firstKey)           bei System.Data.Common.DbConnectionOptions..ctor(String connectionString, Hashtable synonyms, Boolean useOdbcRules)";

            //Act
            var actual = _target.AddNewLines(input);

            //Assert
            var expected =
                @"      System.ArgumentException : Das Format der Initialisierungszeichenfolge stimmt nicht mit der Spezifikation überein, die bei Index '0' beginnt.
      Stack Trace:
           bei System.Data.Common.DbConnectionOptions.GetKeyValuePair(String connectionString, Int32 currentPosition, StringBuilder buffer, Boolean useOdbcRules, String& keyname, String& keyvalue)
           bei System.Data.Common.DbConnectionOptions.ParseInternal(Hashtable parsetable, String connectionString, Boolean buildChain, Hashtable synonyms, Boolean firstKey)
           bei System.Data.Common.DbConnectionOptions..ctor(String connectionString, Hashtable synonyms, Boolean useOdbcRules)";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtml()
        {
            //Arrange
            const string input =
                @"System.Web.HttpUnhandledException (0x80004005): Eine Ausnahme vom Typ &quot;System.Web.HttpUnhandledException&quot; wurde ausgelöst. ---&gt; System.Reflection.TargetInvocationException: Ein Aufrufziel hat einen Ausnahmefehler verursacht. ---&gt; System.InvalidOperationException: Die Sequenz enthält keine Elemente.&#xD;&#xA;   bei System.Linq.Enumerable.First[TSource](IEnumerable`1 source)&#xD;&#xA;";

            //Act
            var actual = _target.DecodeHtml(input);

            //Assert
            var expceted =
                "System.Web.HttpUnhandledException (0x80004005): Eine Ausnahme vom Typ \"System.Web.HttpUnhandledException\" wurde ausgelöst. ---> System.Reflection.TargetInvocationException: Ein Aufrufziel hat einen Ausnahmefehler verursacht. ---> System.InvalidOperationException: Die Sequenz enthält keine Elemente." + Environment.NewLine
                + "   bei System.Linq.Enumerable.First[TSource](IEnumerable`1 source)" + Environment.NewLine;
            Assert.Equal(expceted, actual);
        }
    }
}
