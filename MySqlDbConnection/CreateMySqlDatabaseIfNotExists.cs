using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data.MySqlClient;

namespace DbLayer
{
	public class CreateMySqlDatabaseIfNotExists<TContext>
		: IDatabaseInitializer<TContext>
				where TContext : DbContext {
		public void InitializeDatabase(TContext context) {
			if (context.Database.Exists()) {
				if (!context.Database.CompatibleWithModel(false)) {
					throw new InvalidOperationException(
							"The model has changed!");
				}
			} else {
				CreateMySqlDatabase(context);
			}
		}

		private void CreateMySqlDatabase(TContext context) {
			try {
				// Create as much of the database as we can
				context.Database.Create();

				// No exception? Don't need a workaround
				return;
			} catch (MySqlException ex) {
				// Ignore the parse exception
				if (ex.Number != 1064) {
					throw;
				}
			}

			// Manually create the metadata table
			using (var connection = ((MySqlConnection)context
					.Database.Connection).Clone())
			using (var command = connection.CreateCommand()) {
				command.CommandText =
@"
CREATE TABLE __MigrationHistory (
    MigrationId mediumtext NOT NULL,
    Model mediumblob NOT NULL,
    ProductVersion mediumtext NOT NULL);
 
ALTER TABLE __MigrationHistory
ADD PRIMARY KEY (MigrationId(255));
 
INSERT INTO __MigrationHistory (
    MigrationId,
    Model,
    ProductVersion)
VALUES (
    'InitialCreate',
    @Model,
    @ProductVersion);
";
				command.Parameters.AddWithValue(
						"@Model",
						GetModel(context));
				command.Parameters.AddWithValue(
						"@ProductVersion",
						GetProductVersion());

				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		private byte[] GetModel(TContext context) {
			using (var memoryStream = new MemoryStream()) {
				using (var gzipStream = new GZipStream(
						memoryStream,
						CompressionMode.Compress))
				using (var xmlWriter = XmlWriter.Create(
						gzipStream,
						new XmlWriterSettings { Indent = true })) {
					EdmxWriter.WriteEdmx(context, xmlWriter);
				}

				return memoryStream.ToArray();
			}
		}

		private string GetProductVersion() {
			return typeof(DbContext).Assembly
					.GetCustomAttributes(false)
					.OfType<AssemblyInformationalVersionAttribute>()
					.Single()
					.InformationalVersion;
		}
	}
}

